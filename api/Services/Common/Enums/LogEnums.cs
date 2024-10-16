namespace Services.Common.Enums
{
    public class LogActionFeature
    {
        public const string LogAction = "LogAction";
    }
    public class LogActionMethod
    {
        public const string Read = "GET";
        public const string Create = "POST";
        public const string Update = "PUT";
        public const string Delete = "DELETE";
    }

    public class StrLogActionMethod
    {
        public const string Read = "truy cập";
        public const string ReadOne = "lấy thông tin";
        public const string Create = "thêm mới";
        public const string Update = "cập nhật";
        public const string Delete = "xóa";
    }
}