using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Helpers.Excel
{
    public static class ExcelExport
    {
        public static WorkbookPart CreateSheet(this WorkbookPart workbookpart, string sheetName)
        {
            workbookpart.Workbook.AppendChild<Sheets>(new Sheets());
            workbookpart.WorksheetParts.First().Worksheet = new Worksheet(
                                        new SheetProperties(new PageSetupProperties() { FitToPage = BooleanValue.FromBoolean(true) }),
                                        new SheetViews(new SheetView() { WorkbookViewId = 0, ShowGridLines = new BooleanValue(false) }),
                                        new SheetData()
                                        );

            workbookpart.WorksheetParts.First().Worksheet.Save();
            Sheets sheets = workbookpart.Workbook.GetFirstChild<Sheets>();
            WorksheetPart worksheetPart = workbookpart.WorksheetParts.First();
            string relationshipId = workbookpart.GetIdOfPart(worksheetPart);

            worksheetPart.AddPagePrint();

            // Get a unique ID for the new sheet.
            uint sheetId = 1;
            if (sheets.Elements<Sheet>().Count() > 0)
                sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;

            // Append the new worksheet and associate it with the workbook.
            Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = sheetName };
            sheets.Append(sheet);
            workbookpart.Workbook.Save();
            return workbookpart;
        }

        public static SpreadsheetDocument CreateWorkbookPart(this SpreadsheetDocument document)
        {
            // Add a WorkbookPart to the document.
            WorkbookPart workbookpart = document.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();

            // Add a WorksheetPart to the WorkbookPart.
            WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(
                                        new SheetViews(new SheetView() { WorkbookViewId = 0, ShowGridLines = new BooleanValue(false) }),
                                        new SheetData()
                                        );

            // Add a WorkbookStylesPart to the WorkbookPart.
            WorkbookStylesPart workStylePart = workbookpart.AddNewPart<WorkbookStylesPart>();
            workStylePart.Stylesheet = DefaultStylesheetData();
            return document;
        }

        public static WorksheetPart AddPagePrint(this WorksheetPart worksheetPart)
        {
            PageSetup pageSetup = new PageSetup();
            pageSetup.Orientation = OrientationValues.Portrait;
            pageSetup.Scale = 100;
            pageSetup.FitToWidth = 1;
            pageSetup.FitToHeight = 0;
            pageSetup.PaperSize = 9; // xlPaperA4 - A4 (210 mm x 297 mm)
            worksheetPart.Worksheet.AppendChild(pageSetup);

            return worksheetPart;
        }

        public static WorkbookPart FillCellInfomation(this WorkbookPart workbookpart, Dictionary<string, object> cellInfo)
        {
            SharedStringTablePart shareStringPart;
            if (workbookpart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
            {
                shareStringPart = workbookpart.GetPartsOfType<SharedStringTablePart>().First();
            }
            else
            {
                shareStringPart = workbookpart.AddNewPart<SharedStringTablePart>();
            }
            // Insert a new worksheet.
            WorksheetPart worksheetPart = workbookpart.WorksheetParts.First();
            foreach (var item in cellInfo)
            {
                var dataObject = item.Value.GetType().GetProperties();
                // Insert the text into the SharedStringTablePart.
                var dataValue = dataObject[0].GetValue(item.Value, null)?.ToString();
                InsertSharedStringItem(dataValue, shareStringPart);
                uint indexValue = uint.Parse(Regex.Replace(item.Key, @"\D", ""));
                string cellLName = Regex.Replace(item.Key, @"\d", "");
                Cell cell = InsertCellInWorksheet(cellLName, indexValue, worksheetPart);
                var dataType = (DataType?)dataObject[1].GetValue(item.Value, null);
                switch (dataType)
                {
                    case DataType.NUMBER:
                        cell.DataType = CellValues.Number;

                        break;
                    case DataType.DATE:
                        cell.DataType = CellValues.Date;

                        break;
                    case DataType.DATETIME:
                        cell.DataType = CellValues.Date;

                        break;
                    case DataType.MONTH:
                        cell.DataType = CellValues.Date;
                        break;
                    default:
                        cell.DataType = CellValues.String;
                        break;
                }
                cell.CellValue = new CellValue(dataValue);

            }
            // Save the new worksheet.
            worksheetPart.Worksheet.Save();
            workbookpart.Workbook.Save();
            return workbookpart;
        }

        private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
        {
            Worksheet worksheet = worksheetPart.Worksheet;
            SheetData sheetData = worksheet.GetFirstChild<SheetData>();
            string cellReference = columnName + rowIndex;
            var listRowIndex = sheetData.Elements<Row>().Where(r => r.RowIndex != null && r.RowIndex == rowIndex).ToList();
            // If the worksheet does not contain a row with the specified row index, insert one.
            Row row;

            if (listRowIndex.Count() != 0)
            {
                row = listRowIndex.First();
            }
            else
            {
                row = new Row() { RowIndex = rowIndex };
                sheetData.Append(row);
            }

            // If there is not a cell with the specified column name, insert one.  
            if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
            {
                return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
            }
            else
            {
                // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
                Cell refCell = null;
                foreach (Cell cell in row.Elements<Cell>())
                {
                    if (cell.CellReference.Value.Length == cellReference.Length)
                    {
                        if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                        {
                            refCell = cell;
                            break;
                        }
                    }
                }

                Cell newCell = new Cell() { CellReference = cellReference };
                row.InsertBefore(newCell, refCell);

                worksheet.Save();
                return newCell;
            }
        }
        
        private static int InsertSharedStringItem(string? text, SharedStringTablePart shareStringPart)
        {
            // If the part does not contain a SharedStringTable, create one.
            if (shareStringPart.SharedStringTable == null)
            {
                shareStringPart.SharedStringTable = new SharedStringTable();
            }

            int i = 0;
            // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
            foreach (SharedStringItem item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText == text)
                {
                    return i;
                }

                i++;
            }

            // The text does not exist in the part. Create the SharedStringItem and return its index.
            shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new DocumentFormat.OpenXml.Spreadsheet.Text(text)));
            shareStringPart.SharedStringTable.Save();

            return i;
        }
        public static Cell? GetCell(int rowidx, string columnName, SheetData sheetData)
        {
            if (rowidx < 0) return null;
            var row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowidx).FirstOrDefault();
            if (row != null)
            {
                return row.Elements<Cell>().Where(c => string.Compare(c.CellReference.Value, columnName + rowidx, true) == 0).FirstOrDefault();
            }
            return null;
        }

        public static string ColumnIndexToLetter(int colIndex)
        {
            if (colIndex > 16384)
            {
                return string.Empty;
            }
            int div = colIndex;
            string colLetter = String.Empty;
            while (div > 0)
            {
                var mod = (div - 1) % 26;
                colLetter = (char)(65 + mod) + colLetter;
                div = (int)((div - mod) / 26);
            }
            return colLetter;
        }

        public static void WriteCellValue(int rowidx, 
                                          int colidx, 
                                          SheetData? sheetData, 
                                          string? data,
                                          string? formula = null,
                                          bool isFormula = false,
                                          bool keepIsString = false,
                                          WorkbookPart? workbookPart = null)
        {
            if (sheetData == null)
            {
                return;
            }

            var colName = ColumnIndexToLetter(colidx);
            var cell = GetCell(rowidx, colName, sheetData);
            if (cell != null)
            {

                if (isFormula && formula != null)
                {
                    cell.CellFormula = new CellFormula(formula);
                    return;
                }

                if (data == null)
                {
                    return;
                }

                if (!keepIsString && (double.TryParse(data, out _) || int.TryParse(data, out _)))
                {
                    cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                }
                else
                {
                    cell.DataType = new EnumValue<CellValues>(CellValues.String);
                }

                cell.CellValue = new CellValue(EncodeSpecialCharacters(data));
            }
        }

        public static Row? InsertCopyRow(int rowidx, SheetData sheetData)
        {
            Row? row;
            Row? refRow = sheetData.Elements<Row>().Where(r => rowidx == r.RowIndex).FirstOrDefault();
            if ((refRow != null))
            {
                //Copy row from refRow and insert it
                row = CopyToLine(sheetData, refRow, (uint)rowidx, false);
            }
            else
            {
                row = new Row() { RowIndex = (uint)rowidx };
                sheetData.Append(row);
            }
            return row;
        }

        private static Row? CopyToLine(SheetData sheetData, Row refRow, uint rowidx, bool keepData = true)
        {
            uint newRowIndex;
            var newRow = (Row)refRow.CloneNode(true);

            IEnumerable<Row> rows = sheetData.Descendants<Row>().Where(r => r.RowIndex?.Value >= rowidx);
            foreach (Row row in rows)
            {
                newRowIndex = Convert.ToUInt32(row.RowIndex?.Value + 1);

                foreach (Cell cell in row.Elements<Cell>())
                {
                    if (!keepData)
                    {
                        cell.CellValue = null;
                    }

                    string? cellReference = cell.CellReference?.Value;
                    cell.CellReference = new StringValue(cellReference?.Replace(row.RowIndex.Value.ToString(), newRowIndex.ToString()));
                }
                // Update the row index.
                row.RowIndex = new UInt32Value(newRowIndex);
            }

            sheetData.InsertBefore(newRow, refRow);
            return sheetData.Descendants<Row>().Where(r => r.RowIndex?.Value == rowidx + 1).SingleOrDefault();
        }

        public static string EncodeSpecialCharacters(string data)
        {
            try
            {
                string pattern = "[\x00-\x08\x0B\x0C\x0E-\x1F]";

                MatchCollection matches = Regex.Matches(data, pattern, RegexOptions.Compiled);
                foreach (Match match in matches)
                {
                    string encodedValue = ((int)Convert.ToChar(match.Value)).ToString("x4");
                    data = data.Replace(match.Value, $"_x{encodedValue}_");
                }

                return data;
            }
            catch
            {
                return null;
            }
        }

        public static WorkbookPart FillGridData<T>(this WorkbookPart workbookpart, List<T> data, List<ExcelItem> cellConfigs)
        {
            
            // Append row headder
            Row workRowKey = new Row();
            workRowKey.Hidden = true;
            Row workRow = new Row();
            int currentRowIdx;

            // TODO: Find by sheet name
            SheetData sheetData = workbookpart.WorksheetParts.First().Worksheet.GetFirstChild<SheetData>();

            if (sheetData == null)
            {
                return workbookpart;
            }
            currentRowIdx = sheetData.Elements<Row>().Count();

            int colKeyIdx = 0;
            int colHeaderIdx = 0;
            foreach (var item in cellConfigs)
            {
                if (item.isKeyIncluded)
                {
                    colKeyIdx++;
                    Cell cellKey = CreateCell(item.key, DataType.TEXT, true, false);
                    cellKey.CellReference = GetCellReference(colKeyIdx, currentRowIdx + 1);
                    workRowKey.Append(cellKey);
                }

                colHeaderIdx++;
                Cell cellHeader = CreateCell(item.header, item.type, true, false);
                cellHeader.CellReference = GetCellReference(colHeaderIdx, currentRowIdx + 2);
                workRow.Append(cellHeader);
            }

            if (workRowKey.Count() != 0 && sheetData != null)
            {
                currentRowIdx++;
                workRowKey.RowIndex = new UInt32Value((uint)currentRowIdx);
                sheetData.Append(workRowKey);
            }
            currentRowIdx++;
            workRow.RowIndex = new UInt32Value((uint)currentRowIdx);
            sheetData?.Append(workRow);

            // Append data
            foreach (var dataItem in data)
            {
                currentRowIdx++;
                Row partsRows = GenerateRowContent(dataItem, cellConfigs, currentRowIdx);
                partsRows.RowIndex = new UInt32Value((uint)currentRowIdx);
                sheetData.Append(partsRows);
            }
            //get your columns (where your width is set)
            Columns columns = AutoSize(sheetData, cellConfigs);
            WorksheetPart worksheetPart = workbookpart.WorksheetParts.First();
            worksheetPart.Worksheet.InsertBefore(columns, sheetData);
            worksheetPart.Worksheet.Save();

            workbookpart.WorkbookStylesPart.Stylesheet.Save();
            //workbookpart.WorksheetParts.First().Worksheet.Elements<SheetData>().First().Equals(sheetData);
            workbookpart.Workbook.Save();
            return workbookpart;
        }
        private static Row GenerateRowContent<T>(T entity, List<ExcelItem> cellConfigs, int currentRowIdx = 0)
        {
            Row tRow = new Row();
            Type entType = entity.GetType();
            if (entType.Name == "ExpandoObject")
            {
                IDictionary<string, object> propertyValues = (IDictionary<string, object>)entity;
                foreach (var itemConfig in cellConfigs)
                {
                    var propValue = propertyValues[itemConfig.key];
                    propValue = (propValue != null) ? propValue : "";
                    Cell c = CreateCell(propValue.ToString(), itemConfig.type, false, true);
                    tRow.AppendChild(c);
                }
            }
            else
            {
                IList<PropertyInfo> props = new List<PropertyInfo>(entType.GetProperties());
                int colIdx = 0;
                foreach (var itemConfig in cellConfigs)
                {
                    colIdx++;
                    var itemProp = props.Where(x => x.Name == itemConfig.key).First();
                    var propValue = itemProp.GetValue(entity);
                    propValue = (propValue != null) ? propValue : "";
                    Cell c = CreateCell(propValue.ToString(), itemConfig.type, false, true);
                    if (currentRowIdx > 0)
                    {
                        c.CellReference = GetCellReference(colIdx, currentRowIdx);
                    }
                    tRow.AppendChild(c);
                }
            }

            return tRow;
        }
        private static Columns AutoSize(SheetData sheetData, List<ExcelItem> colConfigs)
        {
            var maxColWidth = GetMaxCharacterWidth(sheetData);

            Columns columns = new Columns();
            //this is the width of my font - yours may be different
            double maxWidth = 7;
            foreach (var item in maxColWidth)
            {
                //width = Truncate([{Number of Characters} * {Maximum Digit Width} + {5 pixel padding}]/{Maximum Digit Width}*256)/256
                double width = colConfigs[item.Key].width ?? 10; // Math.Truncate((item.Value * maxWidth + 5) / maxWidth * 256) / 256;

                //pixels=Truncate(((256 * {width} + Truncate(128/{Maximum Digit Width}))/256)*{Maximum Digit Width})
                double pixels = Math.Truncate(((256 * width + Math.Truncate(128 / maxWidth)) / 256) * maxWidth);

                //character width=Truncate(({pixels}-5)/{Maximum Digit Width} * 100+0.5)/100
                double charWidth = Math.Truncate((pixels - 5) / maxWidth * 100 + 0.5) / 100;

                Column col = new Column() { BestFit = false, Min = (UInt32)(item.Key + 1), Max = (UInt32)(item.Key + 1), CustomWidth = true, Width = (DoubleValue)(width) };
                columns.Append(col);
            }

            return columns;
        }

        private static Dictionary<int, int> GetMaxCharacterWidth(SheetData sheetData)
        {
            //iterate over all cells getting a max char value for each column
            Dictionary<int, int> maxColWidth = new Dictionary<int, int>();
            var rows = sheetData.Elements<Row>();
            UInt32[] numberStyles = new UInt32[] { 5, 6, 7, 8 }; //styles that will add extra chars
            UInt32[] boldStyles = new UInt32[] { 1, 2, 3, 4, 6, 7, 8 }; //styles that will bold
            foreach (var r in rows)
            {
                var cells = r.Elements<Cell>().ToArray();

                //using cell index as my column
                for (int i = 0; i < cells.Length; i++)
                {
                    var cell = cells[i];
                    var cellValue = cell.CellValue == null ? string.Empty : cell.CellValue.InnerText;
                    var cellTextLength = cellValue.Length * 2;

                    if (cell.StyleIndex != null && numberStyles.Contains(cell.StyleIndex))
                    {
                        int thousandCount = (int)Math.Truncate((double)cellTextLength / 4);

                        //add 3 for '.00' 
                        cellTextLength += (3 + thousandCount);
                    }

                    if (cell.StyleIndex != null && boldStyles.Contains(cell.StyleIndex))
                    {
                        //add an extra char for bold - not 100% acurate but good enough for what i need.
                        cellTextLength += 1;
                    }

                    if (maxColWidth.ContainsKey(i))
                    {
                        var current = maxColWidth[i];
                        if (cellTextLength > current)
                        {
                            maxColWidth[i] = cellTextLength;
                        }
                    }
                    else
                    {
                        maxColWidth.Add(i, cellTextLength);
                    }
                }
            }

            return maxColWidth;
        }

        private static Cell CreateCell(string text, DataType? type, bool is_table_header = false, bool is_table_cell = false, VCellStyle? cellStyle = VCellStyle.Normal)
        {
            Cell cell = new Cell();
            if (is_table_header)
            {
                cell.CellValue = new CellValue(text);
                cell.DataType = CellValues.String;
                cell.StyleIndex = 1;
                return cell;
            }

            switch (type)
            {
                case DataType.NUMBER:
                    cell.DataType = CellValues.Number;
                    cell.CellValue = new CellValue(text);
                    if (is_table_cell)
                        cell.StyleIndex = 3;
                    break;
                case DataType.DECIMAL:
                    cell.DataType = CellValues.Number;
                    cell.CellValue = new CellValue(text);
                    if (is_table_cell)
                        cell.StyleIndex = 5;
                    break;
                case DataType.DATE:
                    cell.DataType = CellValues.Date;
                    cell.CellValue = new CellValue(text);
                    break;
                case DataType.DATETIME:
                    cell.DataType = CellValues.Date;
                    cell.CellValue = new CellValue(text);
                    break;
                case DataType.MONTH:
                    cell.DataType = CellValues.Date;
                    cell.CellValue = new CellValue(DateTime.Parse(text));
                    if (is_table_cell)
                        cell.StyleIndex = 4;
                    break;
                default:
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(text);
                    if (cellStyle != VCellStyle.Normal)
                        cell.StyleIndex = UInt32Value.FromUInt32((uint)cellStyle);
                    else if (is_table_cell)
                        cell.StyleIndex = 2;
                    break;
            }
            return cell;
        }
        private static Stylesheet DefaultStylesheetData()
        {
            Stylesheet ss = new Stylesheet();

            //var fontFamily = "Arial";
            var fontFamily = "Arial";
            Fonts fts = new Fonts(
                // Cell normal
                new Font(
                    new FontSize() { Val = 11 },
                    new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                    new FontName() { Val = fontFamily }),
                // Table header
                new Font(
                    new Bold(),
                    new FontSize() { Val = 11 },
                    new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                    new FontName() { Val = fontFamily }),
                // Page header
                new Font(
                    new Bold(),
                    new FontSize() { Val = 14 },
                    new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                    new FontName() { Val = fontFamily })
            );
            ss.Append(fts);

            Fills fills = new Fills(
                new Fill(new PatternFill() { PatternType = PatternValues.Solid, BackgroundColor = new BackgroundColor() { } }), // Don't remove, it excel default
                new Fill(new PatternFill() { PatternType = PatternValues.Solid, BackgroundColor = new BackgroundColor() { } }), // Don't remove, it excel default 
                new Fill(new PatternFill(new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "E8E8E8" } }) { PatternType = PatternValues.Solid }) // start from FillId = 2
            );
            ss.Append(fills);

            Borders borders = new Borders(
                new Border(new LeftBorder(), new RightBorder(), new TopBorder(), new BottomBorder(), new DiagonalBorder()),
                new Border(
                    new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new DiagonalBorder()),
                new Border(
                    new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new DiagonalBorder())
            );

            ss.Append(borders);
            CellFormats csfs = new CellFormats(
                new CellFormat(new Alignment() { WrapText = false }) { FontId = 0, FillId = 0, BorderId = 1 },
                new CellFormat(new Alignment() { WrapText = false, Horizontal = HorizontalAlignmentValues.Center }) { FontId = 1, FillId = 2, BorderId = 1, Alignment = GenerateAlignment(HorizontalAlignmentValues.Center), ApplyFont = true, ApplyFill = true, ApplyBorder = true, ApplyAlignment = true },
                new CellFormat(new Alignment() { WrapText = false }) { FontId = 0, FillId = 0, BorderId = 2, ApplyFont = true, ApplyBorder = true }, // table text cell
                new CellFormat(new Alignment() { WrapText = false }) { FontId = 0, FillId = 0, BorderId = 2, NumberFormatId = 37, ApplyNumberFormat = true, ApplyFont = true, ApplyBorder = true }, // table number cell
                new CellFormat(new Alignment() { WrapText = false }) { FontId = 0, FillId = 0, BorderId = 2, NumberFormatId = 14, ApplyNumberFormat = true, ApplyFont = true, ApplyFill = true, ApplyBorder = true }, // table date cell
                new CellFormat(new Alignment() { WrapText = false }) { FontId = 0, FillId = 0, BorderId = 2, NumberFormatId = 0, ApplyNumberFormat = true, ApplyFont = true, ApplyFill = true, ApplyBorder = true }, // table date cell
                new CellFormat(new Alignment() { WrapText = false, Horizontal = HorizontalAlignmentValues.Center }) { FontId = 2, FillId = 0, BorderId = 0, Alignment = GenerateAlignment(HorizontalAlignmentValues.Center), ApplyFont = true, ApplyBorder = true, ApplyAlignment = true }, // template title
                new CellFormat(new Alignment() { WrapText = false, Horizontal = HorizontalAlignmentValues.Center }) { FontId = 0, FillId = 0, BorderId = 0, Alignment = GenerateAlignment(HorizontalAlignmentValues.Center), ApplyFont = true, ApplyBorder = true, ApplyAlignment = true }, // template title not bold
                new CellFormat(new Alignment() { WrapText = true, Horizontal = HorizontalAlignmentValues.Left }) { FontId = 0, FillId = 0, BorderId = 2, Alignment = GenerateAlignment(HorizontalAlignmentValues.Left, true), ApplyFont = true, ApplyBorder = true, ApplyAlignment = true }, // template title Alignment Right
                new CellFormat(new Alignment() { WrapText = false, Horizontal = HorizontalAlignmentValues.Right }) { FontId = 0, FillId = 0, BorderId = 0, Alignment = GenerateAlignment(HorizontalAlignmentValues.Right), ApplyFont = true, ApplyBorder = true, ApplyAlignment = true }, // template title Alignment Right
                new CellFormat(new Alignment() { WrapText = false, Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }) { FontId = 0, FillId = 0, BorderId = 2, Alignment = GenerateAlignment(HorizontalAlignmentValues.Center), ApplyFont = true, ApplyBorder = true, ApplyAlignment = true }, // Border Alignment center
                new CellFormat(new Alignment() { WrapText = false, Horizontal = HorizontalAlignmentValues.Left }) { FontId = 2, FillId = 0, BorderId = 0, Alignment = GenerateAlignment(HorizontalAlignmentValues.Left), ApplyFont = true, ApplyBorder = true, ApplyAlignment = true } // template title
            );

            ss.Append(csfs);

            return ss;
        }

        private static Alignment GenerateAlignment(HorizontalAlignmentValues values, bool wrapText = false)
        {
            return new Alignment()
            {
                Horizontal = values,
                Vertical = VerticalAlignmentValues.Center,
                WrapText = wrapText
            };
        }

        private static string GetCellReference(int columnIndex, int rowIndex)
        {
            string columnName = GetExcelColumnName(columnIndex);
            return $"{columnName}{rowIndex}";
        }

        private static string GetExcelColumnName(int columnIndex)
        {
            int dividend = columnIndex;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (dividend - modulo) / 26;
            }

            return columnName;
        }
    }
}