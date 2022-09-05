using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Data.SqlClient; //dùng để bắt câu lệnh sql

using QuanLyThuVien.BUS;
using QuanLyThuVien.DTO;

namespace TestHeThong
{
    [TestClass]
    public class TestQLSach
    {
        public TestContext TestContext { get; set; } 
        //[TestInitialize]
        /*
         testcase
         1. Bỏ trống tất cả các trường dữ liệu - Mong đợi: -1
         2. Điền đầy đủ các trường dữ liệu - Mong đợi: 1
         */

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
                @"D:\QuanLyThuVien\TestHeThong\Data\Sach\Sach_Insert.csv", "Sach_Insert#csv", DataAccessMethod.Sequential)]
        public void TestSach_AddBook()
        {
            int expected, ex, actual;
            Sach_BUS book_BUS = new Sach_BUS();
            Sach book = new Sach();
            book.MaSach = TestContext.DataRow[0].ToString();
            book.TenSach = TestContext.DataRow[1].ToString();
            book.TacGia = TestContext.DataRow[2].ToString();
            book.TheLoai = TestContext.DataRow[3].ToString();
            book.NhaXuatBan = TestContext.DataRow[4].ToString();
            book.GiaSach = int.Parse(TestContext.DataRow[5].ToString());
            book.SoLuong = int.Parse(TestContext.DataRow[6].ToString());
            expected = int.Parse(TestContext.DataRow[7].ToString());
            ex = int.Parse(TestContext.DataRow[8].ToString());
            try
            {
                actual = book_BUS.Them(book);
                Assert.AreEqual(expected, actual);
            }
            catch (SqlException exError)
            {
                //Có thể xảy ra lỗi do câu lệnh sql
                Console.WriteLine("Có lỗi xảy ra"); 
                Console.WriteLine(exError.Message);
            }
            finally //dù đúng hay sai cũng thực hiện
            {
                DataTable dt = book_BUS.TimKiem(TestContext.DataRow[0].ToString(), "MaSach");
                int count = int.Parse(dt.Rows.Count.ToString());
                Assert.AreEqual(ex, count);
            }
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
        @"D:\QuanLyThuVien\TestHeThong\Data\Sach\Sach_Delete.csv", "Sach_Delete#csv", DataAccessMethod.Sequential)]
        public void TestSach_Delete()
        {
            int expected1;
            Sach_BUS book1_BUS = new Sach_BUS();
            Sach book1 = new Sach();
            book1.MaSach = TestContext.DataRow[0].ToString();
            expected1 = int.Parse(TestContext.DataRow[1].ToString());
            try
            {
                //Xóa
                book1_BUS.Xoa(book1.MaSach);
            }
            catch (SqlException exError)
            {
                //Có thể xảy ra lỗi do câu lệnh sql
                Console.WriteLine("Có lỗi xảy ra");
                Console.WriteLine(exError.Message);
            }
            finally //dù đúng hay sai cũng thực hiện
            {
                //Kiem tra SQL
                DataTable dt1 = book1_BUS.TimKiem(book1.MaSach, "MaSach");
                int count1 = int.Parse(dt1.Rows.Count.ToString());
                Assert.AreEqual(expected1, count1);
            }
            //lỗi không xóa đc, do có tham chiếu khóa ngoại
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
            @"D:\QuanLyThuVien\TestHeThong\Data\Sach\Sach_Update.csv", "Sach_Update#csv", DataAccessMethod.Sequential)] 

        public void TestSach_Edit()
        {
            bool actual, expected;
            int ex1;

            Sach_BUS book2_BUS = new Sach_BUS();
            Sach book2 = new Sach();
            book2.MaSach = TestContext.DataRow[0].ToString();
            book2.TenSach = TestContext.DataRow[1].ToString();
            book2.TacGia = TestContext.DataRow[2].ToString();
            book2.TheLoai = TestContext.DataRow[3].ToString();
            book2.NhaXuatBan = TestContext.DataRow[4].ToString();
            book2.GiaSach = int.Parse(TestContext.DataRow[5].ToString());
            book2.SoLuong = int.Parse(TestContext.DataRow[6].ToString());
            expected = bool.Parse(TestContext.DataRow[7].ToString());
            ex1 = int.Parse(TestContext.DataRow[8].ToString());
            try
            {
                //kiểm tra Bus
                actual = book2_BUS.Sua(book2);
                Assert.AreEqual(expected, actual);
            }
            catch (SqlException exError)
            {
                //Có thể xảy ra lỗi do câu lệnh sql
                Console.WriteLine("Có lỗi xảy ra");
                Console.WriteLine(exError.Message);
            }
            finally //dù đúng hay sai cũng thực hiện
            {
                //Kiểm tra câu lệnh sql
                DataTable dt2 = book2_BUS.TimKiem(TestContext.DataRow[1].ToString(), "TenSach");//ko thể chỉnh mã sách nên tim theo:
                int count = int.Parse(dt2.Rows.Count.ToString());
                Assert.AreEqual(ex1, count);
            }
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
            @"D:\QuanLyThuVien\TestHeThong\Data\Sach\Sach_Search.csv", "Sach_Search#csv", DataAccessMethod.Sequential)]

        public void TestSach_Search()
        {
            int expected;
            string type, tk;
            int count;
            Sach_BUS book4_BUS = new Sach_BUS();
            Sach book4 = new Sach();
            DataTable dt = new DataTable();
            try
            {
                type = TestContext.DataRow[0].ToString();
                expected = int.Parse(TestContext.DataRow[2].ToString());
                if (type=="")
                {
                    dt = book4_BUS.GetList();
                    count = int.Parse(dt.Rows.Count.ToString());
                    Assert.AreEqual(expected, count);
                }
                else
                {
                    tk = TestContext.DataRow[1].ToString();

                    dt = book4_BUS.TimKiem(tk, type);
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

