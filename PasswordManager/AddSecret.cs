namespace PasswordManager
{
    public partial class AddSecret : Form
    {
        public string Title { get; private set; } = "";
        public string Username { get; private set; } = "";
        public SecretType SecretType { get; private set; } = SecretType.Password;
        public string Secret { get; private set; } = "";

        public AddSecret()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = new Size(450, 500);
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(titleTextBox.Text) || string.IsNullOrWhiteSpace(secretTextBox.Text))
            {
                MessageBox.Show("Title and Secret required!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Title = titleTextBox.Text;
            Username = string.IsNullOrWhiteSpace(usernameTextBox.Text) ? "" : usernameTextBox.Text;
            SecretType = (SecretType)secretTypeComboBox.SelectedIndex;
            Secret = secretTextBox.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
