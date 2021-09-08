using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x00000104;
        private const int WM_KEYDOWN2 = 0x00000100;

        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
        [STAThread]
        static void Main()
        {
            
          
            _hookID = SetHook(_proc);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
          Application.Run(new Form1());
            UnhookWindowsHookEx(_hookID);
        }
        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }



        private delegate IntPtr LowLevelKeyboardProc(
            int nCode, IntPtr wParam, IntPtr lParam);



        private static IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_KEYDOWN2))
            {
                int vkCode = Marshal.ReadInt32(lParam);
                //Console.WriteLine((Keys)vkCode);
                Debug.WriteLine((Keys)vkCode);



                if (((Keys)vkCode) == Keys.RWin ||
                 ((Keys)vkCode) == Keys.LWin ||
                 ((Keys)vkCode) == Keys.Alt ||
                 ((Keys)vkCode) == Keys.Tab ||
                 ((Keys)vkCode) == Keys.Control ||
                 ((Keys)vkCode) == Keys.F4 ||
                 ((Keys)vkCode) == Keys.Delete ||
                 ((Keys)vkCode) == Keys.LShiftKey ||
                 ((Keys)vkCode) == Keys.RShiftKey ||
                 ((Keys)vkCode) == Keys.Escape)
                    return new IntPtr(-1);
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }



        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);



        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);



        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);



        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        ////////////////////////////////////////////////////////////////////////////////////
      
    }
}
