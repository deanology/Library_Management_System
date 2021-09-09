using System;
namespace Library_Management_System.Utils
{
    public class ResponseCodes
    {
        public const string Success = "200";
        public const string InvalidRequest = "400";
        public const string UnexpectedError = "500";
        public const string Conflict = "409";
        public const string Forbidden = "403";
        public const string NotFound = "404";
        public const string Unauthorized = "401";
        public const string UpgradeRequired = "426";
        public const string UnavailableForLegalReasons = "451";
    }
}
