using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageConversionsSample;

public static class EpsConverter
{
    // ChatGPT generated
    // Takes a ~3.5MB file and turns it into a 85MB EPS file
    // TODO: Can the bitmap conversion be avoided and use the source jpeg? GPT implies maybe but writes an invalid file format
    // TODO: It also appears to be scaling the image from 4080x3072 -> 5666x4266, find out where this scaling is occurring (~1.39, similar to PDF scale?)
    public static void ToEpsFile(string imagePath, string epsPath)
    {
        using var image = Image.Load<Rgba32>(imagePath);
        var width = image.Width;
        var height = image.Height;

        using var writer = new StreamWriter(epsPath);
        // EPS Header
        writer.WriteLine("%!PS-Adobe-3.0 EPSF-3.0");
        writer.WriteLine($"%%BoundingBox: 0 0 {width} {height}");
        writer.WriteLine("%%EndComments");

        // Scale and translate the coordinate system
        writer.WriteLine("gsave");
        writer.WriteLine($"{width} {height} scale");

        // Define image data
        writer.WriteLine($"/picstr {width * 3} string def");
        writer.WriteLine($"{width} {height} 8 [ {width} 0 0 {height} 0 0 ]");
        writer.WriteLine("{ currentfile picstr readhexstring pop } bind");
        writer.WriteLine("false 3 colorimage");

        // Write image pixel data as hex
        WriteImageDataAsHex(writer, image);

        writer.WriteLine("grestore");
        writer.WriteLine("showpage");
        writer.WriteLine("%%EOF");
    }

    static void WriteImageDataAsHex(StreamWriter writer, Image<Rgba32> image)
    {
        for (var y = 0; y < image.Height; y++)
        {
            for (var x = 0; x < image.Width; x++)
            {
                var pixel = image[x, image.Height - y - 1]; // Invert y-axis
                writer.Write($"{pixel.R:X2}{pixel.G:X2}{pixel.B:X2} ");
            }
            writer.WriteLine();
        }
    }
}
