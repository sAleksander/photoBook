using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Arrangement
{
    class Layout
    {
        private Layout(Layout.Type layoutType, string nameOfLayout)
        {
            _name = nameOfLayout;
            SetLayoutSettings(layoutType);
        }


        static int CommentFontSize { get; }
        static int CommentOffsetInPixels { get; }

        // String, or enum Type? What is this even for?
        private string _name;
        public string Name { get; }

        private int _numOfImages;
        int NumOfImages { get => _numOfImages; }

        private Rectangle[] _imageConstraints;
        Rectangle[] ImageConstraints { get => _imageConstraints; }

        public enum Type
        {
            OnePicture,
            TwoPictures,
            ThreePictures
        }

        void SetLayoutSettings(Layout.Type layoutType)
        {
            // TODO: Implement necessary settings
            switch (layoutType)
            {
                case Layout.Type.OnePicture:
                    _numOfImages = 1;
                    // TODO: Set image constraints
                    break;

                case Layout.Type.TwoPictures:
                    _numOfImages = 2;
                    // TODO: Set image constraints
                    break;

                case Layout.Type.ThreePictures:
                    _numOfImages = 3;
                    // TODO: Set image constraints
                    break;

                default:
                    throw new Exception("Wrong type of layout chosen & settings not implemented!");
            }
        }


        public static Layout[] CreateAvailableLayouts => new Layout[]
        {
            new Layout(Layout.Type.OnePicture, "Jedno zdjęcie"),
            new Layout(Layout.Type.TwoPictures, "Dwa zdjęcia"),
            new Layout(Layout.Type.ThreePictures, "Trzy zdjęcia")
        };
    }
}
