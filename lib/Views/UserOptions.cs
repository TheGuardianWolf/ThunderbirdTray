using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThunderbirdTray.Views
{
    public partial class UserOptions : Form
    {
        public UserOptions()
        {
            InitializeComponent();
            HookMethodComboBox.SelectedIndex = Properties.Settings.Default.HookMethod;
            ThunderbirdPathTextbox.Text = Properties.Settings.Default.ThunderbirdPath;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            var hasPathChanged = Properties.Settings.Default.ThunderbirdPath != ThunderbirdPathTextbox.Text;

            Properties.Settings.Default.HookMethod = HookMethodComboBox.SelectedIndex;
            Properties.Settings.Default.ThunderbirdPath = ThunderbirdPathTextbox.Text;
            Properties.Settings.Default.Save();

            if (hasPathChanged)
                MessageBox.Show("Thunderbird path has been changed. ThunderbirdTray may require a restart.");

            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SelectPathButton_Click(object sender, EventArgs e)
        {
            var picker = new OpenFileDialog
            {
                CheckFileExists = true,
                Filter = "Thunderbird (thunderbird.exe)|thunderbird.exe"
            };

            if (picker.ShowDialog() == DialogResult.OK)
                ThunderbirdPathTextbox.Text = picker.FileName;
        }
    }
}
