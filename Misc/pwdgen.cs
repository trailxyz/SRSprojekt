using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SRSprojekt.Misc
{
    public class pwdgen
    {
        public static string Hash(string password)
        {
            var sBytes = new UTF8Encoding().GetBytes(password);
            byte[] hbytes;
            using (var alg = new System.Security.Cryptography.SHA256CryptoServiceProvider())
            {
                hbytes = alg.ComputeHash(sBytes);
            }
            return Convert.ToBase64String(hbytes);
        }
    }
}