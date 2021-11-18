# ZmqProto
Библиотека, содержащая методы для создания Protobuf сообщений с изображениями. Часть проекта передачи и обработки изображений с IP-камеры.

*Целевая платформа: .NETFramework 4.7.2 (приложения на этом фреймворке запускались на компьютерах в 528.)*

Any CPU.
## Использование библиотеки
Вариант 1: Скачать из релизов репозитория библиотеку и её компоненты, поместить всё в ваш проект, добавить зависимость вашего проекта от ***Proto.dll***.

Вариант 2: Склонировать репозиторий библиотеки, добавить зависимость вашего проекта от проекта ***Proto***.

_При сборке библиотеки из исходного кода необходимо выбрать Release: будут созданы лишь необходимые бинарные файлы._

## Структура Protobuf сообщения
(1): uint Frame – Номер кадра изображения с IP-камеры. **(обязательно передавать всегда!)**

(2): string ImgReal – Бинарная строка кадра исходного изображения.

(3): string ImgProcessed – Бинарная строка кадра обработанного изображения.

## Примеры использования методов и классов библиотеки
### Через вспомогательные методы и флаги
```
using Proto;


...
byte[] serialized = MsgProto.Serialize(new ImgMessage(num, ImgType.Real, testImgBytes));
...
// TODO: передача серилизованного сообщения по ZeroMQ.
...
ImgMessage imgMsgReceived = MsgProto.Deserialize(serialized);
byte[] imgForProcessing = imgMsgReceived.GetImg(ImgType.Real); // получен массив байт изображения, который необходимо обработать с помощью openCV.
...
// TODO: обработка кадра с помощью openCV.
...
byte[] serialized2 = MsgProto.Serialize(new ImgMessage(imgMsgReceived.Frame, ImgType.Processed, imgProcessed));
...
// TODO: передача серилизованного сообщения по ZeroMQ.
...
ImgMessage imgMsgReceived2 = MsgProto.Deserialize(serialized2);
if (imgMsgReceived2.GetAvailableType() == ImgType.Real)
  {
     var res = imgMsgReceived2.GetImg(imgMsgReceived2.GetAvailableType()); //  такой способ позволяет получать массив байт кадра любого доступного изображения: исходного или обработанного (в данном случае будет получен кадр исходного изображения).
     
     // TODO: действия, если получен кадр исходного изображения
  }
...
if (imgMsgReceived2.GetAvailableType() == ImgType.Processed)
  {
    // TODO: действия, если получен кадр обработанного изображения
  }
...
```
### Явный способ
```
using Proto;


...
byte[] serialized = MsgProto.Serialize(new ImgMessage { Frame = num, ImgReal = MsgProto.ByteToStr(testImgBytes) });
...
// TODO: передача серилизованного сообщения по ZeroMQ.
...
ImgMessage imgMsgReceived = MsgProto.Deserialize(serialized);
byte[] imgForProcessing = MsgProto.StrToByte(imgMsgReceived.ImgReal); // получен массив байт изображения, который необходимо обработать с помощью openCV.
...
// TODO: обработка кадра с помощью openCV.
...
byte[] serialized2 = MsgProto.Serialize(new ImgMessage { Frame = imgMsgReceived.Frame, ImgProcessed = MsgProto.ByteToStr(imgProcessed) });
...
// TODO: передача серилизованного сообщения по ZeroMQ.
...
ImgMessage imgMsgReceived2 = MsgProto.Deserialize(serialized2);
if (imgMsgReceived2.ImgReal != null)
  {
     var res = MsgProto.StrToByte(imgMsgReceived2.ImgReal);
     
     // TODO: действия, если получен кадр исходного изображения
  }
...
if (imgMsgReceived2.ImgProcessed != null)
  {
    var res2 = MsgProto.StrToByte(imgMsgReceived2.ImgProcessed);
  
    // TODO: действия, если получен кадр обработанного изображения
  }
...
```
