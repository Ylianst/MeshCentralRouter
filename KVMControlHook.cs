
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MeshCentralRouter
{
    public class KVMControlHook
    {
        public delegate void KVMCallback(byte keyCodes, byte lParam);
        private KVMCallback _callback;
        private KVMKeyboardHook.LowLevelKeyboardProc _proc;
        private static IntPtr _hook = IntPtr.Zero;
        protected static KVMControl _control;
        internal KVMControlHook()
        {
            _proc = HookCallback;
            AttachKeyboardHook();
        }

        private void AttachKeyboardHook()
        {
            try
            {
                _hook = KVMKeyboardHook.SetHook(_proc);
            }
            catch
            {
                DetachKeyboardHook();
                throw new System.InvalidOperationException("Could not set hook.");
            }
        }

        internal void AttachCallback(KVMCallback callback)
        {
            _callback = callback;
        }

        internal void DetachCallback()
        {
            _callback = null;
        }

        internal static void DetachKeyboardHook()
        {
            if (_hook != IntPtr.Zero)
                KVMKeyboardHook.UnhookWindowsHookEx(_hook);
        }

        public IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                Keys key = (Keys)Marshal.ReadInt32(lParam);
                if ((key == Keys.LWin) || (key == Keys.RWin)) {
                    bool alt = (Control.ModifierKeys & Keys.Alt) != 0;
                    bool control = (Control.ModifierKeys & Keys.Control) != 0;

                    const int WM_KEYDOWN = 0x100;
                    const int WM_KEYUP = 0x101;
                    const int WM_SYSKEYDOWN = 0x104;
                    const int WM_SYSKEYUP = 0x105;

                    byte bkey = (byte)key;
                    byte keyStatus = 255;
                    switch ((int)wParam)
                    {
                        case WM_KEYDOWN:
                            keyStatus = 0;
                            break;
                        case WM_KEYUP:
                            keyStatus = 1;
                            break;
                        case WM_SYSKEYDOWN:
                            //keyStatus = 0; // 4
                            break;
                        case WM_SYSKEYUP:
                            //keyStatus = 1; // 5
                            break;
                        default:
                            return KVMKeyboardHook.CallNextHookEx(_hook, nCode, wParam, lParam);
                    }

                    try
                    {
                        if ((_callback != null) && (keyStatus != 255))
                        {
                            _callback(bkey, keyStatus);
                            return (IntPtr)1;
                        }
                    }
                    catch { }
                }
            }
            return KVMKeyboardHook.CallNextHookEx(_hook, nCode, wParam, lParam);
        }
    }
}

