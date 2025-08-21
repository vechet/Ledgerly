using System.ComponentModel;

namespace Ledgerly.API.Helpers
{
    public enum ApiResponseStatus
    {
        [Description("Username is already taken.")]
        DuplicateUserName = 215,

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

        [Description("Role does not exist.")]
        RoleDoesNotExist = 415,

        [Description("Cannot update or delete record that already deleted.")]
        AlreadyDeleted = 422,

        [Description("Cannot delete default cateogry")]
        CannotDeleteDefaultCategory = 423,

        [Description("Cannot delete other user cateogry")]
        CannotDeleteOtherUserCategory = 424,

        [Description("Cannot delete this cateogry cause there is any transactions are linked to this category")]
        HasTransactionsLinkedAsync = 425,

        [Description("General Error")]
        GeneralError = 999,

        //Login
        [Description("Wrong Username Or Password")]
        WrongUserNameOrPassword = 200,

        [Description("Wrong Password")]
        WrongPassword = 201,

        [Description("User Not Exist")]
        UserNotExist = 406,

        [Description("Not found")]
        NotFound = 404,

        [Description("Unauthorized or No Permission")]
        Forbidden = 403,

        [Description("This resource has been deleted")]
        Gone = 410,

        [Description("Unauthorized")]
        Unauthorized = 401,

        [Description("Method Not Allow")]
        MethodNotAllow = 405,
    }
}