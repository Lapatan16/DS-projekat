using System;
using System.Drawing;
using System.Windows.Forms;
using TouristAgencyApp.Models;
using System.Linq;

namespace TouristAgencyApp.Forms
{
    partial class ClientsForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            
            // Form properties
            this.Text = "👥 Upravljanje klijentima";
            this.Width = 1400;
            this.Height = 700;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Initialize controls
            InitializeForm();
            CreateModernUI();
        }
    }
} 