using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Common.StaticClasses
{
    public static class BusinessManager
    {
        public static string BaseURL = "https://api.esim-go.com/v2.4";
        public static string DefaultPassword = "Dev@123";
        public static string LockedOut = "User is locked out";

        public static string ClientCreated = "Client created successfully";
        public static string ClientNotFound = "Client not found";
        public static string ClientUpdated = "Client updated successfully";
        public static string ClientStatus = "Client Is Active status changed successfully";
        public static string UpdateClientSettings = "These are default client settings";
        public static string ClientSettingsNotFound = "Client settings not found";
        public static string ClientSettingsUpdated = "Client settings updated successfully";

        public static string EmailValidationError { get; } = "Email cannot be null or empty";
        public static string UserNotFound { get; } = "Email not found. Please check the entered email address and try again.";

        public static string EmailSubject = "Forgot Password";
        public static string EmailTo = "ayeshbutt012@gmail.com";

        public static string LoginFailed = "Enter correct email or password.";
        public static string EmailSent = "Email sent successfully";
        public static string EmailBody = "Use this OTP to set your new password ";
        public static string EmailNotReceived = "Error occurred while sending email. Try again !!!";
        public static string OTPDetailsNotAdded = "Error occurred while adding OTP details. Try again !!!";
        public static string OTPFailed = "OTP Failed";
        public static string LinkExpired = "Link Expired. Get a new link!!!";

        public static string PasswordSuccessfullyReset = "Password Successfully Reset";

        public static string GenerateUniqueAlphanumericId(int length)
        {
            if (length < 4 || length > 64)
            {
                throw new ArgumentException("Desired length must be between 4 and 64 characters");
            }

            // Get current timestamp (UTC to avoid timezone issues)
            DateTime now = GetDateTimeNow();
            string timestamp = now.Ticks.ToString(); // Using ticks for highest precision

            // Generate a random component
            string randomComponent = GenerateRandomString(length);

            // Combine components
            string combined = $"{timestamp}_{randomComponent}";

            // Create a hash (using SHA256 for good distribution)
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));

                // Convert hash to base64 and then clean to alphanumeric
                string base64 = Convert.ToBase64String(hashBytes);
                StringBuilder alphanumeric = new StringBuilder();

                foreach (char c in base64)
                {
                    if (char.IsLetterOrDigit(c))
                    {
                        alphanumeric.Append(c);
                    }
                }

                // Trim to desired length
                string result = alphanumeric.ToString();
                return result.Length > length
                    ? result.Substring(0, length)
                    : result.PadRight(length, '0'); // Pad if somehow too short
            }
        }
        private static string GenerateRandomString(int length)
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"; 
            var random = new Random();

            char[] result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = validChars[random.Next(validChars.Length)];
            }
            return new string(result);
        }
    

    public static DateTime GetDateTimeNow()
        {
            return DateTime.UtcNow;
        }


        public static long UnixOffSetTime()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
           
         
        }
        public static string GenerateTRN()
        {
            return $"TRN-{UnixOffSetTime()}";
        }



    }
}
