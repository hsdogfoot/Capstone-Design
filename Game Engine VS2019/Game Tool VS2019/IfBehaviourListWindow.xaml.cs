using System.Collections.Generic;
using System.Windows;

namespace Game_Tool_VS2019
{
    public partial class IfBehaviourListWindow : Window
    {
        public enum eWindowType
        {
            IfStatement,
            Behaviour,
        }

        private TestWindow mParentWindow = null;
        private bool mHideMode = true;
        private eWindowType mType;

        public IfBehaviourListWindow(TestWindow parentWindow)
        {
            mParentWindow = parentWindow;

            InitializeComponent();
        }

        public void ReloadBehaviour()
        {
            IfBehaviourListBox.Items.Clear();

            foreach (var behaviour in PatternManager.BehaviourList)
            {
                IfBehaviourListBox.Items.Add(behaviour.Comment);
            }

            mType = eWindowType.Behaviour;
        }

        public void ReloadIfStatements()
        {
            IfBehaviourListBox.Items.Clear();

            foreach (var ifStatements in PatternManager.IfList)
            {
                IfBehaviourListBox.Items.Add(ifStatements.Comment);
            }

            mType = eWindowType.IfStatement;
        }

        public void Destroy()
        {
            mHideMode = false;
            Close();
        }

        private void OnMouseDoubleClicked_IfBehaviourListBox(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (IfBehaviourListBox.SelectedIndex == -1)
            {
                return;
            }

            MessageBox.Show(string.Format($"{IfBehaviourListBox.SelectedIndex} Selected!"));

            if (mType == eWindowType.Behaviour)
            {
                mParentWindow.AddBehaviourToPattern(PatternManager.BehaviourList[IfBehaviourListBox.SelectedIndex]);
            }
            else if (mType == eWindowType.IfStatement)
            {
                mParentWindow.AddIfStatementToPattern(PatternManager.IfList[IfBehaviourListBox.SelectedIndex]);
            }
            
            Close();
        }

        private void OnVisibleChanged_IfBehaviourWindow(object sender, DependencyPropertyChangedEventArgs e)
        {
            Left = SystemParameters.PrimaryScreenWidth / 2 - Width / 2;
            Top = SystemParameters.PrimaryScreenHeight / 2 - Height / 2;
        }

        private void OnClosingWindow_IfBehaviourWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (mHideMode)
            {
                e.Cancel = true;
                Hide();
            }
            else
            {
                //MessageBox.Show("Close SubWindow");
            }
        }
    }
}
