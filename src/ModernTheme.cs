using System.Drawing;
using System.Windows.Forms;

namespace MeshCentralRouter
{
    internal static class ModernTheme
    {
        internal static readonly Color Canvas = Color.FromArgb(241, 245, 249);
        internal static readonly Color Surface = Color.White;
        internal static readonly Color Text = Color.FromArgb(15, 23, 42);
        internal static readonly Color Muted = Color.FromArgb(100, 116, 139);
        internal static readonly Color Border = Color.FromArgb(203, 213, 225);
        internal static readonly Color Accent = Color.FromArgb(37, 99, 235);
        internal static readonly Color AccentHover = Color.FromArgb(29, 78, 216);
        internal static readonly Font BodyFont = new Font("Segoe UI", 9.5f);
        internal static readonly Font StrongFont = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        internal static readonly Font HeadingFont = new Font("Segoe UI", 18f, FontStyle.Bold);

        internal static void Apply(Control control)
        {
            control.Font = BodyFont;
            control.ForeColor = Text;
            if (control is Panel || control is TabPage || control is UserControl) { control.BackColor = Surface; }
            Button button = control as Button;
            if (button != null) { StyleButton(button, false); }
            TextBox textBox = control as TextBox;
            if (textBox != null) { textBox.BorderStyle = BorderStyle.FixedSingle; textBox.BackColor = Surface; }
            ComboBox comboBox = control as ComboBox;
            if (comboBox != null) { comboBox.FlatStyle = FlatStyle.Flat; comboBox.BackColor = Surface; }
            LinkLabel link = control as LinkLabel;
            if (link != null) { link.LinkColor = Accent; link.ActiveLinkColor = AccentHover; }
            foreach (Control child in control.Controls) { Apply(child); }
        }

        internal static void StyleButton(Button button, bool primary)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = primary ? 0 : 1;
            button.FlatAppearance.BorderColor = Border;
            button.FlatAppearance.MouseOverBackColor = primary ? AccentHover : Canvas;
            button.FlatAppearance.MouseDownBackColor = primary ? AccentHover : Border;
            button.UseVisualStyleBackColor = false;
            button.BackColor = primary ? Accent : Surface;
            button.ForeColor = primary ? Color.White : Text;
            button.Font = StrongFont;
            button.Cursor = Cursors.Hand;
            button.AutoSize = true;
            button.AutoSizeMode = AutoSizeMode.GrowOnly;
            button.Padding = new Padding(12, 6, 12, 6);
            button.MinimumSize = new Size(96, 36);
            button.Margin = new Padding(0, 0, 8, 0);
        }

        internal static void StyleMenu(ContextMenuStrip menu)
        {
            menu.Font = BodyFont;
            menu.Renderer = new ToolStripProfessionalRenderer(new MenuColors());
            menu.ShowImageMargin = false;
            menu.ShowCheckMargin = true;
        }

        private sealed class MenuColors : ProfessionalColorTable
        {
            public override Color ToolStripDropDownBackground { get { return Surface; } }
            public override Color MenuBorder { get { return Border; } }
            public override Color MenuItemBorder { get { return Border; } }
            public override Color MenuItemSelected { get { return Canvas; } }
            public override Color MenuItemSelectedGradientBegin { get { return Canvas; } }
            public override Color MenuItemSelectedGradientEnd { get { return Canvas; } }
            public override Color ImageMarginGradientBegin { get { return Surface; } }
            public override Color ImageMarginGradientMiddle { get { return Surface; } }
            public override Color ImageMarginGradientEnd { get { return Surface; } }
        }
    }
}
