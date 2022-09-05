using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Data.SqlClient; //dùng để bắt câu lệnh sql

using QuanLyThuVien.DTO;
using QuanLyThuVien.BUS;

namespace TestHeThong
{
    [TestClass]
    public class TestDoiMatKhau
    {
        public TestContext TestContext { get; set; }
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
       @"D:\QuanLyThuVien\TestHeThong\Data\DoiMatKhau\Data_DoiMatKhau.csv", "Data_DoiMatKhau#csv", DataAccessMethod.Sequential)] //DataAccessMethod.Sequential: đọc tuần tự từng hàng
        public void TestDoiMK() // Test BUS DAO (Câu lệnh SqL)
        {
            ThanhVien tv = new ThanhVien();
            MatKhau_BUS mk_BUS = new MatKhau_BUS();
            bool expectedMKC, expectedMKM;
            string mk_moi, mk_nl;

            tv.TenDangNhap = TestContext.DataRow[0].ToString();
            tv.MatKhau = TestContext.DataRow[1].ToString();
            mk_moi = TestContext.DataRow[2].ToString();
            mk_nl = TestContext.DataRow[3].ToString();
            expectedMKC = bool.Parse(TestContext.DataRow[4].ToString());
            expectedMKM = bool.Parse(TestContext.DataRow[5].ToString());
            try
            {
                // CheckMKC kiểm tra câu lệnh if thứ 2: ktra username và mkc
                bool checkMKC = mk_BUS.CheckExist(tv.TenDangNhap, tv.MatKhau);
                Assert.AreEqual(expectedMKC, checkMKC);

                if (checkMKC == true) // Đk đúng => thay đổi mkhau
                {
                    tv.MatKhau = mk_moi; // Gán mk mới vào 
                    mk_BUS.DoiMatKhau(tv);
                }
            }
            catch (SqlException exError)
            {
                //Có thể xảy ra lỗi do câu lệnh sql
                Console.WriteLine("Có lỗi xảy ra");
                Console.WriteLine(exError.Message);
            }
            finally //dù đúng hay sai cũng thực hiện
            {
                bool checkMKM = mk_BUS.CheckExist(tv.TenDangNhap, tv.MatKhau); // tv.MK là mk mới
                Assert.AreEqual(expectedMKM, checkMKM); // tìm trong sql xem mk mới đã đổi chưa
            }

            // CheckMKC kiểm tra câu lệnh if thứ 2: ktra username và mkc
            //bool checkMKC = mk_BUS.CheckExist(tv.TenDangNhap, tv.MatKhau);
            //Assert.AreEqual(expectedMKC, checkMKC);

            //if (checkMKC == true) // Đk đúng => thay đổi mkhau
            //{
            //    tv.MatKhau = mk_moi; // Gán mk mới vào 
            //    mk_BUS.DoiMatKhau(tv);
            //}

            //bool checkMKM = mk_BUS.CheckExist(tv.TenDangNhap, tv.MatKhau); // tv.MK là mk mới
            //Assert.AreEqual(expectedMKM, checkMKM); // tìm trong sql xem mk mới đã đổi chưa
        }
    }
}
