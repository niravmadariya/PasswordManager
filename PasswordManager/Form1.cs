using System.Runtime.InteropServices;

namespace PasswordManager
{
    public partial class Form1 : Form
    {
        private List<(VaultItem Item, string Secret)> _allItems = [];
        private SecretType? _currentFilter = null; // null = All
        private VaultService _vault;
        private bool _isVaultOpen;
        private ListViewItem? _hoveredItem = null;

        #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public Form1()
        #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
            InitializeComponent();
            // ADD EVENT HANDLERS
            vaultList.MouseClick += vaultList_MouseClick;
            vaultList.MouseMove += vaultList_MouseMove;
            vaultList.MouseLeave += vaultList_MouseLeave;
            deleteButton.Click += deleteButton_Click;
            InitializeVault();
            CenterLoginControls();  // Center login on start
            UpdateUI();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            using var dialog = new AddSecret();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    var id = _vault.AddItem(
                        dialog.Title,
                        !string.IsNullOrEmpty(dialog.Username) ? dialog.Username : null,
                        dialog.SecretType,
                        dialog.Secret);
                    RefreshVaultList();
                    //MessageBox.Show($"Entry added with ID: {id}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CenterLoginControls()
        {
            var centerX = (this.ClientSize.Width - 450) / 2;
            var centerY = (this.ClientSize.Height - 400) / 2;
            statusLabel.Location = new Point(centerX, centerY);
            statusLabel.Size = new Size(450, 60);
            masterPasswordBox.Location = new Point(centerX, centerY + 80);
            masterPasswordBox.Size = new Size(450, 55);
            unlockButton.Location = new Point(centerX + 150, centerY + 160);
            unlockButton.Size = new Size(150, 55);
            initButton.Location = new Point(centerX + 25, centerY + 160);
            initButton.Size = new Size(400, 40);
            errorLabel.Location = new Point(centerX + 55, centerY + 240);
            errorLabel.Size = new Size(400, 40);
        }

        private void InitializeVault()
        {
            var dbPath = Path.Combine(Application.StartupPath, "vault.db");
            var repo = new VaultRepository(dbPath);
            var crypto = new CryptoService();
            _vault = new VaultService(repo, crypto);
            if (_vault.IsInitialized())
            {
                statusLabel.Text = "Vault exists but locked. Enter master password.";
                initButton.Visible = false;
                unlockButton.Visible = true;
            }
            else
            {
                statusLabel.Text = "No vault found. Initialize new vault first.";
                initButton.Visible = true;
                unlockButton.Visible = false;
            }
        }

        private void UpdateUI()
        {
            bool vaultOpen = _isVaultOpen;
            loginPanel.Visible = !vaultOpen;
            vaultPanel.Visible = vaultOpen;
            lockButton.Enabled = vaultOpen;
            unlockButton.Enabled = !vaultOpen;
            initButton.Enabled = !vaultOpen;
            deleteButton.Enabled = vaultOpen;
            if (vaultOpen)
            {
                RefreshVaultList();
                //MessageBox.Show($"Vault items: {vaultList.Items.Count}\n" + $"VaultPanel visible: {vaultPanel.Visible}\n" + $"ListView bounds: {vaultList.Bounds}");
                // FORCE VAULT PANEL VISIBLE
                vaultPanel.BringToFront();
                vaultList.BringToFront();
            }
            else
            {
                // RESET LOGIN STATE
                statusLabel.Text = _vault.IsInitialized() ? "Vault exists but locked. Enter master password." : "No vault found. Initialize new vault first.";
                errorLabel.Text = "";
                masterPasswordBox.Text = "";
            }
        }

        private void RefreshVaultList()
        {
            vaultList.Items.Clear();
            foreach (var (item, _) in _vault.GetAllDecrypted())
            {
                AddListItem(item, vaultList);
            }
            _allItems = _vault.GetAllDecrypted().ToList();
            ApplyFilterAndBind();
        }

        private void ApplyFilterAndBind()
        {
            vaultList.Items.Clear();
            IEnumerable<(VaultItem Item, string Secret)> filtered = _allItems;
            if (_currentFilter.HasValue)
                filtered = filtered.Where(x => x.Item.SecretType == _currentFilter.Value);
            foreach (var (item, secret) in filtered)
            {
                AddListItem(item, vaultList);
            }
        }

        private void AddListItem(VaultItem item, ListView vaultList)
        {
            var listItem = new ListViewItem(item.Title)
            {
                Tag = item // still store the whole item
            };
            listItem.SubItems.Add(item.Username ?? "");
            //listItem.SubItems.Add(item.SecretType.ToString());
            listItem.SubItems.Add("👁");
            listItem.SubItems.Add("✏");
            listItem.SubItems.Add("🗑");
            listItem.SubItems.Add(item.UpdatedUtc.ToString("dd MMM HH:mm"));
            vaultList.Items.Add(listItem);
        }

        private void NavFilterButton_Click(object? sender, EventArgs e)
        {
            if (sender is not Button btn) return; // Read tag to determine filter            
            if (btn.Tag is SecretType type)
                _currentFilter = type;
            else
                _currentFilter = null; // All            
            foreach (var control in vaultPanel.Controls.OfType<Panel>().First(p => p.Controls.Contains(btn)).Controls.OfType<Button>()) // Visual: reset all nav button backgrounds
            {
                control.BackColor = Color.White;
            }
            btn.BackColor = Color.FromArgb(220, 230, 240); // highlight selected
            ApplyFilterAndBind();
        }

        private void ConfirmAndDelete(VaultItem item)
        {
            var result = MessageBox.Show($"Delete '{item.Title}'?", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                _vault.DeleteItem(item.Id);
                RefreshVaultList();
            }
        }

        private void vaultList_MouseMove(object sender, MouseEventArgs e)
        {
            var hoveredItem = vaultList.HitTest(e.Location).Item;
            if (hoveredItem != _hoveredItem)
            {
                _hoveredItem?.BackColor = Color.White;
                _hoveredItem = hoveredItem;
                _hoveredItem?.BackColor = Color.FromArgb(248, 249, 250);
            }
        }

        private void vaultList_MouseLeave(object sender, EventArgs e)
        {
            _hoveredItem?.BackColor = Color.White;
            _hoveredItem = null;
        }

        private void unlockButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (_vault.OpenVault(masterPasswordBox.Text))
                {
                    _isVaultOpen = true;
                    statusLabel.Text = "Vault unlocked";
                    errorLabel.Text = "";
                    masterPasswordBox.Text = "";
                    UpdateUI();
                }
                else
                {
                    errorLabel.Text = "Wrong master password!";
                }
            }
            catch (Exception ex)
            {
                errorLabel.Text = $"Error: {ex.Message}";
            }
        }

        private void initButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(masterPasswordBox.Text))
            {
                errorLabel.Text = "Enter a strong master password!";
                return;
            }
            try
            {
                _vault.InitializeNewVault(masterPasswordBox.Text);
                _isVaultOpen = true;
                statusLabel.Text = "New vault created and unlocked";
                errorLabel.Text = "";
                masterPasswordBox.Text = "";
                UpdateUI();
            }
            catch (Exception ex)
            {
                errorLabel.Text = $"Error: {ex.Message}";
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (vaultList.SelectedItems.Count == 0) return;

            var listItem = vaultList.SelectedItems[0];
            var item = listItem.Tag as VaultItem;
            var result = MessageBox.Show($"Delete '{item.Title}'?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                _vault.DeleteItem(item.Id);
                RefreshVaultList();
            }
        }

        private void showSecretButton_Click(object sender, EventArgs e)
        {
            if (vaultList.SelectedItems.Count == 0) return;
            var listItem = vaultList.SelectedItems[0];
            var item = listItem.Tag as VaultItem;
            try
            {
                var secret = _vault.GetSecret(item.Id);
                if (secret != null)
                {
                    secretDisplayLabel.Text = secret;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error");
            }
        }

        private void lockButton_Click(object sender, EventArgs e)
        {
            _vault.Lock();
            _isVaultOpen = false;
            secretDisplayLabel.Text = "";
            vaultList.Items.Clear();
            statusLabel.Text = "Vault locked";
            UpdateUI();
        }

        private void ShowEntryDetails(VaultItem item, bool editMode)
        {
            var secret = _vault.GetSecret(item.Id) ?? "";
            using var details = new EntryDetailsForm(item, _vault, editMode);
            if (details.ShowDialog(this) == DialogResult.OK && editMode)
            {
                RefreshVaultList();
            }
        }

        private void vaultList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var hit = vaultList.HitTest(e.Location);
                if (hit.Item != null && hit.SubItem != null)
                {
                    var item = (VaultItem)hit.Item.Tag;
                    var colIndex = hit.Item.SubItems.IndexOf(hit.SubItem);
                    switch (colIndex)
                    {
                        case 4 when hit.SubItem.Text.Contains("👁"):
                            ShowEntryDetails(item, false);
                            break;
                        case 5 when hit.SubItem.Text.Contains("✏"):  // View
                            ShowEntryDetails(item, true);
                            break;
                        case 6 when hit.SubItem.Text.Contains("🗑"):// "Delete"                            
                            ConfirmAndDelete(item);
                            break;
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                var hit = vaultList.HitTest(e.Location).Item;
                if (hit != null)
                {
                    ShowContextMenu(hit, e);
                }
            }
        }

        private void ShowContextMenu(ListViewItem listItem, MouseEventArgs e)
        {
            var item = (VaultItem)listItem.Tag;
            var secret = _vault.GetSecret(item.Id) ?? "";
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("📋 Copy Username", null, (s, e) => Clipboard.SetText(item.Username ?? ""));
            contextMenu.Items.Add("🔑 Copy Password", null, (s, e) => Clipboard.SetText(secret));
            contextMenu.Items.Add("-");
            contextMenu.Items.Add("🗑 Delete", null, (s, e) =>
            {
                if (MessageBox.Show($"Delete '{item.Title}'?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _vault.DeleteItem(item.Id);
                    RefreshVaultList();
                }
            });
            contextMenu.Show(vaultList, e.Location);
        }

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);
    }
}