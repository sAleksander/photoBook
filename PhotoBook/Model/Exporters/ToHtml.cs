using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Exporters
{
    class ToHtml
    {
        #region Constants
        const string MAIN_DIRECTORY = ".\\Website";
        const string RELATIVE_LAYOUT_DIRECTORY = "Layout";
        const string RELATIVE_PHOTOS_DIRECTORY = "Photos";
        const string LAYOUT_DIRECTORY = MAIN_DIRECTORY + "\\" + RELATIVE_LAYOUT_DIRECTORY;
        const string PHOTOS_DIRECTORY = MAIN_DIRECTORY + "\\" + RELATIVE_PHOTOS_DIRECTORY;
        const string IMPORT_LAYOUT_DIRECTORY = ".\\";
        const string IMPORT_PHOTOS_DIRECTORY = ".\\PROCESSED";
        const string FRONT_COVER_FILE = "front.html";
        const string BACK_COVER_FILE = "back.html";
        #endregion

        #region Variables
        public static string FONT_FAMILY = "arial";
        public static string FONT_COLOR = "black";

        int current_page = 0;
        int page_count = 0;
        #endregion

        public ToHtml(int pageAmount)
        {
            page_count = pageAmount;
            FONT_FAMILY = "arial";
            FONT_COLOR = "black";

            Directory.CreateDirectory(MAIN_DIRECTORY);

            Directory.CreateDirectory(LAYOUT_DIRECTORY);

            /*
            foreach (var item in Directory.GetFiles(IMPORT_LAYOUT_DIRECTORY, "*.png", SearchOption.TopDirectoryOnly))
            {
                File.Copy(item, LAYOUT_DIRECTORY + "\\" + Path.GetFileName(item), true);
            }
            */

            Directory.CreateDirectory(PHOTOS_DIRECTORY);

            foreach (var item in Directory.GetFiles(IMPORT_PHOTOS_DIRECTORY, "*.*", SearchOption.TopDirectoryOnly))
            {
                File.Copy(item, PHOTOS_DIRECTORY + "\\" + Path.GetFileName(item), true);
            }
        }

        public void CreateFrontCover(string cssBackground = "", string title = "")
        {
            Debug.WriteLine($"Front: {FONT_COLOR}");
            string page = $@"
            <!doctype html>
            <html lang='pl' style='height: 100%;'>
            <head>
            <meta charset='utf-8'>
            <title>Photo book</title>
            </head>
            <body style='height: 98%; display:flex;'>
            <div style='height: 100%; flex-grow:1;'>
            <div style='height: 95%; display: flex;'>
            <div style='margin-left: 25%; margin-right: 25%; width: 50%; {cssBackground}'>
            <div style='text-align: center; font-size: 4vw; margin-top: 45vh; color: {FONT_COLOR}; font-family: {FONT_FAMILY}'>
            {title}
            </div>
            </div>
            </div>
            <div style='height: 5%; display:flex;'>
            <div style='width:50%;'>
            </div>
            <div style='width:50%; text-align: right;'>
            <button style='font-size: 2.5vh;'>
            <a href='{(page_count == 0 ? "back" : "1")}.html' style='text-decoration: none; color: black;'>
            Next
            </a>
            </button>
            </div>
            </div>
            </div>
            </body>
            </html>
            ";

            File.WriteAllText(MAIN_DIRECTORY + "\\" + FRONT_COVER_FILE, page);
        }

        public void CreateBackCover(string cssBackground = "", string legalInfo = "Photo book created using PhotoBook")
        {
            Debug.WriteLine($"Back: {FONT_COLOR}");
            string page = $@"
            <!doctype html>
            <html lang='pl' style='height: 100%;'>
            <head>
            <meta charset='utf-8'>
            <title>Photo book</title>
            </head>
            <body style='height: 98%; display:flex;'>
            <div style='height: 100%; flex-grow:1;'>
            <div style='height: 95%; display: flex;'>
            <div style='margin-left: 25%; margin-right: 25%; width: 50%; {cssBackground}'>
            <div style='text-align: center; font-size: 1vw; margin-top: 80vh; color: {FONT_COLOR}; font-family: {FONT_FAMILY};'>
            {legalInfo}
            </div>
            </div>
            </div>
            <div style='height: 5%; display:flex;'>
            <div style='width:50%; text-align: left;'>
            <button style='font-size: 2.5vh;'>
            <a href='{(page_count == 0 ? "front" : page_count.ToString())}.html' style='text-decoration: none; color: black;'>
            Prev
            </a>
            </button>
            </div>
            <div style='width:50%; text-align: right;'>
            </div>
            </div>
            </div>
            </body>
            </html>
            ";

            File.WriteAllText(MAIN_DIRECTORY + "\\" + BACK_COVER_FILE, page);
        }


        public class Page
        {
            private List<string> photos = new List<string>();
            private List<string> descriptions = new List<string>();
            private string cssBackground = "";
            private string LOCAL_FONT_COLOR = "white";

            public Page(string cssBackground, string fontColor = "white")
            {
                this.cssBackground = cssBackground;
                LOCAL_FONT_COLOR = fontColor;
            }

            //photo,description
            public void AddPhotoWithDescription(string photo, string description = "")
            {
                photos.Add(photo);
                descriptions.Add(description);
            }
            public string GeneratePhotosOnPage()
            {
            Debug.WriteLine($"Page: {FONT_COLOR}");

                string singlePhoto()
                {
                    string tmp = $@"
                        <div style='height: 90%; margin: 2%;'>
                        <img src='{RELATIVE_PHOTOS_DIRECTORY}/{photos[0]}' style='width: auto; height: 80vh;' alt=''>
                        <div style='text-align: center; height: 10%; font-size: 2vh; color: {LOCAL_FONT_COLOR}; font-family: {FONT_FAMILY};'>
                        {descriptions[0]}
                        </div>
                        </div>
                    ";
                    return tmp;
                }

                string twoPhotos()
                {
                    string tmp = $@"
                        <div style='height: 44%; margin: 2%;'>
                        <img src='{RELATIVE_PHOTOS_DIRECTORY}/{photos[0]}' style='width: 30vw; height: auto;' alt=''>
                        <div style='text-align: center; height: 10%; font-size: 2vh; color: {LOCAL_FONT_COLOR}; font-family: {FONT_FAMILY};'>
                        {descriptions[0]}
                        </div>
                        </div>

                        <div style='height: 44%; margin: 2%;'>
                        <img src='{RELATIVE_PHOTOS_DIRECTORY}/{photos[1]}' style='width: 30vw; height: auto;' alt=''>
                        <div style='text-align: center; height: 10%; font-size: 2vh; color: {LOCAL_FONT_COLOR}; font-family: {FONT_FAMILY};'>
                        {descriptions[1]}
                        </div>
                        </div>
                    ";
                    return tmp;
                }

                string threePhotos()
                {
                    string tmp = $@"
                        <div style='height: 27%; margin: 2%;'>
                        <img src='{RELATIVE_PHOTOS_DIRECTORY}/{photos[0]}' style='width: 26vw; height: auto;' alt=''>
                        <div style='text-align: center; height: 10%; font-size: 2vh; color: {LOCAL_FONT_COLOR}; font-family: {FONT_FAMILY};'>
                        {descriptions[0]}
                        </div>
                        </div>
                        <div style='height: 27%; margin: 2%;'>
                        <img src='{RELATIVE_PHOTOS_DIRECTORY}/{photos[1]}' style='width: 26vw; height: auto;' alt=''>
                        <div style='text-align: center; height: 10%; font-size: 2vh; color: {LOCAL_FONT_COLOR}; font-family: {FONT_FAMILY}';>
                        {descriptions[1]}
                        </div>
                        </div>
                        <div style='height: 27%; margin: 2%;'>
                        <img src='{RELATIVE_PHOTOS_DIRECTORY}/{photos[2]}' style='width: 26vw; height: auto;' alt=''>
                        <div style='text-align: center; height: 10%; font-size: 2vh; color: {LOCAL_FONT_COLOR}; font-family: {FONT_FAMILY};'>
                        {descriptions[2]}
                        </div>
                        </div>
                    ";
                    return tmp;
                }

                string insert = "";

                switch (photos.Count)
                {
                    case 1: insert = singlePhoto(); break;
                    case 2: insert = twoPhotos(); break;
                    case 3: insert = threePhotos(); break;
                }

                string result = $@"
                <div style='width: 50%; display: flex; flex-direction: column; text-align: center; {cssBackground}'>
                {insert}
                </div>
                ";
                return result;
            }

        }

        public void AddBookPages(Page leftPage, Page rightPage)
        {
            current_page++;
            string page = $@"
                <!doctype html>
                <html lang='pl' style='height: 100%;'>
                <head>
                <meta charset='utf-8'>
                <title> Photo book </title>
                </head>
                <body style=' height: 98%; display: flex; '>
                <div style=' height: 100%; flex-grow: 1; '>
                <div style=' height: 95%; display: flex; flex-grow: 1; '>
                {leftPage.GeneratePhotosOnPage()}
                {rightPage.GeneratePhotosOnPage()}
                </div>
                <div style='height: 5%; display: flex; '>
                <div style='width: 50%; '>
                <button style='font-size: 2.5vh; '> 
                <a href='{(current_page <= 1 ? FRONT_COVER_FILE : $"{current_page - 1}.html")}' style='text-decoration: none; color: black; '>
                Prev
                </a>
                </button>
                </div>
                <div style='width: 50%; text-align: right; '>
                <button style=' font-size: 2.5vh; '>
                <a href='{(current_page >= page_count ? BACK_COVER_FILE : $"{current_page + 1}.html")}' style='text-decoration: none; color: black;' >
                Next
                </a>
                </button>
                </div>
                </div>
                </div>
                </body>
                </html>
                ";

            File.WriteAllText(MAIN_DIRECTORY + "\\" + $"{current_page}.html", page);
        }


        #region Css background generator
        public static string CssBackground(string backgroundPath)
        {
            return $@"background-image: url({"\""}{RELATIVE_LAYOUT_DIRECTORY + "/" + backgroundPath}{"\""});";
        }
        public static string CssBackground(int R, int G, int B)
        {
            return $@"background-color: rgb({R},{G},{B});";
        }
        #endregion
    }
}
