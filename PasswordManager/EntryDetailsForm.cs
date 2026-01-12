namespace PasswordManager
{
    public partial class EntryDetailsForm : Form
    {

        private readonly VaultItem _originalItem;
        private readonly VaultService _vault;
        private readonly bool _isEditMode;
        private readonly Image _eyeOpen, _eyeClosed;
        private bool _passwordVisible = false;

        public EntryDetailsForm(VaultItem item, VaultService vault, bool editMode = false)
        {
            _originalItem = item;
            _vault = vault;
            _isEditMode = editMode;
            _eyeOpen = PasswordManager.Properties.Resources.eye_open;
            _eyeClosed = PasswordManager.Properties.Resources.eye_closed;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(700, 550);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Text = _isEditMode ? "Edit Entry" : "Entry Details";
            this.BackColor = Color.FromArgb(253, 253, 253);

            // Editable fields if edit mode
            var titleTextBox = new TextBox
            {
                Text = _originalItem.Title,
                Location = new Point(120, 30),
                Size = new Size(500, 35),
                ReadOnly = !_isEditMode,
                Font = new Font("Segoe UI", 12F)
            };
            var usernameTextBox = new TextBox
            {
                Text = _originalItem.Username ?? "",
                Location = new Point(130, 80),
                Size = new Size(500, 35),
                ReadOnly = !_isEditMode,
                Font = new Font("Segoe UI", 12F)
            };

            // Secret with eye toggle + copy
            var secretBox = new TextBox
            {
                Location = new Point(120, 130),
                Size = new Size(450, 40),
                Font = new Font("Segoe UI", 12F),
                UseSystemPasswordChar = true,
                ReadOnly = !_isEditMode,
                Text = _vault.GetSecret(_originalItem.Id) ?? ""
            };
            var eyeToggle = new PictureBox
            {
                Image = _eyeClosed,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(28, 28),
                Location = new Point(580, 138),
                Cursor = Cursors.Hand
            };
            var copySecretBtn = new Button
            {
                Text = "📋 Copy",
                Location = new Point(620, 135),
                Size = new Size(60, 35),
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            copySecretBtn.Click += (s, e) => Clipboard.SetText(secretBox.Text);

            eyeToggle.Click += (s, e) =>
            {
                _passwordVisible = !_passwordVisible;
                secretBox.UseSystemPasswordChar = !_passwordVisible;
                eyeToggle.Image = _passwordVisible ? _eyeOpen : _eyeClosed;
            };

            // Read-only info
            var createdValue = new Label
            {
                Text = _originalItem.CreatedUtc.ToString("dd MMM yyyy HH:mm"),
                Location = new Point(120, 200),
                Size = new Size(500, 25),
                Font = new Font("Segoe UI", 11F)
            };
            var typeValue = new Label
            {
                Text = _originalItem.SecretType.ToString(),
                Location = new Point(90, 240),
                Size = new Size(500, 25),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.DodgerBlue
            };

            // Buttons
            var buttonPanel = new FlowLayoutPanel
            {
                Location = new Point(150, 450),
                Size = new Size(400, 50),
                FlowDirection = FlowDirection.LeftToRight
            };

            if (_isEditMode)
            {
                var saveBtn = new Button
                {
                    Text = "💾 Save",
                    Size = new Size(100, 45),
                    BackColor = Color.FromArgb(40, 167, 69),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 12F)
                };
                saveBtn.Click += (s, e) =>
                {
                    // Save logic
                    if (string.IsNullOrWhiteSpace(titleTextBox.Text) || string.IsNullOrWhiteSpace(secretBox.Text))
                    {
                        MessageBox.Show("Title and secret are required.", "Validation",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    // persist changes
                    _vault.UpdateItem(
                        _originalItem.Id,
                        titleTextBox.Text,
                        string.IsNullOrWhiteSpace(usernameTextBox.Text) ? null : usernameTextBox.Text,
                        secretBox.Text
                    );
                    var updatedItem = new VaultItem
                    {
                        Id = _originalItem.Id,
                        Title = titleTextBox.Text,
                        Username = usernameTextBox.Text,
                        SecretType = _originalItem.SecretType,
                        CreatedUtc = _originalItem.CreatedUtc,
                        UpdatedUtc = DateTime.UtcNow
                    };
                    // Update via vault service
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                };
                buttonPanel.Controls.Add(saveBtn);
            }

            var closeBtn = new Button
            {
                Text = _isEditMode ? "Cancel" : "Close",
                Size = new Size(100, 45),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12F)
            };
            closeBtn.Click += (s, e) => this.Close();
            buttonPanel.Controls.Add(closeBtn);

            // Labels
            this.Controls.AddRange(new Control[] {
            new Label
            {
                Text = "Title:",
                Location = new Point(30, 35),
                Size = new Size(80, 25),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold)
            },
            titleTextBox,
            new Label { Text = "Username:", Location = new Point(30, 85), Size = new Size(90, 25), Font = new Font("Segoe UI", 12F, FontStyle.Bold) },
            usernameTextBox,
            new Label { Text = "Secret:", Location = new Point(30, 138), Size = new Size(80, 25), Font = new Font("Segoe UI", 12F, FontStyle.Bold) },
            secretBox, eyeToggle, copySecretBtn,
            new Label { Text = "Created:", Location = new Point(30, 205), Size = new Size(80, 25), Font = new Font("Segoe UI", 12F, FontStyle.Bold) },
            createdValue,
            new Label { Text = "Type:", Location = new Point(30, 245), Size = new Size(50, 25), Font = new Font("Segoe UI", 12F, FontStyle.Bold) },
            typeValue,
            buttonPanel
        });
        }
    }
}