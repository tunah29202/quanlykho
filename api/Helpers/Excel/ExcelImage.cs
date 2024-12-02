using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using A = DocumentFormat.OpenXml.Drawing;
using DrwS = DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using img = SixLabors.ImageSharp;
using System.IO;

namespace Helpers.Excel
{
    public class ExcelImage
    {
        public const int MAX_WIDTH_IMG = 150;
        public const int MAX_HEIGHT_IMG = 130;
        public const int INCH_PIXEL_RATE = 96;
        public const int INCH_EMU_RATE = 914400;
        public const double IMG_COLUMN_RATE = 12;
        public const double IMG_ROW_RATE = 72;

        private static ImagePartType GetImageFormat(img.Formats.IImageFormat imgFormat)
        {
            if (imgFormat == null)
                throw new Exception("Image type could not be determined.");

            switch (imgFormat)
            {
                case img.Formats.Bmp.BmpFormat:
                    return ImagePartType.Bmp;

                case img.Formats.Gif.GifFormat:
                    return ImagePartType.Gif;

                case img.Formats.Jpeg.JpegFormat:
                    return ImagePartType.Jpeg;

                case img.Formats.Png.PngFormat:
                    return ImagePartType.Png;

                case img.Formats.Tiff.TiffFormat:
                    return ImagePartType.Tiff;

                default:
                    throw new Exception("Image type could not be determined.");
            }
        }

        public static string GetImageFromExcel(SpreadsheetDocument doc, string imageSavePath, string sheetName, string row, string col)
        {
            DrwS.Picture? picture;
            WorkbookPart? workbookPart = doc.WorkbookPart;
            Sheet? sheet = workbookPart?.Workbook?.GetFirstChild<Sheets>()?.Elements<Sheet>().FirstOrDefault(x => x.Name == sheetName);
            WorksheetPart? worksheetPart = (WorksheetPart?)workbookPart?.GetPartById(sheet.Id);
            TwoCellAnchor? twoCellHodingPicture = worksheetPart?.DrawingsPart?.WorksheetDrawing.OfType<TwoCellAnchor>()
                .Where(c => c.FromMarker?.RowId?.Text == row && c.FromMarker?.ColumnId?.Text == col).FirstOrDefault();
            if(twoCellHodingPicture == null)
            {
                OneCellAnchor? oneCellHodingpicture = worksheetPart?.DrawingsPart?.WorksheetDrawing.OfType<OneCellAnchor>()
                .Where(c => c.FromMarker?.RowId?.Text == row && c.FromMarker?.ColumnId?.Text == col).FirstOrDefault();
                if(oneCellHodingpicture == null)
                {
                    return "";
                }
                picture = oneCellHodingpicture.OfType<DrwS.Picture>().FirstOrDefault();
            }
            else
            {
                picture = twoCellHodingPicture.OfType<DrwS.Picture>().FirstOrDefault();
            }
            if(picture == null)
            {
                return "";
            }
            string? IdofPicture = picture.BlipFill?.Blip?.Embed;
            if(IdofPicture == null)
            {
                return "";
            }
            ImagePart imagePart = (ImagePart?)worksheetPart?.DrawingsPart?.GetPartById(IdofPicture);
            string? imageName = imagePart?.Uri?.ToString();
            string? imageType = imagePart.ContentType;
            if(imageName == null && imagePart == null)
            {
                return "";
            }
            string setImageName = $"{Guid.NewGuid()}{System.IO.Path.GetExtension(imageName)}";
            string pathFile = System.IO.Path.Combine(imageSavePath, setImageName);
            using(FileStream fs = new FileStream(pathFile, FileMode.Create))
            {
                imagePart?.GetStream()?.CopyTo(fs);
            }
            return setImageName;
        }
        public static void AddImage(WorksheetPart worksheetPart,
                                   string imageFileName, string imgDesc,
                                   int colNumber, int rowNumber)
        {
            //Get Image Info
            ImageInfo imgInfo = new ImageInfo();
            img.Image image = img.Image.Load(imageFileName);
            imgInfo.Width = image.Width;
            imgInfo.Height = image.Height;
            switch (image.Metadata.ResolutionUnits)
            {
                case img.Metadata.PixelResolutionUnit.AspectRatio:
                    imgInfo.HorizontalDpi = image.Metadata.HorizontalResolution > 1 ? image.Metadata.HorizontalResolution : INCH_PIXEL_RATE * image.Metadata.HorizontalResolution;
                    imgInfo.VerticalDpi = image.Metadata.VerticalResolution > 1 ? image.Metadata.VerticalResolution : INCH_PIXEL_RATE * image.Metadata.VerticalResolution;
                    break;
                case img.Metadata.PixelResolutionUnit.PixelsPerInch:
                    imgInfo.HorizontalDpi = image.Metadata.HorizontalResolution;
                    imgInfo.VerticalDpi = image.Metadata.VerticalResolution;
                    break;
                case img.Metadata.PixelResolutionUnit.PixelsPerCentimeter:
                    imgInfo.HorizontalDpi = image.Metadata.HorizontalResolution / 2.54f;
                    imgInfo.VerticalDpi = image.Metadata.VerticalResolution / 2.54f;
                    break;
                case img.Metadata.PixelResolutionUnit.PixelsPerMeter:
                    imgInfo.HorizontalDpi = (image.Metadata.HorizontalResolution * 2.54f) / 100;
                    imgInfo.VerticalDpi = (image.Metadata.VerticalResolution * 2.54f) / 100;
                    break;
                default:
                    break;
            }

            imgInfo.Format = GetImageFormat(image.Metadata.DecodedImageFormat);
            image.Dispose();

            using (var imageStream = new FileStream(imageFileName, FileMode.Open))
            {
                AddImage(worksheetPart, imageStream, imgDesc, colNumber, rowNumber, imgInfo);
            }
        }

        public static void AddImage(WorksheetPart worksheetPart,
                                    Stream imageStream, string imgDesc,
                                    int colNumber, int rowNumber, ImageInfo imageInfo)
        {
            var drawingsPart = worksheetPart.DrawingsPart;
            if (drawingsPart == null)
                drawingsPart = worksheetPart.AddNewPart<DrawingsPart>();

            if (!worksheetPart.Worksheet.ChildElements.OfType<Drawing>().Any())
            {
                worksheetPart.Worksheet.Append(new Drawing { Id = worksheetPart.GetIdOfPart(drawingsPart) });
            }

            if (drawingsPart.WorksheetDrawing == null)
            {
                drawingsPart.WorksheetDrawing = new DrwS.WorksheetDrawing();
            }
            var worksheetDrawing = drawingsPart.WorksheetDrawing;

            int newW, newH;
            newW = imageInfo.Width > MAX_WIDTH_IMG ? MAX_WIDTH_IMG : imageInfo.Width;
            newH = newW * imageInfo.Height / imageInfo.Width;

            if (newH > MAX_HEIGHT_IMG)
            {
                newH = MAX_HEIGHT_IMG - 10;
                newW = newH * imageInfo.Width / imageInfo.Height;
            }
            double pixcelW = (double)newW / imageInfo.HorizontalDpi;
            double pixcelH = (double)newH / imageInfo.VerticalDpi;

            // Tính toán độ phân giải ngang và dọc
            var extentsCx = newW * (long)(INCH_EMU_RATE / imageInfo.HorizontalDpi);
            var extentsCy = newH * (long)(INCH_EMU_RATE / imageInfo.VerticalDpi);

            var imagePart = drawingsPart.AddImagePart(imageInfo.Format);
            imagePart.FeedData(imageStream);

            // Tính toán vị trí căn giữa trong ô
            double columnWidthEMU = GetColumnWidthEMU(worksheetPart, colNumber);
            double rowHeightEMU = GetRowHeightEMU(worksheetPart, rowNumber);
            long colOffset = (long)(INCH_EMU_RATE * (columnWidthEMU - pixcelW) / 2);
            long rowOffset = (long)(INCH_EMU_RATE * (rowHeightEMU - pixcelH) / 2);

            var nvps = worksheetDrawing.Descendants<DrwS.NonVisualDrawingProperties>();
            var nvpId = nvps.Count() > 0
                ? (UInt32Value)worksheetDrawing.Descendants<DrwS.NonVisualDrawingProperties>().Max(p => p.Id.Value) + 1
                : 1U;

            var oneCellAnchor = new DrwS.OneCellAnchor(
                new DrwS.FromMarker
                {
                    ColumnId = new DrwS.ColumnId((colNumber - 1).ToString()),
                    RowId = new DrwS.RowId((rowNumber - 1).ToString()),
                    ColumnOffset = new DrwS.ColumnOffset(colOffset.ToString()),
                    RowOffset = new DrwS.RowOffset(rowOffset.ToString())
                },
                new DrwS.Extent { Cx = extentsCx, Cy = extentsCy },
                new DrwS.Picture(
                    new DrwS.NonVisualPictureProperties(
                        new DrwS.NonVisualDrawingProperties { Id = nvpId, Name = "Picture " + nvpId, Description = imgDesc },
                        new DrwS.NonVisualPictureDrawingProperties(new A.PictureLocks { NoChangeAspect = true })
                    ),
                    new DrwS.BlipFill(
                        new A.Blip { Embed = drawingsPart.GetIdOfPart(imagePart), CompressionState = A.BlipCompressionValues.Print },
                        new A.Stretch(new A.FillRectangle())
                    ),
                    new DrwS.ShapeProperties(
                        new A.Transform2D(
                            new A.Offset { X = 0, Y = 0 },
                            new A.Extents { Cx = extentsCx, Cy = extentsCy }
                        ),
                        new A.PresetGeometry { Preset = A.ShapeTypeValues.Rectangle }
                    )
                ),
                new DrwS.ClientData()
            );

            worksheetDrawing.Append(oneCellAnchor);
        }


        static double GetColumnWidthEMU(WorksheetPart worksheetPart, int columnIndex)
        {
            Columns? columns = worksheetPart.Worksheet.GetFirstChild<Columns>();
            if (columns != null)
            {
                Column? column = columns.Elements<Column>().Where(c => c.Min == columnIndex && c.Max == columnIndex).FirstOrDefault();
                if (column != null && column.Width != null)
                {
                    return (double)((double)column.Width.Value / IMG_COLUMN_RATE); // Chuyển đổi từ đơn vị excel width sang inch
                }
            }
            return 0;
        }

        // Hàm tính toán đơn vị EMU cho chiều cao của hàng
        static double GetRowHeightEMU(WorksheetPart worksheetPart, int rowIndex)
        {
            Row? row = worksheetPart.Worksheet.Descendants<Row>().Where(r => r.RowIndex == rowIndex).FirstOrDefault();
            if (row != null && row.Height != null)
            {
                return (double)((double)row.Height.Value / IMG_ROW_RATE); // Chuyển đổi từ đơn vị excel height sang inch
            }
            return 0;
        }

        public class ImageInfo
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public double HorizontalDpi { get; set; }
            public double VerticalDpi { get; set; }
            public ImagePartType Format { get; set; }
            public ImageInfo() { }
        }
    }
}