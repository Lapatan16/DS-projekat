using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TouristAgencyApp.Patterns;
using TouristAgencyApp.Services;

namespace TouristAgencyApp.Forms
{
    public partial class StartupForm : Form
    {
        public StartupForm()
        {
            InitializeComponent();
            LoadConfigs();       
        }

        private void LoadConfigs()
        {
            cmbConfigs.Items.Clear();

            var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "config*.txt");

            foreach (var file in files)
            {
                cmbConfigs.Items.Add(new ConfigItem
                {
                    DisplayName = Path.GetFileName(file),
                    FullPath = file                         
                });
            }

            if (cmbConfigs.Items.Count > 0)
                cmbConfigs.SelectedIndex = 0;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (cmbConfigs.SelectedItem is ConfigItem selected)
            {
                var config = new ConfigManager(selected.FullPath);
                DatabaseFactory.Reset();
                var db = DatabaseFactory.GetDatabaseService(selected.FullPath);
                var mainForm = new MainForm(db, config.AgencyName);
                this.Hide();
                mainForm.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Izaberite validan config fajl.");
            }
        }

        private class ConfigItem
        {
            public string DisplayName { get; set; } = "";
            public string FullPath { get; set; } = "";

            public override string ToString() => DisplayName;
        }
    }
}
