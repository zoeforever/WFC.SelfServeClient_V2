using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace WFC.SelfServeClient.Helper
{
    public class IdCardReaderHelper
    {
        [DllImport("Sdtapi.dll")]
        private static extern int InitComm(int iPort);
        [DllImport("Sdtapi.dll")]
        private static extern int Authenticate();
        [DllImport("Sdtapi.dll")]
        private static extern int ReadBaseInfos(StringBuilder Name, StringBuilder Gender, StringBuilder Folk,
                                                    StringBuilder BirthDay, StringBuilder Code, StringBuilder Address,
                                                        StringBuilder Agency, StringBuilder ExpireStart, StringBuilder ExpireEnd);
        [DllImport("Sdtapi.dll")]
        private static extern int ReadBaseInfosPhoto(StringBuilder Name, StringBuilder Gender, StringBuilder Folk,
                                                    StringBuilder BirthDay, StringBuilder Code, StringBuilder Address,
                                                        StringBuilder Agency, StringBuilder ExpireStart, StringBuilder ExpireEnd, StringBuilder directory);
        [DllImport("Sdtapi.dll")]
        private static extern int ReadBaseInfosFPPhoto(StringBuilder Name, StringBuilder Gender, StringBuilder Folk,
                                                    StringBuilder BirthDay, StringBuilder Code, StringBuilder Address,
                                                        StringBuilder Agency, StringBuilder ExpireStart, StringBuilder ExpireEnd, StringBuilder directory, StringBuilder pucFPMsg, ref int puiFPMsgLen);
        [DllImport("Sdtapi.dll")]
        private static extern int CloseComm();
        [DllImport("Sdtapi.dll")]
        private static extern int ReadBaseMsg(byte[] pMsg, ref int len);
        [DllImport("Sdtapi.dll")]
        private static extern int ReadBaseMsgW(byte[] pMsg, ref int len);
        [DllImport("Sdtapi.dll")]
        private static extern int Routon_IC_FindCard();
        [DllImport("Sdtapi.dll")]
        private static extern int Routon_IC_HL_ReadCardSN(StringBuilder SN);

        /// <summary>
        /// 读取身份证
        /// </summary>
        /// <param name="imagePath">身份证图片信息保存路径，默认为系统临时目录</param>
        /// <returns>IdCardInfo</returns>
        [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions]
        public static IdCardInfo ReadIdCard(string imagePath = "")
        {
            string path = string.IsNullOrEmpty(imagePath) ? Path.GetTempPath() : imagePath;
            if (path.Length > 100)
            {
                throw new Exception("身份证保存路径过长");
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            StringBuilder Name = new StringBuilder(31);
            StringBuilder Gender = new StringBuilder(3);
            StringBuilder Folk = new StringBuilder(10);
            StringBuilder BirthDay = new StringBuilder(9);
            StringBuilder Code = new StringBuilder(19);
            StringBuilder Address = new StringBuilder(71);
            StringBuilder Agency = new StringBuilder(31);
            StringBuilder ExpireStart = new StringBuilder(9);
            StringBuilder ExpireEnd = new StringBuilder(9);
            StringBuilder Dir = new StringBuilder(path);

            int intOpenRet = InitComm(1001);
            if (intOpenRet != 1)
            {
                throw new Exception("001-阅读机具未连接");
            }

            //卡认证
            int intReadRet = Authenticate();
            if (intReadRet != 1)
            {
                CloseComm();
                throw new Exception("002-卡认证失败，请将卡拿起再放下");
            }

            int intReadBaseInfosRet = ReadBaseInfosPhoto(Name, Gender, Folk, BirthDay, Code, Address, Agency, ExpireStart, ExpireEnd, Dir);
            if (intReadBaseInfosRet != 1)
            {
                CloseComm();
                throw new Exception("003-读卡失败");
            }

            int intCloseRet = CloseComm();

            if (Code.ToString() == "")
            {
                throw new Exception("--身份证不能为空");

            }
            return new IdCardInfo
            {
                Name = Name.ToString(),
                Gender = Gender.ToString(),
                Nation = Folk.ToString(),
                BirthDay = BirthDay.ToString(),
                Code = Code.ToString(),
                Address = Address.ToString(),
                Agency = Agency.ToString(),
                ExpireStart = ExpireStart.ToString(),
                ExpireEnd = ExpireEnd.ToString(),
                ImagePath = Path.Combine(path, "photo.bmp"),
            };
        }

        public static string ReadIcCard()
        {

            //打开端口
            int intOpenRet = InitComm(1001);
            if (intOpenRet != 1)
            {
                throw new Exception("阅读机具未连接");
            }

            StringBuilder name = new StringBuilder(1024);
            if (Routon_IC_HL_ReadCardSN(name) == 1)
            {
                //关闭端口
                int intCloseRet = CloseComm();
                return Reverse(name.ToString());
            }
            else
            {
                //关闭端口
                int intCloseRet = CloseComm();
                throw new Exception("读IC卡失败");
            }

            string Reverse(string origin)
            {
                string reverse = string.Empty;
                for (int i = 0; i < origin.Length; i += 2)
                {
                    if (i + 2 > origin.Length)
                    {
                        reverse = origin.Substring(i) + reverse;
                    }
                    else
                    {
                        reverse = origin.Substring(i, 2) + reverse;
                    }
                }
                return reverse;
            }
        }
    }

    /// <summary>
    /// 身份证信息
    /// </summary>
    public class IdCardInfo
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 民族
        /// </summary>
        public string Nation { get; set; }
        /// <summary>
        /// 出生日期(yyyyMMdd)
        /// </summary>
        public string BirthDay { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 家庭住址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 签发机关
        /// </summary>
        public string Agency { get; set; }
        /// <summary>
        /// 有效期开始(yyyyMMdd)
        /// </summary>
        public string ExpireStart { get; set; }
        /// <summary>
        /// 有效期结束(yyyyMMdd)
        /// </summary>
        public string ExpireEnd { get; set; }
        /// <summary>
        /// 头像照路径
        /// </summary>
        public string ImagePath { get; set; }
    }
}
