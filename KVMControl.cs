/*
Copyright 2009-2020 Intel Corporation

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

namespace MeshCentralRouter
{
    public partial class KVMControl : UserControl
    {
        private bool remotepause = true;
        private Bitmap desktop = null;
        private Graphics desktopGraphics = null;
        private uint screenWidth = 0;
        private uint screenHeight = 0;
        private Pen RedPen = new Pen(System.Drawing.Color.Red);
        private Font DebugFont = new Font(FontFamily.GenericSansSerif, 14);
        private int compressionlevel = 60; // 60% compression
        private int scalinglevel = 1024; // 100% scale
        private int frameRate = 100; // Medium frame rate
        private double scalefactor = 1;
        public List<string> displays = new List<string>();
        public ushort currentDisp = 0;
        public bool MouseButtonLeft = false;
        public bool MouseButtonMiddle = false;
        public bool MouseButtonRight = false;
        public double DpiX = 96;
        public double DpiY = 96;
        public KVMViewer parent = null;

        private enum KvmCommands
        {
            Nop = 0,
            Key = 1,
            Mouse = 2,
            Picture = 3,
            Copy = 4,
            Compression = 5,
            Refresh = 6,
            Screen = 7,
            Pause = 8,
            TerminalText = 9,
            CtrlAltDel = 10,
            GetDisplays = 11,
            SetDisplay = 12,
            FrameRateTimer = 13,
            InitTouch = 14,
            Touch = 15,
            Message = 16,
            Disconnect = 59,
            Alert = 65,
            MouseCursor = 88
        }

        private enum KvmMouseButtonCommands
        {
            MOUSEEVENTF_LEFTDOWN = 		0x0002,
            MOUSEEVENTF_RIGHTDOWN =		0x0008,
            MOUSEEVENTF_MIDDLEDOWN =	0x0020,
            MOUSEEVENTF_LEFTUP =		0x0004,
            MOUSEEVENTF_RIGHTUP =		0x0010,
            MOUSEEVENTF_MIDDLEUP =  	0x0040
        }

        [Category("Action")]
        [Description("Fires when the remote desktop size changes.")]
        public event EventHandler DesktopSizeChanged;

        [Category("Action")]
        [Description("Fires when it receives the display list.")]
        public event EventHandler DisplaysReceived;

        public enum ConnectState
        {
            Disconnected,
            Connecting,
            Connected
        }

        public int CompressionLevel { get { return compressionlevel; } set { compressionlevel = value; SendCompressionLevel(); } }
        public int ScalingLevel { get { return scalinglevel; } set { scalinglevel = value; SendCompressionLevel(); } }
        public int FrameRate { get { return frameRate; } set { frameRate = value; SendCompressionLevel(); } }
        public double ScaleFactor { get { return scalefactor; } set { scalefactor = value; } }

        public void SetCompressionParams(int level, int scaling, int framerate) { compressionlevel = level; scalinglevel = scaling; frameRate = framerate; SendCompressionLevel(); }
        public int DesktopWidth { get { return (int)screenWidth; } }
        public int DesktopHeight { get { return (int)screenHeight; } }

        // Debug
        public int bytesent = 0;
        public int byterecv = 0;
        public int tilecount = 0;
        public int tilecopy = 0;
        public bool debugmode = false;

        public KVMControl()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.Selectable, false);
            this.SetStyle(ControlStyles.UserMouse, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(KVMControl_MouseWheel);
        }

        public int ProcessData(byte[] buffer, int off, int len)
        {
            if (len == 0) return 0;
            if (len >= 4)
            {
                KvmCommands btype = (KvmCommands)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(buffer, off));
                ushort blen = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(buffer, off + 2));
                if (len < blen) return 0;

                switch (btype)
                {
                    case KvmCommands.Screen:
                        {
                            if (blen != 8) return blen;
                            uint w = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(buffer, off + 4));
                            uint h = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(buffer, off + 6));
                            if (screenWidth == w && screenHeight == h) break;
                            screenWidth = w;
                            screenHeight = h;
                            desktop = new Bitmap((int)screenWidth, (int)screenHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                            desktopGraphics = Graphics.FromImage(desktop);
                            SendCompressionLevel();
                            Invoke(new SetSizeHandler(SetSize));
                            SendPause(false);
                            SendMouse(2, KvmMouseButtonCommands.MOUSEEVENTF_LEFTUP);
                            SendMouse(2, KvmMouseButtonCommands.MOUSEEVENTF_MIDDLEUP);
                            SendMouse(2, KvmMouseButtonCommands.MOUSEEVENTF_RIGHTUP);
                            return blen;
                        }
                    case KvmCommands.Picture:
                        {
                            Image newtile;
                            ushort tile_x = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(buffer, off + 4));
                            ushort tile_y = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(buffer, off + 6));
                            try { newtile = Image.FromStream(new System.IO.MemoryStream(buffer, off + 8, blen - 8)); } catch (Exception) { return blen; }
                            Rectangle r = new Rectangle((int)tile_x, (int)tile_y, newtile.Width, newtile.Height);
                            Rectangle r3 = new Rectangle((int)((double)tile_x / (double)scalefactor) - 2, (int)((double)tile_y / (double)scalefactor) - 2, (int)((double)newtile.Width / (double)scalefactor) + 4, (int)((double)newtile.Height / (double)scalefactor) + 4);
                            tilecount++;

                            // Winform mode
                            if (desktop != null)
                            {
                                lock (desktop)
                                {
                                    desktopGraphics.DrawImage(newtile, new Rectangle((int)tile_x, (int)tile_y, newtile.Width, newtile.Height), new Rectangle(0, 0, newtile.Width, newtile.Height), GraphicsUnit.Pixel);
                                    if (debugmode)
                                    {
                                        desktopGraphics.DrawRectangle(RedPen, new Rectangle((int)tile_x, (int)tile_y, newtile.Width - 1, newtile.Height - 1));
                                        desktopGraphics.DrawString(string.Format("{0} / {1}kb", tilecount, blen / 2014), DebugFont, RedPen.Brush, new Point((int)tile_x, (int)tile_y));
                                    }
                                }
                                if (scalefactor == 1) Invalidate(r, false); else Invalidate(r3, false);
                            }

                            return blen;
                        }
                    case KvmCommands.Copy:
                        {
                            ushort sourcex = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(buffer, off + 4));
                            ushort sourcey = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(buffer, off + 6));
                            ushort targetx = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(buffer, off + 8));
                            ushort targety = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(buffer, off + 10));
                            ushort tilew   = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(buffer, off + 12));
                            ushort tileh   = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(buffer, off + 14));

                            Rectangle r1 = new Rectangle((int)sourcex, (int)sourcey, (int)tilew, (int)tileh);
                            Rectangle r2 = new Rectangle((int)targetx, (int)targety, (int)tilew, (int)tileh);
                            Rectangle r3 = new Rectangle((int)((double)targetx / (double)scalefactor) - 2, (int)((double)targety / (double)scalefactor) - 2, (int)((double)tilew / (double)scalefactor) + 4, (int)((double)tileh / (double)scalefactor) + 4);

                            // WinForm mode
                            if (desktop != null)
                            {
                                lock (desktop)
                                {
                                    Bitmap bmp = new Bitmap(tilew, tileh);
                                    Graphics g = Graphics.FromImage(bmp);
                                    g.DrawImage(desktop, 0, 0, r1, GraphicsUnit.Pixel);
                                    g.Dispose();
                                    desktopGraphics.Flush();
                                    desktopGraphics.DrawImage(bmp, r2, 0, 0, tilew, tileh, GraphicsUnit.Pixel);
                                    if (debugmode) { desktopGraphics.DrawString("COPY", DebugFont, RedPen.Brush, new Point((int)targetx, (int)targety + 20)); }
                                }
                                tilecopy++;
                            }

                            return blen;
                        }
                    case KvmCommands.GetDisplays:
                        {
                            int i = 0;
                            ushort length = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(buffer, off + 4));
                            displays.Clear();
                            if (length > 0)
                            {
                                for (i = 0; i < length; i++)
                                {
                                    ushort num = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(buffer, off + 6 + i * 2));
                                    if (num == 0xFFFF)
                                    {
                                        displays.Add("All Displays");
                                    }
                                    else
                                    {
                                        displays.Add("Display " + num);
                                    }
                                }
                            }
                            currentDisp = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(buffer, off + 6 + i * 2));
                            if (DisplaysReceived != null) DisplaysReceived(this, null);
                            break;
                        }
                    case KvmCommands.SetDisplay:
                        {
                            currentDisp = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(buffer, off + 4));
                            break;
                        }
                    case KvmCommands.MouseCursor:
                        {
                            if (blen != 5) return blen;
                            ChangeMouseCursor(buffer[off + 4]);
                            break;
                        }
                    default:
                        {
                            // MessageBox.Show("Should never happen!");
                            //Disconnect();
                            return 0;
                        }
                }
                return blen;
            }
            return 0;
        }

        private delegate void ChangeMouseCursorHandler(int cursorId);
        private void ChangeMouseCursor(int cursorId)
        {
            if (this.InvokeRequired) { this.Invoke(new ChangeMouseCursorHandler(ChangeMouseCursor), cursorId);  return; }
            if (cursorId == 0) { this.Cursor = System.Windows.Forms.Cursors.Default; return; } // default
            if (cursorId == 1) { this.Cursor = System.Windows.Forms.Cursors.AppStarting; return; } // progress
            if (cursorId == 2) { this.Cursor = System.Windows.Forms.Cursors.Cross; return; } // crosshair
            if (cursorId == 3) { this.Cursor = System.Windows.Forms.Cursors.Default; return; } // pointer
            if (cursorId == 4) { this.Cursor = System.Windows.Forms.Cursors.Help; return; } // help
            if (cursorId == 5) { this.Cursor = System.Windows.Forms.Cursors.IBeam; return; } // text
            if (cursorId == 6) { this.Cursor = System.Windows.Forms.Cursors.No; return; } // no-drop
            if (cursorId == 7) { this.Cursor = System.Windows.Forms.Cursors.Arrow; return; } // move
            if (cursorId == 8) { this.Cursor = System.Windows.Forms.Cursors.SizeNESW; return; } // nesw-resize
            if (cursorId == 9) { this.Cursor = System.Windows.Forms.Cursors.SizeNS; return; } // ns-resize
            if (cursorId == 10) { this.Cursor = System.Windows.Forms.Cursors.SizeNWSE; return; } // nwse-resize
            if (cursorId == 11) { this.Cursor = System.Windows.Forms.Cursors.SizeWE; return; } // w-resize
            if (cursorId == 12) { this.Cursor = System.Windows.Forms.Cursors.Arrow; return; } // alias
            if (cursorId == 13) { this.Cursor = System.Windows.Forms.Cursors.WaitCursor; return; } // wait
            if (cursorId == 14) { this.Cursor = System.Windows.Forms.Cursors.Default; return; } // none
            if (cursorId == 15) { this.Cursor = System.Windows.Forms.Cursors.No; return; } // not-allowed
            if (cursorId == 16) { this.Cursor = System.Windows.Forms.Cursors.VSplit; return; } // col-resize
            if (cursorId == 17) { this.Cursor = System.Windows.Forms.Cursors.HSplit; return; } // row-resize
            if (cursorId == 18) { this.Cursor = System.Windows.Forms.Cursors.Default; return; } // copy
            if (cursorId == 19) { this.Cursor = System.Windows.Forms.Cursors.SizeAll; return; } // zoom-in
            if (cursorId == 20) { this.Cursor = System.Windows.Forms.Cursors.SizeAll; return; } // zoom-out
        }

        public void Repaint(Region region)
        {
            Invalidate(region);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Do not paint background.
        }

        private delegate void SetSizeHandler();
        private void SetSize()
        {
            if (DesktopSizeChanged != null) { DesktopSizeChanged(this, null); }
        }

        private void KVMControl_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (desktop == null)
            {
                g.FillRectangle(new SolidBrush(Color.Black), e.ClipRectangle);
            }
            else
            {
                lock (desktop)
                {
                    if (scalefactor == 1)
                    {
                        g.DrawImage(desktop, e.ClipRectangle, e.ClipRectangle, GraphicsUnit.Pixel);
                    }
                    else
                    {
                        //g.DrawImage((Image)desktop, new Rectangle(0, 0, Width, Height), 0, 0, screenWidth, screenHeight, GraphicsUnit.Pixel);
                        g.DrawImage((Image)desktop, e.ClipRectangle, (int)((double)e.ClipRectangle.Left * (double)scalefactor), (int)((double)e.ClipRectangle.Top * (double)scalefactor), (int)((double)e.ClipRectangle.Width * (double)scalefactor), (int)((double)e.ClipRectangle.Height * (double)scalefactor), GraphicsUnit.Pixel);
                    }
                }
            }
        }

        public void SendKey(byte key, byte action)
        {
            //if (state != ConnectState.Connected) return;

            BinaryWriter bw = GetBinaryWriter();
            bw.Write(IPAddress.HostToNetworkOrder((short)KvmCommands.Key));
            bw.Write(IPAddress.HostToNetworkOrder((short)6));
            bw.Write((byte)action);
            bw.Write((byte)key);
            Send(bw);
        }

        private void SendKey(KeyEventArgs e, byte action)
        {
            //if (state != ConnectState.Connected) return;

            BinaryWriter bw = GetBinaryWriter();
            bw.Write(IPAddress.HostToNetworkOrder((short)KvmCommands.Key));
            bw.Write(IPAddress.HostToNetworkOrder((short)6));
            bw.Write((byte)action);
            bw.Write((byte)e.KeyCode);
            Send(bw);
        }

        private short LastX = 0;
        private short LastY = 0;
        private void SendMouse(MouseEventArgs e, byte action)
        {
            //if (state != ConnectState.Connected) return;
            byte buttons = 0;
            if (action == 1)
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        buttons |= (byte)KvmMouseButtonCommands.MOUSEEVENTF_LEFTDOWN;
                        break;
                    case MouseButtons.Right:
                        buttons |= (byte)KvmMouseButtonCommands.MOUSEEVENTF_RIGHTDOWN;
                        break;
                    case MouseButtons.Middle:
                        buttons |= (byte)KvmMouseButtonCommands.MOUSEEVENTF_MIDDLEDOWN;
                        break;
                }
            }
            else if (action == 2)
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        buttons |= (byte)KvmMouseButtonCommands.MOUSEEVENTF_LEFTUP;
                        break;
                    case MouseButtons.Right:
                        buttons |= (byte)KvmMouseButtonCommands.MOUSEEVENTF_RIGHTUP;
                        break;
                    case MouseButtons.Middle:
                        buttons |= (byte)KvmMouseButtonCommands.MOUSEEVENTF_MIDDLEUP;
                        break;
                }
            }

            LastX = (short)((double)e.X * (double)scalefactor);
            LastY = (short)((double)e.Y * (double)scalefactor);

            BinaryWriter bw = GetBinaryWriter();
            bw.Write(IPAddress.HostToNetworkOrder((short)KvmCommands.Mouse));
            bw.Write(IPAddress.HostToNetworkOrder((short)(e.Delta == 0 ? 10 : 12)));
            bw.Write((byte)action);
            bw.Write((byte)buttons);
            bw.Write(IPAddress.HostToNetworkOrder(LastX));
            bw.Write(IPAddress.HostToNetworkOrder(LastY));
            if (e.Delta != 0) { bw.Write(IPAddress.HostToNetworkOrder((short)e.Delta)); }
            Send(bw);
        }

        public void SendMouseWheel(MouseEventArgs e, byte action)
        {
            //if (state != ConnectState.Connected) return;

            BinaryWriter bw = GetBinaryWriter();
            bw.Write(IPAddress.HostToNetworkOrder((short)KvmCommands.Mouse));
            bw.Write(IPAddress.HostToNetworkOrder((short)(e.Delta == 0 ? 10 : 12)));
            bw.Write((short)0);
            bw.Write(IPAddress.HostToNetworkOrder(LastX));
            bw.Write(IPAddress.HostToNetworkOrder(LastY));
            if (e.Delta != 0) { bw.Write(IPAddress.HostToNetworkOrder((short)e.Delta)); }
            Send(bw);
        }

        private void SendMouse(byte action, KvmMouseButtonCommands buttons)
        {
            //if (state != ConnectState.Connected) return;

            BinaryWriter bw = GetBinaryWriter();
            bw.Write(IPAddress.HostToNetworkOrder((short)KvmCommands.Mouse));
            bw.Write(IPAddress.HostToNetworkOrder((short)10));
            bw.Write((byte)action);
            bw.Write((byte)buttons);
            bw.Write((short)0);
            bw.Write((short)0);
            Send(bw);
        }

        public void SendCompressionLevel()
        {
            //if (state != ConnectState.Connected) return;

            BinaryWriter bw = GetBinaryWriter();
            bw.Write(IPAddress.HostToNetworkOrder((short)KvmCommands.Compression));
            bw.Write(IPAddress.HostToNetworkOrder((short)10));
            bw.Write((byte)1);                                                                      // Image Type (Reserved, 1 = JPEG)
            bw.Write((byte)compressionlevel);                                                       // Compression Level
            bw.Write(IPAddress.HostToNetworkOrder((short)scalinglevel));                            // Scaling
            bw.Write(IPAddress.HostToNetworkOrder((short)frameRate));                               // Frame Rate
            Send(bw);
        }

        public void SendRefresh()
        {
            //if (state != ConnectState.Connected) return;

            BinaryWriter bw = GetBinaryWriter();
            bw.Write(IPAddress.HostToNetworkOrder((short)KvmCommands.Refresh));
            bw.Write(IPAddress.HostToNetworkOrder((short)4));
            Send(bw);
        }

        public void SendPause(bool paused)
        {
            //if (remotepauseactive == false || paused == remotepause) return;
            //if (paused == remotepause) return;
            remotepause = paused;
            
            BinaryWriter bw = GetBinaryWriter();
            bw.Write(IPAddress.HostToNetworkOrder((short)KvmCommands.Pause));
            bw.Write(IPAddress.HostToNetworkOrder((short)5));
            bw.Write(paused ? (byte)1 : (byte)0);
            Send(bw);
        }

        public void Send(byte[] buffer)
        {
            //if (state == ConnectState.Disconnected) return;
            try
            {
                parent.wc.WriteBinaryWebSocket(buffer, 0, buffer.Length);
                bytesent += buffer.Length;
            }
            catch (Exception) { }
        }

        public void Send(string str)
        {
            //if (state == ConnectState.Disconnected) return;
            try
            {
                parent.wc.WriteStringWebSocket(str);
                bytesent += str.Length;
            }
            catch (Exception) { }
        }

        public void Send(BinaryWriter bw)
        {
            //if (state == ConnectState.Disconnected) { RecycleBinaryWriter(bw); return; }
            try
            {
                parent.wc.WriteBinaryWebSocket(((MemoryStream)bw.BaseStream).GetBuffer(), 0, (int)((MemoryStream)bw.BaseStream).Length);
                bytesent += (int)((MemoryStream)bw.BaseStream).Length;
            }
            catch (Exception) { }
            RecycleBinaryWriter(bw);
        }
        
        private void KVMControl_MouseDown(object sender, MouseEventArgs e)
        {
            SendMouse(e, 1);
        }

        private void KVMControl_MouseUp(object sender, MouseEventArgs e)
        {
            SendMouse(e, 2);
        }

        long lastMouseMove = 0;

        private void KVMControl_MouseMove(object sender, MouseEventArgs e)
        {
            long ct = DateTime.Now.Ticks;
            if ((lastMouseMove + 30) < ct) {
                lastMouseMove = ct;
                SendMouse(e, 0);
            }
        }

        public void KVMControl_MouseWheel(object sender, MouseEventArgs e)
        {
            SendMouse(e, 0);
        }

        private void KVMControl_KeyDown(object sender, KeyEventArgs e)
        {
            SendKey(e, 0);
            e.Handled = true;
        }

        private void KVMControl_KeyUp(object sender, KeyEventArgs e)
        {
            SendKey(e, 1);
            e.Handled = true;
        }

        private void KVMControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void KVMControl_MouseEnter(object sender, EventArgs e)
        {
            this.Focus();
        }

        public void SendCtrlAltDel()
        {
            //if (state != ConnectState.Connected) return;
            BinaryWriter bw = GetBinaryWriter();
            bw.Write(IPAddress.HostToNetworkOrder((short)KvmCommands.CtrlAltDel));
            bw.Write(IPAddress.HostToNetworkOrder((short)4));
            Send(bw);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int WM_KEYDOWN = 0x100;
            const int WM_SYSKEYDOWN = 0x104;

            // Tab keys
            if (msg.Msg == WM_KEYDOWN && msg.WParam.ToInt32() == 9)
            {
                SendKey((byte)msg.WParam.ToInt32(), 0);
                return true;
            }

            // Handle arrow keys
            if (((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN)) && msg.WParam.ToInt32() >= 37 && msg.WParam.ToInt32() <= 40)
            {
                SendKey((byte)msg.WParam.ToInt32(), 0);
                return true;
            }

            return false;
        }

        public void GetDisplayNumbers()
        {
            //if (state != ConnectState.Connected) return;

            BinaryWriter bw = GetBinaryWriter();
            bw.Write(IPAddress.HostToNetworkOrder((short)KvmCommands.GetDisplays));
            bw.Write(IPAddress.HostToNetworkOrder((short)4));
            Send(bw);
        }

        public void SendDisplay(int dispNo)
        {
            //if (state != ConnectState.Connected) return;

            BinaryWriter bw = GetBinaryWriter();
            bw.Write(IPAddress.HostToNetworkOrder((short)KvmCommands.SetDisplay));
            bw.Write(IPAddress.HostToNetworkOrder((short)6));
            bw.Write(IPAddress.HostToNetworkOrder((short)dispNo));
            Send(bw);
        }

        public void SendWindowsKey()
        {
            SendKey(0x5B, 4);   // Windows key down
            SendKey(0x5B, 3);   // Windows key up
        }

        public void SendCharmsKey()
        {
            SendKey(0x5B, 4);   // Windows key down
            SendKey(67, 1);     // C down
            SendKey(67, 2);     // C up
            SendKey(0x5B, 3);   // Windows key up
        }

        // BinaryWriter Object Recycling System
        private static Stack<BinaryWriter> BinaryWriteRecycleList = new Stack<BinaryWriter>();
        public static BinaryWriter GetBinaryWriter() { lock (BinaryWriteRecycleList) { return (BinaryWriteRecycleList.Count == 0) ? new BinaryWriter(new MemoryStream(32000), Encoding.UTF8) : BinaryWriteRecycleList.Pop(); } }
        public static void RecycleBinaryWriter(BinaryWriter obj) { lock (BinaryWriteRecycleList) { ((MemoryStream)obj.BaseStream).SetLength(0); BinaryWriteRecycleList.Push(obj); } }

        // MemoryStream Object Recycling System
        private static Stack<MemoryStream> MemoryStreamRecycleList = new Stack<MemoryStream>();
        public static MemoryStream GetMemoryStream() { lock (MemoryStreamRecycleList) { return (MemoryStreamRecycleList.Count == 0) ? new MemoryStream(32000) : MemoryStreamRecycleList.Pop(); } }
        public static void RecycleMemoryStream(MemoryStream obj) { lock (MemoryStreamRecycleList) { obj.SetLength(0); MemoryStreamRecycleList.Push(obj); } }

        // StringBuilder Object Recycling System
        private static Stack<StringBuilder> StringBuilderRecycleList = new Stack<StringBuilder>();
        public static StringBuilder GetStringBuilder() { lock (StringBuilderRecycleList) { return (StringBuilderRecycleList.Count == 0) ? new StringBuilder(16000) : StringBuilderRecycleList.Pop(); } }
        public static void RecycleStringBuilder(StringBuilder obj) { lock (StringBuilderRecycleList) { obj.Length = 0; StringBuilderRecycleList.Push(obj); } }

        // byte[] Object Recycling System
        private static Stack<byte[]> ByteArrayRecycleList = new Stack<byte[]>();
        private static int ByteArrayRecycleListCount = 0;
        public static byte[] GetByteArray() { lock (ByteArrayRecycleList) { if (ByteArrayRecycleList.Count == 0) ByteArrayRecycleListCount++; return (ByteArrayRecycleList.Count == 0) ? new byte[65535] : ByteArrayRecycleList.Pop(); } }
        public static void RecycleByteArray(byte[] obj) { lock (ByteArrayRecycleList) { if (obj != null) ByteArrayRecycleList.Push(obj); } }

    }
}
