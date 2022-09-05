using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuanLyThuVien.BUS;
using QuanLyThuVien.DAO;
using QuanLyThuVien.DTO;
using System.Data;
using System;

namespace TestHeThong
{
    [TestClass]
    public class TestThongKe
    {
        public TestContext TestContext { get; set; }
        DangMuon_BUS dmBus = new DangMuon_BUS();
        DaTra_BUS dtBUS = new DaTra_BUS();

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
                @"d:\quanlythuvien\TestHeThong\Data\ThongKe\Finding_PM_data.csv", "Finding_PM_data#csv", DataAccessMethod.Sequential)] //DataAccessMethod.Sequential: đọc tuần tự từng hàng

        public void Finding_PM_Testing()
        {
            string keyWord, userName, bookName; //Nếu admin tìm phiếu mượn theo tên người mượn thì UserName = true;
            int kq = 0;

            string s = TestContext.DataRow[0].ToString();
            byte[] utf = System.Text.Encoding.Default.GetBytes(s);
            keyWord = System.Text.Encoding.UTF8.GetString(utf);

            userName = TestContext.DataRow[1].ToString();
            bookName = TestContext.DataRow[2].ToString();
            int expect = int.Parse(TestContext.DataRow[3].ToString());
            if (keyWord == "")
            {
                kq = dmBus.GetList().Rows.Count;

            }
            else
            {
                if (userName == "True")
                    kq = dmBus.TimKiem(keyWord, "HoTen").Rows.Count;
                else if (bookName == "True")
                    kq = dmBus.TimKiem(keyWord, "TenSach").Rows.Count;
            }
            Assert.AreEqual(expect, kq);
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
                @"D:\QuanLyThuVien\TestHeThong\Data\ThongKe\Finding_PT_data.csv", "Finding_PT_data#csv", DataAccessMethod.Sequential)] //DataAccessMethod.Sequential: đọc tuần tự từng hàng

        public void Finding_PT_Testing()
        {
            string keyWord, userName, bookName; //Nếu admin tìm phiếu mượn theo tên người mượn thì UserName = true;
            int kq = 0;

            string s = TestContext.DataRow[0].ToString();
            byte[] utf = System.Text.Encoding.Default.GetBytes(s);
            keyWord = System.Text.Encoding.UTF8.GetString(utf);

            userName = TestContext.DataRow[1].ToString();
            bookName = TestContext.DataRow[2].ToString();
            int expect = int.Parse(TestContext.DataRow[3].ToString());
            if (keyWord == "")
            {
                kq = dtBUS.GetList().Rows.Count;

            }
            else
            {
                if (userName == "True")
                    kq = dtBUS.TimKiem(keyWord, "HoTen").Rows.Count;
                else if (bookName == "True")
                    kq = dtBUS.TimKiem(keyWord, "TenSach").Rows.Count;
            }
            Assert.AreEqual(expect, kq);
            //}
        }
    }
}
