using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Graphics
{
    public class Filter
    {
        public Filter() { 
            SetFilterSettings(Filter.Type.None);
            currentType = Type.None;
        }
        public Filter(Filter.Type filterType)
        {
            SetFilterSettings(filterType);
            currentType = filterType;
        }

        private Type currentType;
        private int[,] _settings;
        int[,] Settings { get => _settings; }

        public enum Type
        {
            None,
            Warm,
            Cold,
            Greyscale
        }

        void SetFilterSettings(Filter.Type filterType)
        {
            // TODO: Test out those settings
            // change to double precision if needed

            switch (filterType) {
                case Filter.Type.Cold:
                    _settings = new int[3, 3]
                    {
                        {2, 0, 0},
                        {0, 2, 0},
                        {0, 0, 3}
                    };
                    break;

                case Filter.Type.Warm:
                    _settings = new int[3, 3]
                    {
                        {3, 0, 0},
                        {0, 2, 0},
                        {0, 0, 2}
                    };
                    break;

                default:
                    _settings = new int[3, 3]
                    {
                        {0, 0, 0},
                        {0, 0, 0},
                        {0, 0, 0}
                    };
                    break;
            }
        }

        // The method below in the future should return a picture with a filter added as an argument
        // Arguments & return type should be added/adjusted as well - originalImagePath & Filter.Type?
        public Bitmap applyFilter(Bitmap originalBitmap)
        {
            Bitmap editedBitmap = originalBitmap;

            switch (currentType)
            {
                case Type.Greyscale:
                    for (int i = 0; i < editedBitmap.Width; i++)
                        for (int j = 0; j < editedBitmap.Height; j++)
                        {
                            Color pixel = editedBitmap.GetPixel(i, j);

                            int average = (pixel.R + pixel.G + pixel.B) / 3;

                            editedBitmap.SetPixel(i, j, Color.FromArgb(average, average, average));
                        }
                    break;

                default:
                    int sum = 0;
                    int R = 0;
                    int G = 0;
                    int B = 0;

                    for (int i = 0; i < _settings.GetLength(0); i++)
                        for (int j = 0; j < editedBitmap.Height; j++)
                            sum += Convert.ToInt32(Settings[i, j]);

                    if (sum <= 0) sum = 1;
                    double K = 1.0 / sum;

                    for (int btmW = 0; btmW < originalBitmap.Width; btmW++)
                    {
                        for (int btmH = 0; btmH < originalBitmap.Height; btmH++)
                        {
                            for (int filW = 0; filW < _settings.GetLength(0); filW++)
                            {
                                for (int filH = 0; filH < _settings.GetLength(0); filH++)
                                {
                                    if (btmW - filW < 0 || btmH - filH < 0) continue;
                                    else
                                    {
                                        Color pixel = originalBitmap.GetPixel(btmW - filW, btmH - filH);
                                        R += _settings[filW, filH] * pixel.R;
                                        G += _settings[filW, filH] * pixel.G;
                                        B += _settings[filW, filH] * pixel.B;
                                    }

                                }
                            }

                            R = Convert.ToInt16(K * R);
                            G = Convert.ToInt16(K * G);
                            B = Convert.ToInt16(K * B);

                            if (R > 255) R = 255;
                            else if (R < 0) R = 0;

                            if (G > 255) G = 255;
                            else if (G < 0) G = 0;

                            if (B > 255) B = 255;
                            else if (B < 0) B = 0;
                            
                            editedBitmap.SetPixel(btmW, btmH, Color.FromArgb(255, R, G, B));

                            R = 0;
                            G = 0;
                            B = 0;
                        }
                    }
                    break;
            }
            return editedBitmap;
        }

        public static Type[] GetAvailableTypes() => new Type[] { Type.Cold, Type.Warm, Type.Greyscale, Type.None };
    }
}
