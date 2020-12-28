using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Serialization
{
    public class Serializer
    {
        private int id = 0;

        Dictionary<int, string> objectsData = new Dictionary<int, string>();

        public int ID { get => id++; }

        public int AddObject(string passedObject)
        {
            // TODO: Check if such object exists in objectsData

            // TODO: Return newly serialized object's id

            return 0;
        }
    }
}
