using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
 
namespace KG1
{
    public partial class MainWindow : Window
    {
 
        bool dragOn = false;
        Point dragStart;
        double a = 100;        
 
        int PointsCount = 1000;
        List<PointController> Points;
 
        public MainWindow()
        {
            InitializeComponent();
 
            SetUpPoints();
            PointController.SetOffset(new Point(0, 0));
PointController.UpdateWindowSize(field.ActualWidth, field.ActualHeight);
        }
 
        private Point GetPointByT(double t)
        {
            double x = 3 * a * t / (1 + t * t * t);
            double y = 3 * a * t * t / (1 + t * t * t);
 
            return new Point(x, y);
        }
 
        private void SetUpPoints()
        {
            field.Children.Clear();
            PointController.SetUpAxis();
            field.Children.Add(PointController.AxisX);
            field.Children.Add(PointController.AxisY);
            field.Children.Add(PointController.CoordsBlock);
 
            Points = new List<PointController>(PointsCount);
            double step = (RightBoundOfT - LeftBoundOfT) / PointsCount;
            for (double t = LeftBoundOfT; t < RightBoundOfT; t += step)
            {
                var point = new PointController(GetPointByT(t));
                field.Children.Add(point.View);
                point.View.MouseMove += ((sender, ea) =>
                {
                    var x = Canvas.GetLeft((Ellipse)sender) + PointController.Offset.X - PointController.Center.X;
                    var y = Canvas.GetTop((Ellipse)sender) + PointController.Offset.Y - PointController.Center.Y;
                    PointController.CoordsBlock.Text = $"({x}, {y})";
                    Canvas.SetLeft(PointController.CoordsBlock, Mouse.GetPosition(this).X + 10);
                    Canvas.SetTop(PointController.CoordsBlock, Mouse.GetPosition(this).Y + 5);
                    PointController.CoordsBlock.Visibility = Visibility.Visible;
                });
                point.View.MouseLeave += ((sender, ea) =>
                {
                    PointController.CoordsBlock.Visibility = Visibility.Hidden;
                });
                Points.Add(point);
            }
        }
 
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            PointController.UpdateWindowSize(field.ActualWidth, field.ActualHeight);
            foreach (var point in Points)
            {
                point.CoordUpdate();
            }
        }
 
        private void field_MouseDown(object sender, MouseButtonEventArgs e)
        {
            dragStart = Mouse.GetPosition(field);
            dragOn = true;
        }
 
        private void field_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragOn)
            {
                var cur = Mouse.GetPosition(field);
                PointController.SetOffset(new Point(PointController.Offset.X - (cur.X - dragStart.X) / PointController.DeltaZoom, PointController.Offset.Y - (cur.Y - dragStart.Y)/PointController.DeltaZoom));
                dragStart = cur;
                var points = new PointCollection();
                foreach (var point in Points)
                {
                    point.CoordUpdate();
                    points.Add(new Point(Canvas.GetLeft(point.View), Canvas.GetTop(point.View)));
                }
            }
        }
 
        private void field_MouseUp(object sender, MouseButtonEventArgs e)
        {
            dragOn = false;
        }
 
        private void field_MouseLeave(object sender, MouseEventArgs e)
        {
            dragOn = false;
        }
 
        private void field_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            PointController.ZoomIt(e.Delta);
            foreach (var point in Points)
            {
                point.CoordUpdate();
            }
        }
 
        private void UpdatePoints()
        {
            if (Points == null || RightBoundOfT == LeftBoundOfT)
            {
                return;
            }
 
            double step = (RightBoundOfT - LeftBoundOfT) / PointsCount;
            var t = LeftBoundOfT;
            foreach (var point in Points)
            {
                point.SetNewCoords(GetPointByT(t));
                t += step;
            }
        }
 
        public double LeftBoundOfT { get; set; } = -5;
        public double RightBoundOfT { get; set; } = -1;
 
        private void ASlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ASlider != null && BSlider != null && Slider != null)
            {
                LeftBoundOfT = ASlider?.Value ?? -10;
                RightBoundOfT = BSlider?.Value ?? -1;
                BSlider.Minimum = ASlider?.Value ?? -10;
                ASlider.Maximum = BSlider?.Value ?? -1;
                a = Slider.Value;
                UpdatePoints();
            }
        }
 
        private void CountSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (CountSlider != null && Points != null)
            {
                var offset = PointController.Offset;
                PointsCount = (int)(CountSlider?.Value ?? 1000);
                SetUpPoints();
                UpdatePoints();
                PointController.SetOffset(offset);
            }
        }
    }
 
    public class PointController
    {
        private Point coords;
        public PointController(Point point)
        {
            View = new Ellipse();
            coords = point;
            View.Width = 5;
 
            View.Height = 5;
            View.Fill = Brushes.Black;
            View.Visibility = Visibility.Visible;
        }
 
        public Point Coords 
        { 
            get => coords;
            set
            {
                coords = value;
            }
        }
 
        public Ellipse View { get; private set; }
 
        public static TextBlock CoordsBlock { get; private set; }
        public static Line CoordsAxisX { get; private set; }
        public static Line CoordsAxisY { get; private set; }
        public static Point Offset { get; set; }
        public static double DeltaZoom { get; set; } = 1;
        public static Point Center { get; set; }
 
        public static Line AxisX;
        public static Line AxisY;
 
        public void CoordUpdate()
        {
            Canvas.SetLeft(View, Center.X - (Offset.X - coords.X) * DeltaZoom);
            Canvas.SetTop(View, Center.Y - (Offset.Y - coords.Y) * DeltaZoom);
        }
 
        public void SetNewCoords(Point point)
        {
            Coords = point;
            CoordUpdate();
        }
 
        public static void SetOffset(Point offset)
        {
            Offset = offset;
            AxisUpdate();
        }
 
        public static void ZoomIt(int delta)
        {
            double res;
            if (delta < 0)
            {
                res = DeltaZoom * 30.0 / (-delta);
            }
            else
            {
                res = DeltaZoom * delta / 30.0;
            }
            if (res < 0.01)
            {
                DeltaZoom = 0.01;
            } 
            else if (res > 100)
            {
                DeltaZoom = 100;
            }
            else
            {
                DeltaZoom = res;
            }
            AxisUpdate();
        }
 
 
        public static void SetUpAxis()
        {
            SolidColorBrush redBrush = new SolidColorBrush();
            redBrush.Color = Colors.Red;
 
            AxisX = new Line();
            AxisX.SnapsToDevicePixels = true;
            AxisX.Stroke = redBrush;
            AxisX.StrokeThickness = 2.5;
 
            AxisY = new Line();
            AxisY.SnapsToDevicePixels = true;
            AxisY.Stroke = redBrush;
            AxisY.StrokeThickness = 2.5;
 
            CoordsBlock = new TextBlock();
            CoordsBlock.Foreground = Brushes.Black;
            CoordsBlock.Visibility = Visibility.Hidden;
        } 
 
        public static void UpdateWindowSize(double x, double y)
        {
            Center = new Point(x / 2, y / 2);
            AxisUpdate();
        }
 
        private static void AxisUpdate()
        {
            AxisX.Y1 = Center.Y - Offset.Y * DeltaZoom;
            AxisX.Y2 = Center.Y - Offset.Y * DeltaZoom;
 
            AxisX.X1 = 0;
            AxisX.X2 = Center.X * 2;
 
            AxisY.X1 = Center.X - Offset.X * DeltaZoom;
            AxisY.X2 = Center.X - Offset.X * DeltaZoom;
 
            AxisY.Y1 = 0;
            AxisY.Y2 = Center.Y * 2;
        }
    }
}
 
