using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Common.StaticClasses
{
    public static class BusinessManager
    {
        public static string DefaultPassword = "Dev@123";
        public static string LockedOut = "User is locked out";

        public static string EmailValidationError { get; } = "Email cannot be null or empty";
        public static string UserNotFound { get; } = "Email not found. Please check the entered email address and try again.";

        public static string EmailSubject = "Forgot Password";
        public static string EmailBody = "Use this OTP to set your new password ";
        public static string EmailNotReceived = "Error occurred while sending email. Try again !!!";
        public static string OTPDetailsNotAdded = "Error occurred while adding OTP details. Try again !!!";
        public static string OTPFailed = "OTP Failed";
        public static string LinkExpired = "Link Expired. Get a new link!!!";

        public static string PasswordSuccessfullyReset = "Password Successfully Reset";
    }
}
