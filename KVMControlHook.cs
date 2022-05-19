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
        }

        internal void AttachKeyboardHook(KVMCallback callback)
        {
            try
            {
                _hook = KVMKeyboardHook.SetHook(_proc);
                _callback = callback;
            }
            catch
            {
                DetachKeyboardHook();
                throw new System.InvalidOperationException("Could not set hook.");
            }
        }

        internal void DetachKeyboardHook()
        {
            _callback = null;
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

