using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections;
using System.Windows.Media;

namespace PolygonEditor
{
    public abstract class GeometryBase
    {
        public string geometryType;                         // Тип фигуры
        public ArrayList controlPoints;                     // Список контрольных точек фигуры с помошью которых может изменяться ее геометрия или ее расположение на панели
        protected FrameworkElement parentPane;              // Панель на которой будет производится отрисовка геометричекой фигуры

        public GeometryBase(FrameworkElement pane)      
        {
            parentPane = pane;                              //  "Привязываем" фигуру к панели на которую будет выводится(рисоваться) фигура
        }

#region Абстрактные методы требующие конретной реализации для каждого типа фигуры

        // Получение координат и параметров фигуры из массива текстовых полей для ввода данных о фигуре
        public abstract void Parse(string[] textboxName);

        // Построение визуального отображения геометрической фигуры
        public abstract Geometry CreateGeometry();          

#endregion

#region Набор вспомогательных методов для парсинга данных из поля ввода координат или параметров

        // Парсер принятой из поля ввода строки в число типа double
        protected double doubleParser(string str)
        {
            try
            {
                return Double.Parse(str);
            }
            catch (Exception err)
            {
                throw new System.ApplicationException("Error: "+ err.ToString());
            }
        }

        // Парсер принятой из поля ввода строки в сруктуру Point
        protected Point pointParser(string str)        
        {
            try
            {
                return Point.Parse(str);
            }
            catch (Exception err)
            {
                MessageBox.Show("Error! Please enter two numeric values separated by a comma or a space, for example: 10,30");
                throw new System.ApplicationException("Error: " + err.ToString());
            }
        }

        // Парсер принятой из поля ввода строки в сруктуру Size
        protected Size sizeParser(string str)        
        {
            Size ret_val = new Size();

            string[] sizeString = str.Split(new char[] { ' ', ',', ';' });

            if (sizeString.Length == 0 || sizeString.Length != 2)
            {
                MessageBox.Show("Error! A size should contain two double that seperated by a space or ',' or ';'");
                throw new System.ApplicationException("Error: a size should contain two double that seperated by a space or ',' or ';'");
            }

            try
            {
                ret_val.Width = Convert.ToDouble(sizeString[0], System.Globalization.CultureInfo.InvariantCulture);
                ret_val.Height = Convert.ToDouble(sizeString[1], System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception err)
            {
                MessageBox.Show("Error! Please enter only two numeric values into the field");
                throw new System.ApplicationException("Error: " + err.ToString());
            }
            
            return ret_val;
        }
#endregion
    }
}
