using ClosedXML.Excel;
using dbbuku.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbbuku.Controllers
{
    public class LaporanController : Controller
    {
        // GET: Laporan
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ExportData() {

            BukuEntities db = new BukuEntities();
            List<TblM_Transaksi> TblM_Transaksi = db.TblM_Transaksi.ToList();

            List<transaksiModel> transaksiLs = new List<transaksiModel>();
            foreach (TblM_Transaksi item in TblM_Transaksi) {
                transaksiModel TransaksiItem = new transaksiModel();

                string NamaUser = db.AspNetUsers.Where(x => x.Id == item.UserId).FirstOrDefault().UserName;
                string NamaBuku = db.tblM_Buku.Where(x => x.ID == item.BukuId).FirstOrDefault().Nama;

                TransaksiItem.ID = item.ID;
                TransaksiItem.NamaUser = NamaUser;
                TransaksiItem.NamaBuku = NamaBuku;
                TransaksiItem.Jumlah = item.Jumlah;
                TransaksiItem.Total = item.Total;

                transaksiLs.Add(TransaksiItem);             
            }

            DataTable dt = ConvertToDataTable(transaksiLs);

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(dt, "transaksi");
                ws.Tables.FirstOrDefault().ShowAutoFilter = false;

                MemoryStream stream = GetStream(wb);
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=transaksi.xlsx");
                Response.ContentType = "application/octet-stream";
                Response.BinaryWrite(stream.ToArray());
                Response.End();
            }

            return null;
        }

        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }

            DataColumn[] stringColumns = table.Columns.Cast<DataColumn>().Where(c => c.DataType == typeof(string)).ToArray();
            foreach (DataRow row in table.Rows)
                foreach (DataColumn col in stringColumns)
                    if (row[col] != DBNull.Value)
                        row.SetField(col, row.Field<string>(col).Trim());

            return table;
        }

        public static MemoryStream GetStream(XLWorkbook excelWorkbook)
        {
            MemoryStream fs = new MemoryStream();
            excelWorkbook.SaveAs(fs);
            fs.Position = 0;
            return fs;
        }
    }

    public class transaksiModel {
        public int ID { get; set; }
        public string NamaUser { get; set; }
        public string NamaBuku { get; set; }
        public int? Jumlah { get; set; }
        public decimal? Total { get; set; }
    }
}