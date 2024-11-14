using Common;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Reflection;

    namespace Helpers.Excel
    {
        public static class ExcelImport
        {
            public static async Task<(List<T>, List<string>)> getDataFromExcel<T>(IFormFile file, int totalCol, string imageStorePath) where T : new()
            {
                using (SpreadsheetDocument doc = SpreadsheetDocument.Open(file.OpenReadStream(), false))
                {
                    Sheet sheet = doc.WorkbookPart.Workbook.Sheets.GetFirstChild<Sheet>();
                    Worksheet worksheet = (doc.WorkbookPart.GetPartById(sheet.Id.Value) as WorksheetPart).Worksheet;
                    IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();
                    Row headerKey = rows.First();
                    var messages = new List<string>();
                    List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
                    List<T> listData = new();
                    Row? previousRow = null;
                    int rowIndex = 0;
                    bool isRowEmpty = false;
                    foreach (Row row in rows)
                    {
                        rowIndex++;
                        if (row != null)
                        {
                            if (rowIndex >= 2)
                            {
                                bool add = true;
                                if (previousRow != null)
                                {
                                    
                                    if (AreRowsEqual(row, previousRow, doc, totalCol, out isRowEmpty))
                                    {
                                        messages.Add("Duplicate rows at index: " + row.RowIndex.Value + "and" + previousRow.RowIndex.Value);
                                        add = false;
                                    } 
                                }
                                if (isRowEmpty)
                                {
                                    break;
                                }
                                if (add)
                                {
                                    Dictionary<string, string> dataTemp = new Dictionary<string, string>();
                                    string firstHeader = getCellValue(doc, (Cell)headerKey.ElementAt(0));
                                    if (firstHeader.ToLower().Contains("image"))
                                    {
                                        string imageName = ExcelImage.GetImageFromExcel(doc, imageStorePath, "sheet1", $"{rowIndex - 1}", "0");
                                        dataTemp.Add(firstHeader, imageName);
                                        for (var col = 1; col < totalCol; col++)
                                        {
                                            string key = getCellValue(doc, (Cell)headerKey.ElementAt(col));
                                            //chỉ số ô (A1, B2)
                                            string cellRef = ColumnIndexToLetter(col + 1) + rowIndex;
                                            Cell? cellTarget = row.Descendants<Cell>().Where(r => cellRef.Equals(r.CellReference)).SingleOrDefault();
                                            string val = "";
                                            if (cellTarget != null)
                                            {
                                                val = getCellValue(doc, cellTarget);
                                            }
                                            if ((key.Contains("price_unit") || key.Contains("quantity")) && string.IsNullOrEmpty(val))
                                            {
                                                val = 0.ToString();
                                            }
                                            dataTemp.Add(key, val);
                                        }
                                    }
                                    else
                                    {
                                        for (var col = 0; col < totalCol; col++)
                                        {
                                            string key = getCellValue(doc, (Cell)headerKey.ElementAt(col));
                                            string cellReference = ColumnIndexToLetter(col + 1) + rowIndex;
                                            Cell? cellTarget = row.Descendants<Cell>().Where(r => cellReference.Equals(r.CellReference)).SingleOrDefault();
                                            string val = "";
                                            if (cellTarget != null)
                                            {
                                                val = getCellValue(doc, cellTarget);
                                            }
                                            if ((key.Contains("price_unit") || key.Contains("quantity")) && string.IsNullOrEmpty(val))
                                            {
                                                val = 0.ToString();
                                            }

                                            dataTemp.Add(key, val);
                                        }
                                        data.Add(dataTemp);
                                        listData.Add(dictionaryToObject<T>(dataTemp));
                                    }
                                    previousRow = row;
                                }
                            }
                        }
                    }
                    doc.Dispose();
                    return (listData, messages);
                }
            }

            public static async Task<(BaseResponse?, MemoryStream?)> getFileError(IFormFile file, Dictionary<int, List<string>> rowErrors, string insertedCol)
            {
                try
                {
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        stream.Position = 0;
                        using(SpreadsheetDocument doc = SpreadsheetDocument.Open(stream, true))
                        {
                            var workBookPart = doc.WorkbookPart;
                            var sheet = workBookPart.Workbook.Descendants<Sheet>().FirstOrDefault();
                            if(sheet != null)
                            {
                                var workSheetPart = (WorksheetPart)workBookPart.GetPartById(sheet.Id);
                                var sheetData = workSheetPart.Worksheet.GetFirstChild<SheetData>();

                                Row headerRow = sheetData.Elements<Row>().First();
                                Cell cellHeader = InsertCellInWorksheetNews(insertedCol.ToUpper(), 2, workSheetPart);
                                cellHeader.CellValue = new CellValue("Lỗi Import");
                                cellHeader.DataType = new EnumValue<CellValues>(CellValues.String);
                                var stylePart = workBookPart.WorkbookStylesPart ?? workBookPart.AddNewPart<WorkbookStylesPart>();
                                cellHeader.ApplyStyle(stylePart, true, false, true);

                                foreach(var entry in rowErrors)
                                {
                                    int rowIndex = entry.Key + 3;
                                    string combinedErrors = string.Join(", ", entry.Value.Where(e => !string.IsNullOrEmpty(e)));
                                    if (!string.IsNullOrEmpty(combinedErrors))
                                    {
                                        var cell = InsertCellInWorksheetNews(insertedCol.ToUpper(), rowIndex, workSheetPart);
                                        cell.CellValue = new CellValue(combinedErrors);
                                        cell.DataType = new EnumValue<CellValues>(CellValues.String);
                                        cell.ApplyStyle(stylePart, false, true, false, true);
                                    }
                                }

                                var headerRow1 = sheetData.Elements<Row>().First();
                                workSheetPart.Worksheet.Save();

                                var headerRow2 = sheetData.Elements<Row>().ElementAt(2); 
                            }
                        }
                        return (null, stream);
                    }
                }
                catch (Exception e)
                {
                    return (new BaseResponse(ResponseCode.SystemError, e.Message), null);
                }
            }
            public static (int,string) CheckValueRow(SpreadsheetDocument doc, Row row, int cellIndex, int rowIndex)
            {
                string val = getCellValue(doc, (Cell)row.ElementAt(cellIndex));
                if (val != null && val.Length > 0)
                    return (rowIndex, val);
                return (0,"");
            }

            public static void ApplyStyle(this Cell cell, WorkbookStylesPart stylePart, bool isBold, bool noFill, bool isCenter = false, bool isWraptext = false, BorderStyleValues? borderStyle = null, string fontColor = "ff0000", string fillColor = "E8E8E8")
            {
                borderStyle = borderStyle ?? BorderStyleValues.Thin;

                if (stylePart.Stylesheet == null)
                    {
                        stylePart.Stylesheet = new Stylesheet();
                    }
                    var styleSheet = stylePart.Stylesheet;
                    string fontName = "Meiryo UI";
                    var font = new Font(
                        new FontSize() { Val = 11 },
                        new Color() { Rgb = new HexBinaryValue() { Value = fontColor } },
                        new FontName() { Val = fontName });
                if(isBold)
                {
                    font.Append(new Bold());
                }
                var fontId = InsertFont(styleSheet, font);
                var fill = new Fill(new PatternFill(new ForegroundColor() { Rgb = new HexBinaryValue() { Value = fillColor } }) { PatternType = PatternValues.Solid });
                if (noFill)
                {
                    fill = new Fill(new PatternFill(new ForegroundColor() { Rgb = new HexBinaryValue() { Value = fillColor } }) { PatternType = PatternValues.None });
                }
                var fillId = InsertFill(styleSheet, fill);

                var borderId = InsertBorder(styleSheet, new Border()
                {
                    LeftBorder = new LeftBorder() { Style = borderStyle },
                    RightBorder = new RightBorder() { Style = borderStyle },
                    TopBorder = new TopBorder() { Style = borderStyle },
                    BottomBorder = new BottomBorder() { Style = borderStyle }
                });
                var formatId = InsertCellFormat(styleSheet, fontId, fillId, borderId, isCenter, isWraptext);
                cell.StyleIndex = formatId;
            }

            private static uint InsertFont(Stylesheet stylesheet, Font font)
            {
                var fonts = stylesheet.Fonts ?? (stylesheet.Fonts = new Fonts());
                fonts.Append(font);
                return (uint)fonts.Count++;
            }

            private static uint InsertFill(Stylesheet stylesheet, Fill fill)
            {
                var fills = stylesheet.Fills ?? (stylesheet.Fills = new Fills());
                fills.Append(fill);
                return (uint)fills.Count++;
            }

            private static uint InsertBorder(Stylesheet stylesheet, Border border)
            {
                var borders = stylesheet.Borders ?? (stylesheet.Borders = new Borders());
                borders.Append(border);
                return (uint)borders.Count++;
            }

            private static uint InsertCellFormat(Stylesheet stylesheet, uint fontId, uint fillId, uint borderId, bool isTextCentered = false, bool wrapText = false)
            {
                var cellFormat = new CellFormat()
                {
                    FontId = fontId,
                    FillId = fillId,
                    BorderId = borderId,
                    ApplyFont = true,
                    ApplyFill = true,
                    ApplyBorder = true,
                    Alignment = new Alignment()
                    {
                        Horizontal = isTextCentered ? HorizontalAlignmentValues.Center : (HorizontalAlignmentValues?)null,
                        WrapText = wrapText ? true : (bool?)null,
                    },
                    ApplyAlignment = isTextCentered
                };

                var cellFormats = stylesheet.CellFormats ?? (stylesheet.CellFormats = new CellFormats());
                cellFormats.Append(cellFormat);
                stylesheet.CellFormats = cellFormats;
                uint newFormatId = (uint)(cellFormats.Count() - 1);
                stylesheet.CellFormats.Count = (uint)cellFormats.Count();
                return newFormatId;
            }


            public static Cell InsertCellInWorksheetNews(string columnName, int rowIndex, WorksheetPart worksheetPart)
            {
                Worksheet worksheet = worksheetPart.Worksheet;
                SheetData sheetData = worksheet.GetFirstChild<SheetData>();
                Row row;

                row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).FirstOrDefault();
                if (row == null)
                {
                    row = new Row() { RowIndex = (uint)rowIndex };
                    sheetData.Append(row);
                }

                Cell cell = new Cell() { CellReference = columnName + rowIndex };
                row.Append(cell);

                return cell;
            }

            public static string ColumnIndexToLetter(int colIndex)
            {
                int div = colIndex;
                string colLetter = String.Empty;
                while(div > 0)
                {
                    var mod = (div - 1) % 26;
                    colLetter = (char)(65 + mod) + colLetter;
                    div = (int)((div - mod) / 26);
                }
                return colLetter;
            }

            private static T dictionaryToObject<T>(Dictionary<string,string> dictionary) where T : new()
            {
                var t = new T();
                PropertyInfo[] properties = t.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (!dictionary.Any(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase)))
                        continue;
                    KeyValuePair<string, string> item = dictionary.First(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase));
                    Type tPropertyType = t.GetType().GetProperty(property.Name).PropertyType;
                    Type newT = Nullable.GetUnderlyingType(tPropertyType) ?? tPropertyType;
                    object newA = Convert.ChangeType(item.Value, newT);
                    t.GetType().GetProperty(property.Name).SetValue(t, newA, null);
                }
                return t;
            }

            public static void addError(Dictionary<int, List<string>> rowErrors, int rowIndex, string error)
            {
                if(!rowErrors.ContainsKey(rowIndex))
                {
                    rowErrors[rowIndex] = new List<string>();
                }
                rowErrors[rowIndex].Add(error); 
            }

            private static bool AreRowsEqual(Row currentRow, Row previousRow, SpreadsheetDocument doc, int totalCol, out bool isRowEmpty)
            {
                var cellIndex = 0;
                isRowEmpty = true;
                for(var i = 0; i < totalCol; i++)
                {
                    string currentVal = getCellValue(doc, (Cell)currentRow.ElementAt(cellIndex));
                    string previousVal = getCellValue(doc, (Cell)previousRow.ElementAt(cellIndex));
                    if (!string.IsNullOrEmpty(currentVal))
                    {
                        isRowEmpty = false;
                    }
                    if(currentVal != previousVal)
                    {
                        return false;
                    }
                    cellIndex++;
                }
                return true;
            }

            private static string getCellValue(SpreadsheetDocument doc, Cell cell)
            {
                if (cell != null && cell.CellValue != null)
                {
                    string value = cell.CellValue.InnerText;
                    if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
                    {
                        return doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ChildElements.ElementAt(int.Parse(value)).InnerText;
                    }
                    else
                    {
                        return value;
                    }
                }
                return string.Empty;
            }
        }
    }
