using NetTopologySuite.IO;

namespace EarClip;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine($"Arguments: {string.Join('|', args)}");
        ProgramParameters parsedArguments = ProcessArguments(args);

        Geometry.Geometry geomA = WktParser.Parse (parsedArguments.Inputs[0]);
        Geometry.Geometry geomB = WktParser.Parse (parsedArguments.Inputs[1]);

        Console.WriteLine (geomA.ToString());
        Console.WriteLine (geomB.ToString());

        WKTReader rdr = new WKTReader();
        NetTopologySuite.Geometries.Geometry poly1 = rdr.Read(geomA.ToString());
        NetTopologySuite.Geometries.Geometry poly2 = rdr.Read(geomB.ToString());

        NetTopologySuite.Geometries.Geometry intersection = poly1.Intersection(poly2);


        //Geometry.Geometry? intersection = Intersector.Intersection (geomA, geomB);

        if (intersection != null) {
            GeomWriter writer = CreateWriter(parsedArguments);
            Console.WriteLine (intersection);
        }   
    }

    private static GeomWriter CreateWriter(ProgramParameters parameters)
    {
        if (parameters.Mode == RunType.CONSOLE)
        {
            return new ConsoleWriter(parameters);
        }
        else if (parameters.Mode == RunType.FILE)
        {
            return new FileWriter(parameters);
        }
        else
        {
            throw new Exception("Unsupported mode.");
        }
    }

    /// <summary>
    /// Processes user arguments and returns a strongly typed parameter object from them.
    /// </summary>
    /// <param name="args">The user arguments</param>
    /// <returns>The parameter object</returns>
    private static ProgramParameters ProcessArguments(string[] args)
    {
        string[] inputPaths = [];
        string inputPath = "", outputPath = "", mode = "";
        foreach (string arg in args)
        {
            string[] kvp = arg.Split('=');
            if (kvp[0] == "mode")
            {
                mode = kvp[1];
            }
            else if (kvp[0] == "input")
            {
                inputPaths = kvp[1].Split(',');
            }
            else if (kvp[0] == "output")
            {
                outputPath = kvp[1];
            }
        }

        if (inputPath.Length != 2)
        {
            throw new ArgumentException("Two input paths must be defined separated with a comma.");
        }
        RunType modeEnum = default;
        if (mode == "file")
        {
            modeEnum = RunType.FILE;
        }
        if (modeEnum == RunType.FILE && outputPath == "")
        {
            throw new ArgumentException("Output path must be defined if mode is set to file.");
        }

        return new ProgramParameters
        {
            Inputs = inputPaths,
            Mode = modeEnum,
            Output = outputPath
        };
    }
}
