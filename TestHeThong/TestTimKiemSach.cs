using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Data.SqlClient; //dùng để bắt câu lệnh sql

using QuanLyThuVien.BUS;
using QuanLyThuVien.DTO;

namespace TestHeThong
{
    [TestClass]
    public class TestTimKiemSach
    {
        public TestContext TestContext { get; set; }
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
        @"D:\QuanLyThuVien\TestHeThong\Data\TimKiemSach\TimSach.csv", "TimSach#csv", DataAccessMethod.Sequential)]
        public void Test_TimKiem_Sach()
        {
            int expected;
            string type, keySearch;
            int count;
            Sach book = new Sach();
            Sach_BUS sachBUS = new Sach_BUS();
            DataTable dt = new DataTable();
            try
            {
                type = TestContext.DataRow[0].ToString();
                keySearch = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(TestContext.DataRow[1].ToString()));
                //byte[] utf = System.Text.Encoding.Default.GetBytes(TestContext.DataRow[1].ToString()); 
                //keySearch = System.Text.Encoding.UTF8.GetString(utf);
                expected = int.Parse(TestContext.DataRow[2].ToString());

                if (type == "")
                {
                    dt = sachBUS.GetList();
                    count = int.Parse(dt.Rows.Count.ToString());
                    Assert.AreEqual(expected, count);
                }
                else
                {
                    //Console.WriteLine("type " +type);
                    //Console.WriteLine("từ tìm "+ keySearch);
                    dt = sachBUS.TimKiem(keySearch, type);
                    count = int.Parse(dt.Rows.Count.ToString());
                    Assert.AreEqual(expected, count);
                }
            }
            catch (SqlException exError)
            {
                //Có thể xảy ra lỗi do câu lệnh sql
                Console.WriteLine("Có lỗi xảy ra");
                Console.WriteLine(exError.Message);
            }
            
        }
    }
}
