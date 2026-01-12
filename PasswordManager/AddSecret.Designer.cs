namespace PasswordManager
{
    partial class AddSecret
    {
        private System.ComponentModel.IContainer components = null;
        private Label titleLabel;
        private TextBox titleTextBox;
        private Label usernameLabel;
        private TextBox usernameTextBox;
        private Label typeLabel;
        private ComboBox secretTypeComboBox;
        private Label secretLabel;
        private TextBox secretTextBox;
        private Button addButton;
        private Button cancelButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.titleLabel = new Label();
            this.titleTextBox = new TextBox();
            this.usernameLabel = new Label();
            this.usernameTextBox = new TextBox();
            this.typeLabel = new Label();
            this.secretTypeComboBox = new ComboBox();
            this.secretLabel = new Label();
            this.secretTextBox = new TextBox();
            this.addButton = new Button();
            this.cancelButton = new Button();
            this.SuspendLayout();

            // Title
            this.titleLabel.Text = "Title:";
            this.titleLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.titleLabel.Location = new Point(25, 30);
            this.titleLabel.Size = new Size(50, 25);

            this.titleTextBox.Font = new Font("Segoe UI", 12F);
            this.titleTextBox.Location = new Point(25, 55);
            this.titleTextBox.Size = new Size(380, 35);

            // Username
            this.usernameLabel.Text = "Username:";
            this.usernameLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.usernameLabel.Location = new Point(25, 105);
            this.usernameLabel.Size = new Size(90, 25);

            this.usernameTextBox.Font = new Font("Segoe UI", 12F);
            this.usernameTextBox.Location = new Point(25, 130);
            this.usernameTextBox.Size = new Size(380, 35);

            // Type
            this.typeLabel.Text = "Type:";
            this.typeLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.typeLabel.Location = new Point(25, 180);
            this.typeLabel.Size = new Size(50, 25);

            this.secretTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.secretTypeComboBox.Font = new Font("Segoe UI", 12F);
            this.secretTypeComboBox.Items.AddRange(new object[] { "Password", "Passcode", "PIN" });
            this.secretTypeComboBox.SelectedIndex = 0;
            this.secretTypeComboBox.Location = new Point(25, 205);
            this.secretTypeComboBox.Size = new Size(380, 40);

            // Secret
            this.secretLabel.Text = "Secret:";
            this.secretLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.secretLabel.Location = new Point(25, 265);
            this.secretLabel.Size = new Size(60, 25);

            this.secretTextBox.Font = new Font("Segoe UI", 12F);
            this.secretTextBox.Location = new Point(25, 290);
            this.secretTextBox.Size = new Size(380, 35);
            this.secretTextBox.UseSystemPasswordChar = true;

            // Buttons
            this.addButton.Font = new Font("Segoe UI", 12F);
            this.addButton.Size = new Size(100, 45);
            this.addButton.Text = "Add";
            this.addButton.BackColor = Color.DodgerBlue;
            this.addButton.ForeColor = Color.White;
            this.addButton.FlatStyle = FlatStyle.Flat;
            this.addButton.Location = new Point(200, 360);
            this.addButton.Click += new EventHandler(this.addButton_Click);

            this.cancelButton.Font = new Font("Segoe UI", 12F);
            this.cancelButton.Size = new Size(100, 45);
            this.cancelButton.Text = "Cancel";
            this.cancelButton.Location = new Point(310, 360);
            this.cancelButton.Click += new EventHandler(this.cancelButton_Click);

            // Form
            this.ClientSize = new Size(450, 430);
            this.Text = "Add New Entry";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Controls.AddRange(new Control[] {
            this.titleLabel, this.titleTextBox,
            this.usernameLabel, this.usernameTextBox,
            this.typeLabel, this.secretTypeComboBox,
            this.secretLabel, this.secretTextBox,
            this.addButton, this.cancelButton
        });

            this.ResumeLayout(false);
        }
    }
}