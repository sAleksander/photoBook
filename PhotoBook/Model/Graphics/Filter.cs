﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Graphics
{
    class Filter
    {
        Filter() => SetFilterSettings(Filter.Type.None);
        Filter (Filter.Type filterType) => SetFilterSettings(filterType);

        private double[,] _settings;
        double[,] Settings { get => _settings; }

        public enum Type
        {
            None,
            Warm,
            Cold,
            Greyscale
        }

        void SetFilterSettings(Filter.Type filterType)
        {
            // TODO: Implement necessary settings
            switch (filterType) {
                case Filter.Type.Cold:
                    _settings = new double[3, 3]
                    {
                        {0, 0, 0},
                        {0, 0, 0},
                        {0, 0, 0}
                    };
                    break;

                case Filter.Type.Warm:
                    _settings = new double[3, 3]
                    {
                        {0, 0, 0},
                        {0, 0, 0},
                        {0, 0, 0}
                    };
                    break;

                case Filter.Type.Greyscale:
                    // REMARK: This should rather use averages of R, G, B values of every pixel rather than matrix
                    _settings = new double[3, 3]
                    {
                        {0, 0, 0},
                        {0, 0, 0},
                        {0, 0, 0}
                    };
                    break;

                case Filter.Type.None:
                    _settings = new double[3, 3]
                    {
                        {0, 0, 0},
                        {0, 0, 0},
                        {0, 0, 0}
                    };
                    break;

                default:
                    // Not sure whether it's needed, due to specific enum argument type provided
                    throw new Exception("Wrong type of filter chosen & settings not implemented!");
            }
        }

        // The method below in the future should return a picture with a filter added as an argument
        // Arguments & return type should be added/adjusted as well - originalImagePath & Filter.Type?
        public static void applyFilter()
        {
        }

        public static Type[] GetAvailableTypes() => new Type[] { Type.Cold, Type.Warm, Type.Greyscale, Type.None };
    }
}
