using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json; 
using System.IO;




namespace DA_tinhoc
{
    public partial class FormQLSV : Form
    {
        private DanhMucSinhVien DMSinhVien = new DanhMucSinhVien();
        private int vitri = 0;
        public FormQLSV()
        {
            InitializeComponent();
        }
        private void HienThiDanhSachSinhVien(DataGridView dgv, List<SinhVien> ds)
        {
            dgv.DataSource = ds.ToList();
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            string ma = txtMaSV.Text;
            string ten = txtTenSV.Text;
            string lop = txtLop.Text;
            string khoa = txtKhoa.Text;
            string dc = txtDiaChi.Text;
            string dt = txtSoDT.Text;
            DateTime ns = dptNgaySinh.Value;
            string gioitinh;
            if (rdbNam.Checked)
            {
                gioitinh = "Nam";
            }
            else
            {
                gioitinh = "Nữ";
            }
            SinhVien sinhvien = new SinhVien(ma, ten, lop, khoa, dc, dt, ns, gioitinh);
            if (DMSinhVien.Them(sinhvien))
            {
                HienThiDanhSachSinhVien(dgvQLSV, DMSinhVien.DsSinhVien);
                MessageBox.Show("Đã thêm sinh viên", "Thông báo", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("Không thêm được sinh viên", "Thông báo", MessageBoxButtons.OK);
            }
        }

        private void dgvQLSV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            vitri = e.RowIndex;
            //
            SinhVien sv = new SinhVien();
            sv = DMSinhVien.DsSinhVien[vitri];
            txtMaSV.Text = sv.MaSV;
            txtTenSV.Text = sv.TenSV;
            txtLop.Text = sv.Lop;
            txtKhoa.Text = sv.Khoa;
            txtDiaChi.Text = sv.Diachi;
            txtSoDT.Text = sv.SoDT;
            dptNgaySinh.Value = sv.NgaySinh;
            if (sv.Gioitinh == "Nam")
            {
                rdbNam.Checked = true;
            }
            else
            {
                rdbNu.Checked = true;
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {

            SinhVien sv = DMSinhVien.DsSinhVien[vitri];
            if (DMSinhVien.Xoa(sv, vitri))
            {
                HienThiDanhSachSinhVien(dgvQLSV, DMSinhVien.DsSinhVien);
                MessageBox.Show("Đã xóa sinh viên", "Thông báo", MessageBoxButtons.OK);
            }
           
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            SinhVien sv = DMSinhVien.DsSinhVien[vitri];

            if (DMSinhVien.Sua(sv, vitri))
            {
                sv.MaSV = txtMaSV.Text;
                sv.TenSV = txtTenSV.Text;
                sv.Lop = txtLop.Text;
                sv.Khoa = txtKhoa.Text;
                sv.Diachi = txtDiaChi.Text;
                sv.SoDT = txtSoDT.Text;

                if (rdbNam.Checked)
                {
                    sv.Gioitinh = "Nam";
                }
                else
                {
                    sv.Gioitinh = "Nữ";
                }
                HienThiDanhSachSinhVien(dgvQLSV, DMSinhVien.DsSinhVien);
                MessageBox.Show("Đã sửa sinh viên", "Thông báo", MessageBoxButtons.OK);
            }
            
        }

        private void LuuFileJson(string filePath)
        {
            try
            {
                // Khởi tạo danh sách sinh viên hiện có
                List<SinhVien> danhSachHienTai = new List<SinhVien>();

                // Kiểm tra nếu file đã tồn tại
                if (File.Exists(filePath))
                {
                    // Đọc nội dung hiện có của file JSON
                    string existingJson = File.ReadAllText(filePath);
                    // Chuyển đổi JSON hiện có thành danh sách sinh viên
                    danhSachHienTai = JsonConvert.DeserializeObject<List<SinhVien>>(existingJson) ?? new List<SinhVien>();
                }

                // Thêm các sinh viên mới vào danh sách hiện tại
                danhSachHienTai.AddRange(DMSinhVien.DsSinhVien);

                // Chuyển đổi danh sách tổng hợp thành JSON
                string json = JsonConvert.SerializeObject(danhSachHienTai, Formatting.Indented);

                // Ghi lại toàn bộ nội dung vào file
                File.WriteAllText(filePath, json);

                MessageBox.Show("Đã lưu danh sách sinh viên vào file JSON.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu file JSON: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Hàm tải danh sách sinh viên từ file .json
        private void LoadFileJson(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                DMSinhVien.DsSinhVien = JsonConvert.DeserializeObject<List<SinhVien>>(json);
                HienThiDanhSachSinhVien(dgvQLSV, DMSinhVien.DsSinhVien);
                MessageBox.Show("Đã tải danh sách sinh viên từ file JSON.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải file JSON: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sự kiện Click cho nút "Lưu File JSON"
        private void btnLuuFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON Files (*.json)|*.json",
                Title = "Lưu file JSON"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                LuuFileJson(saveFileDialog.FileName);
            }
        }

        // Sự kiện Click cho nút "Tải File JSON"
        

        private void btnTaiFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JSON Files (*.json)|*.json",
                Title = "Mở file JSON"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadFileJson(openFileDialog.FileName);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muôn thoát không", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes) 
            { 
                Close();
            }
        }
    }
}
