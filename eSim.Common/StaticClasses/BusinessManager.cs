using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using eSim.EF.Entities;
using Microsoft.AspNetCore.Hosting.Server;
using static System.Net.Mime.MediaTypeNames;

namespace eSim.Common.StaticClasses
{
    public static class BusinessManager
    {
        //middleware base url for 
        //public static string AuthToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJjbGllbnQtaWQiOiJlMzYwOTMzNy1hMTg4LTQzMWItYjg1NC02NjUwOGVkOTg2MzUiLCJleHAiOjE3NDc4NDQ5OTgsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTA1OCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTA1OCJ9.VGnAzeKANa33Es8Tes8kqNo96chTj6sh79799WJYSTU";
        public static string BundleError = "You can either send a single bundle OR multiple bundles, not both.";
        public static string InvalidPayload = "Invalid payload. Provide either single bundle fields or a 'bundles' list.";
        public static string BaseURL = "https://api.esim-go.com/v2.4";

        public static string Exception = "Exception occured";
        public static string NotFound = "Not Found";
        public static string InternalServerError = "Internal Server Error";
        public static string IncompatibleBundle = "Incompatible bundle profile combination";
        public static string DataDumpingIssue = "Error occured while dumping data";
        public static string NoOrderPresent = "No order found";
        public static string QRFailed = "Failed to get QR code";
        public static string ImageMediaContentType = "image/png";
        public static string QRCode = "qr-code.png";
        public static string InvalidIccid = "Invalid Iccid";

        public static string MdwBaseURL = "https://localhost:7264/";
        public static string Countries = "Countries";
        public static string MidlewareLogin = "auth/login";
        public static string CheckEmail = "Subscriber/check-email";
        public static string Subscriber = "Subscriber";
        public static string forgotPass = "Subscriber/forgot-password";
        public static string OTP = "Subscriber/verify-otp";
        public static string resetPass = "Subscriber/reset-password";
        public static string SubscriberId = "SubscriberId";
        public static string Ticekt = "Ticket/list";
        public static string TicketType = "Ticket/Types";
        public static string TicketDetail = "ticket/detail";
        public static string Ticketcomment = "ticket/comment";
        public static string BundelRegion = "bundle/bundles";
        public static string Bundeldetail = "bundle/";
        public static string EsimList = "esims/list";
        public static string EsimHistory = "esims/history";
        public static string EsimDetails = "esims/details";
        public static string AppleInstallUrl = "appleInstallUrl";
        public static string SubscriberInventory = "inventory/subscriber";
        public static string ApplyNewBundle = "esims/apply-bundle-new-esim";
        public static string QRPath(string input) => $"esims/{input}/qr";
        //public static string SubscriberInventory = "inventory/subscriber";



        public static string OTPError = "Invalid User Id";
        public static string DefaultPassword = "Dev@123";
        public static string LockedOut = "User is locked out";
        public static string UserNotFound { get; } = "Email not found. Please check the entered email address and try again.";
        public static string InvalidUser = "User not found";

        public static string ClientCreated = "Client created successfully";
        public static string UserCreated = "User created successfully";
        public static string ClientNotFound = "Client not found";
        public static string ClientUpdated = "Client updated successfully";
        public static string ClientStatus = "Client Is Active status changed successfully";
        public static string UpdateClientSettings = "These are default client settings";
        public static string ClientSettingsNotFound = "Client settings not found";
        public static string ClientSettingsUpdated = "Client settings updated successfully";
        public static string PasswordEmailReceieved = "Verify your email please";
        public static string EmailReceived = "Please confirm your email and login into the system";

        //Email
        public static string EmailSubject = "Forgot Password";
        public static string EmailNotSent = "Error occured, failed to send email";
        public static string VerificationFailed = "Verification Failed";
        public static string AlreadyVerified = "Already Verified";
        public static string EmailNotVerified = "Email not confirmed. Please verify your email before logging in.";
        public static string EmailValidationError { get; } = "Email cannot be null or empty";
        
        public static string Verification_EmailSubject = "Client Verification";
        public static string Password_EmailSubject = "Client Password Available";
        public static string EmailTo = "ayeshbutt012@gmail.com";

        public static string LoginFailed = "Enter correct email or password";
        public static string EmailSent = "Email sent successfully";
        public static string EmailBody = "Use this OTP to set your new password ";
        public static string EmailNotReceived = "Error occurred while sending email. Try again !!!";
        public static string OTPDetailsNotAdded = "Error occurred while adding OTP details. Try again !!!";
        public static string OTPFailed = "OTP Failed";
        public static string LinkExpired = "Link Expired. Get a new link!!!";
        public static string PasswordSuccessfullyReset = "Password Successfully Reset";

        public static string BundleFetched = "Bundle fetched successfully.";
        public static string BundleNotFound = "No bundle found.";
        public static string ReagionNotFound = "No bundles found for the specified region.";
        public static string RegionBundelFetched = "Bundles fetched successfully.";
        //Auth
        public static string InvalidLOgin = "Invalid-credentials";
        //Esim
        public static string EsimNotFound = "No eSIM data found.";
        public static string EmptyEsimList = "No eSIM available";
        public static string EsimDataFetched = "eSIM data fetched successfully.";
        public static string Missingreference = "Missing reference";
        public static string EsimInstallDetail = "Esim install detail fetched successfully ";
        public static string EsimNotCompatible = "eSIM is not compatible with the specified bundle.";
        public static string EsimCompatible = "eSIM is compatible with the specified bundle.";
        


        //Subscriber
        public static string EmailRequired = "Email is required.";
        public static string EmailExist = "Email already exists.";
        public static string EmailAvailable = "Email is available.";
        public static string InvalidMerchant = "Invalid Merchant Details";
        public static string FirstName = "First name must contain English letters only.";
        public static string LastName = "Last name must contain English letters only.";

        public static string Invalidemailformat = "Invalid email format";
        public static string SubscriberSubject = "Welcome to eSim";
        public static string OTPVerified = "OTP verified successfully.";
        public static string OTPSendSuccessfully ="OTP has been created successfully";
        public static string userNotFound = "User not found.";
        public static string IncorrectOldPassword = "Old password is incorrect.";
        public static string PasswordNotMatched = "New password and Confirm password do not match.";
        public static string NoFileProvided = "No image file provided.";
        public static string FileAllowed = "Only image files allowed (.jpg, .png, .gif).";
        public static string FileSize = "File size limit exceeded (5 MB max).";
        public static string Subscribernotfound = "Subscriber not found.";
        public static string ImageUploaded = "Image uploaded and saved to database.";
        public static string Noimagefileprovided = "No image file provided.";
        public static string validemailaddress = "Please enter a valid/full email address.";


        //Ticket

        public static string TicketCreated = "Ticket Created Successfully";
        public static string Ticketnotfound = "Ticket not found.";
        public static string Attachmentuploaded = "Attachment uploaded successfully.";
        public static string Commentadded = "Comment added successfully";
        public static string EsimInstallDetailNotFound = "No esim install detail data found.";
        public static string ListofEsimBundleFetched = "list of Bundles Applied to eSiM fetched successfully.";
        public static string InvalidTicketType = "Invalid Ticket Type selected";
        public static string TicketdetailRetrieved = "Ticket detail retrieved successfully.";









        public static string GetSubscriberBody(string firstName)
        {
            return $"Hi {firstName},\n\nYou are successfully signed up on our platform.\n\nThanks,\neSim Team";
        }
        public static string EmailSendSuccessfully = "User created and email sent successfully.";
        public static string EmailNotSend = "User created, but email sending failed.";

        public static string EmailNotFound = "Email not found.";
        public static string OTPSubject = "OTP";
        public static string OTPBody = $"Your OTP is";
        public static string GetOTPBody(string otp)
        {
            return $"Your OTP is: {otp}.";
        }

        public static string SubscriberNotFound  = "Subscriber not found";
        public static string Subscriberdetail  = "Subscriber detail fetched successfully.";

        public static string SubscriberCreatedSuccessfully = "Subscriber Created successfully";
        public static string Subscriberupdated = "Subscriber updated successfully";
        public static string Error = "An error occurred: ";
        public static string RequiredOTP = "OTP is required.";
        public static string InvalidOTP = "Invalid or expired OTP.";

        public static string PasswordChangedSubject = "Password Changed";
        public static string PasswordChangedBody = "Your password has been changed successfully.";
        public static string PasswordConfirmationEmail = "Password changed but failed to send confirmation email.";




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
        public static string Verification_EmailBody(string userId, string baseUrl,string token)
        {
            return $"Hello, please verify your login by clicking the link below:<br/>\r\n<a href=\"{baseUrl}/Client/EmailConfirmation?userId={userId}&token={token}\">Click here to verify your OTP</a>";
        }
        public static string DateFilterQueryBuilder(DateTime? from, DateTime? to)
        {

            DateTime start = new DateTime(from.Value.Year, from.Value.Month, from.Value.Day, 0, 0, 0, DateTimeKind.Utc);// March 1, 2024, 00:00

            DateTime end = new DateTime(to.Value.Year, to.Value.Month, to.Value.Day, 23, 59, 59, DateTimeKind.Utc);// March 31, 2024, 23:59
           
            // Format to ISO 8601 with milliseconds
            string startDate = $"gte:{start:yyyy-MM-ddTHH:mm:ss.fffZ}";
            string endDate = $"lte:{end:yyyy-MM-ddTHH:mm:ss.fffZ}";

            return $"createdAt={startDate}&createdAt={endDate}";
        }

    }
    public static class PasswordHasher
    {
        // Configuration parameters (can be adjusted)
        private const int SaltSize = 16; // 128 bits
        private const int HashSize = 32; // 256 bits
        private const int Iterations = 100000; // Adjust based on your performance needs
        public static string HashPassword(string password)
        {
            // Generate a random salt
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Create the hash with the salt
            byte[] hash = CreateHash(password, salt);

            // Combine salt and hash
            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            // Convert to base64 for storage
            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Verifies a password against a stored hash
        /// </summary>
        public static bool VerifyPassword(string password, string storedHash)
        {
            // Extract bytes from stored hash
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            // Get the salt from the stored hash
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // Create hash with extracted salt
            byte[] computedHash = CreateHash(password, salt);

            // Compare computed hash with stored hash
            for (int i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != computedHash[i])
                {
                    return false;
                }
            }
            return true;
        }
        
        private static byte[] CreateHash(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password: password,
                salt: salt,
                iterations: Iterations,
                hashAlgorithm: HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(HashSize);
            }
        }
    }
}


