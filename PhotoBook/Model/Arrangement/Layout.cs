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
            #region Mockup
            {
                Layout.Type.TwoPictures, new Layout("Dwa zdjęcia")
                {
                    _numOfImages = 2,
                    _imageConstraints = new Rectangle[]
                    {
                        new Rectangle(
                            100,
                            50,
                            PhotoBook.PageWidthInPixels - 200, // 600
                            (PhotoBook.PageHeightInPixels - (50 + 150 + 150)) / 2 // 575
                        ),
                        new Rectangle(
                            100,
                            50 + (PhotoBook.PageHeightInPixels - (50 + 150 + 150)) / 2 + 150, // 725
                            PhotoBook.PageWidthInPixels - 200, // 600
                            (PhotoBook.PageHeightInPixels - (50 + 150 + 150)) / 2 // 575
                        ),
                    }
                    
                }
            },
            #endregion
        };

        #region Mockup
        static Layout()
        {
            CommentFontSize = 32;
            CommentOffsetInPixels = 50;
        }
        #endregion

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
