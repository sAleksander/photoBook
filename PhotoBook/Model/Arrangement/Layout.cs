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

        string Name { get; }

        int NumOfImages { get; }
        Rectangle[] ImageConstraints { get; }
        
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
                    break;

                case Layout.Type.TwoPictures:                    
                    break;

                case Layout.Type.ThreePictures:                    
                    break;

                default:
                    throw new Exception("Wrong type of layout chosen & settings not implemented!");
            }
        }
        Type[] AvailableLayouts => new Type[] { Type.OnePicture, Type.TwoPictures, Type.ThreePictures };
    }
}
