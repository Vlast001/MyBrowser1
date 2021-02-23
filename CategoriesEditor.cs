using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyBrowser1
{
    public partial class CategoriesEditor : Form
    {
        public string CategoryName { get; set; }
        public string ButtonName { get; set; }

        public CategoriesEditor()
        {
            InitializeComponent();
        }

        private void actionBtn_Click(object sender, EventArgs e)
        {
            if (categoryBox.Text == "")
            {
                MessageBox.Show("Вы не ввели название категории!", "Предупреждение", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else
            {
                CategoryName = categoryBox.Text;
                this.DialogResult = DialogResult.OK;
            }
        }

        private void CategoriesEditor_Load(object sender, EventArgs e)
        {
            actionBtn.Text = ButtonName;
        }
    }
}
