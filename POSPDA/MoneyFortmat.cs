using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POSPDA
{
    public static class MoneyFortmat
    {
        public const int AU_TYPE = 1;
        public const int VN_TYPE = 2;

        public static int FortmatType = 1;

        

        

        /// <summary>
        /// AU: 1000 -> 1.0
        /// VN: 1000 -> 1000.0
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static String Format2(string data)
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
                return String.Format("{0:#,#.00}", value / 1000);
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
        

        /// <summary>
        /// AU: 1000 -> 1.00
        /// VN: 1000 -> 1000.00
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String Format2(double value)
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
        
    }
}