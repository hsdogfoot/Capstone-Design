using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Game_Tool_VS2019
{
    public enum eBlockType
    {
        IfStatement,
        Behaviour,
    }

    public class Block : Border, IComparable<Block>
    {
        public int ID { get; private set; }
        public string Comment { get; private set; }

        private Grid mGrid = new Grid();
        private TextBlock mTextBlock = new TextBlock();

        public Block(int id, string comment, eBlockType type)
        {
            ID = id;
            Comment = comment;

            if (type == eBlockType.IfStatement)
            {
                Background = Brushes.LightCoral;
            }
            else if (type == eBlockType.Behaviour)
            {
                Background = Brushes.LightCyan;
            }

            Width = 150;
            Height = 150;

            mGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            mTextBlock.Text = comment;
            mTextBlock.TextAlignment = TextAlignment.Center;
            mTextBlock.VerticalAlignment = VerticalAlignment.Center;

            mGrid.Children.Add(mTextBlock);
            Grid.SetRow(mTextBlock, 0);
            Grid.SetColumn(mTextBlock, 0);
            Grid.SetRowSpan(mTextBlock, 1);
            Grid.SetColumnSpan(mTextBlock, 1);

            Child = mGrid;
        }

        public int CompareTo(Block other)
        {
            return ID.CompareTo(other.ID);
        }
    }
}
