using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace macro_keep_from_afk
{
    
    public partial class Form1 : Form
    {
        Thread macro;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [Flags]
        private enum InputType
        {
            Mouse = 0,
            Keyboard = 1,
            Hardware = 2
        }

        [Flags]
        private enum KeyEventF
        {
            KeyDown = 0x0000,
            ExtendedKey = 0x0001,
            KeyUp = 0x0002,
            Unicode = 0x0004,
            Scancode = 0x0008,
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, Input[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        private static extern IntPtr GetMessageExtraInfo();

        public static void SendKey(ushort key, string keyType)
        {
            KeyEventF inputKey = KeyEventF.KeyDown;
            if (keyType == "keyup")
            {
                inputKey = KeyEventF.KeyUp;
            }

            Input[] inputs =
            {
                new Input
                {
                    type = (int) InputType.Keyboard,
                    u = new InputUnion
                    {
                        ki = new KeyboardInput
                        {
                            wVk = 0,
                            wScan = key,
                            dwFlags = (uint) (inputKey | KeyEventF.Scancode),
                            dwExtraInfo = GetMessageExtraInfo()
                        }
                    }
                },
            };

            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Input)));
        }

        private struct Input
        {
            public int type;
            public InputUnion u;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct InputUnion
        {
            [FieldOffset(0)] public readonly MouseInput mi;
            [FieldOffset(0)] public KeyboardInput ki;
            [FieldOffset(0)] public readonly HardwareInput hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MouseInput
        {
            public readonly int dx;
            public readonly int dy;
            public readonly uint mouseData;
            public readonly uint dwFlags;
            public readonly uint time;
            public readonly IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KeyboardInput
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public readonly uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct HardwareInput
        {
            public readonly uint uMsg;
            public readonly ushort wParamL;
            public readonly ushort wParamH;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            macro = new Thread(Macro);
            macro.Start();
            textBox1.Text = "Working...";

        }

        private void Macro()
        {
            while (true)
            {
                Process[] processes = Process.GetProcessesByName(textBox2.Text.Trim());
                foreach (var process in processes)
                {
                    if (GetForegroundWindow() == process.MainWindowHandle)
                    {
                        SendKey(0x1E, "keydown");
                        Thread.Sleep(1000);
                        SendKey(0X1E, "keyup");
                        Thread.Sleep(1000);


                        SendKey(0x1F, "keydown");
                        Thread.Sleep(1000);
                        SendKey(0X1F, "keyup");
                        Thread.Sleep(1000);


                        SendKey(0x20, "keydown");
                        Thread.Sleep(1000);
                        SendKey(0X20, "keyup");
                        Thread.Sleep(1000);


                        SendKey(0x11, "keydown");
                        Thread.Sleep(1000);
                        SendKey(0X11, "keyup");
                        Thread.Sleep(1000);

                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "Not Working";
            if (macro != null) macro.Abort(); 
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (macro != null) macro.Abort();
        }
    }
}
