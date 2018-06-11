using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QuanLyThuVien
{
    public partial class QLTGForm : Form
    {
        public QLTGForm()
        {
            InitializeComponent();
        }

        private void bntDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        string strConn = @"Data Source=DESKTOP-SPIMD63;Initial Catalog=QuanLyThuVien;Integrated Security=True";
        SqlConnection conn1;
        public void LoadData()
        {
            SqlDataAdapter da = new SqlDataAdapter("select *from TacGia", conn1);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void QLTGForm_Load(object sender, EventArgs e)
        {
            conn1 = new SqlConnection(strConn);
            conn1.Open();
            LoadData();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                txtMaTG.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["clMaTG"].Value);
                txtTenTG.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["clTenTG"].Value);
                txtGhiChu.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["clGhiChu"].Value);
            }
        }

        private void bntThem_Click(object sender, EventArgs e)
        {
            txtMaTG.Enabled = false;
            bntLuu.Enabled = true;
            txtTenTG.Text = "";
            txtGhiChu.Text = "";
            txtTenTG.Focus();
        }

        private void bntSua_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SuaTG", conn1);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter p = new SqlParameter("@MaTG", txtMaTG.Text);
            cmd.Parameters.Add(p);

            p = new SqlParameter("@TenTG", txtTenTG.Text);
            cmd.Parameters.Add(p);

            p = new SqlParameter("@GhiChu", txtGhiChu.Text);
            cmd.Parameters.Add(p);

            int count = cmd.ExecuteNonQuery();

            if (count > 0)
            {
                MessageBox.Show("Đã sửa", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            else MessageBox.Show("Không thể sửa", "Nontification", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void bntXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa thông tin này ?", "Nontification", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SqlCommand cmd = new SqlCommand("XoaTG", conn1);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter p = new SqlParameter("@MaTG", txtMaTG.Text);
                cmd.Parameters.Add(p);

                int count = cmd.ExecuteNonQuery();

                if (count > 0)
                {
                    MessageBox.Show("Đã xóa", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else MessageBox.Show("Không thể xóa", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            txtMaTG.Text = "";
            txtTenTG.Text = "";
            txtGhiChu.Text = "";
        }

        private void bntLuu_Click(object sender, EventArgs e)
        {
            //Sinh mã tự tăng
            int count1 = 0;
            count1 = dataGridView1.Rows.Count; //đếm tất cả các dòng trong datagridview rồi gán cho count
            string c1 = "";
            int c2 = 0;
            c1 = Convert.ToString(dataGridView1.Rows[count1 - 2].Cells[1].Value);
            c2 = Convert.ToInt32((c1.Remove(0, 4)));//loại bỏ kí tự TG
            if (c2 + 1 < 10)
            {
                txtMaTG.Text = "TG110" + (c2 + 1).ToString();
            }
            else if (c2 + 1 < 100)
            {
                txtMaTG.Text = "TG11" + (c2 + 1).ToString();
            }
            //Kiểm tra dữ liệu trước khi Thêm vào DataGridview
            if (txtTenTG.Text.Trim() == "")
            {
                MessageBox.Show("Không được để trống !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ActiveControl = txtTenTG;
                return;
            }
            // Khai báo và khởi tạo đối tượng Command, truyền vào tên thủ tục tương ứng
            SqlCommand cmd = new SqlCommand("ThemTG", conn1);
            // Khai báo kiểu thực thi là Thực thi thủ tục
            cmd.CommandType = CommandType.StoredProcedure;
            // Khai báo và gán giá trị cho các tham số đầu vào của thủ tục
            // Khai báo tham số thứ nhất @Name - là tên tham số được tạo trong thủ tục
            SqlParameter p = new SqlParameter("@MaTG", txtMaTG.Text);
            cmd.Parameters.Add(p);
            // Khởi tạo tham số thứ 2 trong thủ tục là @Name
            p = new SqlParameter("@TenTG", txtTenTG.Text);
            cmd.Parameters.Add(p);
            // Khởi tạo tham số thứ 3 trong thủ tục là @Color
            p = new SqlParameter("@GhiChu", txtGhiChu.Text);
            cmd.Parameters.Add(p);
            // Thực thi thủ tục
            int count = cmd.ExecuteNonQuery();
            if (count > 0)
            {
                MessageBox.Show("Đã thêm", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            else { MessageBox.Show("không thể thêm", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            bntLuu.Enabled = false;
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            conn1 = new SqlConnection(strConn);
            conn1.Open();
            SqlDataAdapter da = new SqlDataAdapter("select * from TacGia where TenTG like '" +"%"+ txtTimKiem.Text + "%'", conn1);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            conn1.Close();
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].Cells["clSTT"].Value = e.RowIndex + 1;
        }
    }
 }
