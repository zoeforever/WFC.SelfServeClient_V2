using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace WFC.SelfServeClient.Helper
{
    /// <summary>
    /// 控制台管理
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    public static class ConsoleLogHelper
    {
        private const string Kernel32_DllName = "kernel32.dll";

        [DllImport(Kernel32_DllName)]
        private static extern bool AllocConsole();

        [DllImport(Kernel32_DllName)]
        public static extern bool FreeConsole();

        [DllImport(Kernel32_DllName)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport(Kernel32_DllName)]
        private static extern int GetConsoleOutputCP();

        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        public static bool HasConsole
        {
            get { return GetConsoleWindow() != IntPtr.Zero; }
        }

        /// <summary>  
        /// Creates a new console instance if the process is not attached to a console already.  
        /// </summary>  
        public static void Show()
        {
            //#if DEBUG  
            if (!HasConsole)
            {
                AllocConsole();
                InvalidateOutAndError();
            }
            //#endif  
        }

        /// <summary>  
        /// If the process has a console attached to it, it will be detached and no longer visible. Writing to the System.Console is still possible, but no output will be shown.  
        /// </summary>  
        public static void Hide()
        {
            //#if DEBUG  
            if (HasConsole)
            {
                SetOutAndErrorNull();
                FreeConsole();
            }
            //#endif  
        }

        public static void Toggle()
        {
            if (HasConsole)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        static void InvalidateOutAndError()
        {
            Type type = typeof(System.Console);

            System.Reflection.FieldInfo _out = type.GetField("_out",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            System.Reflection.FieldInfo _error = type.GetField("_error",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            System.Reflection.MethodInfo _InitializeStdOutError = type.GetMethod("InitializeStdOutError",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            Debug.Assert(_out != null);
            Debug.Assert(_error != null);

            Debug.Assert(_InitializeStdOutError != null);

            _out.SetValue(null, null);
            _error.SetValue(null, null);

            _InitializeStdOutError.Invoke(null, new object[] { true });
        }
        public static void WriteLine(string msg)
        {
            try
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                Console.WriteLine(msg);
                Console.ForegroundColor = ConsoleColor.Green;
            }
            catch
            { }
        }
        public static void WriteLineError(string msg, [CallerMemberName] string name = "")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            WriteLine($"[{name}]:{ msg}");
        }
        public static void WriteLineDebug(string msg, [CallerMemberName] string name = "")
        {
            Console.ForegroundColor = ConsoleColor.Green;
            WriteLine($"[{name}]:{ msg}");
        }
        public static void WriteLineError(Exception ex, [CallerMemberName] string name = "")
        {
            WriteLineError($"[{name}]:{ex.ToString()}");
        }

        public static void WriteLineWarn(string msg, [CallerMemberName] string name = "")
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            WriteLine($"[{name}]:{ msg}");
        }
        static void SetOutAndErrorNull()
        {
            Console.SetOut(TextWriter.Null);
            Console.SetError(TextWriter.Null);
        }
    }
}
