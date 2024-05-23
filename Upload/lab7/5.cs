using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace KG1
{
    /*
     * Хашимов Амир Азизович
     * М8О-307Б-20
     * Вариант-14
     * Интерполяционный многочлен Лагпранжа по 6 точкам
     */
    public partial class MainWindow : Window
    {
        private const int pointsCount = 10;
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

        // x = t
        // y = y(t)
        private Point GetPointByT(double t, List<PointController> points)
        {
            double y = 0;
            for (int i = 0; i < points.Count; i++)
            {
                double l = 1;
                for (int j = 0; j < points.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    l *= (t - points[j].Coords.X) / (points[i].Coords.X - points[j].Coords.X);
                }
                y += l * points[i].Coords.Y;
            }
            return new Point(t, y);
        }

        private void SetUpPoints()
        {
            SolidColorBrush blackBrush = new SolidColorBrush();
            blackBrush.Color = Colors.Black;
            field.Children.Clear();
            PointController.SetUpAxis();
            PointController.Curve = new Polyline();
            PointController.Curve.StrokeThickness = 4;
            PointController.Curve.Stroke = blackBrush;
            PointController.Curve.Visibility = Visibility.Visible;
            field.Children.Add(PointController.AxisX);
            field.Children.Add(PointController.AxisY);
            field.Children.Add(PointController.CoordsBlock);
            field.Children.Add(PointController.Curve);
            Points = new List<PointController>();
            for (int i = 0; i < pointsCount; i++)
            {
                var point = new PointController(new Point(i * 100, i * 100));
                field.Children.Add(point.View);
                point.View.MouseMove += ((sender, ea) =>
                {
                    if (point.IsCaptured)
                    {
                        var cur = Mouse.GetPosition(field);
                        var dx = (cur.X - point.DragStart.X) / PointController.DeltaZoom;
                        var dy = (cur.Y - point.DragStart.Y) / PointController.DeltaZoom;
                        point.SetNewCoords(new Point(point.Coords.X + dx, point.Coords.Y + dy));
                        point.DragStart = cur;


                        var curvePoints = new PointCollection();
                        double step = (RightBoundOfT - LeftBoundOfT) / PointsCount;
                        for (double t = LeftBoundOfT; t < RightBoundOfT; t += step)
                        {
                            var point = GetPointByT(t, Points);
                            curvePoints.Add(point);
                        }
                        PointController.CurvePoints = curvePoints;
                        PointController.CoordsUpdate();
                    }
                    else
                    {
                        var x = Canvas.GetLeft((Ellipse)sender) + PointController.Offset.X - PointController.Center.X;
                        var y = Canvas.GetTop((Ellipse)sender) + PointController.Offset.Y - PointController.Center.Y;
                        PointController.CoordsBlock.Text = $"({x}, {y})";
                        Canvas.SetLeft(PointController.CoordsBlock, Mouse.GetPosition(this).X + 10);
                        Canvas.SetTop(PointController.CoordsBlock, Mouse.GetPosition(this).Y + 5);
                        PointController.CoordsBlock.Visibility = Visibility.Visible;

                    }
                });
                point.View.MouseLeave += ((sender, ea) =>
                {
                    if (point.IsCaptured)
                    {
                        var cur = Mouse.GetPosition(field);
                        var dx = (cur.X - point.DragStart.X) / PointController.DeltaZoom;
                        var dy = (cur.Y - point.DragStart.Y) / PointController.DeltaZoom;
                        point.SetNewCoords(new Point(point.Coords.X + dx, point.Coords.Y + dy));
                        point.DragStart = cur;
                    }
                    PointController.CoordsBlock.Visibility = Visibility.Hidden;
                });
                point.View.MouseDown += ((sender, ea) =>
                {
                    PointController.CoordsBlock.Visibility = Visibility.Hidden;
                    point.DragStart = Mouse.GetPosition(field);
                    point.IsCaptured = true;
                });
                point.View.MouseUp += ((sender, ea) =>
                {
                    point.IsCaptured = false;
                });
                Points.Add(point);

            }

            var curvePoints = new PointCollection();
            double step = (RightBoundOfT - LeftBoundOfT) / PointsCount;
            for (double t = LeftBoundOfT; t < RightBoundOfT; t += step)
            {
                var point = GetPointByT(t, Points);
                curvePoints.Add(point);
            }
            PointController.Curve.Points = curvePoints;
            PointController.CurvePoints = curvePoints;
            PointController.CoordsUpdate();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            PointController.UpdateWindowSize(field.ActualWidth, field.ActualHeight);
            foreach (var point in Points)
            {
                point.CoordUpdate();
            }
            PointController.CoordsUpdate();

        }

        private void field_MouseDown(object sender, MouseButtonEventArgs e)
        {
            dragStart = Mouse.GetPosition(field);
            dragOn = true;
        }

        private void field_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragOn && Points.All(point => !point.IsCaptured))
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
                PointController.CoordsUpdate();

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
            PointController.CoordsUpdate();

        }

        private void UpdatePoints()
        {
            if (Points == null || RightBoundOfT == LeftBoundOfT)
            {
                return;
            }

            var curvePoints = new PointCollection();
            double step = (RightBoundOfT - LeftBoundOfT) / PointsCount;
            for (double t = LeftBoundOfT; t < RightBoundOfT; t += step)
            {
                var point = GetPointByT(t, Points);
                curvePoints.Add(point);
            }
            PointController.CurvePoints = curvePoints;
            PointController.Curve.Points = curvePoints;
        }

        public double LeftBoundOfT { get; set; } = -10000;
        public double RightBoundOfT { get; set; } = 10000;


        private void CountSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (CountSlider != null && Points != null)
            {
                PointsCount = (int)(CountSlider?.Value ?? 1000);

                var curvePoints = new PointCollection();
                double step = (RightBoundOfT - LeftBoundOfT) / PointsCount;
                for (double t = LeftBoundOfT; t < RightBoundOfT; t += step)
                {
                    var point = GetPointByT(t, Points);
                    curvePoints.Add(point);
                }
                PointController.Curve.Points = curvePoints;
                PointController.CurvePoints = curvePoints;
                PointController.CoordsUpdate();
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
            View.Width = 10;
            View.Height = 10;
            View.Fill = Brushes.Black;
            View.Visibility = Visibility.Visible;
            IsCaptured = false;
        }

        public Point Coords 
        { 
            get => coords;
            set
            {
                coords = value;
            }
        }

        public Point DragStart { get; set; }
        public Ellipse View { get; private set; }

        public bool IsCaptured { get; set; }

        public static TextBlock CoordsBlock { get; private set; }
        public static Line CoordsAxisX { get; private set; }
        public static Line CoordsAxisY { get; private set; }
        public static Point Offset { get; set; }
        public static Polyline Curve { get; set; }
        public static double DeltaZoom { get; set; } = 1;
        public static Point Center { get; set; }

        public static Line AxisX;
        public static Line AxisY;

        public static PointCollection CurvePoints { get; set; }

        public void CoordUpdate()
        {
            Canvas.SetLeft(View, Center.X - (Offset.X - coords.X) * DeltaZoom);
            Canvas.SetTop(View, Center.Y - (Offset.Y - coords.Y) * DeltaZoom);
        }

        public static void CoordsUpdate()
        {
            var points = new PointCollection();
            foreach (var point in CurvePoints)
            {
                points.Add(new Point(Center.X - (Offset.X - point.X) * DeltaZoom, Center.Y - (Offset.Y - point.Y) * DeltaZoom));
            }
            Curve.Points = points;
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
                res = DeltaZoom * 60.0 / (-delta);
            }
            else
            {
                res = DeltaZoom * delta / 60.0;
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
