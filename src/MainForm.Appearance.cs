using System;
using System.Drawing;
using System.Windows.Forms;

namespace MeshCentralRouter
{
    public partial class MainForm
    {
        private Label modernHeaderStatus;
        private Panel modernSearchFrame;

        private int Ui(int value) { return (int)Math.Round(value * DeviceDpi / 96.0); }

        private void InitializeModernAppearance()
        {
            SuspendLayout();
            ModernTheme.Apply(this);
            BackColor = ModernTheme.Canvas;
            panel5.BackColor = mainPanel.BackColor = ModernTheme.Canvas;
            mainPanel.Padding = new Padding(Ui(24));
            mainTabControl.Visible = false;
            ClientSize = new Size(Ui(860), Ui(680));

            // Keep an explicitly supplied top banner; otherwise use a text header that scales with DPI.
            if (!System.IO.File.Exists(System.IO.Path.Combine(Application.StartupPath, @"customization\topbanner.png")))
            {
                pictureBox1.Visible = false;
                Panel header = new Panel { Dock = DockStyle.Top, Height = Ui(88), Padding = new Padding(Ui(28), Ui(18), Ui(28), Ui(12)), BackColor = ModernTheme.Surface };
                Label heading = new Label { Dock = DockStyle.Top, AutoSize = false, Height = Ui(34), Text = Text.Trim(), Font = ModernTheme.HeadingFont, ForeColor = ModernTheme.Text };
                modernHeaderStatus = new Label { Dock = DockStyle.Bottom, Height = Ui(22), Font = ModernTheme.BodyFont, ForeColor = ModernTheme.Muted };
                header.Controls.Add(heading);
                header.Controls.Add(modernHeaderStatus);
                panel5.Controls.Add(header);
                mainPanel.BringToFront();
            }

            BuildLoginLayout();
            BuildTokenLayout();
            BuildCertificateLayout();
            BuildWorkspaceLayout();
            foreach (ContextMenuStrip menu in new[] { mainContextMenuStrip, mappingsContextMenuStrip, devicesContextMenuStrip, trayIconContextMenuStrip })
            {
                ModernTheme.StyleMenu(menu);
            }
            ModernTheme.StyleButton(nextButton1, true);
            ModernTheme.StyleButton(nextButton2, true);
            ModernTheme.StyleButton(addButton, true);
            stateLabel.ForeColor = Color.FromArgb(185, 28, 28);
            stateLabel.AutoSize = false;
            stateLabel.Height = Ui(40);
            label4.ForeColor = Color.FromArgb(154, 52, 18);
            label4.BackColor = Color.FromArgb(255, 247, 237);
            label4.Font = ModernTheme.StrongFont;
            label4.Padding = new Padding(Ui(12));
            label4.AutoSize = true;
            nextButton3.BackColor = Color.FromArgb(255, 237, 213);
            nextButton3.ForeColor = Color.FromArgb(154, 52, 18);
            UpdateModernHeader();
            ResumeLayout(true);
        }

        private TableLayoutPanel Stack(params Control[] controls)
        {
            TableLayoutPanel stack = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = controls.Length, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, BackColor = ModernTheme.Surface, Margin = Padding.Empty };
            stack.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            for (int i = 0; i < controls.Length; i++)
            {
                stack.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                controls[i].Dock = DockStyle.Top;
                controls[i].Margin = new Padding(0, 0, 0, Ui(10));
                stack.Controls.Add(controls[i], 0, i);
            }
            return stack;
        }

        private TableLayoutPanel Card(Panel panel)
        {
            panel.Controls.Clear();
            panel.Padding = new Padding(Ui(24));
            TableLayoutPanel card = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 0, BackColor = ModernTheme.Surface, Margin = Padding.Empty };
            card.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            panel.Controls.Add(card);
            return card;
        }

        private void AddRow(TableLayoutPanel table, Control control, SizeType type, float height)
        {
            int row = table.RowCount++;
            table.RowStyles.Add(new RowStyle(type, height));
            control.Dock = DockStyle.Fill;
            control.Margin = Padding.Empty;
            table.Controls.Add(control, 0, row);
        }

        private Panel Input(Control input)
        {
            input.Dock = DockStyle.Fill;
            input.Margin = Padding.Empty;
            TextBox textBox = input as TextBox;
            if (textBox != null) { textBox.BorderStyle = BorderStyle.None; }
            Panel frame = new Panel { Height = Ui(40), Padding = new Padding(Ui(12), Ui(9), Ui(12), Ui(7)), BackColor = ModernTheme.Canvas };
            frame.Controls.Add(input);
            return frame;
        }

        private Control Field(Label label, Control input)
        {
            label.AutoSize = true;
            label.ForeColor = ModernTheme.Muted;
            Panel frame = Input(input);
            TableLayoutPanel field = Stack(label, frame);
            label.Margin = new Padding(0, 0, 0, Ui(6));
            frame.Margin = Padding.Empty;
            return field;
        }

        private FlowLayoutPanel Actions(params Control[] controls)
        {
            FlowLayoutPanel flow = new FlowLayoutPanel { AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, WrapContents = true, Dock = DockStyle.Fill, Margin = Padding.Empty, BackColor = ModernTheme.Surface };
            foreach (Control control in controls)
            {
                control.Dock = DockStyle.None;
                control.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                control.Margin = new Padding(0, Ui(4), Ui(8), Ui(4));
                flow.Controls.Add(control);
            }
            return flow;
        }

        private TableLayoutPanel Footer(Control left, Control right)
        {
            TableLayoutPanel footer = new TableLayoutPanel { ColumnCount = 2, RowCount = 1, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Dock = DockStyle.Fill, Padding = new Padding(0, Ui(14), 0, 0), Margin = Padding.Empty, BackColor = ModernTheme.Surface };
            footer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));
            footer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
            left.Dock = DockStyle.Fill;
            right.Dock = DockStyle.Fill;
            FlowLayoutPanel rightFlow = right as FlowLayoutPanel;
            if (rightFlow != null) { rightFlow.FlowDirection = FlowDirection.RightToLeft; }
            footer.Controls.Add(left, 0, 0);
            footer.Controls.Add(right, 1, 0);
            return footer;
        }

        private TableLayoutPanel AuthBody(PictureBox logo, Control form)
        {
            TableLayoutPanel body = new TableLayoutPanel { ColumnCount = 2, RowCount = 1, Dock = DockStyle.Fill, BackColor = ModernTheme.Surface, Padding = new Padding(0, Ui(12), 0, 0), Margin = Padding.Empty };
            body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36));
            body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 64));
            logo.Dock = DockStyle.Fill;
            logo.Margin = new Padding(Ui(20), Ui(20), Ui(36), Ui(20));
            logo.SizeMode = PictureBoxSizeMode.Zoom;
            if (!System.IO.File.Exists(System.IO.Path.Combine(Application.StartupPath, @"customization\logo.png")))
            {
                logo.Image = null;
                logo.BackColor = Color.FromArgb(239, 246, 255);
                logo.Paint += DrawRoutingIllustration;
            }
            body.Controls.Add(logo, 0, 0);
            form.Dock = DockStyle.Top;
            body.Controls.Add(form, 1, 0);
            return body;
        }

        private void DrawRoutingIllustration(object sender, PaintEventArgs e)
        {
            Control canvas = (Control)sender;
            float unit = Math.Min(canvas.Width, canvas.Height) / 200f;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.TranslateTransform(canvas.Width / 2f, canvas.Height / 2f);
            e.Graphics.ScaleTransform(unit, unit);
            using (Pen line = new Pen(Color.FromArgb(147, 197, 253), 3))
            using (Brush white = new SolidBrush(Color.White))
            using (Brush accent = new SolidBrush(ModernTheme.Accent))
            {
                foreach (Point point in new[] { new Point(-56, -48), new Point(56, -48), new Point(-56, 48), new Point(56, 48) })
                {
                    e.Graphics.DrawLine(line, Point.Empty, point);
                    e.Graphics.FillEllipse(white, point.X - 18, point.Y - 18, 36, 36);
                    e.Graphics.DrawEllipse(line, point.X - 18, point.Y - 18, 36, 36);
                    e.Graphics.FillEllipse(accent, point.X - 5, point.Y - 5, 10, 10);
                }
                e.Graphics.FillEllipse(accent, -28, -28, 56, 56);
                using (Pen route = new Pen(Color.White, 3))
                {
                    e.Graphics.DrawLine(route, -12, 0, 12, 0);
                    e.Graphics.DrawLine(route, 5, -7, 12, 0);
                    e.Graphics.DrawLine(route, 5, 7, 12, 0);
                }
            }
            e.Graphics.ResetTransform();
        }

        private void AddCustomBottomBanner(TableLayoutPanel card, PictureBox banner)
        {
            if (System.IO.File.Exists(System.IO.Path.Combine(Application.StartupPath, @"customization\bottombanner.png")))
            {
                banner.SizeMode = PictureBoxSizeMode.Zoom;
                AddRow(card, banner, SizeType.Absolute, Ui(40));
            }
        }

        private void BuildLoginLayout()
        {
            TableLayoutPanel card = Card(panel1);
            label1.AutoSize = true;
            label1.ForeColor = ModernTheme.Muted;
            AddRow(card, label1, SizeType.AutoSize, 0);
            rememberPasswordCheckBox.AutoSize = true;
            Control fields = Stack(Field(label26, serverNameComboBox), Field(label27, userNameTextBox), Field(label28, passwordTextBox), rememberPasswordCheckBox, stateLabel);
            AddRow(card, AuthBody(pictureBox2, fields), SizeType.Percent, 100);
            versionLabel.AutoSize = licenseLinkLabel.AutoSize = true;
            versionLabel.ForeColor = ModernTheme.Muted;
            AddRow(card, Footer(Actions(versionLabel), Actions(licenseLinkLabel)), SizeType.AutoSize, 0);
            AddRow(card, Footer(Actions(connectionSettings, installButton), Actions(nextButton1)), SizeType.AutoSize, 0);
            AddCustomBottomBanner(card, pictureBox3);
        }

        private void BuildTokenLayout()
        {
            TableLayoutPanel card = Card(panel2);
            label2.AutoSize = true;
            label2.ForeColor = ModernTheme.Muted;
            AddRow(card, label2, SizeType.AutoSize, 0);
            tokenRememberCheckBox.AutoSize = tokenEmailSentLabel.AutoSize = true;
            Control fields = Stack(Field(label3, tokenTextBox), Actions(emailTokenButton, smsTokenButton, msgTokenButton), tokenEmailSentLabel, tokenRememberCheckBox);
            AddRow(card, AuthBody(pictureBox6, fields), SizeType.Percent, 100);
            AddRow(card, Footer(new Panel { Size = Size.Empty }, Actions(nextButton2, backButton2)), SizeType.AutoSize, 0);
            AddCustomBottomBanner(card, pictureBox4);
        }

        private void BuildCertificateLayout()
        {
            TableLayoutPanel card = Card(panel3);
            label4.AutoSize = label5.AutoSize = true;
            label5.Padding = new Padding(0, Ui(14), 0, Ui(14));
            AddRow(card, label4, SizeType.AutoSize, 0);
            AddRow(card, label5, SizeType.AutoSize, 0);
            certDetailsTextBox.ScrollBars = ScrollBars.Vertical;
            AddRow(card, certDetailsTextBox, SizeType.Percent, 100);
            rememberCertCheckBox.AutoSize = true;
            AddRow(card, Actions(certDetailsButton, rememberCertCheckBox), SizeType.AutoSize, 0);
            AddRow(card, Footer(new Panel { Size = Size.Empty }, Actions(nextButton3, backButton3)), SizeType.AutoSize, 0);
            AddCustomBottomBanner(card, pictureBox5);
        }

        private void BuildWorkspaceLayout()
        {
            TableLayoutPanel card = Card(panel4);
            TableLayoutPanel toolbar = new TableLayoutPanel { ColumnCount = 2, RowCount = 1, Height = Ui(52), Dock = DockStyle.Fill, BackColor = ModernTheme.Surface };
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, Ui(48)));
            modernSearchFrame = Input(searchTextBox);
            modernSearchFrame.Dock = DockStyle.Fill;
            modernSearchFrame.Margin = new Padding(0, 0, Ui(12), Ui(10));
            toolbar.Controls.Add(modernSearchFrame, 0, 0);
            menuLabel.Dock = DockStyle.Fill;
            menuLabel.TextAlign = ContentAlignment.MiddleCenter;
            menuLabel.Font = ModernTheme.HeadingFont;
            toolbar.Controls.Add(menuLabel, 1, 0);
            AddRow(card, toolbar, SizeType.Absolute, Ui(52));
            AddRow(card, devicesTabControl, SizeType.Percent, 100);
            devicesTabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            devicesTabControl.ItemSize = new Size(Ui(150), Ui(40));
            devicesTabControl.SizeMode = TabSizeMode.Fixed;
            devicesTabControl.DrawItem += DrawModernTab;
            AddRow(card, Footer(Actions(openWebSiteButton), Actions(nextButton5, backButton5)), SizeType.AutoSize, 0);
            AddCustomBottomBanner(card, pictureBox7);

            devicesPanel.Dock = DockStyle.Fill;
            devicesPanel.BorderStyle = BorderStyle.None;
            devicesListView.BorderStyle = BorderStyle.None;
            devicesListView.GridLines = false;
            devicesListView.Font = ModernTheme.BodyFont;
            devicesListView.Resize += (sender, e) =>
            {
                int width = Math.Max(Ui(200), devicesListView.ClientSize.Width - Ui(24));
                nameColumnHeader.Width = (int)(width * 0.68);
                stateColumnHeader.Width = width - nameColumnHeader.Width;
            };
            foreach (Label empty in new[] { noDevicesLabel, noSearchResultsLabel, noMapLabel })
            {
                empty.Dock = DockStyle.Fill;
                empty.AutoSize = false;
                empty.TextAlign = ContentAlignment.MiddleCenter;
                empty.Font = ModernTheme.StrongFont;
                empty.ForeColor = ModernTheme.Muted;
            }
            cancelAutoCloseButton1.Dock = DockStyle.Bottom;
            cancelAutoCloseButton1.BringToFront();
            cancelAutoCloseButton1.VisibleChanged += (sender, e) => cancelAutoCloseButton1.BringToFront();

            portMapTabPage.Controls.Clear();
            TableLayoutPanel mappings = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 0, BackColor = ModernTheme.Surface, Padding = new Padding(Ui(8)) };
            mappings.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            portMapTabPage.Controls.Add(mappings);
            mapPanel.AutoScroll = true;
            mapPanel.BorderStyle = BorderStyle.None;
            AddRow(mappings, mapPanel, SizeType.Percent, 100);
            settingsPictureBox.SizeMode = helpPictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
            settingsPictureBox.Size = helpPictureBox.Size = new Size(Ui(36), Ui(36));
            settingsPictureBox.Cursor = helpPictureBox.Cursor = Cursors.Hand;
            AddRow(mappings, Actions(cancelAutoCloseButton2, settingsPictureBox, helpPictureBox, addRelayButton, addButton), SizeType.AutoSize, 0);
        }

        private void DrawModernTab(object sender, DrawItemEventArgs e)
        {
            bool selected = devicesTabControl.SelectedIndex == e.Index;
            using (Brush background = new SolidBrush(selected ? ModernTheme.Surface : ModernTheme.Canvas)) { e.Graphics.FillRectangle(background, e.Bounds); }
            TextRenderer.DrawText(e.Graphics, devicesTabControl.TabPages[e.Index].Text, ModernTheme.StrongFont, e.Bounds, selected ? ModernTheme.Accent : ModernTheme.Muted, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            if (selected)
            {
                using (Brush accent = new SolidBrush(ModernTheme.Accent)) { e.Graphics.FillRectangle(accent, e.Bounds.Left, e.Bounds.Bottom - Ui(3), e.Bounds.Width, Ui(3)); }
            }
            if ((e.State & DrawItemState.Focus) != 0) { ControlPaint.DrawFocusRectangle(e.Graphics, Rectangle.Inflate(e.Bounds, -Ui(4), -Ui(4))); }
        }

        private void UpdateModernHeader()
        {
            if (modernHeaderStatus == null) return;
            string screen = tabPage1.Text;
            if (currentPanel == 2) { screen = tabPage2.Text; }
            if (currentPanel == 3) { screen = tabPage3.Text; }
            if (currentPanel == 4) { screen = meshcentral != null && meshcentral.wsurl != null ? meshcentral.wsurl.Authority : devicesTabPage.Text; }
            modernHeaderStatus.Text = screen + "  ·  " + versionLabel.Text;
        }
    }
}
