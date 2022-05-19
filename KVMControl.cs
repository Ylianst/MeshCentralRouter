/*
Copyright 2009-2022 Intel Corporation

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
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace MeshCentralRouter
{
    public partial class KVMControl : UserControl
    {
        private bool remotepause = true;
        public Bitmap desktop = null;
        private Graphics desktopGraphics = null;
        public uint screenWidth = 0;
        public uint screenHeight = 0;
        private Pen RedPen = new Pen(System.Drawing.Color.Red);
        private Font DebugFont = new Font(FontFamily.GenericSansSerif, 14);
        private int compressionlevel = 60; // 60% compression
        private int scalinglevel = 1024; // 100% scale
        private int frameRate = 100; // Medium frame rate
        private bool swamMouseButtons = false;
        private bool remoteKeyboardMap = false;
        private bool autoSendClipboard = false;
        public bool AutoReconnect = false;
        private double scalefactor = 1;
        public List<ushort> displays = new List<ushort>();
        public ushort currentDisp = 0;
        public bool MouseButtonLeft = false;
        public bool MouseButtonMiddle = false;
        public bool MouseButtonRight = false;
        public double DpiX = 96;
        public double DpiY = 96;
        public KVMViewer parent = null;
        public KVMViewerExtra parentEx = null;
        private readonly KVMControlHook ControlHook;
        private readonly KVMControlHook.KVMCallback KeyboardCallback;
        private bool isHookWanted;
        private bool isHookPriority;
        private bool keyboardIsAttached;
        private long killNextKeyPress = 0;
        private bool controlLoaded = false;
        public Rectangle[] displayInfo = null;
        public Rectangle displayCrop = Rectangle.Empty;
        public Point displayOrigin = Point.Empty;


    //System level functions to be used for hook and unhook keyboard input  
    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int id, LowLevelKeyboardProc callback, IntPtr hMod, uint dwThreadId);
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool UnhookWindowsHookEx(IntPtr hook);
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hook, int nCode, IntPtr wp, IntPtr lp);
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)] 
    private static extern IntPtr GetModuleHandle(string name);
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern short GetAsyncKeyState(Keys key);
    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();


    [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)] public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
    [DllImport("gdi32.dll", ExactSpelling = true)] public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
    [DllImport("gdi32.dll", ExactSpelling = true)] public static extern IntPtr DeleteObject(IntPtr hObject);
    [DllImport("gdi32.dll")] private static extern bool BitBlt(IntPtr hdcDest, Int32 nXDest, Int32 nYDest, Int32 nWidth, Int32 nHeight, IntPtr hdcSrc, Int32 nXSrc, Int32 nYSrc, TernaryRasterOperations dwRop);
    [DllImport("gdi32.dll")] private static extern bool StretchBlt(IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest, int nHeightDest, IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc, TernaryRasterOperations dwRop);
    [DllImport("gdi32.dll")] static extern bool SetStretchBltMode(IntPtr hdc, StretchMode iStretchMode);
    [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)] private static extern bool DeleteDC(IntPtr hdc);
    public enum StretchMode
    {
      STRETCH_ANDSCANS = 1,
      STRETCH_ORSCANS = 2,
      STRETCH_DELETESCANS = 3,
      STRETCH_HALFTONE = 4,
    }

    public enum TernaryRasterOperations
    {
      SRCCOPY = 0x00CC0020, /* dest = source*/
      SRCPAINT = 0x00EE0086, /* dest = source OR dest*/
      SRCAND = 0x008800C6, /* dest = source AND dest*/
      SRCINVERT = 0x00660046, /* dest = source XOR dest*/
      SRCERASE = 0x00440328, /* dest = source AND (NOT dest )*/
      NOTSRCCOPY = 0x00330008, /* dest = (NOT source)*/
      NOTSRCERASE = 0x001100A6, /* dest = (NOT src) AND (NOT dest) */
      MERGECOPY = 0x00C000CA, /* dest = (source AND pattern)*/
      MERGEPAINT = 0x00BB0226, /* dest = (NOT source) OR dest*/
      PATCOPY = 0x00F00021, /* dest = pattern*/
      PATPAINT = 0x00FB0A09, /* dest = DPSnoo*/
      PATINVERT = 0x005A0049, /* dest = pattern XOR dest*/
      DSTINVERT = 0x00550009, /* dest = (NOT dest)*/
      BLACKNESS = 0x00000042, /* dest = BLACK*/
      WHITENESS = 0x00FF0062, /* dest = WHITE*/
    };

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
            Jumbo = 27,
            Disconnect = 59,
            Alert = 65,
            DisplayInfo = 82,
            KeyUnicode = 85,
            MouseCursor = 88
        }

        private enum KvmMouseButtonCommands
        {
            MOUSEEVENTF_LEFTDOWN = 0x0002,
            MOUSEEVENTF_RIGHTDOWN = 0x0008,
            MOUSEEVENTF_MIDDLEDOWN = 0x0020,
            MOUSEEVENTF_LEFTUP = 0x0004,
            MOUSEEVENTF_RIGHTUP = 0x0010,
            MOUSEEVENTF_MIDDLEUP = 0x0040
        }

        [Category("Action")]
        [Description("Fires when the remote desktop size changes.")]
        public event EventHandler DesktopSizeChanged;

        [Category("Action")]
        [Description("Fires when it receives the display list.")]
        public event EventHandler DisplaysReceived;

        public delegate void ScreenAreaUpdatedHandler(Bitmap desktop, Rectangle r);
        public event ScreenAreaUpdatedHandler ScreenAreaUpdated;

        public enum ConnectState
        {
            Disconnected,
            Connecting,
            Connected
        }

        public int CompressionLevel { get { return compressionlevel; } set { compressionlevel = value; SendCompressionLevel(); } }
        public int ScalingLevel { get { return scalinglevel; } set { scalinglevel = value; SendCompressionLevel(); } }
        public int FrameRate { get { return frameRate; } set { frameRate = value; SendCompressionLevel(); } }
        public bool SwamMouseButtons { get { return swamMouseButtons; } set { swamMouseButtons = value; } }
        public bool RemoteKeyboardMap { get { return remoteKeyboardMap; } set { remoteKeyboardMap = value; } }
        public bool AutoSendClipboard { get { return autoSendClipboard; } set { autoSendClipboard = value; } }

        public double ScaleFactor { get { return scalefactor; } set { scalefactor = value; } }

        public void SetCompressionParams(int level, int scaling, int framerate) { compressionlevel = level; scalinglevel = scaling; frameRate = framerate; SendCompressionLevel(); }
        public int DesktopWidth
        {
            get { if (displayCrop == Rectangle.Empty) { return (int)screenWidth; } else { return displayCrop.Width; } }
        }
        public int DesktopHeight
        {
            get { if (displayCrop == Rectangle.Empty) { return (int)screenHeight; } else { return displayCrop.Height; } }
        }

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
            if (Settings.GetRegValue("Exp_KeyboardHook", false))
            {
                ControlHook = new KVMControlHook();
                KeyboardCallback = SendKey;
                isHookWanted = true;
            }
            else
            {
                isHookWanted = false;
            }
            isHookPriority = Settings.GetRegValue("Exp_KeyboardHookPriority", false);
        }

        public void AttachKeyboard()
        {
            //Console.WriteLine(isHookWanted);
            if (!keyboardIsAttached && isHookWanted)
            {
                ControlHook.AttachKeyboardHook(SendKey);
                keyboardIsAttached = true;
            }
        }

        public void DetacheKeyboard()
        {
            if (keyboardIsAttached)
            {
                ControlHook.DetachKeyboardHook();
                keyboardIsAttached = false;
            }
        }

        public int ProcessData(byte[] buffer, int off, int len)
        {
            //Console.Write("ProcessData " + off + ", " + len + "\r\n");
            int jumboHeaderSize = 0;

            if (len == 0) return 0;
            if (len >= 4)
            {
                // Decode the command header
                KvmCommands btype = (KvmCommands)((buffer[off] << 8) + buffer[off + 1]);
                int blen = (buffer[off + 2] << 8) + buffer[off + 3];
                if (len < blen) return 0;

                // Handle JUMBO command
                if ((btype == KvmCommands.Jumbo) && (blen == 8))
                {
                    jumboHeaderSize = 8;
                    blen = (int)IPAddress.NetworkToHostOrder((int)BitConverter.ToUInt32(buffer, off + 4));
                    off += 8;
                    btype = (KvmCommands)((buffer[off] << 8) + buffer[off + 1]);
                }

                // Process the command
                switch (btype)
                {
                    case KvmCommands.Screen:
                        {
                            if (blen != 8) return blen;
                            uint w = (ushort)((buffer[off + 4] << 8) + buffer[off + 5]);
                            uint h = (ushort)((buffer[off + 6] << 8) + buffer[off + 7]);
                            if (screenWidth == w && screenHeight == h) break;
                            screenWidth = w;
                            screenHeight = h;
                            desktop = new Bitmap((int)screenWidth, (int)screenHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                            desktopGraphics = Graphics.FromImage(desktop);
                            SendCompressionLevel();
                            Invoke(new SetSizeHandler(SetSize));
                            SendPause(false);
                            //SendMouse(2, KvmMouseButtonCommands.MOUSEEVENTF_LEFTUP);
                            //SendMouse(2, KvmMouseButtonCommands.MOUSEEVENTF_MIDDLEUP);
                            //SendMouse(2, KvmMouseButtonCommands.MOUSEEVENTF_RIGHTUP);
                            return blen + jumboHeaderSize;
                        }
                    case KvmCommands.Picture:
                        {
                            Image newtile;
                            ushort tile_x = (ushort)((buffer[off + 4] << 8) + buffer[off + 5]);
                            ushort tile_y = (ushort)((buffer[off + 6] << 8) + buffer[off + 7]);
                            try { newtile = Image.FromStream(new System.IO.MemoryStream(buffer, off + 8, blen - 8)); } catch (Exception) { return blen; }
                            //Rectangle r = new Rectangle((int)tile_x, (int)tile_y, newtile.Width, newtile.Height);
                            //Rectangle r3 = new Rectangle((int)Math.Round((double)tile_x / (double)scalefactor) - 2, (int)Math.Round((double)tile_y / (double)scalefactor) - 2, (int)Math.Round((double)newtile.Width / (double)scalefactor) + 4, (int)Math.Round((double)newtile.Height / (double)scalefactor) + 4);
                            tilecount++;

                            // Winform mode
                            if (desktop != null)
                            {
                                lock (desktop)
                                {
                                    Graphics MemGraphics = Graphics.FromImage(newtile);

                                    IntPtr hBitmap = ((Bitmap)newtile).GetHbitmap();
                                    IntPtr memdc = MemGraphics.GetHdc();
                                    IntPtr pOrig = SelectObject(memdc, hBitmap);
                                    IntPtr dcDst = desktopGraphics.GetHdc();


                                    bool bok = BitBlt(dcDst, (int)tile_x, (int)tile_y, (int)newtile.Width, (int)newtile.Height, memdc, (int)0, (int)0, TernaryRasterOperations.SRCCOPY);

                                    IntPtr pNew = SelectObject(memdc, pOrig); // == hBitmap
                                    DeleteObject(pNew);
                                    desktopGraphics.ReleaseHdc(dcDst);
                                    MemGraphics.ReleaseHdc(memdc);
                                    if(debugmode)
                                    {
                                        desktopGraphics.DrawRectangle(RedPen, new Rectangle((int)tile_x, (int)tile_y, newtile.Width - 1, newtile.Height - 1));
                                        desktopGraphics.DrawString(string.Format("{0} / {1}kb", tilecount, blen / 2014), DebugFont, RedPen.Brush, new Point((int)tile_x, (int)tile_y));
                                    }
                                }

                                // Update extra displays if needed
                                Rectangle r = new Rectangle((int)tile_x, (int)tile_y, newtile.Width, newtile.Height);
                                Rectangle rx = new Rectangle(r.X + displayOrigin.X, r.Y + displayOrigin.Y, r.Width, r.Height);
                                //Console.WriteLine(rx.ToString());
                                if (ScreenAreaUpdated != null) ScreenAreaUpdated(desktop, rx);

                                if (displayCrop == Rectangle.Empty)
                                {
                                    if (scalefactor == 1)
                                    {
                                        Invalidate(r, false);
                                    }
                                    else
                                    {
                                        Rectangle r3 = new Rectangle((int)((double)tile_x / (double)scalefactor) - 2, (int)((double)tile_y / (double)scalefactor) - 2, (int)((double)newtile.Width / (double)scalefactor) + 4, (int)((double)newtile.Height / (double)scalefactor) + 4);
                                        Invalidate(r3, false);
                                    }
                                }
                                else if (displayCrop.IntersectsWith(rx) == true)
                                {
                                    Rectangle r2 = new Rectangle(rx.X, rx.Y, rx.Width, rx.Height);
                                    r2.Intersect(displayCrop);

                                    if (scalefactor == 1)
                                    {
                                        Rectangle r3 = new Rectangle(r2.X - displayCrop.X, r2.Y - displayCrop.Y, r2.Width, r2.Height);
                                        Invalidate(r, false);
                                    }
                                    else
                                    {
                                        Rectangle r3 = new Rectangle((int)((double)(r2.X - displayCrop.X) / (double)scalefactor) - 2, (int)((double)(r2.Y - displayCrop.Y) / (double)scalefactor) - 2, (int)((double)r2.Width / (double)scalefactor) + 4, (int)((double)r2.Height / (double)scalefactor) + 4);
                                        Invalidate(r3, false);
                                    }
                                }
                            }

                            return blen + jumboHeaderSize;
                        }
                    case KvmCommands.Copy:
                        {
                            ushort sourcex = (ushort)((buffer[off + 4] << 8) + buffer[off + 5]);
                            ushort sourcey = (ushort)((buffer[off + 6] << 8) + buffer[off + 7]);
                            ushort targetx = (ushort)((buffer[off + 8] << 8) + buffer[off + 9]);
                            ushort targety = (ushort)((buffer[off + 10] << 8) + buffer[off + 11]);
                            ushort tilew = (ushort)((buffer[off + 12] << 8) + buffer[off + 13]);
                            ushort tileh = (ushort)((buffer[off + 14] << 8) + buffer[off + 15]);
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

                            return blen + jumboHeaderSize;
                        }
                    case KvmCommands.GetDisplays:
                        {
                            int i = 0;
                            ushort length = (ushort)((buffer[off + 4] << 8) + buffer[off + 5]);
                            displays.Clear();
                            if (length > 0) { for (i = 0; i < length; i++) { displays.Add((ushort)((buffer[off + 6 + i * 2] << 8) + buffer[off + 7 + i * 2])); } }
                            currentDisp = (ushort)((buffer[off + 6 + i * 2] << 8) + buffer[off + 7 + i * 2]);
                            if (DisplaysReceived != null) DisplaysReceived(this, null);
                            break;
                        }
                    case KvmCommands.SetDisplay:
                        {
                            currentDisp = (ushort)((buffer[off + 4] << 8) + buffer[off + 5]);
                            break;
                        }
                    case KvmCommands.MouseCursor:
                        {
                            if (blen != 5) return blen;
                            ChangeMouseCursor(buffer[off + 4]);
                            break;
                        }
                    case KvmCommands.DisplayInfo:
                        {
                            if ((blen < 4) || (((blen - 4) % 10) != 0)) break;
                            int screenCount = ((blen - 4) / 10);
                            int ptr = off + 4;
                            Rectangle[] xDisplayInfo = new Rectangle[screenCount];

                            for (var i = 0; i < screenCount; i++)
                            {
                                int id = ((buffer[ptr + 0] << 8) + buffer[ptr + 1]);
                                int x = ((buffer[ptr + 2] << 8) + buffer[ptr + 3]);
                                int y = ((buffer[ptr + 4] << 8) + buffer[ptr + 5]);
                                int w = ((buffer[ptr + 6] << 8) + buffer[ptr + 7]);
                                int h = ((buffer[ptr + 8] << 8) + buffer[ptr + 9]);
                                if (x > 32766) { x -= 65536; }
                                if (y > 32766) { y -= 65536; }
                                Rectangle r = new Rectangle(x, y, w, h);
                                xDisplayInfo[id - 1] = r;
                                ptr += 10;
                            }

                            // Set display information
                            displayInfo = xDisplayInfo;
                            break;
                        }
                    default:
                        {
                            // MessageBox.Show("Should never happen!");
                            //Disconnect();
                            return 0;
                        }
                }
                return blen + jumboHeaderSize;
            }
            return 0;
        }

        private delegate void ChangeMouseCursorHandler(int cursorId);
        private void ChangeMouseCursor(int cursorId)
        {
            if (this.InvokeRequired) { this.Invoke(new ChangeMouseCursorHandler(ChangeMouseCursor), cursorId); return; }
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

        public void Repaint(Rectangle rect)
        {
            Invalidate(rect);
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
                    if (displayCrop == Rectangle.Empty) // No cropping
                    {
                        if (scalefactor == 1)
                        {
                            g.DrawImage(desktop, e.ClipRectangle, e.ClipRectangle, GraphicsUnit.Pixel);
                        }
                        else
                        {
                            g.DrawImage((Image)desktop, e.ClipRectangle, (int)((double)e.ClipRectangle.Left * (double)scalefactor), (int)((double)e.ClipRectangle.Top * (double)scalefactor), (int)((double)e.ClipRectangle.Width * (double)scalefactor), (int)((double)e.ClipRectangle.Height * (double)scalefactor), GraphicsUnit.Pixel);
                        }
                    }
                    else
                    {
                        if (scalefactor == 1) // Cropping in effect, this is when we show different displays in different windows
                        {
                            g.DrawImage(desktop, e.ClipRectangle, new Rectangle(e.ClipRectangle.X - displayOrigin.X + displayCrop.X, e.ClipRectangle.Y - displayOrigin.Y + displayCrop.Y, e.ClipRectangle.Width, e.ClipRectangle.Height), GraphicsUnit.Pixel);
                        }
                        else
                        {
                            Rectangle srcRect = new Rectangle((int)((double)(e.ClipRectangle.Left) * (double)scalefactor) - displayOrigin.X + displayCrop.X, (int)((double)(e.ClipRectangle.Top) * (double)scalefactor) - displayOrigin.Y + displayCrop.Y, (int)((double)e.ClipRectangle.Width * (double)scalefactor), (int)((double)e.ClipRectangle.Height * (double)scalefactor));
                            g.DrawImage((Image)desktop, e.ClipRectangle, srcRect, GraphicsUnit.Pixel);
                        }
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

        public void SendUnicodeKey(ushort key, byte action)
        {
            //if (state != ConnectState.Connected) return;

            BinaryWriter bw = GetBinaryWriter();
            bw.Write(IPAddress.HostToNetworkOrder((short)KvmCommands.KeyUnicode));
            bw.Write(IPAddress.HostToNetworkOrder((short)7));
            bw.Write((byte)action);
            bw.Write((byte)(key >> 8));
            bw.Write((byte)(key & 0xFF));
            Send(bw);
        }

        private void SendPress(KeyPressEventArgs e, byte action)
        {
            //if (state != ConnectState.Connected) return;

            if (remoteKeyboardMap == true) return;

            if (killNextKeyPress > 0)
            {
                long t = DateTime.Now.Ticks;
                if ((t - killNextKeyPress) < 10) { killNextKeyPress = 0; return; }
            }

            ushort c = (ushort)e.KeyChar;
            if (c < 32)
            {
                SendKey((byte)c, 0);
            }
            else
            {
                SendUnicodeKey(c, 0);
            }
        }

        private void SendKey(KeyEventArgs e, byte action)
        {
            //if (state != ConnectState.Connected) return;

            if (remoteKeyboardMap == true) { SendKey((byte)e.KeyCode, action); return; } // Use old key system that uses the remote keyboard mapping.
            string keycode = e.KeyCode.ToString();
            if ((action == 0) && (e.Control == false) && (e.Alt == false) && (((e.KeyValue >= 48) && (e.KeyValue <= 57))|| ((e.KeyValue >= 96) && (e.KeyValue <= 105)) || (keycode.Length == 1) || (keycode.StartsWith("Oem") == true))) return;
            if ((e.Control == true) || (e.Alt == true)) { killNextKeyPress = DateTime.Now.Ticks; }
            SendKey((byte)e.KeyCode, action);
            e.Handled = true;
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
                        if (swamMouseButtons)
                        {
                            buttons |= (byte)KvmMouseButtonCommands.MOUSEEVENTF_RIGHTDOWN;
                        }
                        else
                        {
                            buttons |= (byte)KvmMouseButtonCommands.MOUSEEVENTF_LEFTDOWN;
                        }
                        break;
                    case MouseButtons.Right:
                        if (swamMouseButtons)
                        {
                            buttons |= (byte)KvmMouseButtonCommands.MOUSEEVENTF_LEFTDOWN;
                        }
                        else
                        {
                            buttons |= (byte)KvmMouseButtonCommands.MOUSEEVENTF_RIGHTDOWN;
                        }
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
                        if (swamMouseButtons) { buttons |= (byte)KvmMouseButtonCommands.MOUSEEVENTF_RIGHTUP; } else { buttons |= (byte)KvmMouseButtonCommands.MOUSEEVENTF_LEFTUP; }
                        break;
                    case MouseButtons.Right:
                        if (swamMouseButtons) { buttons |= (byte)KvmMouseButtonCommands.MOUSEEVENTF_LEFTUP; } else { buttons |= (byte)KvmMouseButtonCommands.MOUSEEVENTF_RIGHTUP; }
                        break;
                    case MouseButtons.Middle:
                        buttons |= (byte)KvmMouseButtonCommands.MOUSEEVENTF_MIDDLEUP;
                        break;
                }
            }

            if (displayCrop == Rectangle.Empty)
            {
                LastX = (short)((double)e.X * (double)scalefactor);
                LastY = (short)((double)e.Y * (double)scalefactor);
            }
            else
            {
                LastX = (short)((int)((double)e.X * (double)scalefactor) - displayOrigin.X + displayCrop.X);
                LastY = (short)((int)((double)e.Y * (double)scalefactor) - displayOrigin.Y + displayCrop.Y);
            }

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
                if (parent != null)
                {
                    parent.bytesOut += buffer.Length;
                    parent.bytesOutCompressed += parent.wc.SendBinary(buffer, 0, buffer.Length);
                    bytesent += buffer.Length;
                }
                else if (parentEx != null)
                {
                    parentEx.mainKvmControl.Send(buffer);
                }
            }
            catch (Exception) { }
        }

        public void Send(string str)
        {
            //if (state == ConnectState.Disconnected) return;
            try
            {
                if (parent != null)
                {
                    parent.bytesOut += str.Length;
                    parent.bytesOutCompressed += parent.wc.SendString(str);
                    bytesent += str.Length;
                }
                else if (parentEx != null)
                {
                    parentEx.mainKvmControl.Send(str);
                }
            }
            catch (Exception) { }
        }

        public void Send(BinaryWriter bw)
        {
            //if (state == ConnectState.Disconnected) { RecycleBinaryWriter(bw); return; }
            if ((parent != null) && (parent.wc != null))
            {
                try
                {
                    parent.bytesOut += (int)((MemoryStream)bw.BaseStream).Length;
                    parent.bytesOutCompressed += parent.wc.SendBinary(((MemoryStream)bw.BaseStream).GetBuffer(), 0, (int)((MemoryStream)bw.BaseStream).Length);
                    bytesent += (int)((MemoryStream)bw.BaseStream).Length;
                }
                catch (Exception) { }
            }
            else if (parentEx != null)
            {
                parentEx.mainKvmControl.Send(bw);
            }
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
            if ((lastMouseMove + 30) < ct)
            {
                lastMouseMove = ct;
                SendMouse(e, 0);
            }
        }

        public void KVMControl_MouseWheel(object sender, MouseEventArgs e)
        {
            SendMouse(e, 0);
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

        private void KVMControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (!isHookPriority)
            {
                if ((e.KeyCode == Keys.LWin) || (e.KeyCode == Keys.RWin)) return; // Don't process the Windows key
                SendKey(e, 0);
                e.Handled = true;
            }
        }

        private void KVMControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!isHookPriority)
            {
                SendPress(e, 0);
                e.Handled = true;
            }
        }

        private void KVMControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (!isHookPriority)
            {
                SendKey(e, 1);
                e.Handled = true;
            }
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

    // Structure contain information about low-level keyboard input event 
    private struct KBDLLHOOKSTRUCT
    {
      public Keys key;
      public int scanCode;
      public int flags;
      public int time;
      public IntPtr extra;
    }

    public void cropDisplay(Point o, Rectangle r)
    {
        if (IsDisposed || Disposing) return;
        displayCrop = r;
        displayOrigin = o;
        if (controlLoaded)
        {
            Invoke(new SetSizeHandler(SetSize));
            Invalidate();
        }
    }

    private void KVMControl_Load(object sender, EventArgs e)
    {
        controlLoaded = true;
        ProcessModule objCurrentModule = Process.GetCurrentProcess().MainModule;
        objKeyboardProcess = new LowLevelKeyboardProc(captureKey);
        ptrHook = SetWindowsHookEx(13, objKeyboardProcess, GetModuleHandle(objCurrentModule.ModuleName), 0);
    }

    //Declaring Global objects     
    private IntPtr ptrHook;
    private LowLevelKeyboardProc objKeyboardProcess;

    private IntPtr captureKey(int nCode, IntPtr wp, IntPtr lp)
    {
      bool bIsForegroundWindow = false;
      try { bIsForegroundWindow = GetForegroundWindow() == this.ParentForm.Handle; } catch { }
      if(nCode >= 0 && bIsForegroundWindow)
      {
        KBDLLHOOKSTRUCT objKeyInfo = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lp, typeof(KBDLLHOOKSTRUCT));

        // Disabling Windows keys 

        if(objKeyInfo.key == Keys.RWin || objKeyInfo.key == Keys.LWin)
        {
          bool bKeyDown = wp == (IntPtr)0x0100/*WM_KEYDOWN*/;
          SendKey(new KeyEventArgs((Keys)objKeyInfo.key), (byte)(bKeyDown ? 0 : 1));
          //        SendKey(new KeyEventArgs((Keys)objKeyInfo.key), 0);
          return (IntPtr)1; // if 0 is returned then All the above keys will be enabled
                            //      return (IntPtr)0; // if 0 is returned then All the above keys will be enabled


        }
      }
      return CallNextHookEx(ptrHook, nCode, wp, lp);
    }

    bool HasAltModifier(int flags)
    {
      return (flags & 0x20) == 0x20;
    }

  }
}
