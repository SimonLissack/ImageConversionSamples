using ImageConversionsSample;

const string pdfPath = "out.pdf";
const string epsPath = "out.eps";
const string imagePath = "test_image.jpg";

// await PdfConverter.ToPdf(imagePath, pdfPath);
// PdfConverter.ExtractImageFromPdf(pdfPath);

EpsConverter.ToEpsFile(imagePath, epsPath);
