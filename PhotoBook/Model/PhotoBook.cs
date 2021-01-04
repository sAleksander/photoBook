using PhotoBook.Model.Arrangement;
using PhotoBook.Model.Backgrounds;
using PhotoBook.Model.Graphics;
using PhotoBook.Model.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PhotoBook.Model
{
    public class PhotoBook
    {
        public static PhotoBook CreateNew(string projectDirPath)
        {
            PhotoBook photoBook = new PhotoBook();
            photoBook.SaveDirectory = Path.GetFullPath(projectDirPath);
            Directory.SetCurrentDirectory(projectDirPath);

            return photoBook;
        }

        public static PhotoBook Load(string configFilePath)
        {
            PhotoBook photoBook = new PhotoBook();
            photoBook.SaveDirectory = Path.GetDirectoryName(Path.GetFullPath(configFilePath));
            Directory.SetCurrentDirectory(configFilePath);

            photoBook.LoadPhotoBook();

            return photoBook;
        }

        public static PhotoBook CreateMockup()
        {
            if (Directory.Exists("mockup_project"))
            {
                Directory.Delete("mockup_project", true);
            }

            Directory.CreateDirectory("mockup_project");
            Directory.SetCurrentDirectory("mockup_project");

            PhotoBook photoBook = new PhotoBook();
            photoBook.SaveDirectory = Path.GetFullPath(Directory.GetCurrentDirectory());

            photoBook.FrontCover = new Pages.FrontCover();
            photoBook.FrontCover.Title = "Moja fotoksiążka";
            photoBook.FrontCover.Background = new Backgrounds.BackgroundColor(112, 91, 91);

            photoBook.BackCover = new Pages.BackCover();
            photoBook.BackCover.Background = new Backgrounds.BackgroundColor(112, 91, 91);

            photoBook._contentPages = new List<ContentPage>(6);

            for (int i = 0; i < 4; i++)
            {
                ContentPage contentPage = new ContentPage();

                contentPage.Layout = photoBook.AvailableLayouts[Layout.Type.TwoPictures];

                var image = contentPage.LoadImage(0, @"..\placeholder_cropped.png");
                image.CroppingRectangle = new Rectangle(
                    0, 0, 600, 575
                );

                contentPage.SetComment(0, $"Obrazek {2 * i}");

                var image2 = contentPage.LoadImage(1, @"..\placeholder_original.png");
                image2.CroppingRectangle = new Rectangle(
                    203, 115, 697, 668
                );

                contentPage.SetComment(1, $"Obrazek {2 * i + 1}");

                photoBook._contentPages.Add(contentPage);
            }

            return photoBook;
        }
        
        public static string Font { get; } = "Arial";

        public static int PageWidthInPixels { get; } = 800;
        public static int PageHeightInPixels { get; } = 1500;

        private List<ContentPage> _contentPages;
        
        public FrontCover FrontCover { get; private set; }
        public BackCover BackCover { get; private set; }
        public int NumOfContentPages { get => _contentPages.Count; }

        public string SaveDirectory { get; private set; }

        public Dictionary<Layout.Type, Layout> AvailableLayouts { get; } = Layout.CreateAvailableLayouts();

        private PhotoBook() { }

        public (ContentPage, ContentPage) GetContentPagesAt(int index)
        {
            if (index < 0 || index >= _contentPages.Count)
                throw new Exception("Wrong page index chosen!");

            var adjustedIndex = GetAdjustedIndex(index);
            return (_contentPages[adjustedIndex], _contentPages[adjustedIndex + 1]);
        }

        public (ContentPage, ContentPage) CreateNewPages(int index = -1)
        {
            var left = new ContentPage();
            var right = new ContentPage();

            if (index == -1) {
                _contentPages.Add(left);
                _contentPages.Add(right);
                return (left, right);
            }

            if (index < 0 || index > _contentPages.Count)
                throw new Exception("Wrong insert page index chosen!");

            var adjustedIndex = GetAdjustedIndex(index);
            _contentPages.Insert(adjustedIndex, right);
            _contentPages.Insert(adjustedIndex, left);

            return (left, right);
        }

        public void DeletePages(int index)
        {
            if (index < 0 || index >= _contentPages.Count)
                throw new Exception("Wrong remove page index chosen!");

            var adjustedIndex = GetAdjustedIndex(index);
            _contentPages.RemoveRange(adjustedIndex, 2);
        }

        private int GetAdjustedIndex(int index)
        {
            return index % 2 == 0 ? index : index - 1;
        }

        public void LoadPhotoBook()
        {
            if (!Directory.Exists(SaveDirectory))
                throw new Exception("Provided path directory doesn't exist!");
            if (File.Exists(SaveDirectory))
                File.Delete(SaveDirectory);

            var photoBookSaveFile = File.ReadAllLines(SaveDirectory);

            string currentPage = "";
            Stack<string> currentProperty = new Stack<string>();

            byte R = new byte();
            byte G = new byte();
            byte B = new byte();

            string path = "";
            int x = 0;
            int y = 0;
            int width = 0;
            int height = 0;

            int layoutImages = 0;
            List<Image> tempImages = new List<Image>();
            List<string> tempComments = new List<string>();

            FrontCover tempFrontCover = new FrontCover();
            ContentPage tempContentPage = new ContentPage();
            List<ContentPage> tempContentPages = new List<ContentPage>();
            BackCover tempBackCover = new BackCover();


            foreach (string line in photoBookSaveFile)
            {
                if (line.Trim() == "")
                    continue;
                                
                else
                {
                    if (line.Contains(":"))
                    {
                        var separator = line.Trim().LastIndexOf("\":");

                        var element = line.Trim().Substring(1, separator - 1);
                        var value = line.Trim().Substring(separator + 3);

                        if (element == "FrontCover" || element == "ContentPages" || element == "BackCover")
                            currentPage = element;
                        else
                        {
                            switch (currentPage)
                            {
                                case "FrontCover":
                                    switch (element)
                                    {
                                        case "Title":
                                            tempFrontCover.Title = value;
                                            break;
                                        case "R":
                                            currentProperty.Push("BackgroundColor");
                                            R = Convert.ToByte(value);                                            
                                            break;
                                        case "G":
                                            G = Convert.ToByte(value);
                                            break;
                                        case "B":
                                            B = Convert.ToByte(value);
                                            tempFrontCover.Background = new BackgroundColor(R, G, B);
                                            currentProperty.Pop();
                                            break;
                                        case "Path":
                                            path = value;
                                            break;
                                        case "X":
                                            x = Convert.ToInt32(value);
                                            break;
                                        case "Y":
                                            y = Convert.ToInt32(value);
                                            break;
                                        case "Width":
                                            width = Convert.ToInt32(value);
                                            break;
                                        case "Height":
                                            height = Convert.ToInt32(value);
                                            tempFrontCover.Background = new BackgroundImage(new Image(path, x, y, width, height));                                            
                                            break;
                                        default:
                                            break;
                                    }
                                    break;
                                case "ContentPages":
                                    switch (element)
                                    {
                                        case "BackgroundImage":
                                            currentProperty.Push("BackgroundImage");
                                            break;
                                        case "Images":
                                            currentProperty.Push("Images");                                            
                                            break;
                                        case "R":
                                            R = Convert.ToByte(value);
                                            break;
                                        case "G":
                                            G = Convert.ToByte(value);
                                            break;
                                        case "B":
                                            B = Convert.ToByte(value);
                                            tempContentPage.Background = new BackgroundColor(R, G, B);
                                            break;
                                        case "Path":
                                            path = value;
                                            break;
                                        case "X":
                                            if (currentProperty.Peek() == "Images")
                                                currentProperty.Push("Image");
                                            x = Convert.ToInt32(value);
                                            break;
                                        case "Y":
                                            y = Convert.ToInt32(value);
                                            break;
                                        case "Width":
                                            width = Convert.ToInt32(value);
                                            break;
                                        case "Height":
                                            height = Convert.ToInt32(value);

                                            if (currentProperty.Peek() == "BackgroundImage")                                            
                                                tempContentPage.Background = new BackgroundImage(new Image(path, x, y, width, height));                                                
                                            
                                            currentProperty.Pop();
                                            break;
                                        case "CurrentFilter":
                                            tempImages.Add(new Image(path, x, y, width, height));

                                            switch (value) {
                                                case "Filter.Type.Warm":
                                                    tempImages[-1].SetFilter(Filter.Type.Warm);
                                                    break;
                                                case "Filter.Type.Cold":
                                                    tempImages[-1].SetFilter(Filter.Type.Cold);
                                                    break;
                                                case "Filter.Type.Greyscale":
                                                    tempImages[-1].SetFilter(Filter.Type.Greyscale);
                                                    break;
                                                }

                                            currentProperty.Pop();
                                            if (tempImages.Count == layoutImages)                                                
                                                currentProperty.Pop();
                                            break;

                                        case "Comment":
                                            tempComments.Add(value);                                            
                                            break;

                                        default:    
                                            if(currentPage == "ContentPages" && currentProperty.Count == 0 && tempComments.Count == layoutImages)
                                            {
                                                tempContentPages.Add(new ContentPage(tempContentPage, tempImages.ToArray(), tempComments.ToArray()));
                                                tempImages.Clear();
                                                tempComments.Clear();
                                            }
                                            break;
                                    }
                                    break;

                                case "BackCover":
                                    tempBackCover.Background = tempFrontCover.Background;
                                    break;
                            }
                        }
                    }
                }
            }

            FrontCover = tempFrontCover;
            _contentPages = tempContentPages;
            BackCover = tempBackCover;            
        }

        public void SavePhotoBook()
        {
            // List of turples storing paths of original images and their used counterparts
            List<(string, string)> usedImagesPaths = new List<(string, string)>();

            // Deleting those images from project directory that aren't used anymore

            switch (FrontCover.Background)
            {
                case BackgroundImage bgi:
                    usedImagesPaths.Add((bgi.Image.OriginalPath, bgi.Image.OriginalPath));
                    break;
                default:
                    break;
            }

            foreach(ContentPage contentPage in _contentPages)            
                foreach(Image image in contentPage.Images)                
                    if (!usedImagesPaths.Contains((image.OriginalPath, image.DisplayedPath)))
                        usedImagesPaths.Add((image.OriginalPath, image.DisplayedPath));                
            
            // TODO: In the future combine below two searches into one
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

            // Saving photoBookData

            if (!File.Exists(SaveDirectory))
                File.Delete(SaveDirectory);

            StringBuilder jsonContent = new StringBuilder();
            var lvl = 0;

            jsonContent.AppendLine("{");
            lvl++;

            jsonContent.AppendLine("\t\"FrontCover\" : {");
            lvl++;

            void addIndenting(int direction = 0)
            {
                for (int i = 0; i < lvl; i++)
                    jsonContent.Append("\t");

                if (direction > 0)
                    lvl++;
                else if (direction < 0)
                    lvl--;
            }

            addIndenting(1);
            jsonContent.AppendLine($"Title : {FrontCover.Title},");
            

            switch (FrontCover.Background)
            {
                case BackgroundColor bgc:
                    addIndenting(1);
                    jsonContent.AppendLine("BackgroundColor : {");

                    addIndenting();
                    jsonContent.AppendLine($"R : {bgc.R},");
                    
                    addIndenting();
                    jsonContent.AppendLine($"G : {bgc.G},");

                    addIndenting();
                    jsonContent.AppendLine($"B : {bgc.B},");
                    break;
                
                case BackgroundImage bgi:
                    addIndenting(1);
                    jsonContent.AppendLine("BackgroundImage : {");

                    addIndenting();
                    jsonContent.AppendLine($"Path : {bgi.Image.OriginalPath},");
                    
                    addIndenting();
                    jsonContent.AppendLine($"X : {bgi.Image.CroppingRectangle.X},");
                    
                    addIndenting();
                    jsonContent.AppendLine($"Y : {bgi.Image.CroppingRectangle.Y},");
                    
                    addIndenting();
                    jsonContent.AppendLine($"Width : {bgi.Image.CroppingRectangle.Width},");
                    
                    addIndenting(-1);
                    jsonContent.AppendLine($"Height : {bgi.Image.CroppingRectangle.Height},");
                    break;
            }

            addIndenting();
            jsonContent.AppendLine("},");

            addIndenting(1);
            jsonContent.AppendLine("\"ContentPages\" : [");
            
            foreach(ContentPage page in _contentPages) {

                addIndenting(1);
                jsonContent.AppendLine("{");                

                switch (page.Background)
                {
                    case BackgroundColor bgc:
                        addIndenting(1);
                        jsonContent.AppendLine("BackgroundColor : {");

                        addIndenting();
                        jsonContent.AppendLine($"R : {bgc.R},");

                        addIndenting();
                        jsonContent.AppendLine($"G : {bgc.G},");

                        addIndenting(-1);
                        jsonContent.AppendLine($"B : {bgc.B},");
                        break;

                    case BackgroundImage bgi:
                        addIndenting(1);
                        jsonContent.AppendLine("BackgroundImage : {");
                        lvl++;

                        addIndenting();
                        jsonContent.AppendLine($"Path : {bgi.Image.OriginalPath},");

                        addIndenting();
                        jsonContent.AppendLine($"X : {bgi.Image.CroppingRectangle.X},");

                        addIndenting();
                        jsonContent.AppendLine($"Y : {bgi.Image.CroppingRectangle.Y},");

                        addIndenting();
                        jsonContent.AppendLine($"Width : {bgi.Image.CroppingRectangle.Width},");

                        addIndenting(-1);
                        jsonContent.AppendLine($"Height : {bgi.Image.CroppingRectangle.Height},");
                        break;
                }

                addIndenting();
                jsonContent.AppendLine("},");

                addIndenting();
                jsonContent.AppendLine($"\"Layout\" : {page.Layout.Name},");

                addIndenting(1);
                jsonContent.AppendLine("\"Images\" : [");                

                foreach(Image image in page.Images)
                {
                    addIndenting();
                    jsonContent.AppendLine("{");

                    addIndenting();
                    jsonContent.AppendLine($"\"X\" : {image.CroppingRectangle.X},");

                    addIndenting();
                    jsonContent.AppendLine($"\"Y\" : {image.CroppingRectangle.Y},");

                    addIndenting();
                    jsonContent.AppendLine($"\"Width\" : {image.CroppingRectangle.Width},");

                    addIndenting();
                    jsonContent.AppendLine($"\"Height\" : {image.CroppingRectangle.Height}");

                    addIndenting();
                    jsonContent.AppendLine($"\"OriginalPath\" : {image.OriginalPath},");

                    addIndenting();
                    jsonContent.AppendLine($"\"CurrentFilter\" : {image.CurrentFilter.currentType}");

                    addIndenting(-1);
                    jsonContent.AppendLine("},");
                }

                addIndenting();
                jsonContent.AppendLine("],");

                addIndenting(1);
                jsonContent.AppendLine("\"Comments\" : [");                

                foreach(string comment in page.Comments)
                {
                    addIndenting(-1); ;
                    jsonContent.AppendLine($"\"Comment\" : \"{comment}\"");
                }

                addIndenting();
                jsonContent.AppendLine("\"]\"");
            }

            lvl--;
            addIndenting();
            jsonContent.AppendLine("\"]\"");

            jsonContent.AppendLine("}");

            File.WriteAllText(SaveDirectory, jsonContent.ToString());
        }
    }
}
