using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;

namespace BL
{
    public class Excel
    {
        public MemoryStream Prueba()
        {
            try
            {
                IWorkbook Book = new XSSFWorkbook();
                ISheet Sheet = Book.CreateSheet("SheetPrueba");

                Sheet.CreateRow(0).CreateCell(0).SetCellValue("Prueba Ikael");

                MemoryStream ms = new MemoryStream();
                using (MemoryStream tempStream = new MemoryStream())
                {
                    Book.Write(tempStream);
                    var byteArray = tempStream.ToArray();
                    ms.Write(byteArray, 0, byteArray.Length);
                }

                return ms;
            }
            catch(Exception e)
            {
                return null;
            }
        }
    }
}
