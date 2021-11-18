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
    
    [ProtoContract]
    public class ImgMessage
    {
        // СООБЩЕНИЯ //
        
        /// <summary>
        /// Номер кадра изображения.
        /// </summary>
        [ProtoMember(1)] public uint Frame { get; set; }
        /// <summary>
        /// Строка данных, полученная из исходного изображения (содержит null, если таких данных нет).
        /// </summary>
        [ProtoMember(2)] public string ImgReal { get; set; }
        /// <summary>
        /// Строка данных, полученная из обработанного изображения (содержит null, если таких данных нет).
        /// </summary>
        [ProtoMember(3)] public string ImgProcessed { get; set; }

        
        
        // ЛОГИКА И МЕТОДЫ //
        
        public ImgMessage() { } // конструктор для приёма по ZeroMQ (не используется явно)

        public ImgMessage(uint frame, ImgType type, byte[] data)
        {
            Frame = frame;
            if (type == ImgType.Real) ImgReal = MsgProto.ByteToStr(data);
            else ImgProcessed = MsgProto.ByteToStr(data);
        }
        
        public ImgMessage(uint frame, ImgType type, string dataStr)
        {
            Frame = frame;
            if (type == ImgType.Real) ImgReal = dataStr;
            else ImgProcessed = dataStr;
        }

        /// <summary>
        /// Получить массив байт изображения по его типу.
        /// </summary>
        /// <param name="type">Ожидаемый тип изображения.</param>
        /// <returns>Массив байт указанного типа изображения, или null, если изображения с таким типом нет.</returns>
        public byte[] GetImg(ImgType type)
        {
            return MsgProto.StrToByte(type == ImgType.Real ? ImgReal : ImgProcessed);
        }

        /// <summary>
        /// Узнать, что содержится в данном сообщении: исходное или обработанное изображение.
        /// </summary>
        /// <returns>Тип изображения.</returns>
        public ImgType GetAvailableType()
        {
            return GetImg(ImgType.Real) != null ? ImgType.Real : ImgType.Processed;
        }
    }
}