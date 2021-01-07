﻿using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace PhotoBook.Model.Exporters
{
    class ToPdf
    {
        #region constants
        private const string LAYOUTS_DIRECTORY = "./";
        private const string PHOTOS_DIRECTORY = "./PROCESSED/";
        private const string DEFAULT_LEGAL_INFO = "All rights reserved to POLSL";
        private const string PDF_FILENAME = "autoGenerated.pdf";
        private const float A4_HEIGHT = 842;
        private const float A4_WIDTH = 595;
        #endregion

        #region private variables
        private List<string> PAGES = new List<string>();
        #endregion

        #region public variables
        public string FONT_FAMILY = "arial";
        public string FONT_COLOR = "black";
        #endregion

        #region Percent functions
        private string percent(float percentage)
        {
            return (A4_HEIGHT * (percentage / 100)).ToString(NumberFormatInfo.InvariantInfo) + "px";
        }

        private string customPercent(float parentHeight, float percentage)
        {
            return (parentHeight * (percentage / 100)).ToString(NumberFormatInfo.InvariantInfo) + "px";
        }
        #endregion

        public void CreateFrontCover(string cssBackGround, string title = "")
        {
            string page = $@"
            <div style='
                {cssBackGround}
                height: {A4_HEIGHT}px;
                page-break-inside: avoid;
            '>
                <div style='
                    margin-top: {percent(20)};
                    text-align: center;
                    color: {FONT_COLOR};
                    font-family: {FONT_FAMILY};
                    font-size: {percent(6)};
                '>
                {title}
                </div>
            </div>
            ";

            PAGES.Add(page);
        }

        public void CreatePage(List<string> photos, List<string> descriptions, string cssBackground)
        {
            const float margins_height = 2;
            const float description_height = 10;

            if (
                (photos.Count >= 1) &&
                (photos.Count <= 3) &&
                (photos.Count == descriptions.Count)
                )
            {
                float freeSpace = (98 - (photos.Count * margins_height * 2) - (photos.Count * description_height)) / photos.Count;

                string photoBuilder()
                {
                    string result = "";

                    for (int i = 0; i < photos.Count; i++)
                    {
                        result += $@"
                            <div style='
                                height: {percent(freeSpace)};
                                margin: {percent(2)};
                            '>";

                        if (photos[i] != "")
                        {
                            result += $@"
                                <img src='{PHOTOS_DIRECTORY + photos[i]}' style='
                                    height: {percent(freeSpace)};
                                '
                                alt=''
                                >
                            ";
                        }
                        else
                        {
                            result += $@"
                                <div style='height: {percent(freeSpace)}'></div>
                            ";
                        }
                        result += $@"
                                <div style='
                                    height: {percent(10)};
                                    font-size: {percent(2)};
                                    color: {FONT_COLOR};
                                    font-family: {FONT_FAMILY};
                                '>
                                    {descriptions[i]}
                                </div>
                            </div>
                        ";
                    }

                    return result;
                }

                string singlePhoto()
                {
                    string result = $@"
                        <div style='
                            height: {percent(freeSpace)};
                            margin: {percent(2)};
                        '>";
                    if (photos[0] != "")
                    {
                        result += $@"
                            <img src='{PHOTOS_DIRECTORY + photos[0]}' style='
                                width: {customPercent(A4_WIDTH, 80)};
                            alt=''
                            >
                        ";
                    } else
                    {
                        result += $@"
                                <div style='height: {percent(freeSpace)}'></div>
                            ";
                    }
                    result += $@"
                                <div style='
                                    height: {percent(10)};
                                    font-size: {percent(2)};
                                    color: {FONT_COLOR};
                                    font-family: {FONT_FAMILY};
                                '>
                                    {descriptions[0]}
                                </div>
                            </div>
                        ";

                    return result;
                }

                string page = $@"
                <div style='
                    {cssBackground}
                    height: {A4_HEIGHT}px;
                    page-break-inside: avoid;
                    text-align: center;
                '>
                    {(photos.Count > 1 ? photoBuilder() : singlePhoto())}
                </div>
                ";

                PAGES.Add(page);
            }
            else throw new ArgumentException("Wrong input arguments!");
        }

        public void CreateBackCover(string cssBackGround = "", string legalInfo = DEFAULT_LEGAL_INFO)
        {
            string page = $@"
            <div style='
                {cssBackGround}
                height: {A4_HEIGHT}px;
                page-break-inside: avoid;
            '>
                <div style='
                    margin-top: {percent(80)};
                    text-align: center;
                    color: {FONT_COLOR};
                    font-family: {FONT_FAMILY};
                    font-size: {percent(2)};
                '>
                {DEFAULT_LEGAL_INFO}
                </div>
            </div>
            ";

            PAGES.Add(page);
        }

        #region PDF generator helper
        private PdfDocument getPdfDocFrom(string htmlString)
        {
            PdfGenerateConfig config = new PdfGenerateConfig();
            config.PageOrientation = PdfSharp.PageOrientation.Portrait;
            config.PageSize = PdfSharp.PageSize.A4;

            PdfDocument doc = PdfGenerator.GeneratePdf(htmlString, config);

            return doc;
        }

        private static void addPagesToPdf(ref PdfDocument mainDoc, PdfDocument sourceDoc)

        {

            MemoryStream tempMemoryStream = new MemoryStream();
            sourceDoc.Save(tempMemoryStream, false);

            PdfDocument openedDoc = PdfReader.Open(tempMemoryStream, PdfDocumentOpenMode.Import);
            foreach (PdfPage page in openedDoc.Pages)
            {

                mainDoc.AddPage(page);

            }
        }
        #endregion

        public void GeneratePdf()
        {
            if (PAGES.Count != 0)
            {
                PdfDocument pdf = new PdfDocument();
                foreach (var item in PAGES)
                {
                    PdfDocument tmp = getPdfDocFrom(item);
                    addPagesToPdf(ref pdf, tmp);
                }

                pdf.Save(PDF_FILENAME);
            }
        }

        #region Css background generator
        public static string CssBackground(string backgroundPath)
        {
            return $@"background-image: url({"\""}{LAYOUTS_DIRECTORY + backgroundPath}{"\""});";
        }
        public static string CssBackground(int R, int G, int B)
        {
            return $@"background-color: rgb({R},{G},{B});";
        }
        #endregion

    }
}
