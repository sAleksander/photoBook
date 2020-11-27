using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Arrangement
{
    class Layout
    {
        Layout() => SetLayoutSettings(Layout.Type.OnePicture);
        Layout(Layout.Type layoutType) => SetLayoutSettings(layoutType);


        static int CommentFontSize { get; }
        static int CommentOffsetInPixels { get; }

        // String, or enum Type? What is this even for?
        private string _name;
        string Name { get; }

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
                    // TODO: Set layout name
                    break;

                case Layout.Type.TwoPictures:
                    _numOfImages = 2;
                    // TODO: Set image constraints
                    // TODO: Set layout name
                    break;

                case Layout.Type.ThreePictures:
                    _numOfImages = 3;
                    // TODO: Set image constraints
                    // TODO: Set layout name
                    break;

                default:
                    throw new Exception("Wrong type of layout chosen & settings not implemented!");
            }
        }

        // TODO: Add 'changeLayout' method here?

        static Layout.Type[] AvailableLayouts => new Layout.Type[] { Type.OnePicture, Type.TwoPictures, Type.ThreePictures };
    }
}
