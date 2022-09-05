using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Data.SqlClient; //dùng để bắt câu lệnh sql

using QuanLyThuVien.DTO;
using QuanLyThuVien.BUS;

namespace TestHeThong
{
    [TestClass]
    public class TestQLPhieuMuon
    {
        //dùng cho DATA-DRIVEN UNIT TEST
        public TestContext TestContext { get; set; } //đọc đối tượng từ file txt

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
                @"D:\QuanLyThuVien\TestHeThong\Data\PhieuMuon\PM_InsertNullError.csv", "PM_InsertNullError#csv", DataAccessMethod.Sequential)] //DataAccessMethod.Sequential: đọc tuần tự từng hàng
            public void TestPM_Insert_Null_Error()
            {
                bool _CkDG, _CkS, _CkB, both;

                PhieuMuon_BUS muon_BUS = new PhieuMuon_BUS();
                PhieuMuon pm = new PhieuMuon();
                pm.MaPhieu = TestContext.DataRow[0].ToString();
                pm.MaDocGia = TestContext.DataRow[1].ToString();
                pm.MaSach = TestContext.DataRow[2].ToString();
                pm.NgayMuon = DateTime.Parse(TestContext.DataRow[3].ToString());
                pm.NgayPhaiTra = DateTime.Parse(TestContext.DataRow[4].ToString());
                _CkDG = bool.Parse(TestContext.DataRow[5].ToString());
                _CkS = bool.Parse(TestContext.DataRow[6].ToString());
                _CkB = bool.Parse(TestContext.DataRow[7].ToString());
                both = _CkDG && _CkS;
                try
                {
                    Assert.AreEqual(_CkDG, muon_BUS.CheckExist("DOCGIA", pm.MaDocGia));
                    Assert.AreEqual(_CkS, muon_BUS.CheckExist("SACH", pm.MaSach));
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
                @"D:\QuanLyThuVien\TestHeThong\Data\PhieuMuon\PM_Insert.csv", "PM_Insert#csv", DataAccessMethod.Sequential)] 
            public void TestPM_Insert()
            {
                int expected, exC, actual;

                PhieuMuon_BUS muon_BUS = new PhieuMuon_BUS();
                PhieuMuon pm = new PhieuMuon();
                pm.MaPhieu = TestContext.DataRow[0].ToString();
                pm.MaDocGia = TestContext.DataRow[1].ToString();
                pm.MaSach = TestContext.DataRow[2].ToString();
                pm.NgayMuon = DateTime.Parse(TestContext.DataRow[3].ToString());
                pm.NgayPhaiTra = DateTime.Parse(TestContext.DataRow[4].ToString());
                expected = int.Parse(TestContext.DataRow[5].ToString());
                exC = int.Parse(TestContext.DataRow[6].ToString());
                try
                {
                    //kiểm tra Bus có thực hiện thêm
                    actual = muon_BUS.Them(pm);
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
                    DataTable dt = muon_BUS.TimKiem(TestContext.DataRow[0].ToString(), "MaPhieu");
                    int count = int.Parse(dt.Rows.Count.ToString());
                    Assert.AreEqual(exC, count);
                }
            
                
        }
        #endregion
        #endregion

        #region Test thao tác Delete
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
                @"D:\QuanLyThuVien\TestHeThong\Data\PhieuMuon\PM_Delete.csv", "PM_Delete#csv", DataAccessMethod.Sequential)] 
        [TestMethod]
        public void TestPM_Delete()
        {
            int expected;
            PhieuMuon pm = new PhieuMuon();
            PhieuMuon_BUS muon_BUS = new PhieuMuon_BUS();
            pm.MaPhieu = TestContext.DataRow[0].ToString();
            expected = int.Parse(TestContext.DataRow[1].ToString());
            try
            {
                //thực hiện xóa
                muon_BUS.Xoa(pm.MaPhieu);
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
                DataTable dt = muon_BUS.TimKiem(pm.MaPhieu, "MaPhieu");
                int count = int.Parse(dt.Rows.Count.ToString());
                Assert.AreEqual(expected, count);
            }
        }
        #endregion

        #region Test thao tác Update
        //Thao tác kiểm tra mã ĐG và mã S có trong database hay ko tương tự TestInsert_Null_Error

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
            @"D:\QuanLyThuVien\TestHeThong\Data\PhieuMuon\PM_Update.csv", "PM_Update#csv", DataAccessMethod.Sequential)] 

        public void TestPM_Update()
        {
            bool actual, expected;
            int exC;

            PhieuMuon_BUS muon_BUS = new PhieuMuon_BUS();
            PhieuMuon pm = new PhieuMuon();
            pm.MaPhieu = TestContext.DataRow[0].ToString();
            pm.MaDocGia = TestContext.DataRow[1].ToString();
            pm.MaSach = TestContext.DataRow[2].ToString();
            pm.NgayMuon = DateTime.Parse(TestContext.DataRow[3].ToString());
            pm.NgayPhaiTra = DateTime.Parse(TestContext.DataRow[4].ToString());
            expected = bool.Parse(TestContext.DataRow[5].ToString());
            exC = int.Parse(TestContext.DataRow[6].ToString());
            try
            {
                //kiểm tra Bus
                actual = muon_BUS.Sua(pm);
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
                DataTable dt = muon_BUS.TimKiem(TestContext.DataRow[4].ToString().Remove(4), "NgayPhaiTra"); //kiểm tra dựa trên ngày trả, do phương thức tìm là %...% nên chỉ cần lấy năm
                int count = int.Parse(dt.Rows.Count.ToString());
                Assert.AreEqual(exC, count);
            }
            //lỗi hàng 1: sai do câu lệnh sql nhập sai thứ tự
            //lỗi hàng 2: do hệ thống ko có kiểm tra khi mã phiếu không có trong database
        }
        #endregion

        #region Test thao tác tìm kiếm
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
            @"D:\QuanLyThuVien\TestHeThong\Data\PhieuMuon\PM_Search.csv", "PM_Search#csv", DataAccessMethod.Sequential)] 

        public void TestPM_Search()
        {
            int expected;
            string type, keySearch;
            int count;
            PhieuMuon pm = new PhieuMuon();
            PhieuMuon_BUS muon_BUS = new PhieuMuon_BUS();
            DataTable dt = new DataTable();
            try
            {
                type = TestContext.DataRow[0].ToString();
                expected = int.Parse(TestContext.DataRow[2].ToString());
                if (type == "")
                {
                    dt = muon_BUS.GetList();
                    count = int.Parse(dt.Rows.Count.ToString());
                    Assert.AreEqual(expected, count);
                }
                else
                {
                    keySearch = TestContext.DataRow[1].ToString();

                    dt = muon_BUS.TimKiem(keySearch, type);
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
