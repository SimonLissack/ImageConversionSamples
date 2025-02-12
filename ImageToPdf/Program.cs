using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

var document = new PdfDocument();

var page = document.AddPage();
var image = XImage.FromFile("test_image.jpg");

page.Orientation = PageOrientation.Landscape;
page.Width = XUnit.FromPoint(image.PointWidth);
page.Height = XUnit.FromPoint(image.PointHeight);

var gfx = XGraphics.FromPdfPage(page);

gfx.DrawImage(image, 0, 0, image.PixelWidth, image.PixelHeight);

await document.SaveAsync("out.pdf");
