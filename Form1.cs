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
    public partial class Form1 : Form
    {
        private Database1Entities db = new Database1Entities();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://www.google.com");
            LoadCategories();
            //LoadSites();
        }

        private void LoadCategories()
        {
            var categoriesList = db.Categories.ToList();
            comboBox1.DataSource = categoriesList;
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "Id";
            comboBox1.SelectedIndex = 0;
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            urlBox.Text = webBrowser1.Url.ToString();
        }

        //private void urlBox_Click(object sender, EventArgs e)
        //{
        //    bool check = selectBoxText(urlBox);
        //    if (check)
        //    {
        //        urlBox.DeselectAll();
        //    }
        //    else
        //    {
        //        urlBox.SelectAll();
        //    }
        //}

        private bool selectBoxText(TextBox text)
        {
            text.SelectAll();
            return true;
        }

        private void GoToSite()
        {
            string address = urlBox.Text;
            if (address == "")
            {
                MessageBox.Show("Не указан адрес сайта", "Предупреждение", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else
            {
                if (address.IndexOf(".") == -1)
                {
                    address = $"https://www.google.com/search?q={address}";
                }
                else if(address.IndexOf("http") == -1)
                {
                    address = $"https://{address}";
                }
                webBrowser1.Navigate(address);
            }
        }

        private void goPageBtn_Click(object sender, EventArgs e)
        {
            GoToSite();
        }

        private void urlBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GoToSite();
            }
        }

        private void prevPageBtn_Click(object sender, EventArgs e)
        {
            if (webBrowser1.CanGoBack)
            {
                webBrowser1.GoBack();
            }
        }

        private void nextPageBtn_Click(object sender, EventArgs e)
        {
            if (webBrowser1.CanGoForward)
            {
                webBrowser1.GoForward();
            }
        }

        private void RefreshPageBtn_Click(object sender, EventArgs e)
        {
            webBrowser1.Refresh();
        }

        private void homePageBtn_Click(object sender, EventArgs e)
        {
            webBrowser1.GoHome();
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int k = listBox1.SelectedIndex;
            if (k != -1)
            {
                var url = (listBox1.SelectedItem as Sites).Address;
                webBrowser1.Navigate(url);
            }
        }

        private void addCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CategoriesEditor editor = new CategoriesEditor();
            editor.ButtonName = "Добавить категорию";
            if (editor.ShowDialog() == DialogResult.OK)
            {
                db.Categories.Add(new Categories() {Name = editor.CategoryName});
                db.SaveChanges();
                MessageBox.Show("Категория успешно добавлена!", "Сообщение", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                LoadCategories();
            }
        }

        private void delCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CategoriesEditor editor = new CategoriesEditor();
            editor.ButtonName = "Удалить категорию";
            if (editor.ShowDialog() == DialogResult.OK)
            {
                var category = db.Categories.Where(c => c.Name == editor.CategoryName).FirstOrDefault();
                if (category != null)
                {
                    db.Categories.Remove(category);
                    db.SaveChanges();
                    MessageBox.Show("Категория успешно Удалена!", "Сообщение", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    LoadCategories();
                }
                //db.Categories.Remove(new Categories() { Name = editor.CategoryName });
                //db.SaveChanges();
            }
        }

        private void addSiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SitesEditor editor = new SitesEditor(comboBox1, true);
            editor.ActionText = "Добавить сайт в категорию";
            editor.DB = db;
            if (editor.ShowDialog() == DialogResult.OK)
            {
                db = editor.DB;
                db.SaveChanges(); 
                LoadCategories();
            }
        }

        private void delSiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SitesEditor editor = new SitesEditor(comboBox1, false);
            editor.ActionText = "Удалить сайт из категории";
            editor.DB = db;
            if (editor.ShowDialog() == DialogResult.OK)
            {
                db = editor.DB;
                db.SaveChanges();
                LoadCategories();
            }
        }
    }
}
