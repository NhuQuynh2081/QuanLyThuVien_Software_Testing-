using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using QuanLyThuVien.BUS;
using QuanLyThuVien.DAO;
using QuanLyThuVien.DTO;
using System.Data;

namespace TestHeThong
{
    [TestClass]
    public class TestDangKy
    {
        public TestContext TestContext { get; set; }
        ThanhVien_BUS tvBus = new ThanhVien_BUS();
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
                @"D:\QuanLyThuVien\TestHeThong\Data\DangKy\SignUp_Data.csv", "SignUp_Data#csv", DataAccessMethod.Sequential)] //DataAccessMethod.Sequential: đọc tuần tự từng hàng

        public void SignUp_Test()
        {
            string ten, mk1, mk2;
            ten = TestContext.DataRow[0].ToString();
            mk1 = TestContext.DataRow[1].ToString();
            mk2 = TestContext.DataRow[2].ToString();
            int expect = int.Parse(TestContext.DataRow[3].ToString());
            int result;
            if (mk2 == "")
            {
                result = 0;
            }
            if (mk2 != mk1)
            {
                result = 0;
            }
            else
            {
                ThanhVien _tv = new ThanhVien();
                _tv.TenDangNhap = ten;
                _tv.MatKhau = mk1;

                int check = tvBus.DangKy(_tv);
                if (check == 1)
                {
                    result = 1;
                }
                else if (check == -1)
                {
                    result = 0;
                }
                else
                {
                    result = 0;
                }
            }
            if (result == 1)
            {
                string sqlString = string.Format("select * from ACCOUNT where TenDangNhap='{0}'", ten);
                DataProvider dataEx = new DataProvider();
                DataTable kq = dataEx.GetData(sqlString);
                Assert.AreEqual(expect, kq.Rows.Count);

                //Xoá dữ liệu test
                sqlString = string.Format("delete from DOCGIA where MaDocGia = (select MaDocGia from ACCOUNT where ACCOUNT.TenDangNhap = '{0}')", ten);
                dataEx.Excute(sqlString);
                sqlString = string.Format("delete  from ACCOUNT where TenDangNhap='{0}'", ten);
                dataEx.Excute(sqlString);

            }
            Assert.AreEqual(expect, result);



        }
    }
}
