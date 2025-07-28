using Ledgerly.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Udemy.Data;

namespace Ledgerly.Helpers
{
    public static class GlobalFunction
    {
        public static DateTime GetCurrentDateTime()
        {
            var now = DateTime.Now;
            var local = TimeZoneInfo.Local;
            var destinationTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            return TimeZoneInfo.ConvertTime(now, local, destinationTimeZone);
        }

        public static object RenderErrorMessageFromState(ModelStateDictionary modelState)
        {
            return new
            {
                error = "Info(s):\n" + string.Join("\n- ", modelState.Values
                                      .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage))
            };
        }

        //public static async Task<bool> RecordAuditLog(string userId, string controllerName, string methodName, int transactionId, string transactionKeyValue, string description, LedgerlyDbContext db)
        //{
        //    try
        //    {
        //        var log = new AuditLog
        //        {
        //            ControllerName = controllerName,
        //            MethodName = methodName,
        //            TransactionId = transactionId,
        //            TransactionKeyValue = transactionKeyValue,
        //            Description = description,
        //            //CreatedBy = GetCurrentUserId(),
        //            CreatedBy = userId,
        //            CreatedDate = GetCurrentDateTime()
        //        };
        //        await db.AuditLog.AddAsync(log);
        //        await db.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        //public static bool CheckInvalidEod(DateTime transactionDate, LedgerlyDbContext db)
        //{
        //    var lastEod = GetLastEod(db);
        //    var time2 = new DateTime();
        //    if (lastEod == time2)
        //    {
        //        return false;
        //    }
        //    return transactionDate <= lastEod;
        //}

        //public static DateTime GetLastEod(LedgerlyDbContext db)
        //{
        //    var day = db.EndOfDay.OrderByDescending(p => p.TransactionDate).FirstOrDefault(x => x.GlobalParam.KeyName != "Reject");
        //    return day != null ? day.TransactionDate : GlobalVariable.MinDate;
        //}

        //public static async Task<IdentityUser> GetUserById(string userId, LedgerlyDbContext db)
        //{
        //    var user = await db.Users.FindAsync(userId);
        //    return user;
        //}

        //public static async Task<IdentityUser> GetUserByName(string userName, LedgerlyDbContext db)
        //{
        //    var user = await db.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == userName.ToLower());
        //    return user;
        //}
    }
}