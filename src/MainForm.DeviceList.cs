using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MeshCentralRouter
{
    public partial class MainForm
    {
        private void updateDeviceListView(bool applyGroupPreference, string search)
        {
            Dictionary<string, NodeClass> nodes = meshcentral.nodes;
            List<NodeClass> snapshot = new List<NodeClass>();
            if (nodes != null) { lock (nodes) { snapshot.AddRange(nodes.Values); } }

            Dictionary<string, ListViewGroup> groups = new Dictionary<string, ListViewGroup>();
            foreach (ListViewGroup group in devicesListView.Groups)
            {
                groups[(string)group.Tag ?? ""] = group;
            }

            HashSet<ListViewItem> visibleItems = new HashSet<ListViewItem>();
            HashSet<ListViewGroup> visibleGroups = new HashSet<ListViewGroup>();
            List<ListViewGroup> newGroups = new List<ListViewGroup>();
            ListViewItem topItem = devicesListView.Items.Count > 0 ? devicesListView.TopItem : null;
            bool updating = false;
            bool sortNeeded = false;
            int deviceCount = 0;
            string userSearch = null;
            if (search.StartsWith("u:")) { userSearch = search.Substring(2); }
            if (search.StartsWith("user:")) { userSearch = search.Substring(5); }

            // Suspend painting only if something actually changes. Repeated identical events are a no-op.
            Action beginUpdate = () =>
            {
                if (!updating) { devicesListView.BeginUpdate(); updating = true; }
            };

            try
            {
                foreach (NodeClass node in snapshot)
                {
                    if (node.agentid == -1) continue;
                    deviceCount++;
                    MeshClass mesh = null;
                    if (node.meshid != null && meshcentral.meshes != null) { meshcentral.meshes.TryGetValue(node.meshid, out mesh); }
                    node.mesh = mesh;
                    string groupName = mesh != null ? mesh.name : Translate.T(Properties.Resources.IndividualDevices);
                    string displayName = showGroupNamesToolStripMenuItem.Checked && mesh != null ? groupName + " - " + node.name : node.name;
                    bool connected = (node.conn & 1) != 0 || node.mtype == 3;
                    if (!showOfflineDevicesToolStripMenuItem.Checked && !connected) continue;

                    bool matches = string.IsNullOrEmpty(search);
                    if (userSearch == null)
                    {
                        matches |= (displayName ?? "").IndexOf(search, StringComparison.CurrentCultureIgnoreCase) >= 0;
                    }
                    else
                    {
                        matches = userSearch.Length == 0;
                        if (node.users != null)
                        {
                            foreach (string user in node.users)
                            {
                                if (user != null && user.IndexOf(userSearch, StringComparison.CurrentCultureIgnoreCase) >= 0) { matches = true; break; }
                            }
                        }
                    }
                    if (!matches) continue;

                    string groupKey = node.meshid ?? "";
                    ListViewGroup group;
                    if (!groups.TryGetValue(groupKey, out group))
                    {
                        beginUpdate();
                        group = new ListViewGroup(groupName) { Tag = node.meshid };
                        int index = 0;
                        while (index < devicesListView.Groups.Count && string.Compare(devicesListView.Groups[index].Header, groupName, StringComparison.CurrentCulture) < 0) { index++; }
                        devicesListView.Groups.Insert(index, group);
                        groups.Add(groupKey, group);
                        newGroups.Add(group);
                    }
                    else if (group.Header != groupName)
                    {
                        beginUpdate();
                        group.Header = groupName;
                    }
                    visibleGroups.Add(group);

                    ListViewItem item = node.listitem;
                    if (item == null)
                    {
                        item = new ListViewItem(displayName);
                        item.SubItems.Add(node.getStateString());
                        node.listitem = item;
                    }
                    if (!ReferenceEquals(item.Tag, node)) { item.Tag = node; }
                    if (item.Text != displayName) { beginUpdate(); item.Text = displayName; sortNeeded = true; }
                    string state = node.getStateString();
                    if (item.SubItems[1].Text != state) { beginUpdate(); item.SubItems[1].Text = state; }
                    int imageIndex = Math.Max(0, node.icon - 1) * 2 + (connected ? 0 : 1);
                    if (item.ImageIndex != imageIndex) { beginUpdate(); item.ImageIndex = imageIndex; }
                    if (item.Group != group) { beginUpdate(); item.Group = group; }
                    if (item.ListView != devicesListView) { beginUpdate(); devicesListView.Items.Add(item); }
                    visibleItems.Add(item);
                }

                for (int i = devicesListView.Items.Count - 1; i >= 0; i--)
                {
                    if (!visibleItems.Contains(devicesListView.Items[i])) { beginUpdate(); devicesListView.Items.RemoveAt(i); }
                }
                for (int i = devicesListView.Groups.Count - 1; i >= 0; i--)
                {
                    if (!visibleGroups.Contains(devicesListView.Groups[i])) { beginUpdate(); devicesListView.Groups.RemoveAt(i); }
                }
                if (sortNeeded) { devicesListView.Sort(); }

                // Never reset groups expanded/collapsed by the user on ordinary status events.
                foreach (ListViewGroup group in devicesListView.Groups)
                {
                    if (applyGroupPreference || newGroups.Contains(group))
                    {
                        beginUpdate();
                        ListViewExtended.setGrpState(group, ListViewGroupState.Collapsible | (collapseDeviceGroup ? ListViewGroupState.Collapsed : ListViewGroupState.Normal));
                    }
                }
            }
            finally
            {
                if (updating)
                {
                    devicesListView.EndUpdate();
                    if (topItem != null && topItem.ListView == devicesListView && devicesListView.TopItem != topItem)
                    {
                        devicesListView.TopItem = topItem;
                    }
                }
            }

            noDevicesLabel.Visible = deviceCount == 0;
            noSearchResultsLabel.Visible = deviceCount > 0 && visibleItems.Count == 0;
            devicesListView.Visible = visibleItems.Count > 0;
            if (noDevicesLabel.Visible) { noDevicesLabel.BringToFront(); }
            if (noSearchResultsLabel.Visible) { noSearchResultsLabel.BringToFront(); }
        }
    }
}
