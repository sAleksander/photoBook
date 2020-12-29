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

namespace PhotoBook.Model.Pages
{
    public class ContentPage : Page
    {
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
        public Image[] Images { get => _images; }
        private string[] _comments;
        public string[] Comments { get; private set; }

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
    }
}
