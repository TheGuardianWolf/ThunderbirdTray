namespace ThunderbirdTray.Views
{
    partial class UserOptions
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserOptions));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkboxMinimiseThundebird = new System.Windows.Forms.CheckBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.DiscardButton = new System.Windows.Forms.Button();
            this.trayBirdBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.HookMethodComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.SelectPathButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ThunderbirdPathTextbox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trayBirdBindingSource)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkboxMinimiseThundebird);
            this.groupBox1.Location = new System.Drawing.Point(9, 10);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(194, 57);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Startup";
            // 
            // checkboxMinimiseThundebird
            // 
            this.checkboxMinimiseThundebird.AutoSize = true;
            this.checkboxMinimiseThundebird.Checked = global::ThunderbirdTray.Properties.Settings.Default.MinimiseOnStart;
            this.checkboxMinimiseThundebird.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkboxMinimiseThundebird.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ThunderbirdTray.Properties.Settings.Default, "MinimiseOnStart", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkboxMinimiseThundebird.Location = new System.Drawing.Point(13, 28);
            this.checkboxMinimiseThundebird.Margin = new System.Windows.Forms.Padding(2);
            this.checkboxMinimiseThundebird.Name = "checkboxMinimiseThundebird";
            this.checkboxMinimiseThundebird.Size = new System.Drawing.Size(176, 17);
            this.checkboxMinimiseThundebird.TabIndex = 0;
            this.checkboxMinimiseThundebird.Text = "Minimise Thunderbird on launch";
            this.checkboxMinimiseThundebird.UseVisualStyleBackColor = true;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(63, 253);
            this.SaveButton.Margin = new System.Windows.Forms.Padding(2);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(68, 24);
            this.SaveButton.TabIndex = 1;
            this.SaveButton.Text = "OK";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // DiscardButton
            // 
            this.DiscardButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DiscardButton.Location = new System.Drawing.Point(135, 253);
            this.DiscardButton.Margin = new System.Windows.Forms.Padding(2);
            this.DiscardButton.Name = "DiscardButton";
            this.DiscardButton.Size = new System.Drawing.Size(68, 24);
            this.DiscardButton.TabIndex = 2;
            this.DiscardButton.Text = "Cancel";
            this.DiscardButton.UseVisualStyleBackColor = true;
            this.DiscardButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.HookMethodComboBox);
            this.groupBox2.Location = new System.Drawing.Point(9, 72);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(194, 59);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Window Detection Method";
            // 
            // HookMethodComboBox
            // 
            this.HookMethodComboBox.FormattingEnabled = true;
            this.HookMethodComboBox.Items.AddRange(new object[] {
            "UI Automation",
            "Polling"});
            this.HookMethodComboBox.Location = new System.Drawing.Point(13, 25);
            this.HookMethodComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.HookMethodComboBox.Name = "HookMethodComboBox";
            this.HookMethodComboBox.Size = new System.Drawing.Size(175, 21);
            this.HookMethodComboBox.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.SelectPathButton);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.ThunderbirdPathTextbox);
            this.groupBox3.Location = new System.Drawing.Point(9, 135);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(194, 93);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Thunderbird Path Override";
            // 
            // SelectPathButton
            // 
            this.SelectPathButton.Location = new System.Drawing.Point(113, 64);
            this.SelectPathButton.Name = "SelectPathButton";
            this.SelectPathButton.Size = new System.Drawing.Size(75, 23);
            this.SelectPathButton.TabIndex = 7;
            this.SelectPathButton.Text = "Select";
            this.SelectPathButton.UseVisualStyleBackColor = true;
            this.SelectPathButton.Click += new System.EventHandler(this.SelectPathButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label1.Location = new System.Drawing.Point(10, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Supports environment variables";
            // 
            // ThunderbirdPathTextbox
            // 
            this.ThunderbirdPathTextbox.Location = new System.Drawing.Point(13, 38);
            this.ThunderbirdPathTextbox.Name = "ThunderbirdPathTextbox";
            this.ThunderbirdPathTextbox.Size = new System.Drawing.Size(175, 20);
            this.ThunderbirdPathTextbox.TabIndex = 5;
            // 
            // UserOptions
            // 
            this.AcceptButton = this.SaveButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(212, 287);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.DiscardButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserOptions";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ThunderbirdTray Options";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trayBirdBindingSource)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkboxMinimiseThundebird;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button DiscardButton;
        private System.Windows.Forms.BindingSource trayBirdBindingSource;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox HookMethodComboBox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button SelectPathButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ThunderbirdPathTextbox;
    }
}