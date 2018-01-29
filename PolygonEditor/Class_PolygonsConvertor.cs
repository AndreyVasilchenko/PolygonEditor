using System;
using System.Windows;
using System.Collections.Generic;

using CL_MartinezRuedaClipping;

namespace PolygonEditor
{
    public class PoligonsConvertor
    {
        public static GeoPolygon GetGeoPolygon(List<namedParameter> parametrs)
        {
            Point[][] polygon = new Point[1][];
            polygon[0] = new Point[parametrs.Count+1];
            int i = 0;
            for (; i < parametrs.Count; i++)
            {
                polygon[0][i] = (Point)parametrs[i].ParamValue;
            }
            polygon[0][i] = (Point)parametrs[0].ParamValue;

            return new GeoPolygon(polygon);
        }

        public static GeoMultiPolygon GetGeoMultiPolygon(List<namedParameter> parametrs)
        {
            Point[][][] polygon = new Point[1][][];
            polygon[0] = new Point[1][];
            polygon[0][0] = new Point[parametrs.Count + 1];
            int i = 0;
            for (; i < parametrs.Count; i++)
            {
                polygon[0][0][i] = (Point)parametrs[i].ParamValue;
            }
            polygon[0][0][i] = (Point)parametrs[0].ParamValue;

            return new GeoMultiPolygon(polygon);
        }

        public static void AddToMultiPolygon( GeoMultiPolygon mupltiPolygon, List<namedParameter> parametrs)
        {
            if (parametrs != null && parametrs.Count > 0)
            {
                if (mupltiPolygon.coordinates is Point[][][])
                {
                    int length0 = ((Point[][][])mupltiPolygon.coordinates).GetLength(0);
                    Point[][][] new_polygon = new Point[length0 + 1][][];
                    for (int l = 0; l < length0; l++)
                        new_polygon[l] = ((Point[][][])mupltiPolygon.coordinates)[l];

                    new_polygon[length0] = new Point[1][];
                    new_polygon[length0][0] = new Point[parametrs.Count + 1];
                    int i = 0;
                    for (; i < parametrs.Count; i++)
                    {
                        new_polygon[length0][0][i] = (Point)parametrs[i].ParamValue;
                    }
                    new_polygon[length0][0][i] = (Point)parametrs[0].ParamValue;

                    mupltiPolygon.coordinates = new_polygon;
                }
            }
        }

        public static GeoMultiPolygon_RFC7946 GetGeoMultiPolygon_RFC7946(Point[][][] multiPoligon)
        {
            if( multiPoligon == null )
                return (null);
            
            if( multiPoligon.Length == 0 )
                return (null);

            int polygonCount = multiPoligon.GetLength(0);

            if( polygonCount == 0)
                return (null);

            double[][][][] m_poligon = new double[polygonCount][][][];
            for (int i = 0; i < polygonCount; i++)
            {
                int i_len = multiPoligon[i].GetLength(0);
                m_poligon[i] = new double[i_len][][];
                for (int j = 0; j < i_len; j++)
                {
                    int j_len = multiPoligon[i][j].GetLength(0);
                    m_poligon[i][j] = new double[j_len][];
                    for (int k = 0; k < j_len; k++)
                    {
                        m_poligon[i][j][k] = new double[2];
                        m_poligon[i][j][k][0] = multiPoligon[i][j][k].X;
                        m_poligon[i][j][k][1] = multiPoligon[i][j][k].Y;
                    }
                }
            }
            return new GeoMultiPolygon_RFC7946(m_poligon);
        }

        internal List<namedParameter> GetNamedParams(GeoPolygon geo_poligon)
        {
            List<namedParameter> param_list = null;

            if (geo_poligon != null)
            {
                int length = (((Point[][])geo_poligon.coordinates)[0].Length > 3) ? ((Point[][])geo_poligon.coordinates)[0].Length - 1 : ((Point[][])geo_poligon.coordinates)[0].Length;
                if (length > 0)
                {
                    param_list = new List<namedParameter>();
                    for (int i = 0; i < length; i++)
                    {
                        namedParameter p = new namedParameter();
                        p.ParamName = "Point" + (i + 1).ToString();
                        p.ParamValue = Math.Round(((Point[][])geo_poligon.coordinates)[0][i].X).ToString()+","+ Math.Round(((Point[][])geo_poligon.coordinates)[0][i].Y).ToString();
                        param_list.Add(p);
                    }
                }
            }

            return param_list;
        }

        internal static List<namedParameter> GetNamedParams(Point[][] polygon)
        {
            List<namedParameter> param_list = null;

            if (polygon != null)
            {
                int length = (polygon[0].Length > 3) ? polygon[0].Length - 1 : polygon[0].Length;
                if (length > 0)
                {
                    param_list = new List<namedParameter>();
                    for (int i = 0; i < length; i++)
                    {
                        namedParameter p = new namedParameter();
                        p.ParamName = "Point" + (i + 1).ToString();
                        p.ParamValue = Math.Round(polygon[0][i].X).ToString() + "," + Math.Round(polygon[0][i].Y).ToString();
                        param_list.Add(p);
                    }
                }
            }

            return param_list;
        }

        internal static List<namedParameter> GetNamedParams(double[][][] polygon)
        {
            List<namedParameter> param_list = null;

            if (polygon != null)
            {
                int length = (polygon[0].Length > 3) ? polygon[0].Length - 1 : polygon[0].Length;
                if (length > 0)
                {
                    param_list = new List<namedParameter>();
                    for (int i = 0; i < length; i++)
                    {
                        namedParameter p = new namedParameter();
                        p.ParamName = "Point" + (i + 1).ToString();
                        p.ParamValue = Math.Round(polygon[0][i][0]).ToString() + "," + Math.Round(polygon[0][i][1]).ToString();
                        param_list.Add(p);
                    }
                }
            }

            return param_list;
        }

        public List<ShapeParams> GetShapeParams(GeoMultiPolygon geo_multiPoligon, OperationShapeProperty property)
        {
            List<ShapeParams> param_list = null;

            if (geo_multiPoligon != null)
            {
                int polygon_count = ((Point[][][])geo_multiPoligon.coordinates).GetLength(0);

                if (polygon_count > 0)
                {
                    param_list = new List<ShapeParams>();
                    for (int i = 0; i < polygon_count; i++)
                    {
                        ShapeParams p = new ShapeParams();
                        p.ShapeName = "OperationPolygon" + (i + 1).ToString();
                        p.ShapeType = "Polygon";
                        p.StrokeWeight = property.StrokeWeight;
                        p.StrokeColor = property.StrokeColor;
                        p.FillColor = property.FillColor;
                        p.GeometricParams = GetNamedParams(((Point[][][])geo_multiPoligon.coordinates)[i]);
                        param_list.Add(p);
                    }
                }
            }
            return param_list;
        }

        public static List<ShapeParams> GetShapeParams(Point[][][] geo_multiPoligon, OperationShapeProperty property)
        {
            List<ShapeParams> param_list = null;

            if (geo_multiPoligon != null)
            {
                int polygon_count = geo_multiPoligon.GetLength(0);

                if (polygon_count > 0)
                {
                    param_list = new List<ShapeParams>();
                    for (int i = 0; i < polygon_count; i++)
                    {
                        ShapeParams p = new ShapeParams();
                        p.ShapeName = "OperationPolygon" + (i + 1).ToString();
                        p.ShapeType = "Polygon";
                        p.StrokeWeight = property.StrokeWeight;
                        p.StrokeColor = property.StrokeColor;
                        p.FillColor = property.FillColor;
                        p.GeometricParams = GetNamedParams(geo_multiPoligon[i]);
                        param_list.Add(p);
                    }
                }
            }
            return param_list;
        }

        public static List<ShapeParams> GetShapeParams(double[][][][] geo_multiPoligon, OperationShapeProperty property)
        {
            List<ShapeParams> param_list = null;

            if (geo_multiPoligon != null)
            {
                int polygon_count = geo_multiPoligon.GetLength(0);

                if (polygon_count > 0)
                {
                    param_list = new List<ShapeParams>();
                    for (int i = 0; i < polygon_count; i++)
                    {
                        ShapeParams p = new ShapeParams();
                        p.ShapeName = "OperationPolygon" + (i + 1).ToString();
                        p.ShapeType = "Polygon";
                        p.StrokeWeight = property.StrokeWeight;
                        p.StrokeColor = property.StrokeColor;
                        p.FillColor = property.FillColor;
                        p.GeometricParams = GetNamedParams(geo_multiPoligon[i]);
                        param_list.Add(p);
                    }
                }
            }
            return param_list;
        }

    }
}
