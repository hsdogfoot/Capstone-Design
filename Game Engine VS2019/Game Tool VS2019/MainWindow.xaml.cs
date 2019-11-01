using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;

namespace Game_Tool_VS2019
{
    public partial class MainWindow : Window
    {
        private GameViewHwndHost mHwndHost;
        private DispatcherTimer mGameViewTimer;

        public MainWindow()
        {
            InitializeComponent();

            Left = SystemParameters.PrimaryScreenWidth / 2 - Width / 2;
            Top = SystemParameters.PrimaryScreenHeight / 2 - Height / 2;

            PrintMessage("This message will be printed on game framework project.");
        }

        private void OnLoaded_MainWindow(object sender, RoutedEventArgs e)
        {
            mHwndHost = new GameViewHwndHost((int)GamePreview.ActualWidth, (int)GamePreview.ActualHeight);
            GamePreview.Child = mHwndHost;

            mGameViewTimer = new DispatcherTimer();
            mGameViewTimer.Tick += new EventHandler(runGame);
            mGameViewTimer.Interval = TimeSpan.FromMilliseconds(16.67);

            mGameViewTimer.Start();
        }

        private void runGame(object sender, EventArgs e)
        {
            mHwndHost.RunGame();
        }

        private void OnClosed_MainWindow(object sender, EventArgs e)
        {
            mGameViewTimer.Stop();
        }

        [DllImport("GameFramework.dll", EntryPoint = "PrintMessage", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern void PrintMessage([MarshalAs(UnmanagedType.LPWStr)] string message);
    }
}
