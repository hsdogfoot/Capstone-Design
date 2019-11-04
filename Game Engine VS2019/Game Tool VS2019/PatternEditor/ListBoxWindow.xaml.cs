using System.Windows;

namespace Game_Tool_VS2019.PatternEditor
{
    public partial class ListBoxWindow : Window
    {
        public EBlockType Type { get; private set; } = EBlockType.Behaviour;
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

            Type = type;
        }

        public void Destroy()
        {
            mbHideMode = false;
            Close();
        }

        private void OnColsing_ListBoxWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (mbHideMode)
            {
                e.Cancel = true;
                Hide();
            }
        }
    }
}
