using System;
using System.ComponentModel.Design;
using static System.Collections.Specialized.BitVector32;

namespace PasswordManager
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private Panel loginPanel;
        private Panel vaultPanel;
        private TextBox masterPasswordBox;
        private Button unlockButton;
        private Button initButton;
        private Label statusLabel;
        private Label errorLabel;
        private ListView vaultList;
        private Button addButton;
        private Button deleteButton;
        private Button showSecretButton;
        private Button lockButton;
        private TextBox titleTextBox;
        private TextBox usernameTextBox;
        private TextBox secretTextBox;
        private ComboBox secretTypeComboBox;
        private Label titleLabel;
        private Label usernameLabel;
        private Label secretLabel;
        private Label typeLabel;
        private Label secretDisplayLabel;
        private Label footerLabel;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.loginPanel = new Panel();
            this.unlockButton = new Button();
            this.initButton = new Button();
            this.masterPasswordBox = new TextBox();
            this.statusLabel = new Label();
            this.errorLabel = new Label();
            this.vaultPanel = new Panel();
            this.secretDisplayLabel = new Label();
            this.showSecretButton = new Button();
            this.deleteButton = new Button();
            this.vaultList = new ListView();
            this.addButton = new Button();
            this.secretTypeComboBox = new ComboBox();
            this.secretTextBox = new TextBox();
            this.usernameTextBox = new TextBox();
            this.titleTextBox = new TextBox();
            this.typeLabel = new Label();
            this.secretLabel = new Label();
            this.usernameLabel = new Label();
            this.titleLabel = new Label();
            this.lockButton = new Button();
            this.footerLabel = new Label();

            this.loginPanel.SuspendLayout();
            this.vaultPanel.SuspendLayout();
            this.SuspendLayout();

            // FIXED SIZE 1600x1200 - NO RESIZE
            this.ClientSize = new Size(1200, 800);
            this.MinimumSize = new Size(1200, 800);
            this.MaximumSize = new Size(1200, 800);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Text = "Password Vault - By Nirav Madariya";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(253, 253, 253);  // OFF-WHITE

            // LOGIN PANEL (Perfectly centered for 1600px)
            this.loginPanel.Size = new Size(400, 500);
            this.loginPanel.Location = new Point(400, 150);
            this.loginPanel.Dock = DockStyle.Fill;
            this.loginPanel.BackColor = Color.White;
            this.loginPanel.BorderStyle = BorderStyle.FixedSingle;
            this.loginPanel.Controls.AddRange(new Control[] {
                this.statusLabel, this.masterPasswordBox,
                this.unlockButton, this.initButton, this.errorLabel });

            this.statusLabel.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.statusLabel.ForeColor = Color.DarkBlue;
            this.statusLabel.Size = new Size(50, 60);
            this.statusLabel.Location = new Point(300, 40);
            //this.statusLabel.Text = "statusLabel";

            this.masterPasswordBox.Font = new Font("Segoe UI", 14F);
            this.masterPasswordBox.Size = new Size(50, 120);
            this.masterPasswordBox.Location = new Point(300, 45);
            this.masterPasswordBox.UseSystemPasswordChar = true;
            this.masterPasswordBox.PlaceholderText = "Enter master password";

            this.unlockButton.Font = new Font("Segoe UI", 12F);
            this.unlockButton.Size = new Size(125, 200);
            this.unlockButton.Location = new Point(150, 45);  // 550 + 140
            this.unlockButton.Text = "Unlock";
            this.unlockButton.BackColor = Color.DodgerBlue;
            this.unlockButton.ForeColor = Color.White;
            this.unlockButton.FlatStyle = FlatStyle.Flat;
            this.unlockButton.Click += new EventHandler(this.unlockButton_Click);

            this.initButton.Font = new Font("Segoe UI", 11F);
            this.initButton.Size = new Size(125, 200);
            this.initButton.Location = new Point(150, 45);  // 550 + 70
            this.initButton.Text = "Initialize New Vault";
            this.initButton.Visible = false;
            this.initButton.BackColor = Color.DarkGreen;
            this.initButton.ForeColor = Color.White;
            this.initButton.FlatStyle = FlatStyle.Flat;
            this.initButton.Click += new EventHandler(this.initButton_Click);

            this.errorLabel.Font = new Font("Segoe UI", 16F);
            this.errorLabel.ForeColor = Color.Red;
            this.errorLabel.Location = new Point(260, 40);

            // NAV PANEL (Left side)
            var navPanel = new Panel();
            navPanel.Location = new Point(0, 0);
            navPanel.Size = new Size(180, 800);
            navPanel.BackColor = Color.FromArgb(245, 245, 245);

            // NAV HEADER
            var navHeader = new Label
            {
                Text = "Categories",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Location = new Point(20, 20),
                Size = new Size(140, 30)
            };
            navPanel.Controls.Add(navHeader);

            // ALL button
            var btnAll = new Button
            {
                Text = "All",
                Tag = null, // null = no filter
                Font = new Font("Segoe UI", 11F),
                Location = new Point(20, 70),
                Size = new Size(140, 35),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White
            };
            btnAll.Click += NavFilterButton_Click;
            navPanel.Controls.Add(btnAll);

            // Password button
            var btnPassword = new Button
            {
                Text = "Passwords",
                Tag = SecretType.Password,
                Font = new Font("Segoe UI", 11F),
                Location = new Point(20, 115),
                Size = new Size(140, 35),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White
            };
            btnPassword.Click += NavFilterButton_Click;
            navPanel.Controls.Add(btnPassword);

            // Passcode button
            var btnPasscode = new Button
            {
                Text = "Passcodes",
                Tag = SecretType.Passcode,
                Font = new Font("Segoe UI", 11F),
                Location = new Point(20, 160),
                Size = new Size(140, 35),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White
            };
            btnPasscode.Click += NavFilterButton_Click;
            navPanel.Controls.Add(btnPasscode);

            // PIN button
            var btnPin = new Button
            {
                Text = "PINs",
                Tag = SecretType.Pin,
                Font = new Font("Segoe UI", 11F),
                Location = new Point(20, 205),
                Size = new Size(140, 35),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White
            };
            btnPin.Click += NavFilterButton_Click;
            navPanel.Controls.Add(btnPin);

            // Add navPanel to vaultPanel
            this.vaultPanel.Controls.Add(navPanel);

            // VAULT PANEL (Perfect 1600px layout)
            this.vaultPanel.Dock = DockStyle.Fill;
            this.vaultPanel.BackColor = Color.FromArgb(253, 253, 253);
            this.vaultPanel.Visible = false;

            // VAULT PANEL - 5 COLUMN LIST
            this.vaultList.Location = new Point(200, 80);
            this.vaultList.Size = new Size(900, 550);
            this.vaultList.View = View.Details;
            this.vaultList.FullRowSelect = true;
            this.vaultList.GridLines = true;
            this.vaultList.Font = new Font("Segoe UI", 11F);
            this.vaultList.BackColor = Color.White;
            this.vaultList.Columns.Add("Title", 300);
            this.vaultList.Columns.Add("Username", 250);
            
            //this.vaultList.Columns.Add("Type", 120);
            this.vaultList.Columns.Add("View", 50);
            this.vaultList.Columns.Add("Edit", 50);
            this.vaultList.Columns.Add("Delete", 50);
            this.vaultList.Columns.Add("Last Modified", 120);
            this.vaultList.MouseMove += vaultList_MouseMove;
            this.vaultList.MouseLeave += vaultList_MouseLeave;
            this.vaultPanel.Controls.Add(this.vaultList);

            //// Title
            //this.titleLabel.Text = "Title:";
            //this.titleLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            //this.titleLabel.Location = new Point(rightX, 30);
            ////this.vaultPanel.Controls.Add(this.titleLabel);

            //this.titleTextBox.Font = new Font("Segoe UI", 12F);
            //this.titleTextBox.Location = new Point(rightX, 65);
            //this.titleTextBox.Size = new Size(450, 50);
            ////this.vaultPanel.Controls.Add(this.titleTextBox);

            //// Username
            //this.usernameLabel.Text = "Username:";
            //this.usernameLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            //this.usernameLabel.Location = new Point(rightX, 140);
            ////this.vaultPanel.Controls.Add(this.usernameLabel);

            //this.usernameTextBox.Font = new Font("Segoe UI", 12F);
            //this.usernameTextBox.Location = new Point(rightX, 175);
            //this.usernameTextBox.Size = new Size(450, 50);
            ////this.vaultPanel.Controls.Add(this.usernameTextBox);

            //// Type
            //this.typeLabel.Text = "Type:";
            //this.typeLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            //this.typeLabel.Location = new Point(rightX, 250);
            ////this.vaultPanel.Controls.Add(this.typeLabel);

            //this.secretTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            //this.secretTypeComboBox.Font = new Font("Segoe UI", 10F);
            //this.secretTypeComboBox.Items.AddRange(new object[] { "Password", "Passcode", "PIN" });
            //this.secretTypeComboBox.SelectedIndex = 0;
            //this.secretTypeComboBox.Location = new Point(rightX, 285);
            //this.secretTypeComboBox.Size = new Size(450, 55);
            //this.vaultPanel.Controls.Add(this.secretTypeComboBox);

            // Secret
            //this.secretLabel.Text = "Secret:";
            //this.secretLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            //this.secretLabel.Location = new Point(rightX, 360);
            //this.vaultPanel.Controls.Add(this.secretLabel);

            //this.secretTextBox.Font = new Font("Segoe UI", 12F);
            //this.secretTextBox.Location = new Point(rightX, 395);
            //this.secretTextBox.Size = new Size(450, 50);
            //this.secretTextBox.UseSystemPasswordChar = true;
            //this.vaultPanel.Controls.Add(this.secretTextBox);

            // ROUND ADD BUTTON (Bottom Right)
            this.addButton.Size = new Size(70, 70);
            this.addButton.Location = new Point(1090, 680);
            this.addButton.Text = "➕";
            this.addButton.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            this.addButton.BackColor = Color.DodgerBlue;
            this.addButton.ForeColor = Color.White;
            this.addButton.FlatStyle = FlatStyle.Flat;
            this.addButton.FlatAppearance.BorderSize = 0;
            this.addButton.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, 70, 70, 35, 35));
            this.addButton.Click += new EventHandler(this.addButton_Click);
            this.vaultPanel.Controls.Add(this.addButton);

            //this.deleteButton.Font = new Font("Segoe UI", 12F);
            //this.deleteButton.Size = new Size(140, 60);
            //this.deleteButton.Text = "🗑️";
            //this.deleteButton.BackColor = Color.DarkRed;
            //this.deleteButton.ForeColor = Color.White;
            //this.deleteButton.FlatStyle = FlatStyle.Flat;
            //this.deleteButton.Location = new Point(rightX, 200);
            //this.deleteButton.Click += new EventHandler(this.deleteButton_Click);
            //this.vaultPanel.Controls.Add(this.deleteButton);

            //this.showSecretButton.Font = new Font("Segoe UI", 12F);
            //this.showSecretButton.Size = new Size(450, 65);
            //this.showSecretButton.Text = "Show Selected Secret";
            //this.showSecretButton.BackColor = Color.DodgerBlue;
            //this.showSecretButton.ForeColor = Color.White;
            //this.showSecretButton.FlatStyle = FlatStyle.Flat;
            //this.showSecretButton.Location = new Point(rightX, 300);
            //this.showSecretButton.Click += new EventHandler(this.showSecretButton_Click);
            //this.vaultPanel.Controls.Add(this.showSecretButton);

            // 4. SECRET DISPLAY (Hidden until eye clicked)
            this.secretDisplayLabel.Visible = false;
            this.secretDisplayLabel.Location = new Point(80, 200);
            this.secretDisplayLabel.Size = new Size(1400, 900);
            this.secretDisplayLabel.BackColor = Color.FromArgb(0, 0, 0, 0);  // Transparent overlay
            this.vaultPanel.Controls.Add(this.secretDisplayLabel);

            // Lock + Footer
            // LOCK BUTTON (Top Right)
            this.lockButton.Size = new Size(120, 40);
            this.lockButton.Location = new Point(1030, 20);
            this.lockButton.Text = "🔒 Lock";
            this.lockButton.Font = new Font("Segoe UI", 11F);
            this.lockButton.BackColor = Color.FromArgb(220, 53, 69);
            this.lockButton.ForeColor = Color.White;
            this.lockButton.FlatStyle = FlatStyle.Flat;
            this.lockButton.Click += new EventHandler(this.lockButton_Click);
            this.vaultPanel.Controls.Add(this.lockButton);

            // Footer
            this.footerLabel.Text = "AES-256 Password Manager | https://nirav.madariya.com";
            this.footerLabel.Font = new Font("Segoe UI", 12F, FontStyle.Italic);
            this.footerLabel.Location = new Point(30, 1120);  // ← BOTTOM (1200 - 80)
            this.footerLabel.Size = new Size(900, 30);
            this.vaultPanel.Controls.Add(this.footerLabel);

            // Add panels
            this.Controls.Add(this.loginPanel);
            this.Controls.Add(this.vaultPanel);

            // Form closing
            this.FormClosing += (s, e) => { if (_isVaultOpen) _vault.Lock(); };

            this.ResumeLayout(false);
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        #endregion
    }
}