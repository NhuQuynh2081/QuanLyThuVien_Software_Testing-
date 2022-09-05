using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Data.SqlClient; //dùng để bắt câu lệnh sql

using QuanLyThuVien.DTO;
using QuanLyThuVien.BUS;

namespace TestHeThong
{
    [TestClass]
    public class TestQLPhieuTra
    {
        public TestContext TestContext { get; set; }

        #region Test thao tác Insert
            #region Test kiểm tra maDG, maS bên phần code giao diện
            /*
            Th1: MaDG & MaS hợp lý
            Th2.1: MaDG hợp lý, MaS sai
            Th2.2: MaDG hợp lý, MaS NULL
            Th3.1: MaDG sai, MaS hợp lý
            Th3.2: MaDG NULL, MaS hợp lý
             */
            [TestMethod]
            [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
                @"D:\QuanLyThuVien\TestHeThong\Data\PhieuTra\PT_InsertNullError.csv", "PT_InsertNullError#csv", DataAccessMethod.Sequential)] //DataAccessMethod.Sequential: đọc tuần tự từng hàng
            public void TestPT_Insert_Null_Error()
            {
                bool _CkDG, _CkS, _CkB, both;
                PhieuTra_BUS tra_BUS = new PhieuTra_BUS();
                PhieuTra pt = new PhieuTra();

                pt.MaPhieu = TestContext.DataRow[0].ToString();
                pt.MaDocGia = TestContext.DataRow[1].ToString();
                pt.MaSach = TestContext.DataRow[2].ToString();
                pt.NgayTra = DateTime.Parse(TestContext.DataRow[3].ToString());
                _CkDG = bool.Parse(TestContext.DataRow[4].ToString());
                _CkS = bool.Parse(TestContext.DataRow[5].ToString());
                _CkB = bool.Parse(TestContext.DataRow[6].ToString());
                both = _CkDG && _CkS;
                try
                {
                    Assert.AreEqual(_CkDG, tra_BUS.CheckExist("DOCGIA", pt.MaDocGia));
                    Assert.AreEqual(_CkS, tra_BUS.CheckExist("SACH", pt.MaSach));
                    Assert.AreEqual(_CkB, both);
                }
                catch (SqlException exError)
                {
                    //Có thể xảy ra lỗi do câu lệnh sql
                    Console.WriteLine("Có lỗi xảy ra");
                    Console.WriteLine(exError.Message);
                }
            
            }
            #endregion

            #region Test thao tác thêm bên class Bus
            /*
            Th1: MaDG chưa có
            Th2: MaDG đã có
            Th3: MaDG NULL
             */
            [TestMethod]
            [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
                @"D:\QuanLyThuVien\TestHeThong\Data\PhieuTra\PT_Insert.csv", "PT_Insert#csv", DataAccessMethod.Sequential)] //DataAccessMethod.Sequential: đọc tuần tự từng hàng
            public void TestPT_Insert()
            {
                int expected, exC, actual;

                PhieuTra_BUS tra_BUS = new PhieuTra_BUS();
                PhieuTra pt = new PhieuTra();
                pt.MaPhieu = TestContext.DataRow[0].ToString();
                pt.MaDocGia = TestContext.DataRow[1].ToString();
                pt.MaSach = TestContext.DataRow[2].ToString();
                pt.NgayTra = DateTime.Parse(TestContext.DataRow[3].ToString());
                expected = int.Parse(TestContext.DataRow[4].ToString());
                exC = int.Parse(TestContext.DataRow[5].ToString());
                try
                {
                    //kiểm tra Bus có thực hiện thêm
                    actual = tra_BUS.Them(pt);
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
                    //kiểm tra database có thêm mới (nhằm kiểm tra câu lệnh sql)
                    DataTable dt = tra_BUS.TimKiem(TestContext.DataRow[0].ToString(), "MaPhieu");
                    int count = int.Parse(dt.Rows.Count.ToString());
                    Assert.AreEqual(exC, count);
                }
            }
            #endregion
        #endregion

        #region Test thao tác Delete
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
                @"D:\QuanLyThuVien\TestHeThong\Data\PhieuTra\PT_Delete.csv", "PT_Delete#csv", DataAccessMethod.Sequential)] //DataAccessMethod.Sequential: đọc tuần tự từng hàng
        [TestMethod]
        public void TestPT_Delete()
        {
            int expected;
            PhieuTra_BUS tra_BUS = new PhieuTra_BUS();
            PhieuTra pt = new PhieuTra();

            pt.MaPhieu = TestContext.DataRow[0].ToString();
            expected = int.Parse(TestContext.DataRow[1].ToString());
            try
            {
                //thực hiện xóa
                tra_BUS.Xoa(pt.MaPhieu);
            }
            catch (SqlException exError)
            {
                //Có thể xảy ra lỗi do câu lệnh sql
                Console.WriteLine("Có lỗi xảy ra");
                Console.WriteLine(exError.Message);
            }
            finally //dù đúng hay sai cũng thực hiện
            {
                //kiểm tra lại trong database còn mã phiếu đó hay ko
                DataTable dt = tra_BUS.TimKiem(pt.MaPhieu, "MaPhieu");
                int count = int.Parse(dt.Rows.Count.ToString());
                Assert.AreEqual(expected, count);
            }
        }
        #endregion

        #region Test thao tác Update
        //Thao tác kiểm tra mã ĐG và mã S có trong database hay ko tương tự TestInsert_Null_Error

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
            @"D:\QuanLyThuVien\TestHeThong\Data\PhieuTra\PT_Update.csv", "PT_Update#csv", DataAccessMethod.Sequential)] //DataAccessMethod.Sequential: đọc tuần tự từng hàng

        public void TestPT_Update()
        {
            bool actual, expected;
            int exC;

            PhieuTra_BUS tra_BUS = new PhieuTra_BUS();
            PhieuTra pt = new PhieuTra();
            pt.MaPhieu = TestContext.DataRow[0].ToString();
            pt.MaDocGia = TestContext.DataRow[1].ToString();
            pt.MaSach = TestContext.DataRow[2].ToString();
            pt.NgayTra = DateTime.Parse(TestContext.DataRow[3].ToString());
            expected = bool.Parse(TestContext.DataRow[4].ToString());
            exC = int.Parse(TestContext.DataRow[5].ToString());
            try
            {
                //kiểm tra Bus
                actual = tra_BUS.Sua(pt);
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
                DataTable dt = tra_BUS.TimKiem(TestContext.DataRow[3].ToString().Remove(4), "NgayTra"); //kiểm tra dựa trên ngày trả, do phương thức tìm là %...% nên chỉ cần lấy năm
                int count = int.Parse(dt.Rows.Count.ToString());
                Assert.AreEqual(exC, count);
            }//lỗi hàng 2: do hệ thống ko có kiểm tra khi mã phiếu không có trong database
        }
        #endregion

        #region Test thao tác tìm kiếm
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
            @"D:\QuanLyThuVien\TestHeThong\Data\PhieuTra\PT_Search.csv", "PT_Search#csv", DataAccessMethod.Sequential)]

        public void TestPT_Search()
        {
            int expected;
            string type, keySearch;
            int count;
            PhieuTra_BUS tra_BUS = new PhieuTra_BUS();
            PhieuTra pt = new PhieuTra();
            DataTable dt = new DataTable();
            try
            {
                type = TestContext.DataRow[0].ToString();
                expected = int.Parse(TestContext.DataRow[2].ToString());
                if (type == "")
                {
                    dt = tra_BUS.GetList();
                    count = int.Parse(dt.Rows.Count.ToString());
                    Assert.AreEqual(expected, count);
                }
                else
                {
                    keySearch = TestContext.DataRow[1].ToString();

                    dt = tra_BUS.TimKiem(keySearch, type);
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
        #endregion
    }
}
