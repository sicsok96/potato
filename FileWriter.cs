namespace EarClip Error;

public class FileWriter : GeomWriter
{
    public FileWriter(ProgramParameters programParameters) : base(programParameters) { }

    public override void WriteGeometries(List<Geometry.Geometry> geometries)
    {
        // Write to file
        List<string> strGeoms = GeometriesToString(geometries);

        File.WriteAllLines(Parameters.Output!, strGeoms);
    }
}