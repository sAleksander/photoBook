using GalaSoft.MvvmLight.Command;
using PhotoBook.Model.Arrangement;
using PhotoBook.Model.Exporters;
using PhotoBook.Model.Helpers;
using PhotoBook.Model.Pages;
using System.Collections.Generic;
using PhotoBookModel = PhotoBook.Model.PhotoBook;

namespace PhotoBook.ViewModel.Settings
{
    public class BackCoverSettingsViewModel : SettingsViewModel
    {
        private BackCover backCover;
        public BackCover BackCover
        {
            get => backCover;
            private set => Set(nameof(BackCover), ref backCover, value);
        }

        private PhotoBookModel model;

        public BackCoverSettingsViewModel(PhotoBookModel model)
        {
            BackCover = model.BackCover;
            this.model = model;
        }


        private string fileNameExporter(string path)
        {
            string[] tmp = path.Split('\\');
            return tmp[tmp.Length - 1];
        }

        private void prepareImages()
        {
            ImageAdjuster.InitializeProcessedImageDirectory();
            for (int i = 0; i < model.NumOfContentPages; i++)
            {
                for (int j = 0; j < model.GetContentPagesAt(i).Item1.Layout.NumOfImages; j++)
                {
                    PhotoBook.Model.Graphics.Image tmp = model.GetContentPagesAt(i).Item1.GetImage(j);
                    ImageAdjuster.CropImage(
                        tmp.DisplayedPath,
                        tmp.CroppingRectangle.X,
                        tmp.CroppingRectangle.Y,
                        tmp.CroppingRectangle.Width,
                        tmp.CroppingRectangle.Height
                        );
                }
                for (int j = 0; j < model.GetContentPagesAt(i).Item2.Layout.NumOfImages; j++)
                {
                    PhotoBook.Model.Graphics.Image tmp = model.GetContentPagesAt(i).Item2.GetImage(j);
                    ImageAdjuster.CropImage(
                        tmp.DisplayedPath,
                        tmp.CroppingRectangle.X,
                        tmp.CroppingRectangle.Y,
                        tmp.CroppingRectangle.Width,
                        tmp.CroppingRectangle.Height
                        );
                }
            }
        }

        // Page related commands

        public RelayCommand ExportToPdf => new RelayCommand(() =>
        {
            prepareImages();

            ToPdf ob = new ToPdf();

            switch (model.FrontCover.Background)
            {
                case Model.Backgrounds.BackgroundColor bgColor:
                    ob.FONT_COLOR = FontAdjuster.AdjustFont(bgColor.R, bgColor.G, bgColor.B);
                    ob.CreateFrontCover(ToPdf.CssBackground(bgColor.R, bgColor.G, bgColor.B), model.FrontCover.Title);
                    break;
                case Model.Backgrounds.BackgroundImage bgImage:
                    ob.CreateFrontCover(ToPdf.CssBackground(fileNameExporter(bgImage.Image.DisplayedPath)), model.FrontCover.Title);
                    break;
            }

            for (int i = 0; i < model.NumOfContentPages; i++)
            {
                List<string> photos = new List<string>();
                List<string> descriptions = new List<string>();

                for (int j = 0; j < model.GetContentPagesAt(i).Item1.Layout.NumOfImages; j++)
                {
                    photos.Add(fileNameExporter(model.GetContentPagesAt(i).Item1.GetImage(j).DisplayedPath));
                    descriptions.Add(model.GetContentPagesAt(i).Item1.GetComment(j));
                }

                switch (model.GetContentPagesAt(i).Item1.Background)
                {
                    case Model.Backgrounds.BackgroundColor bgColor:
                        ob.FONT_COLOR = FontAdjuster.AdjustFont(bgColor.R, bgColor.G, bgColor.B);
                        ob.CreatePage(photos, descriptions, ToPdf.CssBackground(bgColor.R, bgColor.G, bgColor.B));
                        break;
                    case Model.Backgrounds.BackgroundImage bgImage:
                        ob.CreatePage(photos, descriptions, ToPdf.CssBackground(fileNameExporter(bgImage.Image.DisplayedPath)));
                        break;
                }

                photos = new List<string>();
                descriptions = new List<string>();

                for (int j = 0; j < model.GetContentPagesAt(i).Item2.Layout.NumOfImages; j++)
                {
                    photos.Add(fileNameExporter(model.GetContentPagesAt(i).Item2.GetImage(j).DisplayedPath));
                    descriptions.Add(model.GetContentPagesAt(i).Item2.GetComment(j));
                }

                switch (model.GetContentPagesAt(i).Item2.Background)
                {
                    case Model.Backgrounds.BackgroundColor bgColor:
                        ob.FONT_COLOR = FontAdjuster.AdjustFont(bgColor.R, bgColor.G, bgColor.B);
                        ob.CreatePage(photos, descriptions, ToPdf.CssBackground(bgColor.R, bgColor.G, bgColor.B));
                        break;
                    case Model.Backgrounds.BackgroundImage bgImage:
                        ob.CreatePage(photos, descriptions, ToPdf.CssBackground(fileNameExporter(bgImage.Image.DisplayedPath)));
                        break;
                }
            }

            switch (model.BackCover.Background)
            {
                case Model.Backgrounds.BackgroundColor bgColor:
                    ob.FONT_COLOR = FontAdjuster.AdjustFont(bgColor.R, bgColor.G, bgColor.B);
                    ob.CreateBackCover(ToPdf.CssBackground(bgColor.R, bgColor.G, bgColor.B));
                    break;
                case Model.Backgrounds.BackgroundImage bgImage:
                    ob.CreateBackCover(ToPdf.CssBackground(fileNameExporter(bgImage.Image.DisplayedPath)));
                    break;
            }

            ob.GeneratePdf();
        });

        public RelayCommand ExportToHtml => new RelayCommand(() =>
        {
            prepareImages();

            ToHtml.FONT_COLOR = "red";

            ToHtml ob = new ToHtml(model.NumOfContentPages/2);

            string fileNameExporter(string path)
            {
                string[] tmp = path.Split('\\');
                return tmp[tmp.Length - 1];
            }

            switch (model.FrontCover.Background)
            {
                case Model.Backgrounds.BackgroundColor bgColor:
                    ToHtml.FONT_COLOR = FontAdjuster.AdjustFont(bgColor.R, bgColor.G, bgColor.B);
                    ob.CreateFrontCover(ToHtml.CssBackground(bgColor.R, bgColor.G, bgColor.B), model.FrontCover.Title);
                    break;
                case Model.Backgrounds.BackgroundImage bgImage:
                    ob.CreateFrontCover(ToHtml.CssBackground(fileNameExporter(bgImage.Image.DisplayedPath)), model.FrontCover.Title);
                    break;
            }

            for (int i = 0; i < model.NumOfContentPages; i+=2)
            {
                ToHtml.Page left = new ToHtml.Page(ToHtml.CssBackground(0, 0, 0));

                switch (model.GetContentPagesAt(i).Item1.Background)
                {
                    case Model.Backgrounds.BackgroundColor bgColor:
                        left = new ToHtml.Page(ToHtml.CssBackground(bgColor.R, bgColor.G, bgColor.B),
                            FontAdjuster.AdjustFont(bgColor.R, bgColor.G, bgColor.B));
                        break;
                    case Model.Backgrounds.BackgroundImage bgImage:
                        left = new ToHtml.Page(ToHtml.CssBackground(fileNameExporter(bgImage.Image.DisplayedPath)));
                        break;
                }

                for (int j = 0; j < model.GetContentPagesAt(i).Item1.Layout.NumOfImages; j++)
                {
                    left.AddPhotoWithDescription(
                        fileNameExporter(model.GetContentPagesAt(i).Item1.GetImage(j).DisplayedPath),
                        model.GetContentPagesAt(i).Item1.GetComment(j)
                        );
                }

                ToHtml.Page right = new ToHtml.Page(ToHtml.CssBackground(0, 0, 0));

                switch (model.GetContentPagesAt(i).Item2.Background)
                {
                    case Model.Backgrounds.BackgroundColor bgColor:
                        right = new ToHtml.Page(ToHtml.CssBackground(bgColor.R, bgColor.G, bgColor.B),
                            FontAdjuster.AdjustFont(bgColor.R, bgColor.G, bgColor.B));
                        break;
                    case Model.Backgrounds.BackgroundImage bgImage:
                        right = new ToHtml.Page(ToHtml.CssBackground(fileNameExporter(bgImage.Image.DisplayedPath)));
                        break;
                }

                for (int j = 0; j < model.GetContentPagesAt(i).Item2.Layout.NumOfImages; j++)
                {
                    right.AddPhotoWithDescription(
                        fileNameExporter(model.GetContentPagesAt(i).Item2.GetImage(j).DisplayedPath),
                        model.GetContentPagesAt(i).Item2.GetComment(j)
                        );
                }

                ob.AddBookPages(left, right);
            }

            switch (model.BackCover.Background)
            {
                case Model.Backgrounds.BackgroundColor bgColor:
                    ToHtml.FONT_COLOR = FontAdjuster.AdjustFont(bgColor.R, bgColor.G, bgColor.B);
                    ob.CreateBackCover(ToHtml.CssBackground(bgColor.R, bgColor.G, bgColor.B));
                    break;
                case Model.Backgrounds.BackgroundImage bgImage:
                    ob.CreateBackCover(ToHtml.CssBackground(fileNameExporter(bgImage.Image.DisplayedPath)));
                    break;
            }
        });

    }
}
