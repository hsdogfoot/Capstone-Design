using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Game_Tool_VS2019
{
    class IfAdder : Border
    {
        private TestWindow mParentWindow;

        public IfAdder(TestWindow parentWindow)
        {
            mParentWindow = parentWindow;

            Background = Brushes.White;

            Width = 50;
            Height = 50;

            Margin = new Thickness(5.0, 0.0, 5.0, 0.0);

            Polygon triangle = new Polygon();
            triangle.Points.Add(new Point(0.0, 0.0));
            triangle.Points.Add(new Point(Width, Height / 2));
            triangle.Points.Add(new Point(0.0, Height));
            triangle.Stroke = Brushes.Black;
            triangle.StrokeThickness = 5.0;
            triangle.Fill = Brushes.DarkRed;

            Child = triangle;

            Child.MouseEnter += OnMouseEnter;
            Child.MouseLeave += OnMouseLeave;

            MouseLeftButtonDown += OnMouseLeftButtonDown;
        }

        private void OnMouseEnter(object sender, RoutedEventArgs e)
        {
            Polygon triangle = (Polygon)Child;
            triangle.Fill = Brushes.OrangeRed;
        }

        private void OnMouseLeave(object sender, RoutedEventArgs e)
        {
            Polygon triangle = (Polygon)Child;
            triangle.Fill = Brushes.DarkRed;
        }

        private void OnMouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Polygon triangle = (Polygon)Child;

            if (triangle.Fill == Brushes.OrangeRed)
            {
                mParentWindow.OnButtonClicked_IfAdder();
            }
        }
    }
}
