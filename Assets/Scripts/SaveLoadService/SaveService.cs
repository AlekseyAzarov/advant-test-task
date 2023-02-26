using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using UnityEngine;

namespace ClickerLogic
{
    public class SaveService : ISaveService
    {
        private string SaveDirectory => Application.persistentDataPath;

        public bool IsFileExists(string fileName)
        {
            fileName = $"{SaveDirectory}/{fileName}";
            return File.Exists(fileName);
        }

        public T Load<T>(string fileName)
        {
            if (!IsFileExists(fileName))
            {
                throw new System.Exception($"Trying to load file that doesn't exists. FileName: {fileName}");
            }

            fileName = $"{SaveDirectory}/{fileName}";
            var fileStream = new FileStream(fileName, FileMode.Open);
            var reader = XmlDictionaryReader.CreateTextReader(fileStream, new XmlDictionaryReaderQuotas());
            var serializer = new DataContractSerializer(typeof(T));
            T serializableObject = (T)serializer.ReadObject(reader, true);
            reader.Close();
            fileStream.Close();
            return serializableObject;
        }

        public void Save<T>(T data, string fileName)
        {
            fileName = $"{SaveDirectory}/{fileName}";
            var serializer = new DataContractSerializer(typeof(T));
            var settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t",
            };
            var writer = XmlWriter.Create(fileName, settings);
            serializer.WriteObject(writer, data);
            writer.Close();
        }
    }
}
