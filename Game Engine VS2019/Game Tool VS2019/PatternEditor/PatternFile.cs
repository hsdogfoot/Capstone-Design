using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Game_Tool_VS2019.PatternEditor
{
    public class PatternFile : Border
    {
        public string FilePath { get; private set; }
        public Pattern LoadedPattern { get; private set; }

        private Grid mGrid = new Grid();
        private Image mImage = new Image();
        private TextBlock mTextBlock = new TextBlock();

        public PatternFile(string path, string fileName)
        {
            FilePath = path;
            LoadedPattern = new Pattern(fileName);

            Width = 150.0;
            Height = 150.0;

            Background = Brushes.White;
            BorderThickness = new Thickness(5.0);
            BorderBrush = Brushes.Black;

            Margin = new Thickness(15.0, 0.0, 0.0, 0.0);

            Child = mGrid;
            mGrid.Children.Add(mImage);
            mGrid.Children.Add(mTextBlock);

            mGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(120.0) });
            mGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(5.0) });
            mGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(25.0) });

            mGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(15.0) });
            mGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120.0) });
            mGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(15.0) });

            Grid.SetRow(mImage, 0);
            Grid.SetColumn(mImage, 1);
            Grid.SetRowSpan(mImage, 1);
            Grid.SetColumnSpan(mImage, 1);

            Grid.SetRow(mTextBlock, 2);
            Grid.SetColumn(mTextBlock, 0);
            Grid.SetRowSpan(mTextBlock, 1);
            Grid.SetColumnSpan(mTextBlock, 3);

            mImage.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + string.Format($"\\Resources\\Textures\\PatternImage.png")));

            mTextBlock.Text = "TESTNAMEABCDEDFGHIJKLMNOPQRSTUVWXYZ";
            mTextBlock.FontSize = 20.0;
            mTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            mTextBlock.VerticalAlignment = VerticalAlignment.Center;
        }
    }
}
