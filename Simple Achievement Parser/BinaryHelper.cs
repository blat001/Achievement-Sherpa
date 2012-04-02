using System;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
using System.Runtime.Serialization;

namespace AchievementSherpa.Business
{
    /// <author>Yazeed Hamdan</author>
    /// <summary>
    /// Serialize/Deserialize any object from/to any file or serialize/Deserialize it
    /// into/From bytes array
    /// </summary>
    public static class BinarySerializerHelper
    {
        #region Constants
        /// <summary>
        /// Default Binary File Extension 
        /// </summary>
        private const string FILE_EXTENSION = ".dat";
        /// <summary>
        /// Category for Exceptions thrown
        /// </summary>
        private const string CATEGORY = "BinarySerializerHelper::{0}";

        #endregion

        #region Members
        /// <summary>
        /// Singleton object of BinaryFormatter
        /// </summary>
        private static BinaryFormatter _formatter = new BinaryFormatter();

        #endregion

        #region Methods
        /// <summary>
        /// Serialize Any object to a file
        /// </summary>
        /// <param name="currentObject"><see cref="System.Object"/></param>
        /// <param name="filename">file name and where this file will be saved</param>
        /// <remarks>The application should have enough permissions to
        /// access/create a file on the targeted machine</remarks>
        public static void Serialize(object currentObject, string filename)
        {
            if (null == currentObject)
                throw new ArgumentNullException(string.Format(
                    CATEGORY, "Serialize, Passed Object to Serialize Is Null"));

            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException(string.Format(
                    CATEGORY, "Serialize,Cannot Serialize object, Filename Is Null"));

            //Append File Extension
            filename += FILE_EXTENSION;

            using (Stream fileStream = new FileStream(filename, FileMode.Create))
                _formatter.Serialize(fileStream, currentObject);

        }
        /// <summary>
        /// Serialize any object to a bytes array
        /// </summary>
        /// <param name="currentObject"><see cref="System.Object"/></param>
        /// <returns><see cref="System.Byte"/></returns>
        public static byte[] Serialize(object currentObject)
        {
            if (null == currentObject)
                throw new ArgumentNullException(string.Format(
                    CATEGORY, "Serialize, Object to Serialize Is Null"));

            byte[] binaryData = null;

            using (Stream memoryStream = new MemoryStream())
            {
                _formatter.Serialize(memoryStream, currentObject);
                memoryStream.Position = 0;
                binaryData = new byte[memoryStream.Length];
                memoryStream.Read(binaryData, 0, Convert.ToInt32(memoryStream.Length));
            }

            return binaryData;
        }
        /// <summary>
        /// Deserialize any binary data to an object from a bytes array 
        /// </summary>        
        /// <returns><see cref="System.byte"/></returns>
        /// <remarks>Client should be aware of the object type</remarks>
        public static object Deserialize(byte[] binaryData)
        {
            if (null == binaryData || binaryData.Length == 0)
                throw new ArgumentNullException(string.Format(
                    CATEGORY, "Deserialize, binaryData Is Null or Empty"));

            object deserializedObject = null;

            using (Stream memoryStream = new MemoryStream())
            {
                memoryStream.Write(binaryData, 0, binaryData.Length);
                memoryStream.Position = 0;
                deserializedObject = _formatter.Deserialize(memoryStream);
            }

            return deserializedObject;
        }
        /// <summary>
        /// Deserialize any binary data to an object from a file 
        /// </summary>
        /// <param name="filename">where the binary data is located</param>
        /// <returns><see cref="System.Object"/></returns>
        /// <remarks>Client should be aware of the object type</remarks>
        public static object Deserialize(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException(string.Format(
                    CATEGORY, "Deserialize, Filename Is Null"));

            object deserializedObject = null;

            using (Stream fileStream = File.OpenRead(filename))
                deserializedObject = _formatter.Deserialize(fileStream);

            return deserializedObject;
        }
        #endregion
    }
}
