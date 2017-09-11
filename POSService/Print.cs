using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace POSService
{
    class Print
    {
        public static void previewPdfFile(string filename)
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = filename;
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                throw new Exception("Pdf reader error.\n\nDetailed Error:" + ex.Message);
            }
        }
    }
}
//myProcess.StartInfo.FileName = "acroRd32.exe"; //not the full application path
//myProcess.StartInfo.Arguments = "file.pdf";