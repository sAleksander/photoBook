using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Arrangement
{
    public class Layout
    {
        public enum Type
        {
            OnePicture,
            TwoPictures,
            ThreePictures
        }

        public static Dictionary<Type, Layout> CreateAvailableLayouts() => new Dictionary<Type, Layout>
        {
            {
                Layout.Type.OnePicture, new Layout("Jedno zdjęcie")
                {
                    // TODO: Set image constaints
                }
            },
            {
                Layout.Type.TwoPictures, new Layout("Dwa zdjęcia")
                {
                    // TODO: Set image constaints
                }
            },
            {
                Layout.Type.ThreePictures, new Layout("Trzy zdjęcia")
                {
                    // TODO: Set image constaints
                }
            }
        };

        private Layout(string nameOfLayout)
        {
            _name = nameOfLayout;
        }


        public static int CommentFontSize { get; }
        public static int CommentOffsetInPixels { get; }

        // String, or enum Type? What is this even for?
        private string _name;
        public string Name { get; }

        private int _numOfImages;
        public int NumOfImages { get => _numOfImages; }

        private Rectangle[] _imageConstraints;
        public Rectangle[] ImageConstraints { get => _imageConstraints; }
    }
}
