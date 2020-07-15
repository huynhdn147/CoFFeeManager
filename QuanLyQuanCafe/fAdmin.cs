using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();
        BindingSource tableList = new BindingSource();
        BindingSource accountList = new BindingSource();
        BindingSource categoryList = new BindingSource();
        public Account loginAccount;
        public fAdmin()
        {
            InitializeComponent();
            LoadData();
        }

        #region methods

        List<Food> SearchFoodByName(string name)
        {
            List<Food> listFood = FoodDAO.Instance.SearchFoodByName(name);

            return listFood;
        }
        void LoadData()
        {
            dtgvTable.DataSource = tableList;
            dtgvFood.DataSource = foodList;
            dtgvAccount1.DataSource = accountList;
            dtgvCategory.DataSource = categoryList;
            addCategoryBinding();
            LoadDateTimePickerBill();
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            LoadListFood();
            LoadAccount();
            LoadCategoryIntoCombobox(cbFoodCategory);
            AddFoodBinding();
            AddAccountBinding();
            LoadListCategory();
            LoadListTable();
            AddtableBinding();
            
        }
        
        void addCategoryBinding()
        {
            txbCategoryName.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "name", true, DataSourceUpdateMode.Never));
            txbCategoryID.DataBindings.Add(new Binding("TexT", dtgvCategory.DataSource, "id"));
            
        }
        void LoadListCategory()
        {
            categoryList.DataSource = CategoryDAO.Instance.GetListCategory();
        }
        void AddAccountBinding()
        {
                txbUserName1.DataBindings.Add(new Binding("Text", dtgvAccount1.DataSource, "UserName", true, DataSourceUpdateMode.Never));
                txbDisplayName1.DataBindings.Add(new Binding("Text", dtgvAccount1.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
                numericUpDown1.DataBindings.Add(new Binding("Value", dtgvAccount1.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }
        void AddtableBinding()
        {
            txbTableID.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "id", true, DataSourceUpdateMode.Never));
            txbTableName.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "name", true, DataSourceUpdateMode.Never));
            nmrStatus.DataBindings.Add(new Binding("Value", dtgvTable.DataSource, "status", true, DataSourceUpdateMode.Never));
        }

        void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }
        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
        }

        void AddFoodBinding()
        {
            txbFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }

        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "ID";
        }
        void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();
        }

        void AddAccount(string userName, string displayName, int type)
        {
            try
            {
                if (userName != "" && displayName != "")
                {
                    AccountDAO.Instance.InsertAccount(userName, displayName, type);
                    MessageBox.Show("Thêm tài khoản thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    MessageBox.Show("Thêm tài khoản thất bại");
                }
           }
             catch
            {
                MessageBox.Show("Không thể thêm tài khoản đã tồn tại","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

            LoadAccount();
        }

        void EditAccount(string userName, string displayName, int type)
        {
            if (userName != "" && displayName!="")
            {
                AccountDAO.Instance.UpdateAccount(userName, displayName, type);
                MessageBox.Show("Cập nhật tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật tài khoản thất bại");
            }

            LoadAccount();
        }

        void DeleteAccount(string userName)
        {
            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Vui lòng đừng xóa chính bạn chứ");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại");
            }

            LoadAccount();
        }

        void ResetPass(string userName)
        {
            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại");
            }
        }
        #endregion

        #region events
        private void btnAddAccount_Click(object sender, EventArgs e)
        {
           txbUserName1.Text="";
           txbDisplayName1.Text="";
           numericUpDown1.Value=1;

           
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName1.Text;

            DeleteAccount(userName);
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName1.Text;
            string displayName = txbDisplayName1.Text;
            int type = (int)numericUpDown1.Value;

            EditAccount(userName, displayName, type);
        }


        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string userName = txbUserName1.Text;

            ResetPass(userName);
        }


        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        
        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            foodList.DataSource = SearchFoodByName(txbSearchFoodName.Text);
        }
        private void txbFoodID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;

                    Category cateogory = CategoryDAO.Instance.GetCategoryByID(id);

                    cbFoodCategory.SelectedItem = cateogory;

                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbFoodCategory.Items)
                    {
                        if (item.ID == cateogory.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }

                    cbFoodCategory.SelectedIndex = index;
                }
            }
            catch { }
        }
        private void bntAdd_Click(object sender, EventArgs e)
        {
            pnID.Visible = true;
            bntAdd.Visible = false;
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;

            if (txbFoodName.Text !="" ||cbFoodCategory.Text !="")
            {   FoodDAO.Instance.InsertFood(name, categoryID, price);
                MessageBox.Show("Thêm món thành công");
                LoadListFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm thức ăn");
            }
        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            pnID.Visible = false;
            bntAdd.Visible = true;
            txbFoodName.Text = "";
            cbFoodCategory.Text = "";
            nmFoodPrice.Value = 0;
            
        }
        

        private void btnEditFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;
            int id = Convert.ToInt32(txbFoodID.Text);

            if (FoodDAO.Instance.UpdateFood(id, name, categoryID, price))
            {
                MessageBox.Show("Sửa món thành công");
                LoadListFood();
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa thức ăn");
            }
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbFoodID.Text);

            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa món thành công");
                LoadListFood();
                if (deleteFood != null)
                    deleteFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa thức ăn");
            }
        }
        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }
       
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            
           
        }

        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }

        #endregion              

        private void btnFristBillPage_Click(object sender, EventArgs e)
        {
            txbPageBill.Text = "1";
        }

        private void btnLastBillPage_Click(object sender, EventArgs e)
        {
            int sumRecord = BillDAO.Instance.GetNumBillListByDate(dtpkFromDate.Value, dtpkToDate.Value);

            int lastPage = sumRecord / 10;

            if (sumRecord % 10 != 0)
                lastPage++;

            txbPageBill.Text = lastPage.ToString();
        }

        private void txbPageBill_TextChanged(object sender, EventArgs e)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value, Convert.ToInt32(txbPageBill.Text));
        }

        private void btnPrevioursBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text);

            if (page > 1)
                page--;

            txbPageBill.Text = page.ToString();
        }

        private void btnNextBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text);
            int sumRecord = BillDAO.Instance.GetNumBillListByDate(dtpkFromDate.Value, dtpkToDate.Value);

            if (page < sumRecord)
                page++;

            txbPageBill.Text = page.ToString();
        }

        private void fAdmin_Load(object sender, EventArgs e)
        {
            
            this.USP_GetListBillByDateForReportTableAdapter.Fill(this.QuanLyQuanCafeDataSet.USP_GetListBillByDateForReport,dtpkFromDate.Value,dtpkToDate.Value);

            this.reportViewer1.RefreshReport();
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            txbCategoryID.Visible = false;
            
            txbCategoryName.Text = "";
            bntAccepCategory.Visible=true;
           
        }
        private void bntAccepCategory_Click(object sender, EventArgs e)
        {
            txbCategoryID.Visible = true;

            txbCategoryName.Text = "";
            bntAccepCategory.Visible = false;
            string categoryName = txbCategoryName.Text;
            if (txbCategoryName.Text!=""){
                CategoryDAO.Instance.insertCategory(categoryName);
            
                MessageBox.Show("Thêm Danh muc thành công");
                LoadListCategory();

              
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm Danh muc");
            }
        }
        //private event EventHandler insertCategory;
        //public event EventHandler insertCategory
        //{
        //    add { insertCategory += value; }
        //    remove { insertCategory -= value; }
        //}
        private void btnShowCategory_Click(object sender, EventArgs e)
        {
            LoadListCategory();
        }
        

        void LoadListTable()
        {
            tableList.DataSource = TableDAO.Instance.LoadTableList();
        }
        private void btnShowTable_Click(object sender, EventArgs e)
        {
            LoadListTable();
        }
        
        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            
            int id = Convert.ToInt32(txbCategoryID.Text);
            
            if(CategoryDAO.Instance.DeleteCategory(id))
            {
                MessageBox.Show("Xóa danh muc thành công");
                LoadListCategory();
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa danh muc");
            }
        }

        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbCategoryID.Text);
            string name = txbCategoryName.Text;
            if (CategoryDAO.Instance.UpdateCategory(id, name))
            {
                MessageBox.Show("Sửa món thành công");
                LoadListFood();
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa thức ăn");
            }
            LoadListCategory();
        }

        private void btnAddTable_Click(object sender, EventArgs e)
        {
            txbTableName.Text = "";

            panel13.Visible = true;
            panel14.Visible = true;
            bntAccepTable.Visible = true;
            
        }

        private void bntAccepTable_Click(object sender, EventArgs e)
        {

            //int tbID = Convert.ToInt32(txbTableID.Text);
            string tbName = txbTableName.Text;
            string stt;
            if (nmrStatus.Value == 0)
            {
                stt = "Trống";

            }
            else
            {
                stt = "Có người";
            }
            if (TableDAO.Instance.Inserttable( tbName, stt))
            {
                MessageBox.Show("Thêm bàn thành công", "Thông báo");
            }
            else
            {
                MessageBox.Show("Thêm bàn thất bại", "Thông báo");
            }
            LoadListTable();
            panel13.Visible = false;
            panel14.Visible = false;
            bntAccepTable.Visible = false;
            
        }

        private void btnDeleteTable_Click(object sender, EventArgs e)
        {

        }

        private void bntAcceptAccount_Click(object sender, EventArgs e)
        {
            if (txbUserName1.Text != "" || txbDisplayName1.Text !="")
            {
            string userName = txbUserName1.Text;
            string displayName = txbDisplayName1.Text;
            int type = (int)numericUpDown1.Value;

            AddAccount(userName, displayName, type);
            }
            else
            {
                MessageBox.Show("co loi khi them moi");
            }
            
        }
       
        

        


    }
}
