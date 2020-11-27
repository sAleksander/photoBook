﻿using PhotoBook.Model.Arrangement;
using PhotoBook.Model.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Pages
{
    class ContentPage : Page
    {
        // TODO: Provide appropriate constructors


        Layout Layout { get; set; }

        private Image[] _images;
        Image[] Images { get => _images; }
        private string[] _comments;
        string[] Comments { get; set; }

        void LoadImage(int layoutImageIndex, string imagePath)
        {
            if (layoutImageIndex < 0 || layoutImageIndex >= _images.Length)
                throw new Exception("Pasting photograph at an index out of range!");
            if (!File.Exists(imagePath))
                throw new Exception("File at a given path doesn't exist!");

            // TODO: Add maybe a condition checking the extension of the file?

            _images[layoutImageIndex] = new Image(imagePath);

            // TODO: Inform the others about the changes
        }
        Image GetImage(int layoutImageIndex)
        {
            if (layoutImageIndex < 0 || layoutImageIndex > _images.Length)
                throw new Exception("Getting an image from an index of out range is not possible!");
            
            return _images[layoutImageIndex];
        }

        string GetComment(int commentIndex)
        {
            if (commentIndex < 0 || commentIndex > _comments.Length)
                throw new Exception("Getting a comment from an index of out range is not possible!");

            return "";
        }
        void SetComment(int commentIndex, string contents) 
        {                     
            if (commentIndex< 0 || commentIndex> _comments.Length)
                throw new Exception("Setting a comment on an index of out range is not possible!");
            
            _comments[commentIndex] = contents;

            // TODO: Inform others about changes
        }
    }
}