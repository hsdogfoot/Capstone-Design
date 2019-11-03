using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Game_Tool_VS2019
{
    public partial class TestWindow : Window
    {
        private Pattern mLoadedPattern = null;
        private IfBehaviourListWindow mIfBehaviourListWindow;

        public TestWindow()
        {
            InitializeComponent();

            Left = SystemParameters.PrimaryScreenWidth / 2 - Width / 2;
            Top = SystemParameters.PrimaryScreenHeight / 2 - Height / 2;

            PatternManager.Initialize();

            mIfBehaviourListWindow = new IfBehaviourListWindow(this);
        }

        public void AddBehaviourToPattern(Block behaviour)
        {
            mLoadedPattern.Behaviour = behaviour;

            reload_PatternView();
        }

        public void AddIfStatementToPattern(Block ifStatement)
        {
            mLoadedPattern.IfStatements.AddAfter(new LinkedListNode<Block>(ifStatement));

            reload_PatternView();
        }

        public void OnButtonClicked_IfAdder()
        {
            mIfBehaviourListWindow.ReloadIfStatements();
            mIfBehaviourListWindow.Show();
        }

        private void OnButtonClicked_NewPattern(object sender, RoutedEventArgs e)
        {
            mIfBehaviourListWindow.ReloadBehaviour();
            mIfBehaviourListWindow.Show();

            if (mLoadedPattern != null)
            {
                return;
            }

            mLoadedPattern = new Pattern("temp");
        }

        private void reload_PatternView()
        {
            PatternView.Children.Clear();

            for (int i = mLoadedPattern.IfStatements.Count - 1; i >= 0; --i)
            {
                PatternView.Children.Add(new IfAdder(this));
                PatternView.Children.Add(mLoadedPattern.IfStatements[i]);
            }

            PatternView.Children.Add(new IfAdder(this));
            PatternView.Children.Add(mLoadedPattern.Behaviour);
        }

        private void OnClosing_TestWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (mIfBehaviourListWindow != null)
            {
                mIfBehaviourListWindow.Destroy();
                mIfBehaviourListWindow = null;

                MessageBox.Show("Close!");
            }
        }
    }
}
