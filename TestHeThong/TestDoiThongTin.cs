using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Data.SqlClient; //dùng để bắt câu lệnh sql

using QuanLyThuVien.DTO;
using QuanLyThuVien.BUS;

namespace TestHeThong
{
    [TestClass]
    public class TestDoiThongTin
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
            @"D:\QuanLyThuVien\TestHeThong\Data\DoiThongTin\Data_DoiThongTin.csv", "Data_DoiThongTin#csv", DataAccessMethod.Sequential)]
        public void TestChangeInfo()
        {
            ThanhVien_BUS tv_BUS = new ThanhVien_BUS();
            DocGia dg = new DocGia();
            DangMuon_BUS dm_BUS = new DangMuon_BUS(); //gọi lệnh tìm kiếm từ đây

            int expected;
            //gán dữ liệu
            dg.MaDocGia = TestContext.DataRow[0].ToString();
            //chữ có dấu đọc bị lỗi nên cần gán utf8
            //byte[] utf = System.Text.Encoding.Default.GetBytes(TestContext.DataRow[1].ToString()); //s là chuỗi đọc được từ csv
            //keySearch = System.Text.Encoding.UTF8.GetString(utf);
            dg.HoTen = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(TestContext.DataRow[1].ToString()));
            dg.GioiTinh = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(TestContext.DataRow[2].ToString()));
            dg.NamSinh = DateTime.Parse(TestContext.DataRow[3].ToString());
            dg.DiaChi = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(TestContext.DataRow[4].ToString()));
            expected = int.Parse(TestContext.DataRow[5].ToString());
            try
            {
                //chạy lệnh update
                tv_BUS.sua(dg);
            }
            catch (SqlException exError)
            {
                //Có thể xảy ra lỗi do câu lệnh sql
                Console.WriteLine("Có lỗi xảy ra");
                Console.WriteLine(exError.Message);
            }
            finally //dù đúng hay sai cũng thực hiện
            {
                //do lớp Bus ko trả về kq là thành công hay ko nên sẽ đếm số dòng trong phương thức tìm kiếm
                //kiểm tra xem câu lệnh có hoạt động (tìm kiếm trong database)
                //vì bên lớp ThanhVien_BUS ko có chức năng tìm kiếm nên gọi lệnh bên DangMuon_BUS
                DataTable dt = dm_BUS.TimKiem(dg.HoTen, "HoTen");
                int count = int.Parse(dt.Rows.Count.ToString());
                Assert.AreEqual(expected, count);
            }
            
            
        }
    }
}
