using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MeshCentralRouter;
using Microsoft.Win32;

internal static class UiRegressionTests
{
    private const BindingFlags PrivateInstance = BindingFlags.Instance | BindingFlags.NonPublic;
    private static int assertions;

    [DllImport("advapi32.dll")]
    private static extern int RegOverridePredefKey(IntPtr key, IntPtr newKey);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr SendMessage(IntPtr hwnd, int message, IntPtr wparam, ref LVGROUP group);

    [STAThread]
    private static int Main(string[] args)
    {
        // Redirect HKCU for this process only: tests must not read/overwrite real saved credentials or preferences.
        string scratchPath = @"Software\MeshCentralRouterUiTests-" + Guid.NewGuid().ToString("N");
        using (RegistryKey scratch = Registry.CurrentUser.CreateSubKey(scratchPath))
        {
            IntPtr hkcu = new IntPtr(unchecked((int)0x80000001));
            if (RegOverridePredefKey(hkcu, scratch.Handle.DangerousGetHandle()) != 0)
            {
                Console.Error.WriteLine("Could not isolate test settings.");
                return 1;
            }
            try
            {
                Application.EnableVisualStyles();
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
                Application.SetCompatibleTextRenderingDefault(false);
                Run(args.Length > 0 ? args[0] : null);
                Console.WriteLine("PASS: " + assertions + " UI regression assertions.");
                return 0;
            }
            catch (Exception error)
            {
                Console.Error.WriteLine(error);
                return 1;
            }
            finally
            {
                RegOverridePredefKey(hkcu, IntPtr.Zero);
                Registry.CurrentUser.DeleteSubKeyTree(scratchPath);
            }
        }
    }

    private static T Get<T>(object target, string name)
    {
        return (T)target.GetType().GetField(name, PrivateInstance).GetValue(target);
    }

    private static void Call(MainForm form, string name, params object[] args)
    {
        form.GetType().GetMethod(name, PrivateInstance).Invoke(form, args);
        Application.DoEvents();
    }

    private static void Check(bool condition, string message)
    {
        assertions++;
        if (!condition) throw new InvalidOperationException(message);
    }

    private static void Fits(Control control)
    {
        Check(control.Parent.ClientRectangle.Contains(control.Bounds), control.Name + " is outside its parent: " + control.Bounds + " / " + control.Parent.ClientRectangle);
        Check(control.Width > 20 && control.Height > 10, control.Name + " collapsed.");
        Rectangle screenBounds = control.RectangleToScreen(control.ClientRectangle);
        for (Control ancestor = control.Parent; ancestor != null && !(ancestor is Form); ancestor = ancestor.Parent)
        {
            Check(ancestor.RectangleToScreen(ancestor.ClientRectangle).Contains(screenBounds), control.Name + " " + screenBounds + " is clipped by " + ancestor.GetType().Name + " " + ancestor.Name + " " + ancestor.RectangleToScreen(ancestor.ClientRectangle));
        }
    }

    private static bool IsCollapsed(ListView list, ListViewGroup group)
    {
        int id = (int)typeof(ListViewGroup).GetProperty("ID", PrivateInstance).GetValue(group, null);
        LVGROUP native = new LVGROUP { CbSize = Marshal.SizeOf(typeof(LVGROUP)), Mask = ListViewGroupMask.State, StateMask = (int)ListViewGroupState.Collapsed };
        SendMessage(list.Handle, 0x1000 + 149, new IntPtr(id), ref native);
        return (native.State & ListViewGroupState.Collapsed) != 0;
    }

    private static void Capture(MainForm form, string directory, string name)
    {
        if (directory == null) return;
        Directory.CreateDirectory(directory);
        using (Bitmap bitmap = new Bitmap(form.Width, form.Height))
        {
            form.DrawToBitmap(bitmap, new Rectangle(Point.Empty, form.Size));
            bitmap.Save(Path.Combine(directory, name + ".png"));
        }
    }

    private static void Run(string captureDirectory)
    {
        using (MainForm form = new MainForm(new[] { "-search:startup" }))
        {
            form.forceExit = true;
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(-20000, -20000);
            form.Show();
            Application.DoEvents();
            TextBox search = Get<TextBox>(form, "searchTextBox");
            Check(search.Text == "startup", "Disconnected startup search must be safe.");
            Get<ComboBox>(form, "serverNameComboBox").Text = "mesh.example.com";
            Get<TextBox>(form, "userNameTextBox").Text = "demo";
            foreach (string field in new[] { "serverNameComboBox", "userNameTextBox", "passwordTextBox", "nextButton1", "rememberPasswordCheckBox" }) { Fits(Get<Control>(form, field)); }
            Capture(form, captureDirectory, "login");
            Label errorLabel = Get<Label>(form, "stateLabel");
            errorLabel.Text = "Invalid username or password";
            errorLabel.Visible = true;
            Application.DoEvents();
            Fits(errorLabel);
            Fits(Get<Control>(form, "rememberPasswordCheckBox"));
            Capture(form, captureDirectory, "login-error");
            errorLabel.Visible = false;

            Call(form, "setPanel", 2);
            Get<Button>(form, "emailTokenButton").Visible = true;
            Get<Button>(form, "smsTokenButton").Visible = true;
            Get<Button>(form, "msgTokenButton").Visible = true;
            Application.DoEvents();
            Fits(Get<TextBox>(form, "tokenTextBox"));
            Fits(Get<Button>(form, "nextButton2"));
            Capture(form, captureDirectory, "two-factor");
            Call(form, "setPanel", 3);
            Fits(Get<TextBox>(form, "certDetailsTextBox"));
            Capture(form, captureDirectory, "certificate");

            form.meshcentral = new MeshCentralServer
            {
                wsurl = new Uri("wss://mesh.example.com/control.ashx"),
                nodes = new Dictionary<string, NodeClass>(),
                meshes = new Dictionary<string, MeshClass>()
            };
            MeshCentralServer server = form.meshcentral;
            server.meshes["group-a"] = new MeshClass { meshid = "group-a", name = "Office" };
            for (int i = 0; i < 60; i++)
            {
                NodeClass node = new NodeClass { nodeid = "node-" + i, meshid = "group-a", name = "Workstation " + i.ToString("D2"), conn = 1, icon = 1, agentid = 4, agentcaps = 5, users = new[] { "demo.user" } };
                server.nodes.Add(node.nodeid, node);
            }
            form.collapseDeviceGroup = false;
            search.Text = "";
            Call(form, "setPanel", 4);
            Call(form, "Meshcentral_onNodesChanged", true);
            ListView list = Get<ListView>(form, "devicesListView");
            Check(list.Items.Count == 60, "Initial rows missing.");
            Check(list.Groups.Count == 1, "Initial group missing.");
            Check(!IsCollapsed(list, list.Groups[0]), "Groups must respect expanded preference.");
            Fits(list);
            Capture(form, captureDirectory, "devices");

            list.Items[25].Selected = true;
            list.Items[25].Focused = true;
            list.TopItem = list.Items[20];
            ListViewItem selected = list.SelectedItems[0];
            ListViewItem top = list.TopItem;
            ListViewGroup originalGroup = list.Groups[0];
            using (ListMessages messages = new ListMessages(list.Handle))
            {
                int invalidations = 0;
                list.Invalidated += (sender, e) => invalidations++;
                for (int i = 0; i < 100; i++) { Call(form, "Meshcentral_onNodesChanged", false); }
                Check(list.SelectedItems.Count == 1 && list.SelectedItems[0] == selected, "Repeated updates lost selection.");
                Check(list.FocusedItem == selected, "Repeated updates lost focused row.");
                Check(list.TopItem == top, "Repeated updates moved scroll position.");
                Check(list.Groups[0] == originalGroup, "Repeated updates replaced a group.");
                Check(messages.DeleteAllCount == 0, "Device updates cleared the entire list.");
                Check(messages.RedrawToggleCount == 0, "Identical events should not begin a redraw.");
                Check(invalidations == 0, "Identical events invalidated the list.");

                NodeClass changed = server.nodes["node-25"];
                ListViewItem originalItem = changed.listitem;
                changed.conn = 0;
                Call(form, "Meshcentral_onNodesChanged", false);
                Check(changed.listitem == originalItem && originalItem.SubItems[1].Text == changed.getStateString(), "Status update must mutate only the existing row.");
                Check(list.SelectedItems.Count == 1 && list.SelectedItems[0] == selected, "Status change lost selection.");
                Check(list.TopItem == top, "Status change moved scroll position.");

                ListViewExtended.setGrpState(originalGroup, ListViewGroupState.Collapsible | ListViewGroupState.Collapsed);
                Check(IsCollapsed(list, originalGroup), "Native group collapse failed.");
                server.nodes["node-2"].conn = 0;
                Call(form, "Meshcentral_onNodesChanged", false);
                Check(IsCollapsed(list, originalGroup), "Status updates reset collapsed state.");
                server.nodes["node-2"].conn = 1;
                Call(form, "Meshcentral_onNodesChanged", false);
                Check(IsCollapsed(list, originalGroup), "A second status update reset collapsed state.");
                ListViewExtended.setGrpState(originalGroup, ListViewGroupState.Collapsible);
                Call(form, "Meshcentral_onNodesChanged", false);
                Check(!IsCollapsed(list, originalGroup), "Ordinary updates reset expanded state.");
                form.collapseDeviceGroup = true;
                Call(form, "updateDeviceList", true);
                Check(IsCollapsed(list, originalGroup), "Explicit group preference did not apply.");
                form.collapseDeviceGroup = false;
                Call(form, "updateDeviceList", true);
            }

            server.nodes.Remove("node-59");
            server.meshes["group-b"] = new MeshClass { meshid = "group-b", name = "Branch" };
            server.nodes["node-58"].meshid = "group-b";
            Call(form, "Meshcentral_onNodesChanged", false);
            Check(list.Items.Count == 59 && list.Groups.Count == 2, "Remove or group move failed.");
            Check(server.nodes["node-58"].listitem.Group.Header == "Branch", "Group move not reflected.");
            server.nodes["node-58"].meshid = "group-a";
            server.meshes["group-a"].name = "Main Office";
            Call(form, "Meshcentral_onNodesChanged", false);
            Check(list.Groups.Count == 1 && list.Groups[0].Header == "Main Office", "Obsolete group not removed or rename missing.");
            Check(server.nodes["node-58"].listitem.Text.Contains("Main Office"), "Group display name stale.");
            server.nodes["node-58"].name = "AAA renamed";
            Call(form, "Meshcentral_onNodesChanged", false);
            Check(list.Items[0] == server.nodes["node-58"].listitem, "Renamed rows were not sorted.");

            search.Text = "Workstation 03";
            Application.DoEvents();
            Check(list.Items.Count == 1 && ((NodeClass)list.Items[0].Tag).nodeid == "node-3", "Name filter failed.");
            search.Text = "u:DEMO";
            Application.DoEvents();
            Check(list.Items.Count == 59, "User filter failed.");
            search.Text = "user:missing";
            Application.DoEvents();
            Check(list.Items.Count == 0 && Get<Label>(form, "noSearchResultsLabel").Visible, "Search empty state missing.");
            search.Text = "";
            Get<ToolStripMenuItem>(form, "showOfflineDevicesToolStripMenuItem").Checked = false;
            Call(form, "updateDeviceList", false);
            Check(list.Items.Count == 58, "Offline filtering failed.");
            server.nodes["node-25"].mtype = 3;
            Call(form, "updateDeviceList", false);
            Check(list.Items.Count == 59, "Local devices must remain visible without agent connectivity.");

            form.Size = new Size(form.Width + 180, form.Height + 100);
            Application.DoEvents();
            Fits(list);
            Check(list.Columns[0].Width + list.Columns[1].Width <= list.ClientSize.Width, "Columns exceed resized list.");
            Capture(form, captureDirectory, "devices-resized");
            TabControl tabs = Get<TabControl>(form, "devicesTabControl");
            tabs.SelectedIndex = 1;
            using (MapUserControl map = new MapUserControl { node = server.nodes["node-0"], name = "Remote Desktop", Dock = DockStyle.Top })
            {
                map.UpdateInfo();
                Image mapIcon = Get<PictureBox>(map, "devicePictureBox").Image;
                map.UpdateInfo();
                Check(Get<PictureBox>(map, "devicePictureBox").Image == mapIcon, "Unchanged mapping metadata must not replace its image.");
                Panel maps = Get<Panel>(form, "mapPanel");
                maps.Controls.Add(map);
                Get<Label>(form, "noMapLabel").Visible = false;
                Application.DoEvents();
                Fits(map);
                Fits(Get<Button>(map, "appButton"));
                Capture(form, captureDirectory, "mappings");
                maps.Controls.Remove(map);
            }
            tabs.SelectedIndex = 0;
            server.nodes.Clear();
            Call(form, "updateDeviceList", false);
            Check(list.Items.Count == 0 && list.Groups.Count == 0 && Get<Label>(form, "noDevicesLabel").Visible, "No-devices empty state missing.");
            Capture(form, captureDirectory, "empty-devices");
            server.disconnectMsg = "connected";
            Call(form, "Meshcentral_onStateChanged", 0);
            Check(form.meshcentral == null && form.currentPanel == 1, "Disconnect did not return to login.");
            Check(Get<Label>(form, "noMapLabel").Parent == Get<Panel>(form, "mapPanel"), "Disconnect detached the empty mappings label.");
            search.Text = "disconnected";
            Call(form, "updateDeviceList", false);
            form.Close();
        }

        System.Globalization.CultureInfo originalCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
        try
        {
            foreach (string language in new[] { "da", "de" })
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(language);
                using (MainForm translated = new MainForm(new string[0]))
                {
                    translated.forceExit = true;
                    translated.StartPosition = FormStartPosition.Manual;
                    translated.Location = new Point(-20000, -20000);
                    translated.Show();
                    Application.DoEvents();
                    Fits(Get<Control>(translated, "rememberPasswordCheckBox"));
                    Fits(Get<Control>(translated, "nextButton1"));
                    Call(translated, "setPanel", 2);
                    Get<CheckBox>(translated, "tokenRememberCheckBox").Visible = true;
                    Fits(Get<Control>(translated, "tokenTextBox"));
                    Fits(Get<Control>(translated, "nextButton2"));
                    Call(translated, "setPanel", 3);
                    Fits(Get<Control>(translated, "label4"));
                    Fits(Get<Control>(translated, "certDetailsTextBox"));
                    Fits(Get<Control>(translated, "nextButton3"));
                    translated.Close();
                }
            }
        }
        finally
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = originalCulture;
        }
    }

    private sealed class ListMessages : NativeWindow, IDisposable
    {
        internal int DeleteAllCount;
        internal int RedrawToggleCount;
        internal ListMessages(IntPtr handle) { AssignHandle(handle); }
        protected override void WndProc(ref Message message)
        {
            if (message.Msg == 0x1000 + 9) DeleteAllCount++;
            if (message.Msg == 0x000B) RedrawToggleCount++;
            base.WndProc(ref message);
        }
        public void Dispose() { ReleaseHandle(); }
    }
}
