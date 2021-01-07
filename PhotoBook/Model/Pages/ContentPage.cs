using PhotoBook.Model.Arrangement;
using PhotoBook.Model.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Image = PhotoBook.Model.Graphics.Image;
using PhotoBook.Model.Serialization;
using PhotoBook.Model.Backgrounds;

namespace PhotoBook.Model.Pages
{
    public class ContentPage : Page, SerializeInterface<ContentPage>
    {
        // TOOD: Should layout be required for creating a ContentPage?
        public ContentPage()
        {
        }

        // TODO: This constructor is used when loading ContentPage from JSON,
        //       but it doesn't initialize the Layout
        public ContentPage(ContentPage cPage, Image[] images, string[] comments)
        {
            Background = cPage.Background;
            _images = images;
            _comments = comments;
        }

        private Layout layout;
        public Layout Layout
        {
            get => layout;
            set
            {
                layout = value;
                _images = new Image[layout.NumOfImages];
                _comments = new string[layout.NumOfImages];
            }
        }

        private Image[] _images;
        public Image[] Images {
            get => _images;
            private set
            {
                _images = value;
            }
        }
        private string[] _comments;
        public string[] Comments
        {
            get => _comments;
            private set
            {
                _comments = value;
            }
        }

        #region Image functionality
        public Image LoadImage(int layoutImageIndex, string imagePath)
        {
            if (layout is null)
                throw new Exception("Can't load image - no layout");
            if (layoutImageIndex < 0 || layoutImageIndex >= _images.Length)
                throw new Exception("Pasting photograph at an index out of range!");
            if (!File.Exists(imagePath))
                throw new Exception("File at a given path doesn't exist!");

            Image newImage = new Image(imagePath);
            AutoCropImage(newImage, Layout.ImageConstraints[layoutImageIndex]);

            _images[layoutImageIndex] = newImage;

            return newImage;
        }

        public Image GetImage(int layoutImageIndex)
        {
            if (layout is null)
                throw new Exception("Can't get image - no layout");
            if (layoutImageIndex < 0 || layoutImageIndex >= _images.Length)
                throw new Exception("Getting an image from an index of out range is not possible!");

            return _images[layoutImageIndex];
        }
        #endregion

        #region Comments functionality
        public void SetComment(int commentIndex, string commentContent)
        {
            if (layout is null)
                throw new Exception("Can't set comment - no layout");
            if (commentIndex < 0 || commentIndex >= _comments.Length)
                throw new Exception("Adding, setting or editing a comment to an index of out range is not possible!");

            _comments[commentIndex] = commentContent;
        }

        public string GetComment(int commentIndex)
        {
            if (layout is null)
                throw new Exception("Can't get comment - no layout");
            if (commentIndex < 0 || commentIndex >= _comments.Length)
                throw new Exception("Getting a comment from an index of out range is not possible!");

            return _comments[commentIndex];
        }
        #endregion

        private void AutoCropImage(Image image, Arrangement.Rectangle imageConstraints)
        {
            double scaleX = imageConstraints.Width / image.Width;
            double scaleY = imageConstraints.Height / image.Height;

            if (scaleX < scaleY)
            {
                image.CroppingRectangle = new Arrangement.Rectangle(
                    0, 0,
                    (int)(scaleX * image.Width),
                    (int)(scaleX * image.Height)
                );
            }
            else
            {
                image.CroppingRectangle = new Arrangement.Rectangle(
                    0, 0,
                    (int)(scaleY * image.Width),
                    (int)(scaleY * image.Height)
                );
            }
        }

        public override void setBackground(int R = -1, int G = -1, int B = -1, string path = "", int X = -1, int Y = -1, int Width = -1, int Height = -1)
        {
            if ((R == -1 || G == -1 || B == -1) && (path == "" || X == -1 || Y == -1 || Width == -1 || Height == -1))
                throw new Exception("Incorect data sent to setBackground method for FrontCover");

            if (R == -1 || G == -1 || B == -1)
                Background = new BackgroundImage(new Graphics.Image(path, X, Y, Width, Height));
            else
                Background = new BackgroundColor((byte)R, (byte)G, (byte)B);
        }

        public int SerializeObject(Serializer serializer)
        {
            string contentPage = $"{nameof(Images)}:\n";

            if (_images.Length > 0)
                foreach (Image image in _images)
                {
                    if (image != null)
                        contentPage += $"-&{image.SerializeObject(serializer)}\n";
                    else
                        contentPage += $"-&-1\n";
                }

            contentPage += $"{nameof(Comments)}:\n";

            foreach (string comment in _comments)
                contentPage += $"-\"{comment}\"\n";

            int backgroundID = 0;
            string resultType = "";

            switch (Background)
            {
                case BackgroundColor bgc:
                    backgroundID = bgc.SerializeObject(serializer);
                    resultType = "BackgroundColor";
                    break;

                case BackgroundImage bgi:
                    backgroundID = bgi.SerializeObject(serializer);
                    resultType = "BackgroundImage";
                    break;
            }

            contentPage += $"Background:&{backgroundID},{resultType}";

            int contentPageID = serializer.AddObject(contentPage);

            return contentPageID;
        }

        public ContentPage DeserializeObject(Serializer serializer, int objectID)
        {
            ObjectDataRelay objectData = serializer.GetObjectData(objectID);

            List<Image> tempImageList = new List<Image>();
            List<int> tempImageIndexesList = objectData.Get<List<int>>(nameof(Images));

            foreach (int tempImageIndex in tempImageIndexesList)
            {
                tempImageList.Add(new Image());
                tempImageList[tempImageList.Count - 1] = tempImageList[tempImageList.Count - 1].DeserializeObject(serializer, tempImageIndex);
            }

            _images = tempImageList.ToArray();

            Comments = objectData.Get<List<string>>(nameof(Comments)).ToArray();

            string backgroundType = objectData.Get<string>(nameof(Background));
            int backgroundIndex = objectData.Get<int>(nameof(Background));

            switch (backgroundType)
            {
                case "BackgroundColor":
                    Background = (Background as BackgroundColor).DeserializeObject(serializer, backgroundIndex);
                    break;
                case "BackgroundImage":
                    Background = (Background as BackgroundImage).DeserializeObject(serializer, backgroundIndex);
                    break;
            }

            return this;
        }
    }
}
