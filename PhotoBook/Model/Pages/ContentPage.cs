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
            string contentPageData = serializer.GetObjectData(objectID);

            // TODO: Check if it is correct

            int attributeIndex = contentPageData.IndexOf($"{nameof(Images)}");
            int dividerIndex = contentPageData.IndexOf(':', attributeIndex);
            int endOfLineIndex = contentPageData.IndexOf($"{nameof(Comments)}", dividerIndex);

            List<Image> tempImageList = new List<Image>();

            int currentLine = contentPageData.IndexOf("-&", dividerIndex);
            int endOfCurrentLine = contentPageData.IndexOf("\n", currentLine);

            int imageId = 0;

            while (endOfCurrentLine < endOfLineIndex)
            {
                imageId = int.Parse(contentPageData.Substring(currentLine + 1, endOfLineIndex));
                // Make an empty image object
                tempImageList.Add(new Image(""));
                tempImageList[-1] = tempImageList[-1].DeserializeObject(serializer, imageId);

                currentLine = contentPageData.IndexOf("-&", endOfCurrentLine);
                endOfCurrentLine = contentPageData.IndexOf("\n", endOfCurrentLine);
            }

            Images = tempImageList.ToArray();

            attributeIndex = contentPageData.IndexOf($"{nameof(Comments)}");
            dividerIndex = contentPageData.IndexOf(':', attributeIndex);
            endOfLineIndex = contentPageData.LastIndexOf($"\n", dividerIndex);

            currentLine = contentPageData.IndexOf("-", dividerIndex);
            endOfCurrentLine = contentPageData.IndexOf("\n", currentLine);

            string[] tempComments = new string[Images.Length];
            string commentContent = "";
            // it will be treated as a comment iterator
            imageId = 0;

            // -1 w razie gdyby trafiło się na koniec tekstu
            while (endOfCurrentLine < endOfLineIndex || endOfCurrentLine != -1)
            {
                // currentLine +2 and endOfLineIndex to avoid \" signs
                commentContent = contentPageData.Substring(currentLine + 2, endOfLineIndex - 2);
                tempComments[imageId] = commentContent;
                imageId++;

                currentLine = contentPageData.IndexOf("-&", endOfCurrentLine);
                endOfCurrentLine = contentPageData.IndexOf("\n", endOfCurrentLine);
            }

            Comments = tempComments;

            attributeIndex = contentPageData.IndexOf($"{nameof(Background)}");
            dividerIndex = contentPageData.IndexOf(':', attributeIndex);
            int commaIndex = contentPageData.IndexOf(':', dividerIndex);
            endOfLineIndex = contentPageData.IndexOf("\n", commaIndex);

            int backgroundIndex = int.Parse(contentPageData.Substring(dividerIndex + 2, commaIndex));
            string backgroundType = contentPageData.Substring(commaIndex + 1, endOfLineIndex);

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
