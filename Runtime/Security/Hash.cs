using System;
using System.IO;
using System.IO.Hashing;
using System.Text;

namespace QbGameLib_Utils.Security
{
    public class Hash
    {
        public string MD5Hex(string data)
        {
            return MD5Hex(Encoding.UTF8.GetBytes(data));
        }
        
        public string MD5Hex(byte[] data)
        {
            String hash = String.Empty;
            foreach (byte b in MD5(data))
            {
                hash+=b.ToString("x2");
            }
            return hash;
        }
        
        public byte[] MD5(string data)
        {
            return MD5(Encoding.UTF8.GetBytes(data));
        }
        
        public byte[] MD5(byte[] data)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                return md5.ComputeHash(data);
            }
        }
        
       
    }
}