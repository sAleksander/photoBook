using PhotoBook.Model.Arrangement;
using PhotoBook.Model.Graphics;
using PhotoBook.Model.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model
{
    public class PhotoBook
    {
        public PhotoBook(string savePath) {
            if (!Directory.Exists(savePath))
                throw new Exception("Provided directory path doesn't exist!");

            SavePath = savePath;
            Directory.SetCurrentDirectory(savePath);
        }
        
        public string SavePath { get; private set; }
        public static string Font { get; } = "Arial";

        public static int PageWidthInPixels { get; }
        public static int PageHeightInPixels { get; }


        private List<ContentPage> _contentPages;

        public FrontCover FrontCover { get; }
        public BackCover BackCover { get; }
        public int NumOfContentPages { get => _contentPages.Count; }

        public Dictionary<Layout.Type, Layout> AvailableLayouts { get; } = Layout.CreateAvailableLayouts();

        #region Mockup
        static PhotoBook()
        {
            PageWidthInPixels = 800;
            PageHeightInPixels = 1500;
        }

        public PhotoBook()
        {
            if (!Directory.Exists("\\Images"))
                Directory.CreateDirectory("Images");

            FrontCover = new Pages.FrontCover();
            FrontCover.Title = "Moja fotoksiążka";
            FrontCover.Background = new Backgrounds.BackgroundColor(112, 91, 91);

            BackCover = new Pages.BackCover();
            BackCover.Background = new Backgrounds.BackgroundColor(112, 91, 91);

            _contentPages = new List<ContentPage>(6);

            for (int i = 0; i < 6; i++)
            {
                ContentPage contentPage = new ContentPage();

                contentPage.Layout = AvailableLayouts[Layout.Type.TwoPictures];
                contentPage.Background = new Backgrounds.BackgroundColor(83, 83, 66);

                for (int j = 0; j < 2; j++)
                {
                    var image = contentPage.LoadImage(j, "placeholder_cropped.png");
                    image.CroppingRectangle = new Rectangle(
                        0, 0, image.Width, image.Height
                    );

                    contentPage.AddAndEditComment(j, $"Obrazek {j}");
                }

                _contentPages.Add(contentPage);
            }
        }
        #endregion

        public (ContentPage, ContentPage) GetContentPagesAt(int index)
        {
            #region Mockup
            return (_contentPages[0], _contentPages[1]);
            #endregion

            if (index == -1)
                return (_contentPages[NumOfContentPages - 2], _contentPages[NumOfContentPages - 1]);

            else if (index >= 0 && index < _contentPages.Count)
            {
                byte adjustedIndex = (byte)index;

                if (index % 2 == 1)
                    adjustedIndex = (byte)(index - 1);

                return (_contentPages[adjustedIndex], _contentPages[adjustedIndex + 1]);
            }

            else
                throw new Exception("Wrong page index chosen!");
        }

        public void CreateNewPages(int index = -1)
        {
            #region Mockup
            throw new NotImplementedException("Not available in mockup version");
            #endregion

            if (index == -1){
                _contentPages.Add(new ContentPage());
                _contentPages.Add(new ContentPage());
            }

            else if (index >= 0 && index < _contentPages.Count){
                // Provides inserting pages between sheets & not pages
                byte adjustedIndex = (byte)index;

                if(index % 2 == 1)
                    adjustedIndex = (byte)(index - 1);

                _contentPages.Insert(adjustedIndex, new ContentPage());
                _contentPages.Insert(adjustedIndex, new ContentPage());
            }

            else
                throw new Exception("Wrong insert page index chosen!");

            // Remind about changes here
        }

        public void DeletePages(int index = -1)
        {
            if(index == -1)
            {
                _contentPages.RemoveAt(_contentPages.Count - 1);
                _contentPages.RemoveAt(_contentPages.Count - 1);
            }

            else if (index >= 0 && index < _contentPages.Count)
            {
                byte adjustedIndex = (byte)index;

                if (index % 2 == 1)
                    adjustedIndex = (byte)(index - 1);

                _contentPages.RemoveAt(adjustedIndex);
                _contentPages.RemoveAt(adjustedIndex);
            }

            else
                throw new Exception("Wrong remove page index chosen!");

            // Remind about changes here
        }

        public void EditPage(int index, ContentPage editedPage)
        {
            if (index == -1)
                _contentPages[_contentPages.Count - 1] = editedPage;

            else if (index >= 0 && index < _contentPages.Count)
                _contentPages[index] = editedPage;

            else
                throw new Exception("Wrong edit page index chosen!");

            // Remind about changes here
        }

        public void LoadPhotoBook(string path)
        {
            if (!Directory.Exists(path))
                throw new Exception("Provided directory path doesn't exist!");

            SavePath = path;
            Directory.SetCurrentDirectory(path);

            // TODO: Implement the rest 
        }

        public string SavePhotoBook(string path)
        {
            // List of turples storing paths of original images and their used counterparts
            List<(string, string)> usedImagesPaths = new List<(string, string)>();

            // Deleting those images from project directory that aren't used anymore

            if (FrontCover.Background.GetType().Equals(typeof(Backgrounds.BackgroundImage)))
            {
                // TODO: Solve below's problem with not being able to reference backgroundimage type fields
                // listOfUsedImages.Add(FrontCover.Background.Image.OriginalPath);
            }

            foreach(ContentPage contentPage in _contentPages)            
                foreach(Image image in contentPage.Images)                
                    if (!usedImagesPaths.Contains((image.OriginalPath, image.DisplayedPath)))
                        usedImagesPaths.Add((image.OriginalPath, image.DisplayedPath));                
            

            if (BackCover.Background.GetType().Equals(typeof(Backgrounds.BackgroundImage)))
            {
                // TODO: Solve below's problem with not being able to reference backgroundimage type fields
                // listOfUsedImages.Add(BackCover.Background.Image.OriginalPath);
            }

            // TODO: Combine below two searches into one
            List<string> allOriginalImagesPaths = Directory.GetFiles("OriginalImages", "([^\\s]+(\\.(?i)(jpg|png))$)").ToList();
            List<string> allUsedImagesPaths = Directory.GetFiles("UsedImages", "([^\\s]+(\\.(?i)(jpg|png))$)").ToList();


            var firstPaths = usedImagesPaths.Select(t => t.Item1).ToList();
            var secondPaths = usedImagesPaths.Select(t => t.Item2).ToList();

            for (int i = 0; i <= allOriginalImagesPaths.Count; i++)            
                if (!firstPaths.Contains(allOriginalImagesPaths[i]))
                    File.Delete(allOriginalImagesPaths[i]);

            for (int i = 0; i <= allUsedImagesPaths.Count; i++)
                if (!secondPaths.Contains(allUsedImagesPaths[i]))
                    File.Delete(allUsedImagesPaths[i]);

            // TODO: Implement exporting & saving all photobook info

            return "";
        }
    }
}
