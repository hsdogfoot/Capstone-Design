using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
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

            PatternName.Text = mLoadedPattern.Name;
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

        private void OnLoaded_PatternPanel(object sender, RoutedEventArgs e)
        {
            PatternPanel.Children.Clear();

            if (!Directory.Exists("Save Data\\Pattern"))
            {
                Directory.CreateDirectory("Save Data\\Pattern");
            }

            string[] files = Directory.GetFiles("Save Data\\Pattern");

            FileStream fileStream;
            StreamReader streamReader;
            string[] ifStatementsData;

            PatternFile patternFile;

            foreach (string filePath in files)
            {
                if ((fileStream = File.Open(filePath, FileMode.Open)) == null)
                {
                    continue;
                }

                streamReader = new StreamReader(fileStream);

                patternFile = new PatternFile(filePath, streamReader.ReadLine());

                ifStatementsData = streamReader.ReadLine().Split(',');
                for (int i = 1; i < ifStatementsData.Length; ++i)
                {
                    patternFile.LoadedPattern.IfStatements.AddLast(PatternMaker.IfStatements[int.Parse(ifStatementsData[i])].Clone());
                }

                patternFile.LoadedPattern.Behaviour = PatternMaker.Behaviours[int.Parse(streamReader.ReadLine())].Clone();
                patternFile.MouseLeftButtonDown += OnMouseLeftButtonDown_PatternFile;

                PatternPanel.Children.Add(patternFile);

                streamReader.Close();
                fileStream.Close();
            }
        }

        private void OnMouseLeftButtonDown_PatternFile(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2)
            {
                return;
            }

            MessageBox.Show("DoubleClicked.");
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

        private void OnClick_SavePatternButton(object sender, RoutedEventArgs e)
        {
            if (mLoadedPattern == null)
            {
                MessageBox.Show("작성된 행동 규칙이 없습니다.");

                return;
            }

            string fileName = string.Format($"PatternData_{mLoadedPattern.Name}.dat");
            FileStream stream = File.Open($"Save Data/Pattern/{fileName}", FileMode.OpenOrCreate);

            StreamWriter streamWriter = new StreamWriter(stream);
            streamWriter.WriteLine(mLoadedPattern.Name);

            streamWriter.Write(mLoadedPattern.IfStatements.Count);
            LinkedListNode<Block> block = mLoadedPattern.IfStatements.Last;
            while (block != null)
            {
                streamWriter.Write($",{block.Value.ID}");

                block = block.Previous;
            }

            streamWriter.WriteLine("");

            streamWriter.Write(mLoadedPattern.Behaviour.ID);

            streamWriter.Flush();
            streamWriter.Close();

            stream.Close();
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
