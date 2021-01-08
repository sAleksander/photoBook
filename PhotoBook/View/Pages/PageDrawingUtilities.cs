using System;
using System.Windows.Media;

using WPFRectangle = System.Windows.Shapes.Rectangle;
using PhotoBookModel = PhotoBook.Model.PhotoBook;
using PhotoBook.Model.Backgrounds;

namespace PhotoBook.View.Pages
{
    static class PageDrawingUtilities
    {
        public static WPFRectangle CreateBackgroundRectangle(Background background)
        {
            var rectangle = new WPFRectangle()
            {
                Width = PhotoBookModel.PageWidthInPixels,
                Height = PhotoBookModel.PageHeightInPixels,
            };

            switch (background)
            {
                // TODO: Implement background images
                case BackgroundImage bgImage:
                    throw new NotImplementedException();
                case BackgroundColor bgColor:
                    var color = Color.FromRgb(bgColor.R, bgColor.G, bgColor.B);
                    rectangle.Fill = new SolidColorBrush(color);
                    break;
            }

            return rectangle;
        }
    }
}
