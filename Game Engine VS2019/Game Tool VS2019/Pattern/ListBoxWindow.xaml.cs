using System.Windows;

namespace Game_Tool_VS2019.Pattern
{
    public partial class ListBoxWindow : Window
    {
        private EBlockType mType = EBlockType.Behaviour;
        private bool mbHideMode = true;

        public ListBoxWindow()
        {
            InitializeComponent();
        }

        public void ReloadList(EBlockType type)
        {
            PatternPartsList.Items.Clear();

            switch (type)
            {
                case EBlockType.IfStatement:
                    foreach (var part in PatternMaker.IfStatements)
                    {
                        PatternPartsList.Items.Add(part.Comment);
                    }
                    break;
                case EBlockType.Behaviour:
                    foreach (var part in PatternMaker.Behaviours)
                    {
                        PatternPartsList.Items.Add(part.Comment);
                    }
                    break;
                default:
                    break;
            }

            mType = type;
        }

        public void Destroy()
        {
            mbHideMode = false;
            Close();
        }

        private void OnVisibleChanged_ListBoxWindow(object sender, DependencyPropertyChangedEventArgs e)
        {
            Left = SystemParameters.PrimaryScreenWidth / 2 - Width / 2;
            Top = SystemParameters.PrimaryScreenHeight / 2 - Height / 2;
        }

        private void OnColsing_ListBoxWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (mbHideMode)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void OnMouseDoubleClick_PatternPartsList(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        }
    }
}
