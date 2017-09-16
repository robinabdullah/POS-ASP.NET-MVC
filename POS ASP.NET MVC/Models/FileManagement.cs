using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Web;

namespace POS_ASP.NET_MVC.Models
{
    public class FileManagement
    {
        public static string filePath = Directory.GetCurrentDirectory() + @"\Files";

        public static string ProductTypePath = filePath + @"\Product Type.dat";
        public static string ProductModelPath = filePath + @"\Product Model.dat";
        public static string ColorPath = filePath + @"\Color.dat";
        public static string ProductRegPath = filePath + @"\Product Reg.dat";
        public static string ReceiptSavingLocation_datFile = filePath + @"\Receipt File Path.dat";

        public static string ReceiptSavingPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        public static string receipt = filePath + @"\receipt form field.pdf";
        public static string generatedReceipt = ReceiptSavingPath + @"\Fillied Receipt.pdf";
        private static void GrantAccess(string fullPath)
        {
            var directoryInfo = new DirectoryInfo(fullPath);
            var directorySecurity = directoryInfo.GetAccessControl();
            var currentUserIdentity = WindowsIdentity.GetCurrent();
            var fileSystemRule = new FileSystemAccessRule(currentUserIdentity.Name, FileSystemRights.Read, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow); directorySecurity.AddAccessRule(fileSystemRule); directoryInfo.SetAccessControl(directorySecurity);
        }
        public static List<string> getProductReg()
        {
            try
            {
                var txts = File.ReadAllLines(ProductRegPath).ToList();
                return txts;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while reading Registration file. \n\nDetailed Error:" + ex.Message + ex.StackTrace);
            }
        }
        public static bool saveProductReg(string org, string newMac, string date)
        {
            try
            {
                //GrantAccess(ProductRegPath);
                string temp = String.Format("{0}\n{1}\n{2}", org, newMac, date);
                //File.SetAttributes(ProductRegPath, FileAttributes.Normal);
                File.WriteAllText(ProductRegPath, temp);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static bool checkReceiptSavingLocation()
        {
            string txt = "";
            try
            {
                txt = File.ReadLines(ReceiptSavingLocation_datFile).First();
            }
            catch { }

            if (txt == "")
            {
                ReceiptSavingPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                throw new Exception("No custom location is set yet. Receipt pdf will be saved on your Desktop.");
            }
            else if (Directory.Exists(txt) == false)
            {
                ReceiptSavingPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                throw new Exception("The custom location for saving invoice receipt is invalid. Please change the location. Otherwise pdf will be saved on your Desktop.");
            }
            ReceiptSavingPath = txt;// if the saving path is valid

            return true;
        }
        public static void saveNewSavingPathtoDatFile(string savingPath)
        {
            try
            {
                File.WriteAllText(ReceiptSavingLocation_datFile, savingPath);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<string> getAllProductModel()
        {
            //setPath();

            try
            {
                var txts = File.ReadAllLines(ProductModelPath).ToList();

                return txts;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static List<string> getAllColor()
        {
            try
            {
                var txts = File.ReadAllLines(ColorPath).ToList();

                return txts;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static void setNewProductType(string type)
        {
            //setPath();
            File.AppendAllText(ProductTypePath, "\n" + type);
        }
        public static void setNewProductModel(string model)
        {
            //setPath();
            File.AppendAllText(ProductModelPath, "\n" + model);
        }
        public static void setNewColor(string color)
        {
            //setPath();
            File.AppendAllText(ColorPath, "\n" + color);
        }
        public static bool IsColorAlreadyExists(string color)
        {
            //setPath();
            List<string> col = FileManagement.getAllColor();

            foreach (string obj in col)
                if (obj == color)
                    return true;

            return false;
        }
        public static bool setDefaultInvoiceFilePath()
        {
            return true;
        }

    }

}