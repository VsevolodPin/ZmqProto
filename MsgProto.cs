using System;
using System.IO;
using System.Text;
using ProtoBuf;

namespace Proto
{
    /// <summary>
    /// Обработчик сообщений с изображениями (ImgMessage).
    /// </summary>
    public static class MsgProto
    {
        /// <summary>
        /// Серилизовать сообщение с изображением в массив байт.
        /// </summary>
        /// <param name="imgMsg">Сообщение с изображением.</param>
        /// <returns>Серилизованное сообщение.</returns>
        public static byte[] Serialize(ImgMessage imgMsg)
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
        /// Десерилизовать сообщение с изображением из массива байт в полноценный класс.
        /// </summary>
        /// <param name="serializedMsg">Серилизованное сообщение.</param>
        /// <returns>Сообщение с изображением (ImgMessage).</returns>
        public static ImgMessage Deserialize(byte[] serializedMsg)
        {
            if (serializedMsg == null) return null;

            try
            {
                using (var stream = new MemoryStream(serializedMsg))
                {
                    return (ImgMessage) Serializer.Deserialize(typeof(ImgMessage), stream);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        /// <summary>
        /// Перевод byte[] изображения в string для передачи в ImgMessage.
        /// </summary>
        /// <param name="bytes">Изображение.</param>
        /// <returns>Строка данных.</returns>
        public static string ByteToStr(byte[] bytes)
        {
            return bytes != null ? Encoding.ASCII.GetString(bytes) : null;
        }
        
        /// <summary>
        /// Обратный перевод string (строки данных) в byte[] изображения для последующей обработки с помощью openCV.
        /// </summary>
        /// <param name="str">Строка данных.</param>
        /// <returns>Изображение.</returns>
        public static byte[] StrToByte(string str)
        {
            return str != null ? Encoding.ASCII.GetBytes(str) : null;
        }
    }
}