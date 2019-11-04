using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;

using Game_Tool_VS2019.PatternEditor;

namespace Game_Tool_VS2019
{
    public partial class MainWindow : Window
    {
        private GameViewHwndHost mHwndHost;
        private DispatcherTimer mGameViewTimer;

        private ListBoxWindow mListBoxWindow = new ListBoxWindow();
        private Pattern mLoadedPattern = null;
        private int mInsertTargetIndex = -1;

        public MainWindow()
        {
            InitializeComponent();

            Left = SystemParameters.PrimaryScreenWidth / 2 - Width / 2;
            Top = SystemParameters.PrimaryScreenHeight / 2 - Height / 2;

            PrintMessage("This message will be printed on game framework project.");

            PatternMaker.Initialize();
            mListBoxWindow.MouseDoubleClick += addParts;
            mListBoxWindow.IsVisibleChanged += OnVisibleChanged_ListBoxWindow;
        }

        private void runGame(object sender, EventArgs e)
        {
            mHwndHost.RunGame();
        }

        private void addParts(object sender, EventArgs e)
        {
            if (mListBoxWindow.PatternPartsList.SelectedIndex == -1)
            {
                return;
            }

            switch (mListBoxWindow.Type)
            {
                case EBlockType.IfStatement:
                    Debug.Assert(mInsertTargetIndex != -1);

                    if (mInsertTargetIndex == 0)
                    {
                        mLoadedPattern.IfStatements.AddFirst(PatternMaker.IfStatements[mListBoxWindow.PatternPartsList.SelectedIndex]);
                        break;
                    }

                    int index = 1;
                    LinkedListNode<Block> node = mLoadedPattern.IfStatements.First;
                    while (node != null)
                    {
                        if (mInsertTargetIndex == index)
                        {
                            mLoadedPattern.IfStatements.AddAfter(node, PatternMaker.IfStatements[mListBoxWindow.PatternPartsList.SelectedIndex]);
                        }

                        node = node.Next;
                        ++index;
                    }
                    break;
                case EBlockType.Behaviour:
                    mLoadedPattern.Behaviour = PatternMaker.Behaviours[mListBoxWindow.PatternPartsList.SelectedIndex];
                    break;
                default:
                    break;
            }

            mListBoxWindow.Close();
            reloadPatternView();
        }

        private void reloadPatternView()
        {
            PatternViewPanel.Children.Clear();

            StatementAdder adder;

            LinkedListNode<Block> node = mLoadedPattern.IfStatements.Last;
            int index = mLoadedPattern.IfStatements.Count;
            while (node != null)
            {
                adder = new StatementAdder(index);
                adder.MouseLeftButtonUp += OnMouseLeftButtonUp_StatementAdder;
                PatternViewPanel.Children.Add(adder);
                PatternViewPanel.Children.Add(node.Value.Clone());

                node = node.Previous;
                --index;
            }
            
            adder = new StatementAdder(0);
            adder.MouseLeftButtonUp += OnMouseLeftButtonUp_StatementAdder;
            PatternViewPanel.Children.Add(adder);
            PatternViewPanel.Children.Add(mLoadedPattern.Behaviour.Clone());
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

        private void OnClick_CreatePatternButton(object sender, RoutedEventArgs e)
        {
            if (mLoadedPattern != null)
            {
                MessageBox.Show("!!");

                return;
            }

            mLoadedPattern = new Pattern("Temp");

            if (mListBoxWindow.IsVisible)
            {
                MessageBox.Show(";");
                mListBoxWindow.Close();
            }

            mListBoxWindow.ReloadList(EBlockType.Behaviour);
            mListBoxWindow.Show();
        }

        private void OnMouseLeftButtonUp_StatementAdder(object sender, RoutedEventArgs e)
        {
            if (mListBoxWindow.IsVisible)
            {
                MessageBox.Show(";");
                mListBoxWindow.Close();
            }

            StatementAdder adder = (StatementAdder)sender;
            mInsertTargetIndex = adder.DependencyIndex;

            mListBoxWindow.ReloadList(EBlockType.IfStatement);
            mListBoxWindow.Show();
        }

        private void OnVisibleChanged_ListBoxWindow(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (mListBoxWindow.IsVisible)
            {
                mListBoxWindow.Left = SystemParameters.PrimaryScreenWidth / 2 - mListBoxWindow.Width / 2;
                mListBoxWindow.Top = SystemParameters.PrimaryScreenHeight / 2 - mListBoxWindow.Height / 2;
            }
            else
            {
                mInsertTargetIndex = -1;
            }
        }

        private void OnClosing_MainWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Debug.Assert(mListBoxWindow != null);

            mListBoxWindow.Destroy();
        }

        private void OnClosed_MainWindow(object sender, EventArgs e)
        {
            mGameViewTimer.Stop();
        }

        [DllImport("GameFramework.dll", EntryPoint = "PrintMessage", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern void PrintMessage([MarshalAs(UnmanagedType.LPWStr)] string message);
    }
}
