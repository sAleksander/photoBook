using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Serialization
{
    public class Serializer
    {
        private int id = 0;

        private Dictionary<int, string> objectsData = new Dictionary<int, string>();

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
                if (line == "")
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
                    stringObjectBuilder.Append($"{line}\n");
            }
        }

        public ObjectDataRelay GetObjectData(int objectID)
        {
            if (objectID < -1)
                throw new Exception("Incorrect object id provided!");

            ObjectDataRelay data;
            int corectID = objectID;

            if (objectID == -1)
                corectID = objectsData.Keys.Last();

            Dictionary<string, string> passedDictionary = new Dictionary<string, string>();

            string objectString = objectsData[corectID];

            int charIterator = 0;
            int colonIndex = 0;
            int colonBehindArrayIndex = 0;
            int endOfLineIndex = 0;
            string key;
            string value = "";

            while(charIterator < objectString.Length)
            {
                colonIndex = objectString.IndexOf(":", charIterator);

                key = objectString.Substring(charIterator, colonIndex - charIterator);

                endOfLineIndex = objectString.IndexOf('\n', colonIndex);

                // Case with arrays
                if (colonIndex + 1 == endOfLineIndex)
                {
                    value = "";

                    colonBehindArrayIndex = objectString.IndexOf(':', endOfLineIndex);
                    endOfLineIndex = objectString.LastIndexOf("\n", colonBehindArrayIndex);

                    // Iterate over all lines in an array
                    for (int valueStringIndex = colonIndex + 3; valueStringIndex < endOfLineIndex; valueStringIndex += 2)
                    {
                        var tempEndLine = objectString.IndexOf("\n", valueStringIndex);

                        // to avoid minus sign
                        value += $"{objectString.Substring(valueStringIndex, tempEndLine - valueStringIndex)}\n";

                        valueStringIndex = tempEndLine;
                    }
                }

                // General properties
                else
                    value = objectString.Substring(colonIndex + 1, endOfLineIndex - colonIndex);

                passedDictionary.Add(key, value);

                // Pass the end of line
                charIterator = endOfLineIndex + 1;
            }

            data = new ObjectDataRelay(passedDictionary);

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

        public T Deserialize<T>(int objectID)
            where T : class, SerializeInterface<T>, new()
        {
            var deserializedObject = new T();

            deserializedObject.DeserializeObject(this, objectID);

            return deserializedObject;
        }
    }
}
