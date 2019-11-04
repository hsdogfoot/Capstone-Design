using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Game_Tool_VS2019.PatternEditor
{
    public class Block : Border
    {
        public int ID { get; private set; }
        public string Comment { get; private set; }
        public EBlockType Type { get; private set; }

        private Grid mGrid = new Grid();
        private TextBlock mTextBlock = new TextBlock();

        public Block(int id, string comment, EBlockType type)
        {
            ID = id;
            Comment = comment;
            Type = type;

            Width = 200.0;
            Height = 125.0;

            switch (Type)
            {
                case EBlockType.IfStatement:
                    Background = Brushes.LightBlue;
                    break;
                case EBlockType.Behaviour:
                    Background = Brushes.LightYellow;
                    break;
                default:
                    break;
            }

            Child = mGrid;
            mGrid.Children.Add(mTextBlock);

            mGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            Grid.SetRow(mTextBlock, 0);
            Grid.SetColumn(mTextBlock, 0);
            Grid.SetRowSpan(mTextBlock, 1);
            Grid.SetColumnSpan(mTextBlock, 1);

            mTextBlock.Text = comment;
            mTextBlock.TextAlignment = TextAlignment.Center;
            mTextBlock.VerticalAlignment = VerticalAlignment.Center;
        }

        public Block Clone()
        {
            Block block = new Block(ID, Comment, Type);

            return block;
        }
    }
}
