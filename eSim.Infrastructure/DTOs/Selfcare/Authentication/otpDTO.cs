using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Infrastructure.DTOs.Selfcare.Authentication
{
    public  class otpDTO
    {
        public string Otp1 { get; set; }
        public string Otp2 { get; set; }
        public string Otp3 { get; set; }
        public string Otp4 { get; set; }
        public string Otp5 { get; set; }
        public string Otp6 { get; set; }

        public string FullOtp => $"{Otp1}{Otp2}{Otp3}{Otp4}{Otp5}{Otp6}";
    }
}
