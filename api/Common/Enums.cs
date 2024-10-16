namespace Common
{
    public class Modules
    {
        public static string Core = "Core";
        public static string Log = "Log";
    }

    public class Screen
    {
        public static string Message = "Message";
        public static string LogAction = "LogAction";
        public static string User = "User";
        public static string Carton = "Carton";
        public static string Consignee = "Consignee";
        public static string GroupProduct = "GroupProduct";
        public static string Ingredient = "Ingredient";
        public static string Invoice = "Invoice";
        public static string Packing = "Packing";
        public static string Product = "Product";
        public static string Shipper = "Shipper";
        public static string Warehouse = "Warehouse";
        public static string Package = "Package";
        public static string Auth = "Auth";
        public static string Resource = "Resource";
        public static string ChangePassword = "ChangePassword";
        public static string Login = "Login";
        public static string ForgotPassword = "ForgotPassword";
        public static string Customer = "Customer";
    }

    public class ScreenKey
    {
        public static string COMMON = "Common";
    }

    public class MessageKey
    {
        public static string I_001 = "I_001";
        public static string I_002 = "I_002";
        public static string I_003 = "I_003";

        public static string E_001 = "E_001";
        public static string E_002 = "E_002";
        public static string E_003 = "E_003";
        public static string E_004 = "E_004";
        public static string E_005 = "E_005";
        public static string E_007 = "E_007";
        public static string E_008 = "E_008";
        public static string E_009 = "E_009";
        public static string E_010 = "E_010";

        public static string BE_003 = "BE_003";

        public static string W_001 = "W_001";
        public static string W_002 = "W_002";

        //Success
        public const string S_CREATE = "CreateSuccess";
        public const string S_UPDATE = "UpdateSuccess";
        public const string S_DELETE = "DeleteSuccess";
        public const string S_CHANGE = "ChangeSuccess";
        public const string S_SEND_MAIl = "SendMailSuccess";
        public const string S_CHECK_CODE = "CodeSuccess";
        public const string S_FORGOT = "ForgotSuccess";
        public const string S_IMPORT = "ImportSuccess";

        //Error
        public const string E_CREATE = "CreateError";
        public const string E_UPDATE = "UpdateError";
        public const string E_DELETE = "DeleteError";
        public const string E_CHANGE = "ChangeError";
        public const string E_EXPORT = "ExportError";
        public const string E_SAVE_IMAGE = "SaveImageFail";
        public const string E_GET_CODE = "GetCodeError";
        public const string E_SEND_MAIL = "SendMailError";
        public const string E_CHECK_CODE = "CodeError";
        public const string E_FORGOT = "ForgotError";
        public const string E_LOGIN = "LoginError";

        //NotFound
        public const string NOT_FOUND = "NotFound";
        public const string U_NOT_FOUND = "UserNotFound";
        public const string TEMPLATE_NOT_FOUND = "TemplateNotFound";
        public const string FILE_NOT_NULL = "FileNotNull";
        public const string FILE_NOT_FOUND = "FileNotFound";
        public const string LOGIN_NOT_FOUND = "LoginNotFound";

        //Invalid
        public const string I_DUPLICATE_001 = "DuplicateTrackingNumber";
        public const string I_DUPLICATE_002 = "DuplicateJancode";
        public const string I_DUPLICATE_003 = "DoublicateRow";
        public const string I_DUPLICATE_004 = "DoublicateCode";

        //Warning
        public const string W_DELETE = "DeleteWarning";
        public const string W_DELETE_LIST = "DeleteListWarning";

        //Error File
        public const string E_FILE_INFO = "E_FILE_INFO";
        public const string E_FILE_VALID = "E_FILE_VALID";
        public const string E_FILE_FORMAT = "E_FILE_FORMAT";
        public const string E_FILE_NOT_FOUND = "E_FILE_NOT_FOUND";

    }

    public enum ResponseCode
    {
        Success = 0,
        SystemError = 1,
        NotFound = 2,
        Invalid = 3,
        UnAuthorized = 4
    }

    public class CompanyType
    {
        public const string PLACE = "place";
        public const string BRANCH = "branch";
        public const string CUSTOMER = "customer";
        public const string SUPPLIER = "supplier";
        public const string OUTSOURCER = "outsourcer";
        public const string DESTINATION = "destination";
        public const string TRANSPOST = "transpost";
        public const string MAKER = "maker";
    }

    public class InvoiceExcel
    {
        public const string FILE_NAME = "INVOICE_TEMPLATE.xlsx";
        public const string SHEET_INVOICE = "INVOICE";
        public const string SHEET_PACKINGLIST = "PACKINGLIST";
        public const string SHEET_INGREDIENT = "INGREDIENT";
        public const int COLUMN_START_INDEX = 13;
    }

    public class PackingExcel
    {
        public const string FILE_NAME = "PACKING_TEMPLATE.xlsx";
        public const int COLUMN_START_INDEX = 3;
    }
}