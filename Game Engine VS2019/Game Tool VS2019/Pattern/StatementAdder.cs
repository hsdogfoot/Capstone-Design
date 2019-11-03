using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Game_Tool_VS2019.Pattern
{
    class StatementAdder : Border
    {
        private Polygon mPolygon = new Polygon();

        public StatementAdder(TestWindow parentWindow)
        {
            Width = 50.0;
            Height = 50.0;

            Background = Brushes.White;

            Margin = new Thickness(5.0, 0.0, 5.0, 0.0);

            Child = mPolygon;

            mPolygon.Points.Add(new Point(0.0, 0.0));
            mPolygon.Points.Add(new Point(Width, Height / 2));
            mPolygon.Points.Add(new Point(0.0, Height));

            mPolygon.Stroke = Brushes.Black;
            mPolygon.StrokeThickness = 5.0;
            
            mPolygon.Fill = Brushes.DarkRed;

            Child.MouseEnter += OnMouseEnter;
            Child.MouseLeave += OnMouseLeave;
        }

        private void OnMouseEnter(object sender, RoutedEventArgs e)
        {
            mPolygon.Fill = Brushes.OrangeRed;
        }

        private void OnMouseLeave(object sender, RoutedEventArgs e)
        {
            mPolygon.Fill = Brushes.DarkRed;
        }
    }
}
