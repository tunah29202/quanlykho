namespace Helpers
{
    

    public enum DataType
    {
        TEXT = 0,
        NUMBER = 1,
        DATE = 2,
        DATETIME = 3,
        MONTH = 4,
        DECIMAL = 5,
    }

    public enum CellAlign
    {
        LEFT = 0,
        CENTER = 1,
        RIGHT = 2,
    }

    public enum VCellStyle : uint
    {
        Normal = 0,
        NormalCenter = 1,
        TableText = 2,
        TableNumber = 3,
        TableDate = 4,
        TableNumberNormal = 5,
        PageTitle = 6,
        AlignCenter = 7,
        AlignLeft = 8,
        AlignRight = 9,
        BorderAlignCenter = 10,
        PageTitleLeft = 11
    }
}
