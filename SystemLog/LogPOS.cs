using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemLog
{
   public class LogPOS
    {
       public static void WriteLog(string data)
       {
           //DateTime dt = DateTime.Now;
           //System.IO.StreamWriter lStreamWriter = null;
           //try
           //{
           //    string directoryPath = "StoredFilesLog";
           //    if (!System.IO.Directory.Exists(directoryPath))
           //        System.IO.Directory.CreateDirectory(directoryPath);
           //    string fileName = System.Windows.Forms.Application.StartupPath + "\\StoredFilesLog\\LogPOS_" + dt.Day.ToString("00") + "_" + dt.Month.ToString("00") + "_" + dt.Year + ".log";
           //    if (!System.IO.File.Exists(fileName))
           //    {
           //        lStreamWriter = new System.IO.StreamWriter(fileName);
           //    }
           //    else
           //    {
           //        lStreamWriter = System.IO.File.AppendText(fileName);
           //    }
           //    lStreamWriter.WriteLine(dt.ToString() + ":::" + data);
           //}
           //catch (Exception)
           //{
           //}
           //finally
           //{
           //    try
           //    {
           //        lStreamWriter.Close();
           //    }
           //    catch (Exception)
           //    {
           //    }
           //}
       }
    }
}
