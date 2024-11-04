using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using EarClip.Geometry;

namespace EarClip;

public static class WktParser
{
    public static Geometry.Geometry Parse(string filePath)      
    {
        
        string wkt = File.ReadAllText(filePath);
        if (wkt.Contains("POINT"))
        {
            return Geometry.Point.ReadFromWkt(wkt); 
        }
        else if (wkt.Contains("LINESTRING"))
        {
            return Line.ReadFromWkt(wkt);
        }
        else if (wkt.Contains("POLYGON"))
        {
            return Polygon.ReadFromWkt(wkt);
        }
        else
        {
            throw new Exception("Unsuppoted geometry type in WKT string or invalid WKT string");
        }
    }
}
