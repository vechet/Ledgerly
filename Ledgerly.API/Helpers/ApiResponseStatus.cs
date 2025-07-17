using System.ComponentModel;

namespace Ledgerly.Helpers
{
    public enum ApiResponseStatus
    {
        [Description("Internal Error")]
        InternalError = 500,

        [Description("Success")]
        Success = 0,

        [Description("Query success but no records found")]
        QuerySuccessButNoRecordsFound = 210,

        [Description("Invalid Request Parameter")]
        InvalidParameter = 101,

        [Description("Invalid Password Format")]
        InvalidPasswordFormat = 102,

        [Description("Duplicate Name")]
        DuplicateName = 409,

        [Description("Duplicate Barcode")]
        DuplicateBarcode = 410,

        [Description("Invalid Eod")]
        InvalidEod = 411,

        [Description("RecordDetailNotExist")]
        RecordDetailNotExist = 412,

        [Description("Duplicate BaseUm With The Other ")]
        DuplicateBaseUm = 413,

        [Description("Duplicate Email")]
        DuplicateEmail = 414,

        [Description("Already Deleted")]
        AlreadyDeleted = 422,

        [Description("General Error")]
        GeneralError = 999,

        //Login
        [Description("Wrong Username Or Password")]
        WrongUserNameOrPassword = 200,

        [Description("Not found")]
        NotFound = 404,

        [Description("This resource has been deleted")]
        Gone = 410,

        [Description("Unauthorized")]
        Unauthorized = 401,

        [Description("Method Not Allow")]
        MethodNotAllow = 405,
    }
}