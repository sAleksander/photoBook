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

            // Inform about changes
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

        public int SerializeObject(Serializer serializer)
        {
            string contentPage = $"{Images}:\n";

            foreach (Image image in _images)
                contentPage += $"-&{image.SerializeObject(serializer)}\n";

            contentPage += $"{Comments}:\n";

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
            ObjectDataRelay objectData = serializer.GetObjectData2(objectID);

            List<Image> tempImageList = new List<Image>();
            List<int> tempImageIndexesList = objectData.Get<List<int>>(nameof(Images));

            foreach (int tempImageIndex in tempImageIndexesList)
            {
                tempImageList.Add(new Image());
                tempImageList[-1] = tempImageList[-1].DeserializeObject(serializer, tempImageIndex);
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
