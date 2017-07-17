using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printer
{
   public class MoneyFortmat
    {
        public const int AU_TYPE = 1;
        public const int VN_TYPE = 2;

        public int FortmatType { get; set; }

        public MoneyFortmat(int formatType)
        {
            FortmatType = formatType;
        }

        public double Round(double value, int index)
        {
            if (index > 0)
            {
                string s = String.Format("{0:0." + new String('0', index) + "}", value);
                return Convert.ToDouble(s);
            }
            return value;
        }

        public String FormatCurenMoney(double value)
        {
            return String.Format("{0:0.00}", value);
        }

        /// <summary>
        /// 1000 -> 1000.00
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public String FormatCurenMoney2(double value)
        {
            value = Math.Round(value, 2);
            return String.Format("{0:0.00}", value);
        }

        /// <summary>
        /// AU: 1000 -> 1.000
        /// VN: 1000 -> 1000.000
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public String Format(double value)
        {
            if (FortmatType == AU_TYPE)
            {
                return String.Format("{0:0.00}", value / 1000);
            }
            else
            {
                return String.Format("{0:0,0}", value);
            }
        }

        /// <summary>
        /// AU: 1000 -> 1.000
        /// VN: 1000 -> 1000.000
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public String Format(string data)
        {
            double value = 0;
            try
            {
                value = Convert.ToDouble(data);
            }
            catch (Exception)
            {
            }
            if (FortmatType == AU_TYPE)
            {
                return String.Format("{0:0.000}", value / 1000);
            }
            else
            {
                return String.Format("{0:0,0}", value);
            }
        }

        /// <summary>
        /// 1000 -> 1000
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public String FormatNorman(string data)
        {
            double value = 0;
            try
            {
                value = Convert.ToDouble(data);
            }
            catch (Exception)
            {
            }
            return String.Format("{0:0,0}", value);
        }

        /// <summary>
        /// AU: 1000 -> 1.0
        /// VN: 1000 -> 1000.0
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public String Format2(string data)
        {
            double value = 0;
            try
            {
                value = Convert.ToDouble(data);
            }
            catch (Exception)
            {
            }
            if (FortmatType == AU_TYPE)
            {
                //return String.Format("{0:#,#.00}", value / 1000);
                return String.Format("{0:0.00}", value / 1000);
            }
            else
            {
                return String.Format("{0:0,0}", value);
            }
        }

        /// <summary>
        /// AU: 1000 -> 1000.00
        /// VN: 1000 -> 1000.0
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public String FormatNew2(string data)
        {
            double value = 0;
            try
            {
                value = Convert.ToDouble(data);
            }
            catch (Exception)
            {
            }
            if (FortmatType == AU_TYPE)
            {
                return String.Format("{0:0.00}", value);
            }
            else
            {
                return String.Format("{0:0,0}", value);
            }
        }

        /// <summary>
        /// AU: 1000 -> 1.00
        /// VN: 1000 -> 1000.00
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public String Format2(double value)
        {
            if (FortmatType == AU_TYPE)
            {
                return String.Format("{0:#,#.00}", value / 1000);
            }
            else
            {
                return String.Format("{0:0,0}", value);
            }
        }

        /// <summary>
        /// 1000 -> 1000.00
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public String FormatNew2(double value)
        {
            if (FortmatType == AU_TYPE)
            {
                return String.Format("{0:0.00}", value);
            }
            else
            {
                return String.Format("{0:0,0}", value);
            }
        }

        /// <summary>
        /// AU: 1000 -> 1.000
        /// VN: 1000 -> 1000.000
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public String Format3(double value)
        {
            if (FortmatType == AU_TYPE)
            {
                return String.Format("{0:0.0}", value / 1000);
            }
            else
            {
                return String.Format("{0:0,0}", value);
            }
        }
        /// <summary>
        /// 1000 -> 1000.000
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public String FormatNew3(double value)
        {
            return String.Format("{0:0.000}", value);
        }


        public String getFortMat3(double value)
        {
            //double resuilt = 0;
            if (FortmatType == AU_TYPE)
            {
                return String.Format("{0:#,#}", value);
            }
            else
            {
                return String.Format("{0:0,0}", value);
            }
        }

        public String getFortMat2(double value)
        {
            //double resuilt = 0;
            if (FortmatType == AU_TYPE)
            {
                return String.Format("{0:0.00}", value);
            }
            else
            {
                return String.Format("{0:0,0}", value);
            }
        }

        public double getFortMat(string price)
        {
            double resuilt = 0;
            try
            {
                resuilt = Convert.ToDouble(price);
            }
            catch (Exception ex)
            {


            }
            if (FortmatType == AU_TYPE)
            {
                return resuilt * 1000;
            }
            else
            {
                return resuilt;
            }
        }

        public double getFortMatNew(string price)
        {
            double resuilt = 0;
            try
            {
                resuilt = Convert.ToDouble(price);
            }
            catch (Exception ex)
            {

            }
            if (FortmatType == AU_TYPE)
            {
                return resuilt;
            }
            else
            {
                return resuilt;
            }
        }

        public double getFortMat(double price)
        {
            //double resuilt = 0;
            if (FortmatType == AU_TYPE)
            {
                return price * 1000;
            }
            else
            {
                return price;
            }
        }

        public String Convert3to2(double price)
        {
            price = Math.Round(price, 2);
            return String.Format("{0:0.00}", price);
        }

        /// <summary>
        /// Chuyển kiểu số về kiểu có dấu ngăn chia ngàn, triệu, tỉ
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string FormatNumber(int value)
        {
            return String.Format("{0:#,##0}", value);
        }

        public int NumberToInt(string value)
        {
            return Convert.ToInt32(value.Replace(",", "").Trim());
        }
    }
}
