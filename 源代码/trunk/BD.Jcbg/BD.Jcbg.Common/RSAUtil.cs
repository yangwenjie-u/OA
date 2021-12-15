using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace BD.Jcbg.Common
{
    public class RSAUtil
    {
        private static void CreateRSAKey()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            RSAParameters keys = rsa.ExportParameters(true);
            String pkxml = "<root>\n<Modulus>" + ToHexString(keys.Modulus) + "</Modulus>";
            pkxml += "\n<Exponent>" + ToHexString(keys.Exponent) + "</Exponent>\n</root>";
            String psxml = "<root>\n<Modulus>" + ToHexString(keys.Modulus) + "</Modulus>";
            psxml += "\n<Exponent>" + ToHexString(keys.Exponent) + "</Exponent>";
            psxml += "\n<D>" + ToHexString(keys.D) + "</D>";
            psxml += "\n<DP>" + ToHexString(keys.DP) + "</DP>";
            psxml += "\n<P>" + ToHexString(keys.P) + "</P>";
            psxml += "\n<Q>" + ToHexString(keys.Q) + "</Q>";
            psxml += "\n<DQ>" + ToHexString(keys.DQ) + "</DQ>";
            psxml += "\n<InverseQ>" + ToHexString(keys.InverseQ) + "</InverseQ>\n</root>";

            SaveToFile("publickey.xml", pkxml);
            SaveToFile("privatekey.xml", psxml);

        }
        private static RSACryptoServiceProvider CreateRSADEEncryptProvider(String privateKeyFile)
        {
            RSAParameters parameters1;
            parameters1 = new RSAParameters();
            //StreamReader reader1 = new StreamReader(privateKeyFile);
            XmlDocument document1 = new XmlDocument();
            //document1.LoadXml(reader1.ReadToEnd());
            document1.LoadXml("<root><Modulus>C58B3C310D39E68CD95E93DA07570D9F06E4E4670BEF59D4005C385F407D8D62892EA7A8C73CC7B4C7E88C1F25365247CB9E0C5F1469A035ADF546AB7874440DD42C64F88207863283A9D0C670D4E0D20621858427BC3567BE422E99D4417F27F629BFCC5256F644C46B9B0BEB471D25BC8F38988F325D9B420194F0C15FF0ED</Modulus><Exponent>010001</Exponent><D>B0F046C8B4CD10D945F5B5BECB6648F81107C2211E8A93CE154884E1A47510C79A926470038D485F7FD3A6765B316DBF8AFD149DB13DEED745EA75132F400C9737650F3276E6BAC02E0FE7234AF0CFDD05B32DA2F597B2F26D4D5619B57E2998907821E8C2D088CF91583662A064A9F81AA8936B4419AA39A8BA90D33AAE8ECD</D><DP>BCBD6B50F617588C7C604C5B7193148A88E2468D9FA48A525839CC1A1ABB54D839A399716D966D48485FB9C61F10B70103D925C252DEACC8C06022E39E9216A9</DP><P>E657F22FB793E9363C82658A694A9B8D7F7FA01EFF57C89498D681F43F7CBA483008A2431E2F75050AC622D90212ADA650409330B95ABF4BE65665CEC563ED4B</P><Q>DB8C08CB6CAB861A6E697F45D76FD8FDCCBC67D7E242420CFC741FDA760724F4507A33B30EE9EA38433F8D5AEDE20C78BC1B93D5E290580205EEE33509224FA7</Q><DQ>BF391727E7BCF60EEB8063E47722EA4315223CE8622007DBBBBF81470C6A689B8BC50466BF64AA26DB1BED88F78D5E0383041A1DBF3AAE0D0511EF3FF076BC1D</DQ><InverseQ>325D26A0A0B8031210BEC4C320BFD48B182AFAE85BE80A422C10499E1421AA92D6F9ED24C8FA543B62E994C526C30A265D9330143A0CAC69012BE7A498431DBE</InverseQ></root>");
            XmlElement element1 = (XmlElement)document1.SelectSingleNode("root");
            parameters1.Modulus = ReadChild(element1, "Modulus");
            parameters1.Exponent = ReadChild(element1, "Exponent");
            parameters1.D = ReadChild(element1, "D");
            parameters1.DP = ReadChild(element1, "DP");
            parameters1.DQ = ReadChild(element1, "DQ");
            parameters1.P = ReadChild(element1, "P");
            parameters1.Q = ReadChild(element1, "Q");
            parameters1.InverseQ = ReadChild(element1, "InverseQ");
            CspParameters parameters2 = new CspParameters();
            parameters2.Flags = CspProviderFlags.UseMachineKeyStore;
            RSACryptoServiceProvider provider1 = new RSACryptoServiceProvider();
            provider1.ImportParameters(parameters1);
            return provider1;
        }
        private static RSACryptoServiceProvider CreateRSAEncryptProvider(string Modulus, string Exponent)
        {
            RSAParameters parameters1;
            parameters1 = new RSAParameters();
            if (String.IsNullOrEmpty(Modulus))
            {
                parameters1.Modulus = hexToBytes("C58B3C310D39E68CD95E93DA07570D9F06E4E4670BEF59D4005C385F407D8D62892EA7A8C73CC7B4C7E88C1F25365247CB9E0C5F1469A035ADF546AB7874440DD42C64F88207863283A9D0C670D4E0D20621858427BC3567BE422E99D4417F27F629BFCC5256F644C46B9B0BEB471D25BC8F38988F325D9B420194F0C15FF0ED");
            }
            else
            {
                parameters1.Modulus = hexToBytes(Modulus);
            }
            if (String.IsNullOrEmpty(Exponent))
            {
                parameters1.Exponent = hexToBytes("010001");
            }
            else
            {
                parameters1.Exponent = hexToBytes(Exponent);
            }
            CspParameters parameters2 = new CspParameters();
            parameters2.Flags = CspProviderFlags.UseDefaultKeyContainer;
            RSACryptoServiceProvider provider1 = new RSACryptoServiceProvider();
            provider1.ImportParameters(parameters1);
            return provider1;
        }

        private static byte[] ReadChild(XmlElement parent, string name)
        {
            XmlElement element1 = (XmlElement)parent.SelectSingleNode(name);
            return hexToBytes(element1.InnerText);
        }

        private static string ToHexString(byte[] bytes) // 0xae00cf => "AE00CF "
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }
                hexString = strB.ToString();
            }
            return hexString;
        }
        private static byte[] hexToBytes(String src)
        {
            int l = src.Length / 2;
            String str;
            byte[] ret = new byte[l];

            for (int i = 0; i < l; i++)
            {
                str = src.Substring(i * 2, 2);
                ret[i] = Convert.ToByte(str, 16);
            }
            return ret;
        }

        private static void SaveToFile(String filename, String data)
        {
            System.IO.StreamWriter sw = System.IO.File.CreateText(AppDomain.CurrentDomain.BaseDirectory + filename);
            sw.WriteLine(data);
            sw.Close();
        }

        public static string EnCrypt(string str)
        {
            RSACryptoServiceProvider rsaencrype = CreateRSAEncryptProvider("", "");

            String text = str;

            int size = rsaencrype.KeySize;

            byte[] data = new UTF8Encoding().GetBytes(text);

            if (data.Length <= size / 8 - 11)
            {
                byte[] endata = rsaencrype.Encrypt(data, false);

                return ToHexString(endata);
            }
            else
            {
                var result = "";
                var maxsize = size / 8 - 11;
                for (int i = 0; i < (data.Length + maxsize - 1) / maxsize; i++)
                {
                    if (data.Length >= (i * maxsize + maxsize))
                    {
                        byte[] myByte = new byte[maxsize];
                        for (int j = 0; j < maxsize; j++)
                        {
                            myByte[j] = data[i * maxsize + j];
                        }
                        result += ToHexString(rsaencrype.Encrypt(myByte, false));
                    }
                    else
                    {
                        byte[] myByte = new byte[data.Length - i * maxsize];
                        for (int j = 0; j < data.Length - i * maxsize; j++)
                        {
                            myByte[j] = data[i * maxsize + j];
                        }
                        result += ToHexString(rsaencrype.Encrypt(myByte, false));
                    }
                }
                return result;
            }
        }

        public static string DoEncrypt(string hexstr)
        {
            RSACryptoServiceProvider rsadeencrypt = CreateRSADEEncryptProvider("privatekey.xml");

            byte[] miwen = hexToBytes(hexstr);

            int size = rsadeencrypt.KeySize;

            int maxsize = size / 8;

            if (miwen.Length <= maxsize)
            {

                byte[] dedata = rsadeencrypt.Decrypt(miwen, false);

                return System.Text.UTF8Encoding.UTF8.GetString(dedata);
            }
            else
            {
                var result = "";
                for (int i = 0; i < (miwen.Length + maxsize - 1) / maxsize; i++)
                {
                    if (miwen.Length >= (i * maxsize + maxsize))
                    {
                        byte[] myByte = new byte[maxsize];
                        for (int j = 0; j < maxsize; j++)
                        {
                            myByte[j] = miwen[i * maxsize + j];
                        }
                        byte[] dedata = rsadeencrypt.Decrypt(myByte, false);

                        result += System.Text.UTF8Encoding.UTF8.GetString(dedata);
                    }
                    else
                    {
                        byte[] myByte = new byte[miwen.Length - i * maxsize];
                        for (int j = 0; j < miwen.Length - i * maxsize; j++)
                        {
                            myByte[j] = miwen[i * maxsize + j];
                        }
                        byte[] dedata = rsadeencrypt.Decrypt(myByte, false);

                        result += System.Text.UTF8Encoding.UTF8.GetString(dedata);
                    }
                }
                return result;
            }
        }

        public static bool CheckToken(string token, out string UserName, int yxsj = 5)
        {
            UserName = "";
            try
            {
                var s = RSAUtil.DoEncrypt(token);
                var flag = CheckMd5Hash(s);
                var sp = s.Split('|');
                DateTime d = DateTime.Parse(sp[1]);
                UserName = sp[0];
                if ((DateTime.Now - d).TotalMinutes >= 5)//令牌有效时间5分钟
                {
                    return false;
                }
                return flag;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 校验MD5是否正确
        /// </summary>
        /// <param name="inpurString"></param>
        /// <returns></returns>
        public static bool CheckMd5Hash(string inpurString)
        {
            try
            {
                var str = inpurString.Split('|');
                if (str[2] == StringToMD5Hash(str[0] + "&" + str[1] + "&" + "~!@#$%^&*()_+?"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string StringToMD5Hash(string inputString, bool isUtf8 = true)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encryptedBytes = null;
            if (isUtf8)
            {
                encryptedBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            }
            else
            {
                encryptedBytes = md5.ComputeHash(Encoding.Default.GetBytes(inputString));
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                sb.AppendFormat("{0:x2}", encryptedBytes[i]);
            }
            return sb.ToString();
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetMd5Hash(string UserName, string time)
        {
            return StringToMD5Hash(UserName + "&" + time + "&" + "~!@#$%^&*()_+?");
        }

        /// <summary>
        /// 获取令牌
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public static string GetToken(string UserName, string ID, string UserPwd)
        {
            var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(UserName.ToLower())) + "|" + time + "|" + GetMd5Hash(UserName, time) + "|" + ID + "|" + Convert.ToBase64String(Encoding.UTF8.GetBytes(UserPwd));
            token = RSAUtil.EnCrypt(token);
            return token;
        }
    }
}
