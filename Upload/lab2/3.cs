using System.Windows;
using System.Windows.Input;

namespace Lab2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            VM = new ViewModel();
            Model = new WindowModel(VM);
            Model.StartupHandler();
            DataContext = VM;
            InitializeComponent();
        }

        public ViewModel VM;
        public WindowModel Model;
        private Point dragStart;
        private bool dragOn;

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
                VM.RotationY += (int)(cur.X - dragStart.X);
                VM.RotationX += (int)(cur.Y - dragStart.Y);
                dragStart = cur;
            }
        }

        private void field_MouseUp(object sender, MouseButtonEventArgs e)
        {
            dragOn = false;
        }

        private void field_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            ZoomIt(e.Delta);
        }

        private void ZoomIt(int delta)
        {
            double res;
            if (delta < 0)
            {
                res = VM.Scale * 80.0 / (-delta);
            }
            else
            {
                res = VM.Scale * delta / 80.0;
            }
            if (res < 1)
            {
                VM.Scale = 1;
            }
            else if (res > 100)
            {
                VM.Scale = 100;
            }
            else
            {
                VM.Scale = (int)res;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void field_MouseLeave(object sender, MouseEventArgs e)
        {
            dragOn = false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Lab2
{
    public class ViewModel: INotifyPropertyChanged
    {
        private bool magic;
        private bool invisibleLines;
        private bool perspective;
        private int rotationX;
        private int rotationY;
        private int rotationZ;
        private ImageSource image;
        private int scale;

        public event PropertyChangedEventHandler PropertyChanged;
        public bool Magic
        {
            get
            {
                return magic;
            }
            set
            {
                magic = value;
                OnPropertyChanged();
            }
        }
        public bool InvisibleLines
        {
            get
            {
                return invisibleLines;
            }
            set
            {
                invisibleLines = value;
                OnPropertyChanged();
            }
        }
        public bool Perspective
        {
            get
            {
                return perspective;
            }
            set
            {
                perspective = value;
                OnPropertyChanged();
            }
        }
        public int RotationX
        {
            get
            {
                return rotationX;
            }
            set
            {
                rotationX = value;
                OnPropertyChanged();
            }
        }
        public int RotationY
        {
            get {  return rotationY;}
            set {
                rotationY = value;
                OnPropertyChanged();
            }
        }
        public int RotationZ
        {
            get
            {
                return rotationZ;
            }
            set
            {
                rotationZ = value;
                OnPropertyChanged();
            }
        }
        public int Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
                OnPropertyChanged();
            }
        }
        public ImageSource Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                OnPropertyChanged();
            }
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

using Lab2.Projection;
using System;
using System.Threading;
using System.Windows;

namespace Lab2
{
    public class WindowModel
    {
        private readonly ViewModel _viewModel;
        private readonly Scene _scene;
        private Figure _prism;
        private const int _fps = 50;

        public WindowModel(ViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.Scale = 50;
            _scene = new Scene(600, 400);
            _prism = new Figure(300, 400);
            _prism.CameraOffset = 100;
            _prism.FocusDistance = 400;
        }

        // VARIANT 15
        public void WindowLooper()
        {
            if(_viewModel.Magic)
            {
                _viewModel.RotationX = (_viewModel.RotationX + 1) % 360;
                _viewModel.RotationY = (_viewModel.RotationY + 2) % 360;
                _viewModel.RotationZ = (_viewModel.RotationZ + 3) % 360;
            }
            _prism.AngleX = _viewModel.RotationX;
            _prism.AngleY = _viewModel.RotationY;
            _prism.AngleZ = _viewModel.RotationZ;
            _prism.Scale = _viewModel.Scale / 100f;
            _prism.PerspectiveEnabled = _viewModel.Perspective;
            _prism.InvisibleLines = !_viewModel.InvisibleLines;
            _prism.Project(_scene);
        }

        public void StartupHandler()
        {
            new Thread(() =>
            {
                while (true)
                {
                    long ms = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                    WindowLooper();
                    UpdateImage();
                    Thread.Sleep(Math.Max(0, (1000 / _fps) - (int)(DateTimeOffset.Now.ToUnixTimeMilliseconds() - ms)));
                }
            }).Start();
        }

        private void UpdateImage()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _viewModel.Image = _scene.GetSource();
            });
            _scene.Clear(_viewModel.Magic);
        }
    }
}

using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Lab2.Projection
{
    public class Scene
    {
        public int Width { get; }
        public int Height { get; }

        private Bitmap sceneBmp;
        internal Graphics graphics;

        public Scene(int w, int h)
        {
            Width = w;
            Height = h;
            sceneBmp = new Bitmap(Width, Height, System.Drawing.Imaging.PixelFormat.Format16bppRgb565);
            graphics = Graphics.FromImage(sceneBmp);
        }

        public void Clear(bool needMagic)
        {
            graphics.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(needMagic ? 20 : 255, 220, 239, 255)), new Rectangle(0, 0, Width, Height));
        }

        public BitmapSource GetSource()
        {
            var bitmapData = sceneBmp.LockBits(
                new Rectangle(0, 0, sceneBmp.Width, sceneBmp.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, sceneBmp.PixelFormat);
            var bitmapSource = BitmapSource.Create(
                bitmapData.Width, bitmapData.Height,
                sceneBmp.HorizontalResolution, sceneBmp.VerticalResolution,
                PixelFormats.Bgr565, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);
            sceneBmp.UnlockBits(bitmapData);
            return bitmapSource;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Lab2.Projection
{
    public class Figure
    {
        private Point3D[] vertexes;
        private Surface[] faces;

        public Figure(int w, int h)
        {
            int lowerRadius = w / 2;
            int upperRadius = w / 8;
            int height = h / 2;
            double step =  Math.PI / 5;

            vertexes = new Point3D[20];
            for (int i = 0; i < 10; i++)
            {
                vertexes[2 * i] = new Point3D((int)(Math.Cos(i * step) * lowerRadius), (int)(Math.Sin(i * step) * lowerRadius), -height);
                vertexes[2 * i + 1] = new Point3D((int)(Math.Cos(i * step) * upperRadius), (int)(Math.Sin(i * step) * upperRadius), height);
            }
            faces = new Surface[12];
            for (int i = 0, index = 0; index < 10; i += 2, index++)
            {
                faces[index] = new Surface(new int[] { i, (i + 1) % 20, (i + 3) % 20, (i + 2) % 20 });
            }
            faces[10] = new Surface(new int[] { 0, 2, 4, 6, 8, 10, 12, 14, 16, 18 });
            faces[11] = new Surface(new int[] { 19, 17, 15, 13, 11, 9, 7, 5, 3, 1 });
        }

        public int AngleX { get; set; }
        public int AngleY { get; set; }
        public int AngleZ { get; set; }
        public int CameraOffset { get; set; }
        public int FocusDistance { get; set; }
        public bool PerspectiveEnabled { get; set; }
        public bool InvisibleLines { get; set; }
        public float Scale { get; set; }

        public void Project(Scene scene)
        {
            var vtx = Vertexes();
            var fcs = Faces();
            ApplyMatrixOp(vtx, new double[,]
            {
                { Math.Cos(DegToRad(AngleZ)), -Math.Sin(DegToRad(AngleZ)), 0, 0 },
                { Math.Sin(DegToRad(AngleZ)),  Math.Cos(DegToRad(AngleZ)), 0, 0 },
                {                          0,                           0, 1, 0 },
                {                          0,                           0, 0, 1 }
            });
            ApplyMatrixOp(vtx, new double[,]
            {
                { 1,                          0,                           0, 0 },
                { 0, Math.Cos(DegToRad(AngleX)), -Math.Sin(DegToRad(AngleX)), 0 },
                { 0, Math.Sin(DegToRad(AngleX)),  Math.Cos(DegToRad(AngleX)), 0 },
                { 0,                          0,                           0, 1 },
            });
            ApplyMatrixOp(vtx, new double[,]
            {
                {  Math.Cos(DegToRad(AngleY)), 0,  Math.Sin(DegToRad(AngleY)), 0 },
                {                           0, 1,                           0, 0 },
                { -Math.Sin(DegToRad(AngleY)), 0,  Math.Cos(DegToRad(AngleY)), 0 },
                {                           0, 0,                           0, 1 }
            });
            ApplyMatrixOp(vtx, new double[,]
            {
                { Scale,     0,     0, 0 },
                {     0, Scale,     0, 0 },
                {     0,     0, Scale, 0 },
                {     0,     0,     0, 1 }
            });
            ApplyMatrixOp(vtx, new double[,]
            {
                { 1, 0,            0, 0 },
                { 0, 1,            0, 0 },
                { 0, 0,            1, 0 },
                { 0, 0, CameraOffset, 1 }
            });
            if (PerspectiveEnabled)
            {
                ApplyPerspective(vtx, FocusDistance);
            }
            Draw(scene, vtx, fcs);
            //DrawPoints(scene, vtx);
        }

        private void ApplyPerspective(Point3D[] vtx, float fd)
        {
            for (int i = 0; i < vtx.Length; i++)
            {
                var v = vtx[i];
                var d = fd + v.Z;
                if (d <= 0)
                {
                    d = 0.001f;
                }
                var x = (int)(v.X * fd / d);
                var y = (int)(v.Y * fd / d);
                vtx[i] = new Point3D(x, y, 0);
            }
        }

        private void ApplyMatrixOp(Point3D[] vtx, double[,] mtx)
        {
            for (int i = 0; i < vtx.Length; i++)
            {
                var v = vtx[i];
                var x = (int)(mtx[0, 0] * v.X + mtx[1, 0] * v.Y + mtx[2, 0] * v.Z + mtx[3, 0]);
                var y = (int)(mtx[0, 1] * v.X + mtx[1, 1] * v.Y + mtx[2, 1] * v.Z + mtx[3, 1]);
                var z = (int)(mtx[0, 2] * v.X + mtx[1, 2] * v.Y + mtx[2, 2] * v.Z + mtx[3, 2]);
                vtx[i] = new Point3D(x, y, z);
            }
        }

        private void Draw(Scene sc, Point3D[] vtx, Surface[] surfaces)
        {
            var hw = sc.Width / 2;
            var hh = sc.Height / 2;
            var lines = new List<Line>();
            foreach (var surface in surfaces)
            {
                var pts = surface.Vertexes.Select(vi => new Point(vtx[vi].X + hw, vtx[vi].Y + hh)).ToArray();
                var pen = new Pen(Brushes.Black, 3);
                foreach (var l in surface.GetLines(vtx))
                {
                    if (!lines.Contains(l))
                    {
                        l.Type = InvisibleLines ? LineType.Invisible : LineType.Dotted;
                        lines.Add(l);
                    }
                }
                if (!surface.IsHidden(vtx))
                {
                    foreach (var l in surface.GetLines(vtx))
                    {
                        var fl = lines[lines.IndexOf(l)];
                        fl.Type = LineType.Solid;
                    }
                }
            }
            foreach (var line in lines)
            {
                var pen = new Pen(Brushes.Black, 3);
                switch (line.Type)
                {
                    case LineType.Invisible:
                        continue;
                    case LineType.Solid:
                        pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                        break;
                    case LineType.Dotted:
                        pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                        break;
                }
                sc.graphics.DrawLine(pen, new Point(line.V1.X + hw, line.V1.Y + hh), new Point(line.V2.X + hw, line.V2.Y + hh));
            }
        }


        public double DegToRad(double angle)
        {
            return (Math.PI / 180) * angle;
        }

        public Point3D[] Vertexes()
        {
            return (Point3D[])vertexes.Clone();
        }

        public Surface[] Faces()
        {
            return faces;
        }
    }
}

using System.Collections.Generic;

namespace Lab2.Projection
{
    public struct Point3D
    {
        public int X, Y, Z;

        public Point3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override bool Equals(object obj)
        {
            return obj is Point3D v && X == v.X && Y == v.Y && Z == v.Z;
        }
    }
    
    public class Line
    {
        public Point3D V1, V2;
        public LineType Type;

        public Line(Point3D v1, Point3D v2)
        {
            V1 = v1;
            V2 = v2;
            Type = LineType.Solid;
        }

        public override bool Equals(object obj)
        {
            return obj is Line ln && ((V1.Equals(ln.V1) && V2.Equals(ln.V2)) || (V2.Equals(ln.V1) && V1.Equals(ln.V2)));
        }
    }

    public enum LineType
    {
        Invisible,
        Solid,
        Dotted
    }

    public class Surface
    {
        public int[] Vertexes { get; set; }

        public Surface(int[] vertexes)
        {
            Vertexes = vertexes;
        }

        public IEnumerable<Line> GetLines(Point3D[] vtx)
        {
            if (Vertexes.Length < 2)
            {
                yield break;
            }
            yield return new Line(vtx[Vertexes[Vertexes.Length - 1]], vtx[Vertexes[0]]);
            for (int i = 0; i < Vertexes.Length - 1; i++)
            {
                yield return new Line(vtx[Vertexes[i]], vtx[Vertexes[i + 1]]);
            }
        }

        public bool IsHidden(Point3D[] vertexes)
        {
            var p0 = vertexes[Vertexes[0]];
            var p1 = vertexes[Vertexes[1]];
            var p2 = vertexes[Vertexes[2]];
            var Ах = p1.X - p0.X;
            var Ау = p1.Y - p0.Y;
            var Вх = p2.X - p1.X;
            var Ву = p2.Y - p1.Y;
            var Z = Ах * Ву - Ау * Вх;
            return Z < 0;
        }
    }
}
