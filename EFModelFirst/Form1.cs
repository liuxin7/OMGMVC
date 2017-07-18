using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EFModelFirst
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //  上下文 
            VATPRINT_CBSO_oldEntities VATCon = new VATPRINT_CBSO_oldEntities();
            //  数据实体 对象 
            VAT02_UserType userType = new VAT02_UserType();
            //  返回结果
            int i = 0;
            #region   添加
            //userType.UserTypeName = "地球村密码";
            //VATCon.VAT02_UserType.Add(userType);   //  吧实体的数据传给 上下文 
            //  i = VATCon.SaveChanges();                 //  所做的修改  保存到数据库
            #endregion

            #region   修改
            //userType.UserTypeName = "地球村";
            //userType.UserTypeID = 12;
            //// 把所有 修改状态的实体 生成  update 
            //VATCon.Entry(userType).State = EntityState.Modified;
            //i = VATCon.SaveChanges(); //  所做的修改  保存到数据库
            #endregion
            #region  删除
            //VAT02_UserType tt = VATCon.VAT02_UserType.Find(12);   // 通过 ID  找到到数据
            //// 第一种删除方式      
            //VATCon.Entry(tt).State = EntityState.Deleted;
            //// 第二种删除 方式    
            //VATCon.VAT02_UserType.Remove(tt);

            //VATCon.SaveChanges();
            #endregion
            #region    查询
            //var list = from sm in VATCon.VAT02_UserType
            //           where sm.UserTypeName.Contains("MIDH")
            //           select sm;
            //string str = string.Empty;   // 用于获取数据
            //foreach (var item in list)
            //{
            //    str += item.UserTypeName;
            //}

            #region  好玩的

            //int length = str.Length;  //  获取字符的 从长度
            //int index = 0; //  用于 取 下标
            //while (true)
            //{
            //    string fistStr = str.Substring(index);
            //    index++;
            //    System.Threading.Thread.Sleep(100);    // 等待 一会 
            //    if (index >= length) // 到头时    从新抛
            //    {
            //        index = 0;
            //    }
            //    textBox1.Text = fistStr;
            //}
            #endregion


            #endregion

        }

        /// <summary>
        ///  MD5  加密  因为 MD5 是不可逆的   解密不了    用des 解密
        /// </summary>
        /// <param name="msg">加密的字符串</param>
        /// <returns></returns>
        public static string MD5Create(string msg)
        {
            // 创建一个 md5 
            MD5 md5 = MD5.Create();
            //  采用utf8编码  因为不同编码返回的 byte不同
            byte[] bs = Encoding.UTF8.GetBytes(msg);
            // 计算MD5 值
            byte[] md5Byte = md5.ComputeHash(bs);
            //释放 资源
            md5.Clear();

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < md5Byte.Length; i++)
            {
                sb.Append(md5Byte[i].ToString("x2"));  // 十进制
            }
            return sb.ToString();
        }

        /// <summary>
        /// DES  加密 对称的加密算法
        /// </summary>
        /// <param name="desStr">需要加密的字符</param>
        /// <param name="key">秘钥   8位   </param>
        /// <param name="iV">向量 8位  如果加密的时候用的这个密码   解密的时候  向量不对 是解密不了的</param>
        /// <returns></returns>
        public static string DesCreate(string desStr, string key, string iV)
        {
            byte[] byKey = ASCIIEncoding.ASCII.GetBytes(key);  //秘钥   
            byte[] byIV = ASCIIEncoding.ASCII.GetBytes(iV);    // 向量 

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            int i = cryptoProvider.KeySize;
            MemoryStream ms = new MemoryStream();
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);
            StreamWriter sw = new StreamWriter(cst);
            sw.Write(desStr);
            sw.Flush();
            cst.FlushFinalBlock();
            sw.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
        }


        /// <summary>
        /// DES  解密
        /// </summary>
        /// <param name="desStr">需要解密的 字符</param>
        /// <param name="key">秘钥  必须是8位</param>
        /// <param name="IV">IV向量 也是8位的  可以只传一个 key   这个也用key的变量</param>
        /// <returns></returns>
        public static string DesCryp(string desStr, string key, string iV)
        {
            byte[] byKey = ASCIIEncoding.ASCII.GetBytes(key);  //秘钥   
            byte[] byIV = ASCIIEncoding.ASCII.GetBytes(iV);    // 向量 
            string str = string.Empty;
            try
            {
                byte[] byStr = Convert.FromBase64String(desStr);   // 吧需要解密的字符 转换
                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();  // DES 
                MemoryStream ms = new MemoryStream(byStr);
                CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cst);
                str = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            tmd5.Text = MD5Create(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tdes.Text = DesCryp(tmd5.Text, tkey.Text, tIV.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tmd5.Text = DesCreate(textBox1.Text, tkey.Text, tIV.Text);
        }
    }
}
