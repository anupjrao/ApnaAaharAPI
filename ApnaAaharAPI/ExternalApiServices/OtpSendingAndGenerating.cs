using ApnaAahar.Repository.Models;
using ApnaAahar.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace ApnaAaharAPI.ExternalApiServices
{
    public class OtpSendingAndGenerating
    {
        private readonly IUserServices _UserServices;

        public OtpSendingAndGenerating(IUserServices userServices)
        {
            _UserServices = userServices;
        }

        /// <summary>
        /// method to check a user exits or not with give email id or phone no or both and sending Otp accordingly
        /// </summary>
        /// <param name="usersFromUI"></param>
        /// <returns></returns>
        public async Task<Users> SendOtpByPHoneOrEmail(Users usersFromUI, int choice)
        {
            Users user = new Users();
            switch (choice)
            {
                case 0:
                    user = await _UserServices.GetUsersByEmail(usersFromUI);
                    if(user!=null && user.FarmerDetails.Count!=0 && user.FarmerDetails.FirstOrDefault(farmer=>farmer.IsApproved==true)==null)
                    {
                        user = null;
                    }
                    break;
                case 1:
                    user = await _UserServices.GetUserByPhoneNo(usersFromUI);
                    if (user != null && user.FarmerDetails.Count != 0 && user.FarmerDetails.FirstOrDefault(farmer => farmer.IsApproved == true) == null)
                    {
                        user = null;
                    }
                    break;
                case 2:
                    user = await _UserServices.GetUsersByEmail(usersFromUI);
                    if (user != null && user.FarmerDetails.Count != 0 && user.FarmerDetails.FirstOrDefault(farmer => farmer.IsApproved == true) == null)
                    {
                        user = null;
                        break;
                    }
                    if (user.PhoneNumber != usersFromUI.PhoneNumber)
                    {
                        user = null;
                    }
                    break;
                default:
                    break;
            }

            return user;
        }

        /// <summary>
        /// method to generate a random number in between 100000-999999
        /// </summary>
        /// <returns></returns>
        public long GenerateOtp()
        {
            long otp = 0;
            Random randomNumber = new Random();
            otp = randomNumber.Next(100000, 999999);
            return  otp;
        }

        /// <summary>
        /// method to generate otp and send that to user
        /// </summary>
        /// <param name="usersFromUI"></param>
        /// <returns></returns>
        public async Task<long> sendOtp(Users usersFromUI)
        {
            long Otp = 0;
            if (usersFromUI.PhoneNumber.Length == 0 && await this.SendOtpByPHoneOrEmail(usersFromUI, 0) != null)
            {
                Otp = this.GenerateOtp();
                Otp = Otp * this.SendOtpToEmail(Otp, usersFromUI);
            }
            else if (usersFromUI.Email.Length == 0 && await this.SendOtpByPHoneOrEmail(usersFromUI, 1) != null)
            {
                Otp = this.GenerateOtp();
                Otp = Otp * this.SendOtpPhone(Otp, usersFromUI);
            }
            else
            {
                if (usersFromUI.Email.Length != 0 && usersFromUI.PhoneNumber.Length != 0 && await this.SendOtpByPHoneOrEmail(usersFromUI, 2) != null)
                {
                    Otp = this.GenerateOtp();
                    int result = this.SendOtpToEmail(Otp, usersFromUI);
                    result = this.SendOtpPhone(Otp, usersFromUI);
                    Otp = Otp * result;
                }
            }
            return Otp;
        }

        /// <summary>
        /// method to send otp in a gmail account
        /// </summary>
        /// <param name="otp"></param>
        /// <returns></returns>
        
        public int SendOtpToEmail(long otp, Users user)
        {
            try
            {
                string message = "<h3>Dear User,</h3><br />" +
                    otp.ToString()+
                    " is SECRET OTP to reset the passwrod.<br />" +
                    "OTP valid for 1 min. Please do not share OTP with anyone.<br /><br />" +
                       "<h4>Sincerely,</h4><br/>" +
                        "<h4>Apna Aahar Service Team</h4>";

                MailMessage mailMessage = new MailMessage("apnaaaharorchard@gmail.com", user.Email);
                mailMessage.Body = message;
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = "Apna Aahar: Otp for reseting Password";
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = "apnaaaharorchard@gmail.com",
                    Password = "Password@123"
                };
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// method to send otp to Phone
        /// </summary>
        /// <param name="otp"></param>
        /// <returns></returns>
        public int SendOtpPhone(long Otp, Users users)
        {
            string messageText = " is SECRET OTP to reset the password. OTP valid for 1 min. Please do not share OTP with anyone.";
            try
            {
                string accountSid = "AC0d53180df655ab1b8daba6d2c972f7f2";
                string authToken = "9116b682d0940c08f11fa33140e99d04";

                TwilioClient.Init(accountSid, authToken);

                var message = MessageResource.Create(
                    body: Otp.ToString()+messageText,
                    from: new Twilio.Types.PhoneNumber("+14159686771"),
                    to: new Twilio.Types.PhoneNumber("+91"+users.PhoneNumber)
                );
                return 1;
            }
            catch(Exception)
            {
                return 0;
            }
        }
    }
}
