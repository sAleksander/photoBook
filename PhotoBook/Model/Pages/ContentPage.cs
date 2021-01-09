using PhotoBook.Model.Arrangement;
using PhotoBook.Model.Backgrounds;
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
        public delegate void ImageChangedEventHandler(int layoutIndex);
        public event ImageChangedEventHandler ImageChanged;

        public delegate void CommentChangedEventHandler(int layoutIndex);
        public event CommentChangedEventHandler CommentChanged;

        private Layout layout;
        public Layout Layout
        {
            get => layout;
            set
            {
                layout = value;
                _images = new Image[layout.NumOfImages];
                _comments = new string[layout.NumOfImages];
                InvokePropertyChanged(nameof(Layout));
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

        // TOOD: Should layout be required for creating a ContentPage?
        public ContentPage()
        {
            Background = new BackgroundColor(83, 83, 66);
        }

        // TODO: This constructor is used when loading ContentPage from JSON,
        //       but it doesn't initialize the Layout
        public ContentPage(ContentPage cPage, Image[] images, string[] comments)
        {
            Background = cPage.Background;
            _images = images;
            _comments = comments;
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

            ImageChanged?.Invoke(layoutImageIndex);

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

            CommentChanged?.Invoke(commentIndex);

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
            double constraintsRatio = (double)imageConstraints.Width / imageConstraints.Height;
            double imageRatio = (double)image.Width / image.Height;

            if (constraintsRatio > imageRatio)
            {
                image.CroppingRectangle = new Arrangement.Rectangle(
                    0, 0,
                    (int)(image.Width),
                    (int)(image.Width / constraintsRatio)
                );
            }
            else
            {
                image.CroppingRectangle = new Arrangement.Rectangle(
                    0, 0,
                    (int)(image.Height * constraintsRatio),
                    (int)(image.Height)
                );
            }
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

            contentPage += $"{nameof(Layout)}:{Layout.SerializeObject(serializer)}\n";

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

            Layout = serializer.Deserialize<Layout>(objectData.Get<int>(nameof(Layout)));

            List<Image> tempImageList = new List<Image>();
            List<int> tempImageIndexesList = objectData.Get<List<int>>(nameof(Images));

            foreach (int tempImageIndex in tempImageIndexesList)
                tempImageList.Add(serializer.Deserialize<Image>(tempImageIndex));

            // Checking if image list contains only null values;
            var nullImagesCount = 0;

            for(int i = 0; i < tempImageList.Count; i++)
                if(tempImageList[i].DisplayedPath == null)
                {
                    nullImagesCount++;
                    tempImageList[i] = null;
                }

            if (nullImagesCount != tempImageList.Count)
                _images = tempImageList.ToArray();

            Comments = objectData.Get<List<string>>(nameof(Comments)).ToArray();

            string backgroundType = objectData.Get<string>(nameof(Background));
            int backgroundIndex = objectData.Get<int>(nameof(Background));

            switch (backgroundType)
            {
                case "BackgroundColor":
                    Background = serializer.Deserialize<BackgroundColor>(backgroundIndex);
                    break;
                case "BackgroundImage":
                    Background = serializer.Deserialize<BackgroundImage>(backgroundIndex);
                    break;
            }

            return this;
        }
    }
}
