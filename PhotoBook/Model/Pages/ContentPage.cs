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
        Image[] Images { get => _images; }
        private string[] _comments;
        string[] Comments { get; set; }

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

            // TODO: Add maybe a condition checking the extension of the file?

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

        public string GetComment(int commentIndex)
        {
            if (commentIndex < 0 || commentIndex > _comments.Length)
                throw new Exception("Getting a comment from an index of out range is not possible!");

            return _comments[commentIndex];
        }
        public void SetComment(int commentIndex, string contents)
        {
            if (commentIndex< 0 || commentIndex> _comments.Length)
                throw new Exception("Setting a comment on an index of out range is not possible!");

            _comments[commentIndex] = contents;

            // TODO: Inform others about changes
        }
    }
}
