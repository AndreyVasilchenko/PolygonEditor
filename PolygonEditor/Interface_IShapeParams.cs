using System.Collections.Generic;


namespace PolygonEditor
{
    public interface IShapeParams
    {
        string ShapeName { get; set; }
        string ShapeType { get; set; }
        int StrokeWeight { get; set; }
        string StrokeColor { get; set; }
        string FillColor { get; set; }
        List<namedParameter> GeometricParams { get; set; }
    }

    public class namedParameter
    {
        public string ParamName { get; set; }
        public object ParamValue { get; set; }
    }

    public class ShapeParams : IShapeParams
    {
        public string ShapeName { get; set; }
        public string ShapeType { get; set; }
        public int StrokeWeight { get; set; }
        public string StrokeColor { get; set; }
        public string FillColor { get; set; }
        public List<namedParameter> GeometricParams { get; set; }
    }
}
