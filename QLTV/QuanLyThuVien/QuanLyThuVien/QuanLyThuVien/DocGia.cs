﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace TUNG
{
    public partial class DocGia : Form
    {
        public DocGia()
        {
            InitializeComponent();
        }
        string strConn = @"Data Source=DESKTOP-SPIMD63;Initial Catalog=QuanLyThuVien;Integrated Security=True";
        SqlConnection conn;
        private void LoadData()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM DocGia", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void DocGia_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(strConn);
            conn.Open();
            LoadData();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                MaDG.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["clMa"].Value);
                TenDG.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["clTen"].Value);
                Address.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["clAdd"].Value);
                SDT.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["clSDT"].Value);
                ngaySinh.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["clNS"].Value);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muôn xóa bản ghi đang chọn không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SqlCommand cmd = new SqlCommand("XoaDocGia", conn);
                cmd.CommandType = CommandType.StoredProcedure;



                SqlParameter p = new SqlParameter("@Ma", MaDG.Text);
                cmd.Parameters.Add(p);

                int count = cmd.ExecuteNonQuery();

                if (count > 0)
                {
                    MessageBox.Show("Xóa thành công!");
                    LoadData();
                }
                else MessageBox.Show("Không thể xóa bản ghi hiện thời!");
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            //sinh ma tu tang
            int count1 = 0;
            count1 = dataGridView1.Rows.Count;//dem cac dong trong datagridview roi gan cho count
            string c1 = "";
            int c2 = 0;
            c1 = Convert.ToString(dataGridView1.Rows[count1-2].Cells[1].Value);

            c2 = Convert.ToInt32(c1.Remove(0,4));
            if(c2+1<10)
            {
                MaDG.Text="DG110"+(c2+1).ToString();
            }
            else if(c2+1<100)
            {
                MaDG.Text="DG11" + (c2+1).ToString();

            }

            SqlCommand cmd = new SqlCommand("ThemDocGia", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter p = new SqlParameter("@Ma", MaDG.Text);
            cmd.Parameters.Add(p);

            p = new SqlParameter("@NS", ngaySinh.Value);
            cmd.Parameters.Add(p);

            p = new SqlParameter("@Ten", TenDG.Text);
            cmd.Parameters.Add(p);

            p = new SqlParameter("@Add", Address.Text);
            cmd.Parameters.Add(p);

            p = new SqlParameter("@SDT", SDT.Text);
            cmd.Parameters.Add(p);
            // Thực thi thủ tục
            int count = cmd.ExecuteNonQuery();
            if (count > 0)
            {
                MessageBox.Show("Thêm mới thành công");
                LoadData();
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SuaDocGia", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter p = new SqlParameter("@Ma", MaDG.Text);
            cmd.Parameters.Add(p);

            p = new SqlParameter("@Ten", TenDG.Text);
            cmd.Parameters.Add(p);

            p = new SqlParameter("@NS", ngaySinh.Value);
            cmd.Parameters.Add(p);

            p = new SqlParameter("@Add", Address.Text);
            cmd.Parameters.Add(p);

            p = new SqlParameter("@SDT", SDT.Text);
            cmd.Parameters.Add(p);

            int count = cmd.ExecuteNonQuery();

            if (count > 0)
            {
                MessageBox.Show("Sửa thành công!");
                LoadData();
            }
            else MessageBox.Show("Không sửa được!");
        }

        private void buttonThem_Click(object sender, EventArgs e)
        {
            MaDG.Enabled = false;
            TenDG.Text = "";
            buttonAdd.Enabled = true;
            
            Address.Text = "";
            SDT.Text = "";
            MaDG.Focus();
        }
        public void CheckExist(DataTable tbl,string filterExpression)
        {
            if (filterExpression =="")
            {
                //LoadData();
            }
            DataRow[] rows = tbl.Select(filterExpression);
            if(rows.Length <=0)
            {
                MessageBox.Show("Không tìm thấy", "Nontification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            tbl = ((DataTable)this.dataGridView1.DataSource).Clone();
            for(int i=0; i<rows.Length;i++)
            {
                DataRow row = tbl.NewRow();
                row[0] = rows[i].ItemArray[0].ToString();
                row[1] = rows[i].ItemArray[1].ToString();
                row[2] = rows[i].ItemArray[2].ToString();
                row[3] = rows[i].ItemArray[3].ToString();
                row[4] = rows[i].ItemArray[4].ToString();
                tbl.Rows.Add(row);
            }
            dataGridView1.DataSource = tbl; 
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            string filter = "TenDG='" + Search.Text + "'";
            CheckExist((DataTable)this.dataGridView1.DataSource,filter);
            if (Search.Text == "") { LoadData(); }
            
        }

       
        private void Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string filter = "TenDG='" + Search.Text + "'";
                CheckExist((DataTable)this.dataGridView1.DataSource, filter);
                if (Search.Text == "") { LoadData(); }
                Search.Focus();
            }

        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].Cells["clNo"].Value = e.RowIndex + 1;
        }

        private void Search_TextChanged(object sender, EventArgs e)
        {
            conn = new SqlConnection(strConn);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter("select * from DocGia where TenDG like '" + "%" + Search.Text + "%'", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
        }
     }
}
