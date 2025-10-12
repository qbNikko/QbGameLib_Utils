using System.Linq;
using System.Text;
using UnityEngine;
#if _NEWTONSOFT_JSON_ENABLE_
using Newtonsoft.Json;
#endif

namespace QbGameLib_Utils.Security
{
    public class CryptoUtils
    {
        
        public static T Decrypt<T>(byte[] key, byte[] iv, byte[] data) where T : class
        {
            byte[] bytes = Decrypt(key, iv, data);
            
#if _NEWTONSOFT_JSON_ENABLE_
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes));
#else
            return JsonUtility.FromJson<T>(Encoding.UTF8.GetString(bytes));
#endif
        }

        public static byte[] Decrypt(byte[] key, byte[] iv, byte[] data)
        {
            using (Crypto crypto = new Crypto(key, iv))
            {
                return crypto.Decrypt(data);
            }
        }

        public static byte[] Encrypt(byte[] key, byte[] iv, object data)
        {
#if _NEWTONSOFT_JSON_ENABLE_
            return Encrypt(key, iv, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)));
#else
            return Encrypt(key, iv, Encoding.UTF8.GetBytes(JsonUtility.ToJson(data)));
#endif
        }

        public static byte[] Encrypt(byte[] key, byte[] iv, byte[] data)
        {
            using (Crypto crypto = new Crypto(key, iv))
            {
                return crypto.Encrypt(data);
            }
        }

        public static T Decrypt<T>(int len, byte[] data) where T : class
        {
            byte[] bytes = Decrypt(len, data);
#if _NEWTONSOFT_JSON_ENABLE_ 
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes));
#else
            return JsonUtility.FromJson<T>(Encoding.UTF8.GetString(bytes));
#endif
        }

        public static byte[] Decrypt(int len, byte[] data)
        {
            byte[] key = data.Take(len).ToArray();
            byte[] iv = data.Skip(len).Take(len).ToArray();
            using (Crypto crypto = new Crypto(key, iv))
            {
                return crypto.Decrypt(data.Skip(len * 2).ToArray());
            }
        }

        public static byte[] Encrypt(int len, object data)
        {
#if _NEWTONSOFT_JSON_ENABLE_   
            return Encrypt(len, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)));
#else  
            return Encrypt(len, Encoding.UTF8.GetBytes(JsonUtility.ToJson(data)));
#endif
        }

        public static byte[] Encrypt(int len, byte[] data)
        {
            byte[] key = new byte[len];
            byte[] iv = new byte[len];
            Crypto.Gen(ref key);
            Crypto.Gen(ref iv);
            using (Crypto crypto = new Crypto(key, iv))
            {
                byte[] result = crypto.Encrypt(data);
                byte[] all = new byte[len * 2 + result.Length];
                int index = 0;
                foreach (byte b in key)
                {
                    all[index] = b;
                    index++;
                }

                foreach (byte b in iv)
                {
                    all[index] = b;
                    index++;
                }

                foreach (byte b in result)
                {
                    all[index] = b;
                    index++;
                }

                return all;
            }
        }
    }
}