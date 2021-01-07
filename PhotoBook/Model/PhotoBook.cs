using PhotoBook.Model.Arrangement;
using PhotoBook.Model.Backgrounds;
using PhotoBook.Model.Graphics;
using PhotoBook.Model.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoBook.Model.Serialization;
using System.Diagnostics;

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

            Directory.SetCurrentDirectory(projectDirPath);
            PhotoBook photoBook = new PhotoBook();
            photoBook.SavePath = Path.GetFullPath(projectDirPath);

            photoBook.FrontCover = new FrontCover();
            photoBook.BackCover = new BackCover();

            return photoBook;
        }

        public PhotoBook(string savePath) {
            if (!Directory.Exists(savePath))
                throw new Exception("Provided directory path doesn't exist!");

            SavePath = savePath;
            Directory.SetCurrentDirectory(savePath);
            LoadPhotoBook();
        }

        public string SavePath { get; private set; }
        public static string Font { get; private set; } = "Arial";

        public static int PageWidthInPixels { get; }
        public static int PageHeightInPixels { get; }


        private List<ContentPage> _contentPages = new List<ContentPage>();

        public FrontCover FrontCover { get; private set; }
        public BackCover BackCover { get; private set; }
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
            if (!Directory.Exists("\\OriginalImages"))
                Directory.CreateDirectory("OriginalImages");

            if (!Directory.Exists("\\UsedImages"))
                Directory.CreateDirectory("UsedImages");

            FrontCover = new Pages.FrontCover();
            FrontCover.Title = "Moja fotoksiążka";
            FrontCover.Background = new Backgrounds.BackgroundColor(112, 91, 91);

            BackCover = new Pages.BackCover();
            BackCover.Background = new Backgrounds.BackgroundColor(112, 91, 91);
        }
        #endregion

        public (ContentPage, ContentPage) GetContentPagesAt(int index)
        {
            if (index < 0 || index >= _contentPages.Count)
                throw new Exception("Wrong page index chosen!");

            var adjustedIndex = GetAdjustedIndex(index);
            return (_contentPages[adjustedIndex], _contentPages[adjustedIndex + 1]);
        }

        public void CreateNewPages(int index = -1)
        {
            if (index == -1) {
                _contentPages.Add(new ContentPage());
                _contentPages.Add(new ContentPage());
                return;
            }

            if (index < 0 || index >= _contentPages.Count)
                throw new Exception("Wrong insert page index chosen!");

            var adjustedIndex = GetAdjustedIndex(index);
            _contentPages.Insert(adjustedIndex, new ContentPage());
            _contentPages.Insert(adjustedIndex, new ContentPage());
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
            SavePath = "C:\\Users\\Wojtek\\Desktop\\MyPhotoBook";

            serializer = new Serializer();

            serializer.LoadData($"{SavePath}\\saveFile.pbf");

            DeserializeObject(serializer);
        }

        public void SavePhotoBook()
        {
            SerializeObject(serializer);

            serializer.SaveObjects($"{SavePath}\\saveFile.pbf");
        }

        public Serializer serializer;

        public int SerializeObject(Serializer s)
        {
            serializer = new Serializer();

            string photoBook = $"{nameof(SavePath)}:{SavePath}\n";

            photoBook += $"{nameof(Font)}:{Font}\n";

            photoBook += $"{nameof(FrontCover)}:&{FrontCover.SerializeObject(serializer)}\n";

            photoBook += $"{nameof(_contentPages)}:\n";

            foreach (ContentPage content_page in _contentPages)
                photoBook += $"-&{content_page.SerializeObject(serializer)}\n";

            photoBook += $"{nameof(BackCover)}:&{BackCover.SerializeObject(serializer)}";

            serializer.AddObject(photoBook);

            serializer.SaveObjects(SavePath);

            return 0;
        }

        public PhotoBook DeserializeObject(Serializer serializer, int objectID = -1)
        {
            ObjectDataRelay objectData = serializer.GetObjectData(-1);

            Font = objectData.Get<string>(nameof(Font));

            // Front cover

            FrontCover = FrontCover.DeserializeObject(serializer, objectData.Get<int>(nameof(FrontCover)));

            // Content pages

            _contentPages.Clear();

            List<int> contentPagesIndexes = objectData.Get<List<int>>(nameof(_contentPages));

            foreach (var contentPageIndex in contentPagesIndexes)
            {
                _contentPages.Add(new ContentPage());
                _contentPages[_contentPages.Count - 1] = _contentPages[_contentPages.Count - 1].DeserializeObject(serializer, contentPageIndex);
            }

            // Back cover

            BackCover = BackCover.DeserializeObject(serializer, objectData.Get<int>(nameof(BackCover)));

            return new PhotoBook();
        }
    }
}
