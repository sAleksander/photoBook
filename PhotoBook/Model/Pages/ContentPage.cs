using PhotoBook.Model.Arrangement;
using PhotoBook.Model.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Pages
{
    public class ContentPage : Page
    {
        // TODO: Provide appropriate constructors

        #region Mockup
        public ContentPage()
        {
            _images = new Image[2];
            _comments = new string[2];
        }
        #endregion

        public Layout Layout { get; set; }

        private Image[] _images;
        public Image[] Images { get => _images; }
        private string[] _comments;
        string[] Comments { get; set; }

        #region Image functionality
        public Image LoadImage(int layoutImageIndex, string imagePath)
        {
            #region Mockup
            Image testImage = new Image(imagePath);
            _images[layoutImageIndex] = testImage;
            return testImage;
            #endregion

            if (layoutImageIndex < 0 || layoutImageIndex >= _images.Length)
                throw new Exception("Pasting photograph at an index out of range!");
            if (!File.Exists(imagePath))
                throw new Exception("File at a given path doesn't exist!");

            Image newImage = new Image(imagePath);
            _images[layoutImageIndex] = newImage;

            // TODO: Inform the others about the changes

            return newImage;
        }

        public Image GetImage(int layoutImageIndex)
        {
            if (layoutImageIndex < 0 || layoutImageIndex > _images.Length)
                throw new Exception("Getting an image from an index of out range is not possible!");

            return _images[layoutImageIndex];
        }

        public void EditImage(int layoutImageIndex, Image editedImage)
        {
            // TODO: This whether this code will be even necessary

            if (layoutImageIndex < 0 || layoutImageIndex > _images.Length)
                throw new Exception("Editing an image from an index of out range is not possible!");

            _images[layoutImageIndex] = editedImage;

            // Inform about changes
        }
        #endregion

        #region Comments functionality
        public void AddAndEditComment(int commentIndex, string commentContent)
        {
            if (commentIndex < 0 || commentIndex > _comments.Length)
                throw new Exception("Adding, setting or editing a comment to an index of out range is not possible!");

            _comments[commentIndex] = commentContent;

            // Inform about changes
        }
        public string GetComment(int commentIndex)
        {
            if (commentIndex < 0 || commentIndex > _comments.Length)
                throw new Exception("Getting a comment from an index of out range is not possible!");

            return _comments[commentIndex];
        }
        #endregion
    }
}
