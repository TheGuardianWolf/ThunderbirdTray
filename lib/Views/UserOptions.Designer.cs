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
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trayBirdBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkboxMinimiseThundebird);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(258, 85);
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
            this.checkboxMinimiseThundebird.Location = new System.Drawing.Point(17, 34);
            this.checkboxMinimiseThundebird.Name = "checkboxMinimiseThundebird";
            this.checkboxMinimiseThundebird.Size = new System.Drawing.Size(232, 21);
            this.checkboxMinimiseThundebird.TabIndex = 0;
            this.checkboxMinimiseThundebird.Text = "Minimise Thunderbird on launch";
            this.checkboxMinimiseThundebird.UseVisualStyleBackColor = true;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(84, 311);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(90, 30);
            this.SaveButton.TabIndex = 1;
            this.SaveButton.Text = "OK";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // DiscardButton
            // 
            this.DiscardButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DiscardButton.Location = new System.Drawing.Point(180, 311);
            this.DiscardButton.Name = "DiscardButton";
            this.DiscardButton.Size = new System.Drawing.Size(90, 30);
            this.DiscardButton.TabIndex = 2;
            this.DiscardButton.Text = "Cancel";
            this.DiscardButton.UseVisualStyleBackColor = true;
            this.DiscardButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // UserOptions
            // 
            this.AcceptButton = this.SaveButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 353);
            this.Controls.Add(this.DiscardButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserOptions";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ThunderbirdTray Options";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trayBirdBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkboxMinimiseThundebird;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button DiscardButton;
        private System.Windows.Forms.BindingSource trayBirdBindingSource;
    }
}