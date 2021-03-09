using ApnaAahar.Repository.Models;
using ApnaAahar.Services;
using ApnaAaharAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ApnaAaharAPI.ExternalApiServices
{
    public class FarmerDetailsSendingAndGenerating
    {
        private readonly IFarmerServices _farmerServices;

        public FarmerDetailsSendingAndGenerating(IFarmerServices farmerServices)
        {
            _farmerServices = farmerServices;
        }

        /// <summary>
        /// Sends Email to both buyer and farmers
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> SendResponseToEmailAsync(BuyerResponse user)
        {
            try
            {      
                Users farmer= await _farmerServices.GetFarmersDetailsByUserId(user.UserId);
                string message = "";
                string messageToFarmer = "";
                if (user.isAccepted)
                {
                    message = "<h3>Dear User,</h3><br />" +
                       " Below are the details of the farmer you tried contacting:  <br />" + "Farmer Email:"
                        + farmer.Email + "<br/> " + farmer.PhoneNumber.ToString() + "<br/> " + farmer.Location+ "<br/> " +
                        "Please contact your farmer as soon as possible.<br /><br />" +
                           "<h4>Sincerely,</h4><br/>" +
                            "<h4>Apna Aahar Service Team</h4>";
                    messageToFarmer= "<h3>Dear User,</h3><br />" +
                       " Below are the details of the buyer whose request has been accepted by you:  <br />" + "Buyer Email: "
                        + user.Email + "<br/> " +"Buyer Contact Details: "+ "<br/>" + user.PhoneNumber+
                        "You will be contacted soon.<br /><br />" +
                           "<h4>Sincerely,</h4><br/>" +
                            "<h4>Apna Aahar Service Team</h4>";
                    sendEmail(message, user.Email);
                    sendEmail(messageToFarmer, farmer.Email);

                    
                }
                else
                {
                    message = "<h3>Dear User,</h3><br />" +
                       " Due to some unfortunate reason, our farmer had to decline your request. <br/> " +
                        "Please try contacting another farmer.<br /><br />" +
                           "<h4>Sincerely,</h4><br/>" +
                            "<h4>Apna Aahar Service Team</h4>";
                    messageToFarmer = "<h3>Dear User,</h3><br />" +
                      "You have declined the request successfully. <br />" + "Requested By: "
                       + user.Email + "<br/> "  +
                          "<h4>Sincerely,</h4><br/>" +
                           "<h4>Apna Aahar Service Team</h4>";
                    sendEmail(message, user.Email);
                    sendEmail(messageToFarmer, farmer.Email);
                }
                _farmerServices.DeleteBuyerRequest(user.ContactRequestId);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void sendEmail(string message, string email)
        {
            MailMessage mailMessage = new MailMessage("apnaaaharorchard@gmail.com", email);
            mailMessage.Body = message;
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = "Apna Aahar: Response Recieved.";
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "apnaaaharorchard@gmail.com",
                Password = "Password@123"
            };
            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);

            

        }
        

    }
}
