using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.IO;

namespace ImageConversionsSample;

public static class PdfConverter
{
    public static async Task ToPdf(string imagePath, string pdfPath)
    {
        var newPdf = new PdfDocument();

        var newPage = newPdf.AddPage();
        var image = XImage.FromFile(imagePath);

        newPage.Orientation = PageOrientation.Landscape;
        newPage.Width = XUnit.FromPoint(image.PointWidth);
        newPage.Height = XUnit.FromPoint(image.PointHeight);

        var gfx = XGraphics.FromPdfPage(newPage);

        gfx.DrawImage(image, 0, 0, image.PixelWidth, image.PixelHeight);

        await newPdf.SaveAsync(pdfPath);
    }

    public static void ExtractImageFromPdf(string pdfPath)
    {
        // Based on https://www.pdfsharp.net/wiki/ExportImages-sample.ashx
        var document = PdfReader.Open(pdfPath);

        var firstPage = document.Pages[0];
        var resources = firstPage.Elements.GetDictionary("/Resources")!;
        var xObjects = resources.Elements.GetDictionary("/XObject")!;
        var elements = xObjects.Elements.Values.ToList();

        for (var i = 0; i < xObjects.Elements.Values.Count; i++)
        {
            var item = elements[i];
            if (item is PdfReference { Value: PdfDictionary xObject } && xObject.Elements.GetString("/Subtype") == "/Image")
            {
                // Assumes image is always jpeg, refer to link above for (partial) png handling
                var stream = xObject.Stream.Value;
                var fs = new FileStream($"found_image_{i}.jpeg", FileMode.Create, FileAccess.Write);
                var bw = new BinaryWriter(fs);
                bw.Write(stream);
                bw.Close();
            }
        }
    }
}
