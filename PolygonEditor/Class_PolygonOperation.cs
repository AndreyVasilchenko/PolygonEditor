using System;
using System.Windows;
using CL_MartinezRuedaClipping;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace PolygonEditor
{
    public enum BooleanOperations
    {
        [Description("None")]
        None,
        [Description("Intersection")]
        Intersection,
        [Description("XOR")]
        XOR,
        [Description("Intersect+XOR")]
        Intersect_and_XOR,
        [Description("Difference")]
        Difference,
        [Description("Union")]
        Union
    }

    public enum OPERATION_OBJECT_TYPE
    {
        Subject = 0,
        Clipper = 1,
        Result = 2
    }


    public class PolygonOperation
    {
        public string OperationName;
        public GeoMultiPolygon_RFC7946 SubjectGeometry;
        public GeoMultiPolygon_RFC7946 ClipperGeometry;
        public BooleanOperations OperationType;
        public GeoMultiPolygon_RFC7946 ResultGeometry;
    }

    public class OperationShapeProperty
    {
        public int StrokeWeight {get;set;}
        public string StrokeColor { get; set; }
        public string FillColor {get;set;}
    }
}
