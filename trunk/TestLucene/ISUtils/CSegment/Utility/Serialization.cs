using System;
using System.Collections.Generic;
using System.Text;

namespace Lwh.ChineseSegment.Utility
{
    internal static class Serialization
    {
        public static bool SerializeXml(string filePath, object target)
        {
            if (target == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }

            try
            {
                System.Xml.Serialization.XmlSerializerFactory xmlSerializerFactory = new System.Xml.Serialization.XmlSerializerFactory();
                System.Xml.Serialization.XmlSerializer xmlSerializer = xmlSerializerFactory.CreateSerializer(target.GetType(), target.GetType().Name);
                System.IO.Stream stream = new System.IO.FileStream(filePath, System.IO.FileMode.OpenOrCreate);
                xmlSerializer.Serialize(stream, target);
                stream.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static object DeserializeXml(string filePath, Type targetType)
        {
            if (targetType == null)
            {
                return null;
            }

            if (!Validator.IsValidFile(filePath))
            {
                return null;
            }

            try
            {
                System.Xml.Serialization.XmlSerializerFactory xmlSerializerFactory = new System.Xml.Serialization.XmlSerializerFactory();
                System.Xml.Serialization.XmlSerializer xmlSerializer = xmlSerializerFactory.CreateSerializer(targetType, targetType.Name);
                System.IO.Stream stream = new System.IO.FileStream(filePath, System.IO.FileMode.Open);
                object obj = xmlSerializer.Deserialize(stream);
                stream.Close();
                return obj;
            }
            catch
            {
                return null;
            }

        }


        public static bool SerializeBin(string filePath, object target)
        {
            if (target == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }

            try
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                System.IO.Stream stream = new System.IO.FileStream(filePath, System.IO.FileMode.OpenOrCreate);
                formatter.Serialize(stream, target);
                stream.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static object DeserializeBin(string filePath, Type targetType)
        {
            if (targetType == null)
            {
                return null;
            }

            if (!Validator.IsValidFile(filePath))
            {
                return null;
            }

            try
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                System.IO.Stream stream = new System.IO.FileStream(filePath, System.IO.FileMode.Open);
                object obj = formatter.Deserialize(stream);
                stream.Close();
                return obj;
            }
            catch
            {
                return null;
            }

        }
    }
}
