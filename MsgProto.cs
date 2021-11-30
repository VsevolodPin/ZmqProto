using System;
using System.IO;
using System.Text;
using ProtoBuf;

namespace Proto
{
    /// <summary>
    /// Преобразователь кадров изображения в серилизованные сообщения (и обратно).
    /// </summary>
    public static class MsgProto
    {
        /// <summary>
        /// Серилизовать кадр изображения (объект класса ImgFrame) в массив байт.
        /// </summary>
        /// <param name="imgMsg">Кадр изображения.</param>
        /// <returns>Серилизованное сообщение (массив байт).</returns>
        public static byte[] Serialize(ImgFrame imgMsg)
        {
            if (imgMsg == null) return null;

            try
            {
                using (var stream = new MemoryStream())
                {
                    Serializer.Serialize(stream, imgMsg);
                    return stream.ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Десерилизовать кадр изображения из массива байт в объект класса ImgFrame.
        /// </summary>
        /// <param name="serializedMsg">Серилизованное сообщение.</param>
        /// <returns>Кадр изображения (объект ImgFrame).</returns>
        public static ImgFrame Deserialize(byte[] serializedMsg)
        {
            if (serializedMsg == null) return null;

            try
            {
                using (var stream = new MemoryStream(serializedMsg))
                {
                    return (ImgFrame) Serializer.Deserialize(typeof(ImgFrame), stream);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        /// <summary>
        /// Перевод изображения из массива байт в бинарную строку для хранения в ImgFrame и последующей передачи.
        /// </summary>
        /// <param name="bytes">Массив байт изображения.</param>
        /// <returns>Бинарная строка.</returns>
        public static string ByteToStr(byte[] bytes)
        {
            if (bytes == null) return null;
            var chars = new char[bytes.Length / sizeof(char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
        
        /// <summary>
        /// Перевод изображения из бинарной строки в массив байт для последующей обработки с помощью openCV.
        /// </summary>
        /// <param name="str">Бинарная строка.</param>
        /// <returns>Массив байт изображения.</returns>
        public static byte[] StrToByte(string str)
        {
            if (str == null) return null;
            var bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}