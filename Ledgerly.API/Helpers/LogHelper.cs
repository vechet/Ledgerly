using Ledgerly.Helpers;
using NLog;
using System.Text.Json;

namespace Ledgerly.API.Helpers
{
    public class LogHelper
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public static void Info(string serivceName, string methodName, object req, int code, string msg)
        {
            _logger.Info($"{serivceName}/{methodName}, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{code}', ErrorMessage:'{msg}'");
        }

        public static void Info(string serivceName, string methodName, object req, ApiResponseStatus status)
        {
            _logger.Info($"{serivceName}/{methodName}, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{status.Value()}', ErrorMessage:'{status.Description()}'");
        }

    }
}
