using System;
using System.Windows;
using System.Collections.Generic;

namespace CL_MartinezRuedaClipping
{
    public class GeoGeometry
    {
        public readonly string type;
        public object coordinates;

        public GeoGeometry(string _type, object _coordinates)
        {
            type = _type;
            coordinates = _coordinates;
        }
    }

    public class GeoPolygon : GeoGeometry
    {
        public GeoPolygon()
            : base("Polygon", new Point[][] { })
        { }

        public GeoPolygon(Point[][] Poligon)
            : base("Polygon", Poligon)
        { }
    }

    public class GeoMultiPolygon : GeoGeometry
    {
        public GeoMultiPolygon()
            : base("MultiPolygon", new Point[][][] { })
        { }

        public GeoMultiPolygon(Point[][][] multiPoligon)
            : base("MultiPolygon", multiPoligon)
        { }
    }

    public class GeoMultiPolygon_RFC7946
    {
        public readonly string type = "MultiPolygon";
        public double[][][][] coordinates;

        public GeoMultiPolygon_RFC7946(double[][][][] multiPoligon)
        {
            coordinates = multiPoligon;
        }
    }

    //public class GeoPoint : GeoGeometry
    //{
    //    type= "Point";
    //    Point coordinates;
    //}

    //public class GeoMultiPoint : GeoGeometry
    //{
    //    type= "MultiPoint";
    //    Point[] coordinates;
    //}

    //public class GeoLineString : GeoGeometry
    //{
    //    type= "LineString";
    //    Point[] coordinates;
    //}

    //public class GeoMultiLineString : GeoGeometry
    //{
    //    type= "MultiLineString";
    //    Point[][] coordinates;
    //}

    //public class GeoGeometryCollection : GeoGeometry
    //{
    //    type= "GeometryCollection";
    //    GeoGeometry[] geometries;
    //}

    //public class Feature
    //{
    //    public readonly string type= "Feature";
    //    public GeoGeometry geometry;
    //    public object properties;
    //}

    //public class FeatureCollection
    //{
    //    public readonly string type= "FeatureCollection";
    //    public Feature[] features;
    //}

}
