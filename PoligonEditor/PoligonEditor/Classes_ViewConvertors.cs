using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace PolygonEditor
{
    public class BooleanConverter_AND : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int count = values.Count();
            if (count < 1)
                return false;

            bool booling = true;
            for (int i = 0; i < count; i++)
                booling = booling && System.Convert.ToBoolean(values[i]);

            return booling;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            int count = targetTypes.Count();

            object[] boolings = new object[count];
            for (int i = count - 1; i > -1; i--)
                boolings[i] = value;

            return boolings;
        }
    }

    public class BooleanConverter_OR : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int count = values.Count();
            if (count < 1)
                return false;

            bool booling = false;
            for (int i = 0; i < values.Count(); i++)
                booling = booling || System.Convert.ToBoolean(values[i]);

            return booling;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            int count = targetTypes.Count();

            object[] boolings = new object[count];
            for (int i = count - 1; i > -1; i--)
                boolings[i] = value;

            return boolings;
        }
    }

    public class BooleanConverter_NOT : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !System.Convert.ToBoolean(value);
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !System.Convert.ToBoolean(value);
        }
    }

    public class NumericToBooleanConverter : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(parameter != null)
                return System.Convert.ToInt32(value) >= System.Convert.ToInt32(parameter);
            else
                return System.Convert.ToInt32(value) > 0;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null)
                return System.Convert.ToBoolean(value) == true ? parameter : 0;
            else
                return System.Convert.ToBoolean(value) == true ? 1 : 0;
        }
    }

    public class ShapeTypeToBooleanConverter : IValueConverter
    {
        SHAPE_TYPE LastType;

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool ret = false;

            if (value is SHAPE_TYPE && parameter is string)
            {
                switch ((string)parameter)
                {
                    case "Point":
                        ret = EqualityComparer<SHAPE_TYPE>.Default.Equals((SHAPE_TYPE)value, SHAPE_TYPE.Point);
                        if (ret) LastType = SHAPE_TYPE.Point;
                        break;
                    case "Line":
                        ret = EqualityComparer<SHAPE_TYPE>.Default.Equals((SHAPE_TYPE)value, SHAPE_TYPE.Line);
                        if (ret) LastType = SHAPE_TYPE.Line;
                        break;
                    case "PolyLine":
                        ret = EqualityComparer<SHAPE_TYPE>.Default.Equals((SHAPE_TYPE)value, SHAPE_TYPE.PolyLine);
                        if (ret) LastType = SHAPE_TYPE.PolyLine;
                        break;
                    case "Polygon":
                        ret = EqualityComparer<SHAPE_TYPE>.Default.Equals((SHAPE_TYPE)value, SHAPE_TYPE.Polygon);
                        if (ret) LastType = SHAPE_TYPE.Polygon;
                        break;
                }
            }
            return ret;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SHAPE_TYPE ret = LastType;

            if (System.Convert.ToBoolean(value) && parameter is string)
            {
                switch ((string)parameter)
                {
                    case "Point":
                        ret = SHAPE_TYPE.Point;
                        break;
                    case "Line":
                        ret = SHAPE_TYPE.Line;
                        break;
                    case "PolyLine":
                        ret = SHAPE_TYPE.PolyLine;
                        break;
                    case "Polygon":
                        ret = SHAPE_TYPE.Polygon;
                        break;
                }
            }
            return ret;
        }
    }

    public class ModeToBooleanConverter : IValueConverter
    {
        EDITOR_MODE LastMode;

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool ret = false;

            if (value is EDITOR_MODE && parameter is string)
            {
                switch ((string)parameter)
                {
                    case "Selection":
                        ret = EqualityComparer<EDITOR_MODE>.Default.Equals((EDITOR_MODE)value, EDITOR_MODE.Selection);
                        if (ret) LastMode = EDITOR_MODE.Selection;
                        break;
                    case "DrawNew":
                        ret = EqualityComparer<EDITOR_MODE>.Default.Equals((EDITOR_MODE)value, EDITOR_MODE.DrawNew);
                        if (ret) LastMode = EDITOR_MODE.DrawNew;
                        break;
                    case "Edit":
                        ret = EqualityComparer<EDITOR_MODE>.Default.Equals((EDITOR_MODE)value, EDITOR_MODE.Edit);
                        if (ret) LastMode = EDITOR_MODE.Edit;
                        break;
                    case "PolygonOperations":
                        ret = EqualityComparer<EDITOR_MODE>.Default.Equals((EDITOR_MODE)value, EDITOR_MODE.PolygonOperations);
                        if (ret) LastMode = EDITOR_MODE.PolygonOperations;
                        break;
                }
            }
            return ret;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            EDITOR_MODE ret = LastMode;

            if (System.Convert.ToBoolean(value) && parameter is string)
            {
                switch ((string)parameter)
                {
                    case "Selection":
                        ret = EDITOR_MODE.Selection;
                        break;
                    case "DrawNew":
                        ret = EDITOR_MODE.DrawNew;
                        break;
                    case "Edit":
                        ret = EDITOR_MODE.Edit;
                        break;
                    case "PolygonOperations":
                        ret = EDITOR_MODE.PolygonOperations;
                        break;
                }
            }
            return ret;
        }
    }

    public class ModeToContentConverter : IValueConverter
    {
        EDITOR_MODE LastMode;

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string ret = "";

            if (value is EDITOR_MODE)
            {
                switch ((EDITOR_MODE)value)
                {
                    case EDITOR_MODE.Selection:
                        //ret = "UpdateCoords";
                        LastMode = EDITOR_MODE.Selection;
                        break;
                    case EDITOR_MODE.DrawNew:
                        ret = "InsertShape";
                        LastMode = EDITOR_MODE.DrawNew;
                        break;
                    case EDITOR_MODE.Edit:
                        ret = "UpdateCoords";
                        LastMode = EDITOR_MODE.Edit;
                        break;
                    case EDITOR_MODE.PolygonOperations:
                        ret = "PolygonOperations";
                        LastMode = EDITOR_MODE.PolygonOperations;
                        break;
                }
            }
            return ret;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            EDITOR_MODE ret = LastMode;

            if (value is string)
            {
                switch ((string)value)
                {
                    case "":
                        ret = EDITOR_MODE.Selection;
                        break;
                    case "InsertShape":
                        ret = EDITOR_MODE.DrawNew;
                        break;
                    case "UpdateCoords":
                        ret = EDITOR_MODE.Edit;
                        break;
                    case "PolygonOperations":
                        ret = EDITOR_MODE.PolygonOperations;
                        break;
                }
            }
            return ret;
        }
    }


    public class BooleanConverter<T> : IValueConverter
    {
        public T True { get; set; }
        public T False { get; set; }

        public BooleanConverter(T trueValue, T falseValue)
        {
            True = trueValue;
            False = falseValue;
        }

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool && ((bool)value) ? True : False;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is T && EqualityComparer<T>.Default.Equals((T)value, True);
        }
    }

    public class BooleanConverter_OR<T> : IMultiValueConverter
    {
        public T True { get; set; }
        public T False { get; set; }

        public BooleanConverter_OR(T trueValue, T falseValue)
        {
            True = trueValue;
            False = falseValue;
        }

        public virtual object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int count = values.Count();
            if (count < 1)
                return false;

            bool booling = false;
            for (int i = 0; i < values.Count(); i++)
                booling = booling || ((bool)values[i]); //System.Convert.ToBoolean(values[i]);

            return (booling ? True : False);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            int count = targetTypes.Count();

            object[] boolings = new object[count];
            for (int i = count - 1; i > -1; i--)
                boolings[i] = EqualityComparer<T>.Default.Equals((T)value, True);

            return boolings;
        }
    }

    public sealed class BooleanToVisibilityConverter : BooleanConverter<Visibility>
    {
        public BooleanToVisibilityConverter() :
            base(Visibility.Visible, Visibility.Collapsed)
        { }
    }

    public sealed class BooleanToVisibilityConverter_OR : BooleanConverter_OR<Visibility>
    {
        public BooleanToVisibilityConverter_OR() :
            base(Visibility.Visible, Visibility.Collapsed)
        { }
    }
}
