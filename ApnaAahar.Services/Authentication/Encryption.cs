using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ApnaAahar.Services.Authentication
{
   public class Encryption
    {

        /// <summary>
        /// Encrypting the password entered by user to match with database
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string EncryptingPassword(string password)
        {

            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
    }
}
