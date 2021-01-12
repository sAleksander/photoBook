using PhotoBook.Model.Arrangement;
using PhotoBook.Model.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using PhotoBook.Model.Serialization;
using System.Runtime.Serialization;


namespace PhotoBook.Model
{
    public class PhotoBook : SerializeInterface<PhotoBook>
    {
        public static PhotoBook CreateNew(string projectDirPath)
        {

            if (!Directory.Exists(projectDirPath))
                Directory.CreateDirectory(projectDirPath);
            else
            {
                System.IO.FileAttributes attr = File.GetAttributes(projectDirPath);
                var extension = Path.GetExtension(projectDirPath);

                if (!attr.HasFlag(FileAttributes.Directory))
                    throw new Exception("The given path for photobook location isn't a directory");
                if (extension != string.Empty)
                    throw new Exception("File location provided when creating a new PhotoBook object");
            }

            PhotoBook photoBook = new PhotoBook();
            photoBook.SaveDirectory = Path.GetFullPath(projectDirPath);
            Directory.SetCurrentDirectory(projectDirPath);

            photoBook.FrontCover.Title = "Moja fotoksiążka";

            var (left, right) = photoBook.CreateNewPages();

            left.Layout = photoBook.AvailableLayouts[Layout.Type.TwoPictures];
            left.SetComment(0, "Opis");
            left.SetComment(1, "Opis");

            right.Layout = photoBook.AvailableLayouts[Layout.Type.OnePicture];
            right.SetComment(0, "Opis");

            return photoBook;
        }

        public static PhotoBook Load(string configFilePath)
        {
            PhotoBook photoBook = new PhotoBook();
            photoBook.SaveDirectory = Path.GetDirectoryName(Path.GetFullPath(configFilePath));
            Directory.SetCurrentDirectory(photoBook.SaveDirectory);

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

            photoBook.FrontCover.Title = "Moja fotoksiążka";
            photoBook.FrontCover.Background = new Backgrounds.BackgroundColor(112, 91, 91);

            photoBook.BackCover.Background = new Backgrounds.BackgroundColor(112, 91, 91);

            photoBook._contentPages = new List<ContentPage>(6);

            for (int i = 0; i < 2; i++)
            {
                ContentPage contentPage = new ContentPage();

                contentPage.Layout = photoBook.AvailableLayouts[Layout.Type.TwoPictures];

                contentPage.LoadImage(0, @"..\placeholder_cropped.png");
                contentPage.SetComment(0, $"Obrazek {3 * i}");

                contentPage.LoadImage(1, @"..\placeholder_original.png");
                contentPage.SetComment(1, $"Obrazek {3 * i + 1}");

                photoBook._contentPages.Add(contentPage);

                contentPage = new ContentPage();

                contentPage.Layout = photoBook.AvailableLayouts[Layout.Type.OnePicture];

                contentPage.LoadImage(0, @"..\placeholder_cropped.png");
                contentPage.SetComment(0, $"Obrazek {3 * i + 2}");

                photoBook._contentPages.Add(contentPage);
            }

            return photoBook;
        }

        public static string Font { get; } = "Arial";

        public static int PageWidthInPixels { get; } = 790;
        public static int PageHeightInPixels { get; } = 1120;

        private List<ContentPage> _contentPages = new List<ContentPage>();

        public FrontCover FrontCover { get; private set; } = new FrontCover();
        public BackCover BackCover { get; private set; } = new BackCover();
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
            serializer = new Serializer();

            serializer.LoadData($"{SaveDirectory}\\saveFile.pbf");

            DeserializeObject(serializer);
        }

        public void SavePhotoBook()
        {
            SerializeObject(serializer);

            serializer.SaveObjects($"{SaveDirectory}\\saveFile.pbf");
        }


        public Serializer serializer;

        public int SerializeObject(Serializer s)
        {
            serializer = new Serializer();

            string photoBook = $"{nameof(SaveDirectory)}:{SaveDirectory}\n";

            photoBook += $"{nameof(FrontCover)}:&{FrontCover.SerializeObject(serializer)}\n";

            photoBook += $"{nameof(_contentPages)}:\n";

            foreach (ContentPage content_page in _contentPages)
                photoBook += $"-&{content_page.SerializeObject(serializer)}\n";

            photoBook += $"{nameof(BackCover)}:&{BackCover.SerializeObject(serializer)}";

            serializer.AddObject(photoBook);

            serializer.SaveObjects($"{SaveDirectory}\\saveFile.pbf");

            return 0;
        }

        public PhotoBook DeserializeObject(Serializer serializer, int objectID = -1)
        {
            ObjectDataRelay objectData = serializer.GetObjectData(-1);

            // Front cover
            FrontCover = serializer.Deserialize<FrontCover>(objectData.Get<int>(nameof(FrontCover)));

            // Content pages
            _contentPages.Clear();

            List<int> contentPagesIndexes = objectData.Get<List<int>>(nameof(_contentPages));

            foreach (var contentPageIndex in contentPagesIndexes)
                _contentPages.Add(serializer.Deserialize<ContentPage>(contentPageIndex));

            // Back cover
            BackCover = serializer.Deserialize<BackCover>(objectData.Get<int>(nameof(BackCover)));

            return null;
        }
    }
}
