using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace PolygonEditor
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected ViewModelBase()
        {}


        [field: NonSerialized]                                                                  //  Событие не включается в код при сериализации
        public event PropertyChangedEventHandler PropertyChanged;                               //  

        //--- Метод вызываемый при изменении свойства заданного в параметре его именем
        protected void OnPropertyChanged(string propertyName)                       
        {
            VerifyPropertyName(propertyName);                                                   //  Проверяем наличие свойства заданного параметром

            if (PropertyChanged != null)                                                        //  Если на событие изменения свойства есть подписчики,
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));              //      выполненяем методы, подписанные на это событие
            }
        }


        protected virtual bool ThrowOnInvalidPropertyNames { get; private set; }                //  Свойство определяющее надо ли вызывать исключение при неверно заданном имени свойства

        //--- Проверка соответствия заданного имени свойства наличию такого свойства
        [Conditional("DEBUG")]                                                                  //  При трассировке программы в отладчике
        [DebuggerStepThrough]                                                                   //  данный метод будет пропускаться в нем без трассировки(обходить его)
        public void VerifyPropertyName(string propertyName)
        {
            if (null == TypeDescriptor.GetProperties(this)[propertyName])                       //      Если свойства соответсвующего заданному имени нет, то
            {
                if (ThrowOnInvalidPropertyNames)                                                //          Если обявлен вызов исключения
                    throw new Exception("Invalid property name: " + propertyName);              //              взываем его
                else                                                                            //          Иначе
                    Debug.Fail("Invalid property name: " + propertyName);                       //              выдаем сообщение в отладчик
            }
        }

    }
}
