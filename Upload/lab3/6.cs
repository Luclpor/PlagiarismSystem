using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
 
namespace lab3CG
{
    class Ellipsoid
    {
        public Ellipsoid(int NumberOfCircles, int NumberOfParabols) 
        {
            triangleList = new List<Triangle>();
            pointsList = new List<MyPoint>();
            calcEllipsoid(NumberOfCircles, NumberOfParabols);
 
            illuminate = false;
        }
 
        public void calcEllipsoid(int NumberOfCircles, int NumberOfParabols)
        {
            triangleList.Clear();
            double t0 = 0, tn = 3, dt = (tn - t0) / NumberOfCircles;
            List<MyPoint> points = new List<MyPoint>();
 
            List<MyPoint> temp = new List<MyPoint>();
            for (int i = 0; i < NumberOfCircles; ++i)
            {
                double x = t0 + i * dt;
                double z = Math.Sqrt(9 - x * x);
                points.Add(new MyPoint(x, 0, -z));
                //temp.Add(new MyPoint(x, 0, -z));
            }
 
            //points.AddRange(temp);
 
            double u0 = 0, un = 2 * Math.PI, du = (un - u0) / NumberOfParabols;
            for (int j = 0; j < NumberOfParabols; ++j)
            {
                List<MyPoint> cur_points = new List<MyPoint>();
                List<MyPoint> next_points = new List<MyPoint>();
 
                double angle = u0 + j * du;
                double next_angle = u0 + (j + 1) * du;
                RotationMatrix r = new RotationMatrix('Z', angle);
                RotationMatrix rr = new RotationMatrix('Z', next_angle);
                for (int i = 0; i < NumberOfCircles; ++i)
                {
                    cur_points.Add(r * points[i]);
                    if (j == NumberOfParabols - 1)
                    {
                        next_points.Add(points[i]);
                        pointsList.Add(points[i]);
                    }
                    else
                    {
                        next_points.Add(rr * points[i]);
                        pointsList.Add(rr * points[i]);
                    }
                }
 
                int nn = cur_points.Count;
                for (int i = 0; i < nn - 1; ++i)
                {
                    Triangle a = new Triangle(cur_points[i], next_points[i + 1], cur_points[i + 1]);
                    Triangle b = new Triangle(cur_points[i], next_points[i], next_points[i + 1]);
                    triangleList.Add(a);
                    triangleList.Add(b);
                }
 
                MyPoint center = new MyPoint(0, 0, 0);
                Triangle top = new Triangle(cur_points[nn - 1], next_points[nn - 1], center);
                triangleList.Add(top);
            }
        }
 
        public void draw(Matrix preobr, Pen pen, Graphics g, MyPoint svet, double[,] iNofLight, double[,] kMat)
        {
            for (int i = 0; i < triangleList.Count; ++i)
            {
                Triangle t = preobr * triangleList[i];
                t.draw(pen, g, illuminate, svet, iNofLight, kMat);
            }
        }
 
        private List<Triangle> triangleList;
        private List<MyPoint> pointsList;
        public bool illuminate;
    }
}

Form1.cs
private void Form1_Paint(object sender, PaintEventArgs e)
        {
            iNofLight[0, 0] = (double)buttonOfColor.BackColor.R * ((double)trackBarRas.Value / 5) / 255;
            iNofLight[0, 1] = (double)buttonOfColor.BackColor.G * ((double)trackBarRas.Value / 5) / 255;
            iNofLight[0, 2] = (double)buttonOfColor.BackColor.B * ((double)trackBarRas.Value / 5) / 255;
 
            iNofLight[1, 0] = (double)buttonOfColor.BackColor.R * ((double)trackBarFon.Value / 5) / 255;
            iNofLight[1, 1] = (double)buttonOfColor.BackColor.G * ((double)trackBarFon.Value / 5) / 255;
            iNofLight[1, 2] = (double)buttonOfColor.BackColor.B * ((double)trackBarFon.Value / 5) / 255;
 
            iNofLight[2, 0] = (double)buttonOfColor.BackColor.R * ((double)trackBarMir.Value / 5) / 255;
            iNofLight[2, 1] = (double)buttonOfColor.BackColor.G * ((double)trackBarMir.Value / 5) / 255;
            iNofLight[2, 2] = (double)buttonOfColor.BackColor.B * ((double)trackBarMir.Value / 5) / 255;
 
 
            kMat[0, 0] = double.Parse(textBoxRasR.Text) / 10;
            kMat[0, 1] = double.Parse(textBoxRasG.Text) / 10;
            kMat[0, 2] = double.Parse(textBoxRasB.Text) / 10;
 
            kMat[1, 0] = double.Parse(textBoxFonR.Text) / 10;
            kMat[1, 1] = double.Parse(textBoxFonG.Text) / 10;
            kMat[1, 2] = double.Parse(textBoxFonB.Text) / 10;
 
            kMat[2, 0] = double.Parse(textBoxMirR.Text) / 10;
            kMat[2, 1] = double.Parse(textBoxMirG.Text) / 10;
            kMat[2, 2] = double.Parse(textBoxMirB.Text) / 10;
 
 
            Pen pen = new Pen(Color.Black, 1.0f);
            e.Graphics.DrawRectangle(new Pen(Color.Red, 3.0f), (int)light.x, - (int)light.y, 1, 1);
 
            double zoom_level = (scale + mashtabK) / 1000;
            double coeff = Math.Max(e.ClipRectangle.Width, e.ClipRectangle.Height) * zoom_level;
 
            ShiftMatrix sh = new ShiftMatrix(e.ClipRectangle.Width / 2, e.ClipRectangle.Height / 2, 0);
            ScalingMatrix sc = new ScalingMatrix(coeff, coeff, coeff);
            RotationMatrix rtx = new RotationMatrix('X', my * Math.PI / 180.0);
            RotationMatrix rty = new RotationMatrix('Y', -mx * Math.PI / 180.0);
            Matrix preobr = sh * rtx * rty * sc;
 
            ellipsoid.draw(preobr, pen, e.Graphics, light, iNofLight, kMat);
        }