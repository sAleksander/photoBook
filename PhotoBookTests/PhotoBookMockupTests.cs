using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhotoBook.Model.Arrangement;
using PhotoBook.Model.Backgrounds;
using PhotoBook.Model.Graphics;
using PhotoBook.Model.Pages;
using PhotoBookModel = PhotoBook.Model.PhotoBook;

namespace PhotoBookTests
{
    [TestClass]
    public class PhotoBookMockupTests
    {
        private PhotoBookModel photoBook = new PhotoBookModel();

        [TestMethod]
        public void CheckMockup()
        {
            Assert.IsTrue(PhotoBookModel.PageWidthInPixels > 0);
            Assert.IsTrue(PhotoBookModel.PageHeightInPixels > 0);

            Assert.IsNotNull(photoBook.FrontCover);
            Assert.IsNotNull(photoBook.FrontCover.Background);
            Assert.IsNotNull(photoBook.FrontCover.Background as BackgroundColor);
            Assert.IsTrue(photoBook.FrontCover.Title.Length > 0);

            Assert.IsNotNull(photoBook.BackCover);
            Assert.IsNotNull(photoBook.BackCover.Background);
            Assert.IsNotNull(photoBook.BackCover.Background as BackgroundColor);

            Assert.IsTrue(photoBook.NumOfContentPages > 0);

            (var firstPage, var secondPage) = photoBook.GetContentPagesAt(0);

            foreach (var page in new ContentPage[2] { firstPage, secondPage }) {
                Assert.IsNotNull(page);

                Assert.IsNotNull(page.Background);
                Assert.IsNotNull(page.Background as BackgroundColor);

                Assert.AreEqual(photoBook.AvailableLayouts[Layout.Type.TwoPictures], page.Layout);

                Assert.IsTrue(page.GetComment(0).Length > 0);
                Assert.IsTrue(page.GetComment(1).Length > 0);

                for (int i = 0; i < 2; i++)
                {
                    var image = firstPage.GetImage(i);

                    Assert.IsNotNull(image);

                    Assert.IsTrue(image.CurrentFilter == Filter.Type.None);

                    Assert.IsTrue(image.Width > 0);
                    Assert.IsTrue(image.Height > 0);

                    Assert.IsTrue(image.OriginalPath.Length > 0);
                    Assert.IsTrue(image.OriginalPath == image.DisplayedPath);

                    Assert.IsNotNull(image.CroppingRectangle);
                    Assert.IsTrue(image.CroppingRectangle.Width > 0);
                    Assert.IsTrue(image.CroppingRectangle.Height > 0);
                }
            }
        }
    }
}
