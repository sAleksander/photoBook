using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Serialization
{
    public class ObjectDataRelay : ObjectDataRetrieverInterface<string>
    {
        public ObjectDataRelay(Dictionary<string, string> passedDictionary)
        {
            objectData = passedDictionary;
        }

        Dictionary<string, string> objectData = new Dictionary<string, string>();

        public T Get<T>(string propertyName)
        {
            // Background id
            if (typeof(T) == typeof(int) && propertyName == "Background")
            {
                string backgroundString = objectData[propertyName];

                int backgroundID = int.Parse(backgroundString.Substring(backgroundString.IndexOf('&') + 1, backgroundString.IndexOf(',') - (backgroundString.IndexOf('&') + 1)));

                return (T)Convert.ChangeType(backgroundID, typeof(T));
            }

            // Background type
            else if (typeof(T) == typeof(string) && propertyName == "Background")
            {
                string backgroundString = objectData[propertyName];

                string backgroundType = backgroundString.Substring(backgroundString.IndexOf(',') + 1, backgroundString.Length - 1 - (backgroundString.IndexOf(',') + 1));

                return (T)Convert.ChangeType(backgroundType, typeof(T));
            }

            // Page indexes & image indexes
            else if (typeof(T) == typeof(List<int>))
            {
                string pageIndexesString = objectData[propertyName];

                List<int> pageIndexes = new List<int>();

                int charIterator = pageIndexesString.IndexOf('&');
                int index = 0;

                while (charIterator != -1 && charIterator < pageIndexesString.Length)
                {
                    // charIterator + 1 - to get avoid & sign
                    index = int.Parse(pageIndexesString.Substring(charIterator + 1, pageIndexesString.IndexOf("\n", charIterator) - (charIterator + 1)));

                    pageIndexes.Add(index);

                    charIterator = pageIndexesString.IndexOf('&', charIterator + 1);
                    if (charIterator < 0)
                        break;
                }

                return (T)Convert.ChangeType(pageIndexes, typeof(T));
            }

            // Comments
            else if (typeof(T) == typeof(List<string>))
            {
                string commentsString = objectData[propertyName];

                List<string> comments = new List<string>();

                int charIterator = commentsString.IndexOf('\"');
                string comment = "";

                while (charIterator != -1 && charIterator < commentsString.Length)
                {
                    comment = commentsString.Substring(charIterator + 1, commentsString.IndexOf("\"\n", charIterator) - (charIterator + 1));

                    if (comment.Contains("\n"))
                        comment = comment.Trim('\n');

                    if (comment.Contains("\""))
                        comment = comment.Trim('\"');

                    comments.Add(comment);

                    charIterator = commentsString.IndexOf('\"', commentsString.IndexOf("\"\n", charIterator) + 1);
                }

                return (T)Convert.ChangeType(comments, typeof(T));
            }

            else if (typeof(T) == typeof(int) || typeof(T) == typeof(byte))
            {
                var result = objectData[propertyName];

                if (result.StartsWith("&"))
                    result = result.Substring(1);

                if (result.Contains("\n"))
                    result = result.Trim('\n');

                return (T)Convert.ChangeType(result, typeof(T));
            }

            else if (typeof(T) == typeof(int))
                return (T)Convert.ChangeType(objectData[propertyName].Trim(), typeof(T));

            else if (typeof(T) == typeof(string))
            {
                var result = objectData[propertyName];

                if (result.StartsWith("&"))
                    result = result.Substring(1);

                if (result.Contains("\n"))
                    result = result.Trim('\n');

                if (result.Contains("\""))
                    result = result.Trim('\"');

                return (T)Convert.ChangeType(result, typeof(T));
            }

            else
                throw new Exception("Wrong data passed to get method when deserializing!");
        }
    }
}
