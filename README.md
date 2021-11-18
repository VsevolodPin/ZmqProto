# ZmqProto
Библиотека, содержащая методы для создания Protobuf сообщений с изображениями. Часть проекта передачи и обработки изображений с IP-камеры.

* Целевая платформа: .NETFramework 4.7.2 *(приложения на этом фреймворке запускались на компьютерах в 528.)*
* Any CPU.
## Использование библиотеки
Вариант 1: Скачать из релизов репозитория библиотеку и её компоненты, поместить всё в ваш проект, добавить зависимость вашего проекта от ***Proto.dll***.

Вариант 2: Склонировать репозиторий библиотеки, добавить зависимость вашего проекта от проекта ***Proto***.

</br>

_При сборке библиотеки из исходного кода необходимо выбрать Release: будут созданы лишь необходимые бинарные файлы._

## Структура Protobuf сообщения (т.е. кадра изображения)
(1): uint Frame – Номер кадра изображения с IP-камеры. **(обязательно передавать всегда!)**

(2): string ImgReal – Бинарная строка исходного изображения.

(3): string ImgProcessed – Бинарная строка обработанного изображения.

</br>

**В одном кадре может передаваться либо бинарная строка исходного изображения, либо обработанного!**

## Примеры использования методов и классов библиотеки
### Через вспомогательные методы и флаги
```
using Proto;


...
byte[] serialized = MsgProto.Serialize(new ImgFrame(num, ImgType.Real, testImgBytes));
...
// TODO: передача серилизованного сообщения по ZeroMQ.
...
ImgFrame imgFrmReceived = MsgProto.Deserialize(serialized);
byte[] imgForProcessing = imgFrmReceived.GetData(ImgType.Real); // получен массив байт изображения, который необходимо обработать с помощью openCV.
...
// TODO: обработка кадра с помощью openCV.
...
byte[] serialized2 = MsgProto.Serialize(new ImgFrame(imgFrmReceived.Number, ImgType.Processed, imgProcessed));
...
// TODO: передача серилизованного сообщения по ZeroMQ.
...
ImgFrame imgFrmReceived2 = MsgProto.Deserialize(serialized2);
if (imgFrmReceived2.GetAvailableType() == ImgType.Real)
  {
     var res = imgFrmReceived2.GetData(imgFrmReceived2.GetAvailableType()); //  такой способ позволяет получать массив байт кадра любого доступного изображения: исходного или обработанного (в данном случае будет получен кадр исходного изображения).
     
     // TODO: действия, если получен кадр исходного изображения
  }
...
if (imgFrmReceived2.GetAvailableType() == ImgType.Processed)
  {
    // TODO: действия, если получен кадр обработанного изображения
  }
...
```
### Явный способ
```
using Proto;


...
byte[] serialized = MsgProto.Serialize(new ImgFrame { Number = num, ImgReal = MsgProto.ByteToStr(testImgBytes) });
...
// TODO: передача серилизованного сообщения по ZeroMQ.
...
ImgFrame imgFrmReceived = MsgProto.Deserialize(serialized);
byte[] imgForProcessing = MsgProto.StrToByte(imgFrmReceived.ImgReal); // получен массив байт изображения, который необходимо обработать с помощью openCV.
...
// TODO: обработка кадра с помощью openCV.
...
byte[] serialized2 = MsgProto.Serialize(new ImgFrame { Number = imgFrmReceived.Number, ImgProcessed = MsgProto.ByteToStr(imgProcessed) });
...
// TODO: передача серилизованного сообщения по ZeroMQ.
...
ImgFrame imgFrmReceived2 = MsgProto.Deserialize(serialized2);
if (imgFrmReceived2.ImgReal != null)
  {
     var res = MsgProto.StrToByte(imgFrmReceived2.ImgReal);
     
     // TODO: действия, если получен кадр исходного изображения
  }
...
if (imgFrmReceived2.ImgProcessed != null)
  {
    var res2 = MsgProto.StrToByte(imgFrmReceived2.ImgProcessed);
  
    // TODO: действия, если получен кадр обработанного изображения
  }
...
```
