﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThuVien;
using System.IO;
namespace DoAn
{
    public partial class UserControlSanPham : UserControl
    {

        DBconnect conn = new DBconnect("QL_SHOPTHOITRANG");
        SqlDataAdapter aa = new SqlDataAdapter();
        DataColumn[] primarykey = new DataColumn[1];
        public UserControlSanPham()
        {
            InitializeComponent();
        }
        public void createSanPham()
        {
            string str = "select * from SANPHAM";
            aa = conn.getDataAdapter(str, "SANPHAM");
            primarykey[0] = conn.Dset.Tables["SANPHAM"].Columns["MASANPHAM"];
            conn.Dset.Tables["SANPHAM"].PrimaryKey = primarykey;
        }
        private void Load_Combobox_LoaiSanPham()
        {
            string sql = "SELECT * FROM LOAISANPHAM";
            DataTable dt = conn.getDataTable(sql, "LOAISANPHAM");
            cboLoaiSanPham.DataSource = dt;
            cboLoaiSanPham.DisplayMember = "TENLOAI";
            cboLoaiSanPham.ValueMember = "MALOAI";

            cboLoaiSanPham.SelectedIndex = 0;
        }

        private void Load_Combobox_ThuongHieu()
        {
            string sql = "SELECT * FROM THUONGHIEU";
            DataTable dt = conn.getDataTable(sql, "THUONGHIEU");
            cboThuongHieu.DataSource = dt;
            cboThuongHieu.DisplayMember = "TENTHUONGHIEU";
            cboThuongHieu.ValueMember = "MATHUONGHIEU";

            cboThuongHieu.SelectedIndex = 0;
        }
        private void loadLaiData()
        {
            string loadLaiDuLieu = "SELECT * FROM SANPHAM";
            dGVSanPham.DataSource = conn.LoadData(loadLaiDuLieu);
        }
        private void UserControlSanPham_Load(object sender, EventArgs e)
        {
            this.dGVSanPham.DefaultCellStyle.ForeColor = Color.Black;
            createSanPham();
            Load_Combobox_LoaiSanPham();
            Load_Combobox_ThuongHieu();
            loadDuLieu();
        }
        string paths = Application.StartupPath.Substring(0, Application.StartupPath.Length - 10);
        private void btnUpHinh_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.InitialDirectory = "C:\\";
            open.Filter = "Image File (*.jpg)|*.jpg|All File (*.*)|*.*";
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string name = System.IO.Path.GetFileName(open.FileName);
                string luu = paths + "\\img\\" + name;
                try
                {
                    FileStream fs = new FileStream(open.FileName, FileMode.Open, FileAccess.Read);
                    System.IO.File.Copy(open.FileName, luu);

                    MessageBox.Show("Upload file ảnh thành công", "Thông báo");
                    txtTenHinh.Text = name;
                    //picHinh.Image = Image.FromFile(luu);
                    picHinh.Image = System.Drawing.Image.FromStream(fs);
                    //  picHinh.ImageLocation = open.FileName;
                    fs.Close();

                }
                catch
                {
                    MessageBox.Show("Hình ảnh đã tồn tại hoặc trùng tên, vui lòng kiểm tra lại");
                }
            }
        }
        private void loadDuLieu()
        {
            dGVSanPham.DataSource = conn.Dset.Tables["SANPHAM"];
        }

        private void dGVSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (e.RowIndex == -1) return;
            txtMaSanPham.Text = dGVSanPham.Rows[index].Cells[0].Value.ToString();
            txtTenSanPham.Text = dGVSanPham.Rows[index].Cells[1].Value.ToString();
            cboLoaiSanPham.Text = dGVSanPham.Rows[index].Cells[2].Value.ToString();
            cboThuongHieu.Text = dGVSanPham.Rows[index].Cells[3].Value.ToString();
            txtDonGia.Text = dGVSanPham.Rows[index].Cells[4].Value.ToString();
            txtSoLuongSP.Text = dGVSanPham.Rows[index].Cells[5].Value.ToString();
            txtLoiNhuan.Text = dGVSanPham.Rows[index].Cells[6].Value.ToString();
            txtMoTa.Text = dGVSanPham.Rows[index].Cells[7].Value.ToString();
            txtNgayCapNhat.Text = dGVSanPham.Rows[index].Cells[8].Value.ToString();
            txtTenHinh.Text = dGVSanPham.Rows[index].Cells[9].Value.ToString();
            try
            {
                if (txtTenHinh.Text != " " && txtTenHinh.Text != "" && txtTenHinh.Text != null)
                {
                    string url = paths + "\\img\\" + txtTenHinh.Text;
                    picHinh.Image = Image.FromFile(url);
                    FileStream fs = new FileStream(url, FileMode.Open, FileAccess.Read);
                    picHinh.Image = System.Drawing.Image.FromStream(fs);
                    fs.Close();
                }
                else
                {
                    picHinh.Image = Image.FromFile(paths + "\\img\\no-image-available-icon-6.jpg");
                }
            }
            catch
            {
                picHinh.Image = Image.FromFile(paths + "\\img\\no-image-available-icon-6.jpg");
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (conn.searchSanPham(txtTimKiem.Text) != null)
            {
                dGVSanPham.DataSource = conn.searchSanPham(txtTimKiem.Text);
            }
            else MessageBox.Show("Không tìm thấy");
        }

        private void BtnTroLai_Click(object sender, EventArgs e)
        {
            loadDuLieu();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            txtMaSanPham.Enabled = true;
            txtTenSanPham.Text = "";
            txtDonGia.Text = "";
            txtMoTa.Text = "";
            txtTenHinh.Text = "";
            txtSoLuongSP.Text = "";
            txtLoiNhuan.Text = "";
            txtMaSanPham.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaSanPham.Text == "" || txtTenSanPham.Text == "" || txtDonGia.Text == "" || txtMoTa.Text == "" || txtSoLuongSP.Text == "")
                {
                    MessageBox.Show("Không được để trống");
                }
                else
                {
                    string masanpham = txtMaSanPham.Text.Trim();
                    string tensanpham = txtTenSanPham.Text.Trim();
                    int dongia = int.Parse(txtDonGia.Text.Trim());
                    string mota = txtMoTa.Text.Trim();
                    string loaisanpham = cboLoaiSanPham.SelectedValue.ToString().Trim();
                    string thuonghieu = cboThuongHieu.SelectedValue.ToString().Trim();
                    string tenhinh = txtTenHinh.Text.Trim();
                    int soluong = int.Parse(txtSoLuongSP.Text.Trim());
                    float loi = float.Parse(txtLoiNhuan.Text.Trim());
                    string ngaycapnhat = DateTime.ParseExact(txtNgayCapNhat.Text, "dd/MM/yyyy", null).ToString("yyyy/MM/dd");
                    string strSQL = "SELECT COUNT(*) FROM SANPHAM WHERE MASANPHAM = '" + masanpham + "'";
                    bool kq = conn.kiemTraTrung(strSQL);
                   

                    if (kq == true)
                    {
                        MessageBox.Show("Đã tồn tại mã sản phẩm này: " + masanpham);
                        return;
                    }

                    strSQL = "INSERT SANPHAM VALUES('" + masanpham + "',N'" + tensanpham + "','" + loaisanpham + "','" + thuonghieu + "'," + dongia + "," + soluong + ",'" + loi + "',N'" + mota + "','" + ngaycapnhat + "','" + tenhinh + "')";

                    conn.updateTODB(strSQL);
                    loadLaiData();

                    MessageBox.Show("Thêm thành công nha ^^");
                }

            }
            catch
            {
                MessageBox.Show("thất bại");
            }
           
            
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string masanpham = txtMaSanPham.Text.Trim();

                string strSQL = "SELECT COUNT(*) FROM SANPHAM WHERE MASANPHAM = '" + masanpham + "'";

                bool kq = conn.kiemTraTrung(strSQL);

                if (kq == false)
                {
                    MessageBox.Show("Không tồn tại mã sản phẩm này: " + masanpham);
                    return;
                }
                strSQL = "DELETE SANPHAM WHERE MASANPHAM = '" + masanpham + "'";
                conn.updateTODB(strSQL);
                MessageBox.Show("Xóa thành công nha ^^");
                loadLaiData();
            }
            catch
            {
                MessageBox.Show("thất bại");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                string masanpham = txtMaSanPham.Text.Trim();
                string tensanpham = txtTenSanPham.Text.Trim();
                int dongia = int.Parse(txtDonGia.Text.Trim());
                string mota = txtMoTa.Text.Trim();
                int soluong = int.Parse(txtSoLuongSP.Text.Trim());
                float loi = float.Parse(txtLoiNhuan.Text.Trim());
                string loaisanpham = cboLoaiSanPham.SelectedValue.ToString().Trim();
                string thuonghieu = cboThuongHieu.SelectedValue.ToString().Trim();
                string tenhinh = txtTenHinh.Text.Trim();
                string ngaycapnhat = DateTime.ParseExact(txtNgayCapNhat.Text, "dd/MM/yyyy", null).ToString("yyyy/MM/dd");
                string strSQL = "SELECT COUNT(*) FROM SANPHAM WHERE MASANPHAM = '" + masanpham + "'";
                bool kq = conn.kiemTraTrung(strSQL);
                if (kq == false)
                {
                    MessageBox.Show("Không tồn tại mã sản phầm này: " + masanpham);
                    return;
                }

                strSQL = "UPDATE SANPHAM SET TENSANPHAM = N'" + tensanpham + "', MALOAI = '" + loaisanpham + "',MATHUONGHIEU = '" + thuonghieu + "',DONGIA = " + dongia + ",SOLUONGSP = " + soluong + ",LOINHUAN = '" + loi + "',MOTA = N'" + mota + "',NGAYCAPNHAT = '" + ngaycapnhat + "',HINHANH = '" + tenhinh + "' WHERE MASANPHAM = '" + masanpham + "'";
                conn.updateTODB(strSQL);
                loadLaiData();
                MessageBox.Show("Sửa thành công nha ^^");
            }
            catch
            {
                MessageBox.Show("thất bại");
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }

}
