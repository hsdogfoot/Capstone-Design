using System.Runtime.InteropServices;
using System.Windows;

namespace Game_Tool_VS2019
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Left = SystemParameters.PrimaryScreenWidth / 2 - Width / 2;
            Top = SystemParameters.PrimaryScreenHeight / 2 - Height / 2;

            PrintMessage("This message will be printed on game framework project.");
        }

        [DllImport("GameFramework.dll", EntryPoint = "PrintMessage", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern void PrintMessage([MarshalAs(UnmanagedType.LPWStr)] string message);
    }
}
