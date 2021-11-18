using ProtoBuf;

namespace Proto
{
    /// <summary>
    /// Тип изображения.
    /// </summary>
    public enum ImgType
    {
        /// <summary>
        /// Исходное изображение.
        /// </summary>
        Real,
        /// <summary>
        /// Обработанное изображение.
        /// </summary>
        Processed
    }
    
    /// <summary>
    /// Кадр с изображением (будущее сообщение).
    /// </summary>
    [ProtoContract]
    public class ImgFrame
    {
        // СОДЕРЖАНИЕ СООБЩЕНИЯ //
        
        /// <summary>
        /// Номер кадра изображения.
        /// </summary>
        [ProtoMember(1)] public uint Number { get; set; }
        /// <summary>
        /// Бинарная строка кадра исходного изображения (= null, если таких данных в этом кадре нет).
        /// </summary>
        [ProtoMember(2)] public string ImgReal { get; set; }
        /// <summary>
        /// Бинарная строка кадра обработанного изображения (= null, если таких данных в этом кадре нет).
        /// </summary>
        [ProtoMember(3)] public string ImgProcessed { get; set; }

        
        
        // ЛОГИКА И МЕТОДЫ //
        
        public ImgFrame() { } // конструктор для приёма по ZeroMQ (не используется явно)

        public ImgFrame(uint number, ImgType type, byte[] data)
        {
            Number = number;
            if (type == ImgType.Real) ImgReal = MsgProto.ByteToStr(data);
            else ImgProcessed = MsgProto.ByteToStr(data);
        }
        
        public ImgFrame(uint number, ImgType type, string dataStr)
        {
            Number = number;
            if (type == ImgType.Real) ImgReal = dataStr;
            else ImgProcessed = dataStr;
        }

        /// <summary>
        /// Получить массив байт изображения по типу изображения.
        /// </summary>
        /// <param name="type">Тип изображения.</param>
        /// <returns>Массив байт указанного типа изображения, или null, если изображения с таким типом нет в этом кадре.</returns>
        public byte[] GetData(ImgType type)
        {
            return MsgProto.StrToByte(type == ImgType.Real ? ImgReal : ImgProcessed);
        }

        /// <summary>
        /// Узнать, что содержится в данном кадре: исходное или обработанное изображение.
        /// </summary>
        /// <returns>Тип изображения.</returns>
        public ImgType GetAvailableType()
        {
            return GetData(ImgType.Real) != null ? ImgType.Real : ImgType.Processed;
        }
    }
}