using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Util
{
    public static class Criptografia
    {
        public static String CriptografaMd5(String senha)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(senha));

            String SenhaCriptografada = BitConverter.ToString(s).Replace("-", "").ToLower();

            return SenhaCriptografada;
        }
    }
}
