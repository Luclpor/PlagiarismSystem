using System.Drawing;
namespace lab7CG
{
public class NURBS
{
private int N = 40;
public int w = 1;
private PointF[] dataPoints;

public NURBS(PointF[] points)
{
dataPoints = points;
Invalidate();
}

public PointF[] DrawingPoints { get; private set; }

public PointF[] DataPoints
{
get { return dataPoints; }
set
{
dataPoints = value;
Invalidate();
}
}

public PointF this[int i]
{
get { return dataPoints[i]; }
set
{
dataPoints[i] = value;
Invalidate();
}
}

public void Invalidate()
{
DrawingPoints = new PointF[N + 1];
float dt = 1f / N;
float t = 0f;
for (int i = 0; i <= N; i++)
{
DrawingPoints[i] = B(t);
t += dt;
}
}

private PointF B(float t)
{
float c0 = (1 - t) * (1 - t) * (1 - t) * (1 - t) * (1 - t);
float c1 = (1 - t) * (1 - t) * (1 - t) * (1 - t) * 5 * t / w;
float c2 = (1 - t) * (1 - t) * (1 - t) * t * t * 10;
float c3 = (1 - t) * (1 - t) * t * t * t * 10 / w ;
float c4 = t * t * t * t * 5 * (1 - t);
float c5 = t * t * t * t * t;
float x = c0 * dataPoints[0].X + c1 * dataPoints[1].X + c2 * dataPoints[2].X + c3 * dataPoints[3].X + c4 * dataPoints[4].X + c5 * dataPoints[5].X;
float y = c0 * dataPoints[0].Y + c1 * dataPoints[1].Y + c2 * dataPoints[2].Y + c3 * dataPoints[3].Y + c4 * dataPoints[4].Y + c5 * dataPoints[5].Y;
return new PointF(x, y);
}

public void Draw(Graphics g)
{
Pen pen = new Pen(System.Drawing.SystemColors.Highlight, 2f);
g.DrawLines(pen, DrawingPoints);
}
}
}

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace lab7CG
{
public partial class Form1 : Form
{
NURBS nurb;
Marker[] markers = new Marker[6];

public Form1()
{
InitializeComponent();
markers[0] = new Marker(100, 200);
markers[1] = new Marker(150, 250);
markers[2] = new Marker(200, 150);
markers[3] = new Marker(250, 200);
markers[4] = new Marker(300, 250);
markers[5] = new Marker(350, 300);

for (int index = 0; index < markers.Length; index++)
{
Marker marker = markers[index];
int i = index;
marker.OnDrag += f =>
{
nurb[i] = f;
pictureBox.Invalidate();
};
marker.OnMouseDown += f => { Cursor = Cursors.Hand; };
}

nurb = new NURBS(markers.Select(m => m.Location).ToArray());
}

private void pictureBox_Paint(object sender, PaintEventArgs e)
{
e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
Pen pen = new Pen(Color.Gray, 1f);
e.Graphics.DrawLines(pen, markers.Select(m => m.Location).ToArray());
foreach (Marker marker in markers)
{
marker.Draw(e.Graphics);
}
Pen windowsFunctionPen = new Pen(Color.Red, 2f);

nurb.Draw(e.Graphics);
}

private void pictureBox_MouseMove(object sender, MouseEventArgs e)
{
if (e.Button == MouseButtons.Left)
{
foreach (Marker marker in markers)
{
marker.MouseMove(e);
Thread.Sleep(0);
}
}
}

private void pictureBox_MouseDown(object sender, MouseEventArgs e)
{
foreach (Marker marker in markers)
{
marker.MouseDown(e);
}
}

private void pictureBox_MouseUp(object sender, MouseEventArgs e)
{
foreach (Marker marker in markers)
{
marker.MouseUp();
}
Cursor = Cursors.Arrow;
}

private void trackBar1_Scroll(object sender, System.EventArgs e)
{
nurb.w = trackBar1.Value;
Refresh();
}
}
}
