using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using QuanLyThuVien.BUS;
using QuanLyThuVien.DTO;

namespace TestHeThong
{
    [TestClass]
    public class TestDangNhap
    {
        public TestContext TestContext { get; set; }
        ThanhVien_BUS tvBus = new ThanhVien_BUS();
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
                @"D:\QuanLyThuVien\TestHeThong\Data\DangNhap\Login_Data.csv", "Login_Data#csv", DataAccessMethod.Sequential)] //DataAccessMethod.Sequential: đọc tuần tự từng hàng

        public void Login_Testing()
        {
            string tk, mk;
            bool mongdoi;
            tk = TestContext.DataRow[0].ToString();
            mk = TestContext.DataRow[1].ToString();
            mongdoi = bool.Parse(TestContext.DataRow[2].ToString());
            Assert.AreEqual(mongdoi, tvBus.DangNhap(tk, mk));
        }
    }
}
