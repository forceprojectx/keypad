using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Threading;


namespace StormMacro
{
    class KeyHooks
    {
        #region widows structure definitions

        [StructLayout(LayoutKind.Sequential)]
        private class KeyboardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        #endregion

        #region Windows function imports
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int SetWindowsHookEx(
            int idHook,
            HookProc lpfn,
            IntPtr hMod,
            int dwTheadId
            );
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int CallNextHookEx(
            int idHook,
            int nCode,
            int wParam,
            IntPtr lParam
            );

        private delegate int HookProc(
            int nCode,
            int wParam,
            IntPtr lParam
            );

        [DllImport("user32.dll")]
        private static extern int ToAscii(
            int uVirtKey,
            int uScanCode,
            byte[] lpbKeyState,
            byte[] lpwTransKey,
            int fuState
            );

        [DllImport("user32.dll")]
        private static extern int GetKeboardState(byte[] pbKeyState);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern short GetKeyState(int vKey);

        #endregion

        #region windows constants

        private const int WH_KEYBOARD_LL = 13;
        private const int WH_KEYBOARD = 2;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;

        private const int WM_SYSKEYDOWN = 0x104;
        private const int WM_SYSKEYUP = 0x105;

        private const byte VK_SHIFT = 0x10;
        private const byte VK_CAPITAL = 0x14;
        private const byte VK_NUMLOCK = 0x90;
        private const byte VK_MENU = 0x12;
        private const byte VK_CONTROL = 0x11;
        #endregion

        #region key enums
        public enum KeysWindows
        {
            A = 0x41,
            Add = 0x6b,
            Alt = 0x40000,
            Apps = 0x5d,
            Attn = 0xf6,
            B = 0x42,
            Back = 8,
            BrowserBack = 0xa6,
            BrowserFavorites = 0xab,
            BrowserForward = 0xa7,
            BrowserHome = 0xac,
            BrowserRefresh = 0xa8,
            BrowserSearch = 170,
            BrowserStop = 0xa9,
            C = 0x43,
            Cancel = 3,
            Capital = 20,
            CapsLock = 20,
            Clear = 12,
            Control = 0x20000,
            ControlKey = 0x11,
            Crsel = 0xf7,
            D = 0x44,
            D0 = 0x30,
            D1 = 0x31,
            D2 = 50,
            D3 = 0x33,
            D4 = 0x34,
            D5 = 0x35,
            D6 = 0x36,
            D7 = 0x37,
            D8 = 0x38,
            D9 = 0x39,
            Decimal = 110,
            Delete = 0x2e,
            Divide = 0x6f,
            Down = 40,
            E = 0x45,
            End = 0x23,
            Enter = 13,
            EraseEof = 0xf9,
            Escape = 0x1b,
            Execute = 0x2b,
            Exsel = 0xf8,
            F = 70,
            F1 = 0x70,
            F10 = 0x79,
            F11 = 0x7a,
            F12 = 0x7b,
            F13 = 0x7c,
            F14 = 0x7d,
            F15 = 0x7e,
            F16 = 0x7f,
            F17 = 0x80,
            F18 = 0x81,
            F19 = 130,
            F2 = 0x71,
            F20 = 0x83,
            F21 = 0x84,
            F22 = 0x85,
            F23 = 0x86,
            F24 = 0x87,
            F3 = 0x72,
            F4 = 0x73,
            F5 = 0x74,
            F6 = 0x75,
            F7 = 0x76,
            F8 = 0x77,
            F9 = 120,
            FinalMode = 0x18,
            G = 0x47,
            H = 0x48,
            HanguelMode = 0x15,
            HangulMode = 0x15,
            HanjaMode = 0x19,
            Help = 0x2f,
            Home = 0x24,
            I = 0x49,
            IMEAccept = 30,
            IMEAceept = 30,
            IMEConvert = 0x1c,
            IMEModeChange = 0x1f,
            IMENonconvert = 0x1d,
            Insert = 0x2d,
            J = 0x4a,
            JunjaMode = 0x17,
            K = 0x4b,
            KanaMode = 0x15,
            KanjiMode = 0x19,
            KeyCode = 0xffff,
            L = 0x4c,
            LaunchApplication1 = 0xb6,
            LaunchApplication2 = 0xb7,
            LaunchMail = 180,
            LButton = 1,
            LControlKey = 0xa2,
            Left = 0x25,
            LineFeed = 10,
            LMenu = 0xa4,
            LShiftKey = 160,
            LWin = 0x5b,
            M = 0x4d,
            MButton = 4,
            MediaNextTrack = 0xb0,
            MediaPlayPause = 0xb3,
            MediaPreviousTrack = 0xb1,
            MediaStop = 0xb2,
            Menu = 0x12,
            Modifiers = -65536,
            Multiply = 0x6a,
            N = 0x4e,
            Next = 0x22,
            NoName = 0xfc,
            None = 0,
            NumLock = 0x90,
            NumPad0 = 0x60,
            NumPad1 = 0x61,
            NumPad2 = 0x62,
            NumPad3 = 0x63,
            NumPad4 = 100,
            NumPad5 = 0x65,
            NumPad6 = 0x66,
            NumPad7 = 0x67,
            NumPad8 = 0x68,
            NumPad9 = 0x69,
            O = 0x4f,
            Oem1 = 0xba,
            Oem102 = 0xe2,
            Oem2 = 0xbf,
            Oem3 = 0xc0,
            Oem4 = 0xdb,
            Oem5 = 220,
            Oem6 = 0xdd,
            Oem7 = 0xde,
            Oem8 = 0xdf,
            OemBackslash = 0xe2,
            OemClear = 0xfe,
            OemCloseBrackets = 0xdd,
            Oemcomma = 0xbc,
            OemMinus = 0xbd,
            OemOpenBrackets = 0xdb,
            OemPeriod = 190,
            OemPipe = 220,
            Oemplus = 0xbb,
            OemQuestion = 0xbf,
            OemQuotes = 0xde,
            OemSemicolon = 0xba,
            Oemtilde = 0xc0,
            P = 80,
            Pa1 = 0xfd,
            Packet = 0xe7,
            PageDown = 0x22,
            PageUp = 0x21,
            Pause = 0x13,
            Play = 250,
            Print = 0x2a,
            PrintScreen = 0x2c,
            Prior = 0x21,
            ProcessKey = 0xe5,
            Q = 0x51,
            R = 0x52,
            RButton = 2,
            RControlKey = 0xa3,
            Return = 13,
            Right = 0x27,
            RMenu = 0xa5,
            RShiftKey = 0xa1,
            RWin = 0x5c,
            S = 0x53,
            Scroll = 0x91,
            Select = 0x29,
            SelectMedia = 0xb5,
            Separator = 0x6c,
            Shift = 0x10000,
            ShiftKey = 0x10,
            Sleep = 0x5f,
            Snapshot = 0x2c,
            Space = 0x20,
            Subtract = 0x6d,
            T = 0x54,
            Tab = 9,
            U = 0x55,
            Up = 0x26,
            V = 0x56,
            VolumeDown = 0xae,
            VolumeMute = 0xad,
            VolumeUp = 0xaf,
            W = 0x57,
            X = 0x58,
            XButton1 = 5,
            XButton2 = 6,
            Y = 0x59,
            Z = 90,
            Zoom = 0xfb
        }

        public enum KeysUSB
        {
            A = 4,/*usb*/
            B = 5,/*usb*/
            C = 6,/*usb*/
            D = 7,/*usb*/
            E = 8,/*usb*/
            F = 9,/*usb*/
            G = 10,/*usb*/
            H = 11,/*usb*/
            I = 12,/*usb*/
            J = 13,/*usb*/
            K = 14,/*usb*/
            L = 15,/*usb*/
            M = 16,/*usb*/
            N = 17,/*usb*/
            O = 18,/*usb*/
            P = 19,/*usb*/
            Q = 20,/*usb*/
            R = 21,/*usb*/
            S = 22,/*usb*/
            T = 23,/*usb*/
            U = 24,/*usb*/
            V = 25,/*usb*/
            W = 26,/*usb*/
            X = 27,/*usb*/
            Y = 28,/*usb*/
            Z = 29,/*usb*/
            D0 = 39,/*usb*/
            D1 = 30,/*usb*/
            D2 = 31,/*usb*/
            D3 = 32,/*usb*/
            D4 = 33,/*usb*/
            D5 = 34,/*usb*/
            D6 = 35,/*usb*/
            D7 = 36,/*usb*/
            D8 = 37,/*usb*/
            D9 = 38,/*usb*/
            F1 = 58,/*usb*/
            F2 = 59,/*usb*/
            F3 = 60,/*usb*/
            F4 = 61,/*usb*/
            F5 = 62,/*usb*/
            F6 = 63,/*usb*/
            F7 = 64,/*usb*/
            F8 = 65,/*usb*/
            F9 = 66,/*usb*/
            Up = 82,/*usb*/
            Add = 87,/*usb*/
            Alt = 0xe2,/*usb left alt/menu*/
            End = 0x4d,/*usb*/
            F10 = 67,/*usb*/
            F11 = 68,/*usb*/
            F12 = 69,/*usb*/
            F13 = 0x68,/*usb*/
            F14 = 0x69,/*usb*/
            F15 = 0x6a,/*usb*/
            F16 = 0x6b,/*usb*/
            F17 = 0x6c,/*usb*/
            F18 = 0x6d,/*usb*/
            F19 = 0x6e,/*usb*/
            F20 = 0x6f,/*usb*/
            F21 = 0x70,/*usb*/
            F22 = 0x71,/*usb*/
            F23 = 0x72,/*usb*/
            F24 = 0x73,/*usb*/
            Pa1 = 0x87,
            Tab = 0x2b,/*usb*/
            Apps = 0x65,/*usb*/
            Attn = 0x9a,/*usb*/
            Back = 0x2a,/*usb*/
            Down = 81,/*usb*/
            Help = 0x75,/*usb*/
            Home = 0x4a,/*usb*/
            Left = 80,/*usb*/
            LWin = (0x08 | 0x8000),/*usb 0xe3  */
            Menu = (0x04 | 0x8000),/*usb 0xe2  */
            Next = 0x2f,
            None = 0x24,
            Oem1 = 0x49,
            Oem2 = 30,
            Oem3 = 0x35,/*usb tilde key?*/
            Oem4 = 0x1c,
            Oem5 = 0x1f,
            Oem6 = 0x1d,
            Oem7 = 0x2d,
            Oem8 = 0x4a,
            Play = 0x17,
            RWin = (0x80 | 0x8000),/*usb 0xe7  */
            Zoom = 0x15,
            Clear = 0x9c,/*usb*/
            Crsel = 0xa3,/*usb*/
            Enter = 0x58,/*usb*/
            Exsel = 0xa4,/*usb*/
            LMenu = (0x04 | 0x8000),/*usb 0xe2  */
            Pause = 0x48,/*usb*/
            Print = 0x87,
            Prior = 0xa2,
            Right = 79,/*usb*/
            RMenu = (0x40 | 0x8000),/*usb 0xe6  */
            Shift = 0xa4,
            Sleep = 160,
            Space = 0x2c,/*usb*/
            Cancel = 0x4d,
            Delete = 0x4c,/*usb*/
            Divide = 0x54,/*usb*/
            Escape = 0x29,/*usb*/
            Insert = 0x49,/*usb*/
            NoName = 0xb2,
            Oem102 = 0x87,
            Packet = 0x87,
            PageUp = 0x4b,/*usb*/
            Return = 0x28,/*usb*/
            Scroll = 0x47,/*usb*/
            Select = 0xfc,
            Capital = 0x39,/*usb*/
            Control = 0x90,
            Decimal = 0x63,/*usb*/
            Execute = 0x61,
            KeyCode = 0x62,
            LButton = 0x63,
            MButton = 100,
            NumLock = 0x53,/*usb*/
            NumPad0 = 98,/*usb*/
            NumPad1 = 89,/*usb*/
            NumPad2 = 90,/*usb*/
            NumPad3 = 91,/*usb*/
            NumPad4 = 92,/*usb*/
            NumPad5 = 93,/*usb*/
            NumPad6 = 94,/*usb*/
            NumPad7 = 95,/*usb*/
            NumPad8 = 96,/*usb*/
            NumPad9 = 97,/*usb*/
            OemPipe = 0x31,/*usb*/
            Oemplus = 0x2e,/*usb*/
            RButton = 0xde,
            CapsLock = 0x39,/*usb*/
            EraseEof = 0xe2,
            KanaMode = 0x88,/*usb*/
            LineFeed = 0xdd,
            Multiply = 0x55,/*usb*/
            OemClear = 0x9c,/*usb*/
            Oemcomma = 0x36,/*usb*/
            OemMinus = 0x2d,/*usb*/
            Oemtilde = 0x35,/*usb*/
            PageDown = 0x4e,/*usb*/
            ShiftKey = (0x02 | 0x8000),/*usb 0xe1  */
            Snapshot = 0x46,/*usb*/
            Subtract = 0x56,/*usb*/
            VolumeUp = 0x80,/*usb*/
            XButton1 = 80,
            XButton2 = 0xfd,
            FinalMode = 0xe7,
            HanjaMode = 0x22,
            IMEAccept = 0x21,
            IMEAceept = 0x13,
            JunjaMode = 250,
            KanjiMode = 0x8a,/*usb*/
            LShiftKey = (0x02 | 0x8000),/*usb 0xe1  */
            MediaStop = 0x21,
            Modifiers = 0xe5,
            OemPeriod = 0x37,/*usb*/
            OemQuotes = 0x34,/*usb*/
            RShiftKey = (0x20 | 0x8000),/*usb 0xe5  */
            Separator = 0xa3,
            ControlKey = 13,
            HangulMode = 0x27,
            IMEConvert = 0xa5,
            LaunchMail = 0xa1,
            ProcessKey = 0x5c,
            VolumeDown = 0x81,/*usb*/
            VolumeMute = 0x7f,/*usb*/
            BrowserBack = 0x29,
            BrowserHome = 0xb5,
            BrowserStop = 0x6c,
            HanguelMode = 0x10000,
            LControlKey = (0x01 | 0x8000),/*usb 0xe0  */
            OemQuestion = 0x38,/*usb*/
            PrintScreen = 0x46,/*usb*/
            RControlKey = (0x10 | 0x8000),/*usb 0xe4  */
            SelectMedia = 0x6d,
            OemBackslash = 0x38,/*usb*/
            OemSemicolon = 0x33,/*usb*/
            BrowserSearch = 0x55,
            IMEModeChange = 0x26,
            IMENonconvert = 0x56,
            BrowserForward = 0xae,
            BrowserRefresh = 0xad,
            MediaNextTrack = 0xaf,
            MediaPlayPause = 0x57,
            OemOpenBrackets = 0x2f,/*usb*/
            BrowserFavorites = 5,
            OemCloseBrackets = 0x30,/*usb*/
            LaunchApplication1 = 0x59,
            LaunchApplication2 = 90,
            MediaPreviousTrack = 0xfb
        }

        public enum KeysScancode
        {
            Escape = 0x01,
            F1 = 0x3B,
            F2 = 0x3C,
            F3 = 0x3D,
            F4 = 0x3E,
            F5 = 0x3F,
            F6 = 0x40,
            F7 = 0x41,
            F8 = 0x42,
            F9 = 0x43,
            F10 = 0x44,
            F11 = 0x57,
            F12 = 0x58,
            Snapshot = 0x37,
            Scroll = 0x46,
            Pause = 0x45,
            VolumeMute = 0x00,
            VolumeUp = 0x00,
            VolumeDown = 0x00,
            MediaStop = 0x00,
            MediaPreviousTrack = 0x00,
            MediaPlayPause = 0x00,
            MediaNextTrack = 0x00,
            Oem3 = 0x29,
            D1 = 0x02,
            D2 = 0x03,
            D3 = 0x04,
            D4 = 0x05,
            D5 = 0x06,
            D6 = 0x07,
            D7 = 0x08,
            D8 = 0x09,
            D9 = 0x0A,
            D0 = 0x0B,
            OemQuotes = 0x28,
            OemCloseBrackets = 0x1B,
            Back = 0x0E,
            Insert = 0x52,
            Home = 0x47,
            PageUp = 0x49,
            NumLock = 0x45,

            Divide = 0x35,
            Multiply = 0x37,
            Subtract = 0x4A,
            Tab = 0x0F,
            Q = 0x10,
            W = 0x11,
            E = 0x12,
            R = 0x13,
            T = 0x14,
            Y = 0x15,
            U = 0x16,
            I = 0x17,
            O = 0x18,
            P = 0x19,
            OemMinus = 0x0C,
            Oemplus = 0x0D,
            OemPipe = 0x2B,
            Delete = 0x53,
            End = 0x4F,
            PageDown = 0x51,
            NumPad7 = 0x47,
            NumPad8 = 0x48,
            NumPad9 = 0x49,
            Add = 0x4E,
            Capital = 0x3A,
            A = 0x1E,
            S = 0x1F,
            D = 0x20,
            F = 0x21,
            G = 0x22,
            H = 0x23,
            J = 0x24,
            K = 0x25,
            L = 0x26,
            OemSemicolon = 0x27,
            OemOpenBrackets = 0x1A,
            Enter = 0x1C,
            NumPad4 = 0x4B,
            NumPad5 = 0x4C,
            NumPad6 = 0x4D,
            LShiftKey = 0x2A,
            Z = 0x2C,
            X = 0x2D,
            C = 0x2E,
            V = 0x2F,
            B = 0x30,
            N = 0x31,
            M = 0x32,
            Oemcomma = 0x33,
            OemPeriod = 0x34,
            OemQuestion = 0x35,
            RShiftKey = 0x36,
            Up = 0x48,
            NumPad1 = 0x4F,
            NumPad2 = 0x50,
            NumPad3 = 0x51,

            LControlKey = 0x1D,
            LWin = 0x5B,
            LMenu = 0x38,
            Space = 0x39,
            RMenu = 0x38,
            RWin = 0x5C,
            Apps = 0x5D,
            RControlKey = 0x1D,
            Left = 0x4B,
            Down = 0x50,
            Right = 0x4D,
            NumPad0 = 0x52,
            Decimal = 0x53,
        }



        #endregion
        public KeyHooks()
        {
            //Start();
        }

        //destructor
        ~KeyHooks()
        {
            Stop(false);
        }

        //events for external listeners
        public delegate void KeyPressEventHandler(object sender, KeyPressEventArgs e);
        public event KeyPressEventHandler KeyPress;

        private int hKeyboardHook = 0;
        private static HookProc KeyboardHookProcedure;

        public void Stop()
        {
            Stop(true);
        }

        public void Stop(bool throwErrors)
        {
            //only stop if its started
            if (hKeyboardHook != 0)
            {
                int retKeyboard = UnhookWindowsHookEx(hKeyboardHook);
                hKeyboardHook = 0;
                if (retKeyboard == 0 && throwErrors)
                {
                    int errorcode = Marshal.GetLastWin32Error();
                    throw new Win32Exception(errorcode);
                }
            }
        }

        public void Start()
        {
            //create an instance of keyhook only if it is not already installed
            if (hKeyboardHook == 0)
            {
                KeyboardHookProcedure = new HookProc(KeyboardHookProc);

                //a trap for young players
                System.Diagnostics.Debug.WriteLine(
                    System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName
                    );
                //this works in .net 2 but not in 4!
                System.Diagnostics.Debug.WriteLine("assembly hProc:: " +
                    Marshal.GetHINSTANCE(
                    System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0]
                    )
                    );

                using (System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess())
                using (System.Diagnostics.ProcessModule module = process.MainModule)
                {
                    IntPtr hModule = GetModuleHandle(module.ModuleName);
                    System.Diagnostics.Debug.WriteLine("hModule from interop::" + hModule);
                    hKeyboardHook = SetWindowsHookEx(
                        WH_KEYBOARD_LL,
                        KeyboardHookProcedure,
                        hModule,
                        0
                        );
                }
                if (hKeyboardHook == 0)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    System.Windows.MessageBox.Show(
                        errorCode.ToString(),
                        "error hooking keyboard"
                        );
                    Stop(false);
                    throw new Win32Exception(errorCode);
                }
            }
        }//end start

        private int KeyboardHookProc(
            int nCode,
            Int32 wParam,
            IntPtr lParam
            )
        {
            //indicates if any of the undrlying events set e.handled flag
            bool handled = false;
            //make sure hooks are installed and someone is listening to
            //the events
            if ((nCode >= 0) && (KeyPress != null))
            {
                KeyboardHookStruct myKeyBoardHookStruct =
                    (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));

                //bool isDownShift = (GetKeyState(VK_SHIFT) & 0x80) == 0x80 ? true : false;
                //bool isDownCapslock = (GetKeyState(VK_CAPITAL) != 0) ? true : false;
                //bool isDownCtrl = (GetKeyState(VK_CONTROL) & 0x80) == 0x80 ? true : false;
                //bool isDownAlt = (GetKeyState(VK_MENU) & 0x80) == 0x80 ? true : false;
                

                KeyPress(this, 
                    new KeyPressEventArgs(myKeyBoardHookStruct.vkCode,
                        (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN),
                        myKeyBoardHookStruct.scanCode
                        )                        
                 );
                
                handled = true;


            }
            //if event handled in application, consume event
            if (handled)
                return 1;
            else
                return CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);
        }// end keyboardhook proc


        #region events and errata

        public class KeyPressEventArgs : EventArgs
        {
            public KeyPressEventArgs(int key, bool iskeydown, int scancode)
            {
                _key = key;
                _down = iskeydown;
                Scancode = scancode;
            }
            private int _key;
            private bool _down;
            public int Scancode { get; set; }
            public bool IsKeyDown
            {
                get { return _down; }
            }
            public int Key
            {
                set
                {
                    _key = value;
                }
                get
                {
                    return _key;
                }
            }
        }

        #endregion
    }//namespace
}
