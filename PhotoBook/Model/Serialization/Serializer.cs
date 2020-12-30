using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Serialization
{
    public class Serializer
    {
        private int id = 0;

        Dictionary<int, string> objectsData = new Dictionary<int, string>();

        public int ID
        {
            get => id++;
            private set {
                id = value;
            }
        }

        public int AddObject(string passedObject)
        {
            int referenceID = -1;

            if (objectsData.ContainsValue(passedObject))
                referenceID = objectsData.FirstOrDefault(x => x.Value == passedObject).Key;

            if (referenceID == -1) {
                referenceID = ID;
                objectsData.Add(referenceID, passedObject);
            }

            return referenceID;
        }

        public void LoadData(string saveFilePath)
        {
            if (!File.Exists(saveFilePath))
                throw new Exception("Provided save file doesn't exist!");

            objectsData.Clear();

            string[] saveFileContent = File.ReadAllLines(saveFilePath);
            string idLine = "";
            StringBuilder stringObjectBuilder = new StringBuilder();
            int tempID;

            foreach(string line in saveFileContent)
            {
                if (line == "\n")
                {
                    tempID = int.Parse(idLine.Substring(idLine.LastIndexOf(":") + 1));

                    ID = tempID;

                    objectsData.Add(tempID, stringObjectBuilder.ToString());

                    stringObjectBuilder.Clear();
                    idLine = "";

                    continue;
                }

                if (idLine == "")
                    idLine = line;
                else
                    stringObjectBuilder.Append(line);
            }
        }

        public string GetObjectData(int objectID)
        {
            if (objectID < -1)
                throw new Exception("Incorrect object id provided!");

            string data = "";
            int corectID = objectID;

            if (objectID == -1)
                corectID = objectsData.Keys.Last();

            data = objectsData[corectID];

            return data;
        }

        public void SaveObjects(string saveFilePath)
        {
            if (File.Exists(saveFilePath))
                File.Delete(saveFilePath);

            string saveFileContent = "";

            foreach(var item in objectsData)
                saveFileContent += $"id:{item.Key}\n{item.Value}\n\n";

            File.WriteAllText(saveFilePath, saveFileContent);
        }
    }
}
