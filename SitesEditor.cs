using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyBrowser1
{
    public partial class SitesEditor : Form
    {
        public string ActionText { get; set; }
        public Database1Entities DB { get; set; }
        private bool check;

        public SitesEditor(ComboBox combo, bool ifYesAddIfNoRemove)
        {
            InitializeComponent();
            this.comboBox1.DataSource = (List<Categories>)combo.DataSource;
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "Id";
            comboBox1.SelectedIndex = 0;

            check = ifYesAddIfNoRemove;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int k = comboBox1.SelectedIndex;
            if (k != -1)
            {
                var sitesList = (comboBox1.SelectedItem as Categories).Sites.ToList();
                listBox1.DataSource = sitesList;
                listBox1.DisplayMember = "Name";
            }
        }

        private void SitesEditor_Load(object sender, EventArgs e)
        {
            this.actionButton.Text = ActionText;
            if (check)
            {
                SiteNameTextBox.Text = "";
                return;
            }
            if (!check)
            {
                SiteUrlTextBox.Visible = false;
                //SiteNameTextBox.Visible = false;
                //label1.Visible = false;
                SiteNameTextBox.ReadOnly = true;
                label1.Text = $"Текущий выбраный сайт:";
                label2.Text = $"Выбирите сайт и подтвердите{Environment.NewLine}удаление";
                //return;
            }
        }


        private bool IsUrlValid(string url)
        {
            return Regex.IsMatch(url, @"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
        }

        private Database1Entities ActionAdd(string address, string name, Database1Entities db)
        {
            
                db.Sites.Add(new Sites()
                {
                    Address = address, Name = name, CategoryId = comboBox1.SelectedIndex + 1,
                    Categories = comboBox1.SelectedItem as Categories
                });
                return db;
        }

        private Database1Entities ActionRemove(string name, Database1Entities db)
        {

            var currentSite = db.Sites.Where(s => s.Name == name).FirstOrDefault();
            db.Sites.Remove(currentSite);

            return db;
        }

        private void LoadCategories()
        {
            var categoriesList = DB.Categories.ToList();
            comboBox1.DataSource = categoriesList;
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "Id";
            //comboBox1.SelectedIndex = 0;
        }

        private void actionButton_Click(object sender, EventArgs e)
        {
            string siteName;
            if (check)
            {
                string address = SiteUrlTextBox.Text;
                siteName = SiteNameTextBox.Text;
                if (address == "" || siteName == "")
                {
                    MessageBox.Show("Не указан адрес сайта или имя сайта", "Предупреждение", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    if (address.IndexOf("http") == -1)
                    {
                        address = $"https://{address}";
                    }
                    if (IsUrlValid(address))
                    {
                        DB = ActionAdd(address, siteName, DB);
                        LoadCategories();
                    }
                    else
                    {
                        MessageBox.Show("Адресс сайта не валидный, попробуйте еще", "Предупреждение", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                siteName = (listBox1.SelectedItem as Sites).Name;
                DB = ActionRemove(siteName, DB);
                LoadCategories();
            }
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!check)
            {
                SiteNameTextBox.Text = (listBox1.SelectedItem as Sites).Name;
            }
        }
    }
}
