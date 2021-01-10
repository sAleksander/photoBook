using PhotoBook.Model.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Arrangement
{
    public class Layout : SerializeInterface<Layout>
    {
        public enum Type
        {
            OnePicture,
            TwoPictures
        }

        public static Dictionary<Type, Layout> CreateAvailableLayouts() => new Dictionary<Type, Layout>
        {
            {
                Layout.Type.OnePicture, new Layout("Jedno zdjęcie")
                {
                    _numOfImages = 1,
                    _imageConstraints = new Rectangle[]
                    {
                        new Rectangle(120, 160, 560, 800),
                    }
                }
            },
            {
                Layout.Type.TwoPictures, new Layout("Dwa zdjęcia")
                {
                    _numOfImages = 2,
                    _imageConstraints = new Rectangle[]
                    {
                        new Rectangle(120, 100, 560, 400),
                        new Rectangle(120, 620, 560, 400),
                    }
                }
            }
        };

        private Layout(string nameOfLayout)
        {
            _name = nameOfLayout;
        }

        public Layout()
        {
        }

        public static int CommentFontSize { get; } = 32;
        public static int CommentOffsetInPixels { get; } = 25;

        private string _name;
        public string Name { get => _name; }

        private int _numOfImages;
        public int NumOfImages { get => _numOfImages; }

        private Rectangle[] _imageConstraints;
        public Rectangle[] ImageConstraints { get => _imageConstraints; }

        public int SerializeObject(Serializer serializer)
        {
            string layout;

            switch (_numOfImages)
            {
                case 1:
                    layout = $"currentLayout:{Layout.Type.OnePicture}";
                    break;
                case 2:
                    layout = $"currentLayout:{Layout.Type.TwoPictures}";
                    break;
                default:
                    throw new Exception("Wrong type of layout chosen!");
            }

            int layoutID = serializer.AddObject(layout);

            return layoutID;
        }

        public Layout DeserializeObject(Serializer serializer, int objectID)
        {
            ObjectDataRelay objectData = serializer.GetObjectData(objectID);

            Dictionary<Type, Layout> tempDictionary = CreateAvailableLayouts();

            string layout = objectData.Get<string>("currentLayout");

            Enum.TryParse(layout, out Layout.Type layoutEnum);

            if(layoutEnum != Layout.Type.OnePicture && layoutEnum != Layout.Type.TwoPictures)
                throw new Exception("Wrong layout type met when deserializing layout!");
            else
            {
                _numOfImages = tempDictionary[layoutEnum]._numOfImages;
                _imageConstraints = tempDictionary[layoutEnum]._imageConstraints;
            }

            return this;
        }
    }
}
