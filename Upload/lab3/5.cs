using System;
using System.Drawing;
using System.Windows.Forms;

namespace lab3CG
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // точка света
            light = new MyPoint(1, 0, 0, 1);

            // двумерные матрицы для освещения
            iNofLight = new double[3, 3];
            kMat = new double[3, 3];

            // создание объекта класса Ellipsoid
            ellipsoid = new Ellipsoid(int.Parse(textBoxOfNumberOfCircles.Text), int.Parse(textBoxOfNumberOfParabols.Text));
            // передаём в конструктор число окружностей и число парабол

            // текущие координаты курсора и координаты его предыдущего положения
            mx = 0;
            my = 0;
            cx = 0;
            cy = 0;

            // масштаб
            scale = 30;
            mashtabK = 0;

            isMouseDown = false;
        }

        // объявление переменны
        int mx, my, cx, cy;
        float scale;
        double mashtabK;
        MyPoint light;
        double[,] iNofLight;
        double[,] kMat;
        bool isMouseDown;
        Ellipsoid ellipsoid;
 // задание координат освещения, диапозона цветов и построение графика
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // рассеянное
            kMat[0, 0] = 1;
            kMat[0, 1] = 1;
            kMat[0, 2] = 1;

            // фоновое
            kMat[1, 0] = 1;
            kMat[1, 1] = 1;
            kMat[1, 2] = 1;

            // зеркальное
            kMat[2, 0] = 1;
            kMat[2, 1] = 1;
            kMat[2, 2] = 1;

            // Освещение, диапозон цветов от начальных значений
            iNofLight[0, 0] = (double)buttonOfColor.BackColor.R * ((double)trackBarRas.Value / 5) / 255;
            iNofLight[0, 1] = (double)buttonOfColor.BackColor.G * ((double)trackBarRas.Value / 5) / 255;
            iNofLight[0, 2] = (double)buttonOfColor.BackColor.B * ((double)trackBarRas.Value / 5) / 255;

            iNofLight[1, 0] = (double)buttonOfColor.BackColor.R * ((double)trackBarFon.Value / 5) / 255;
            iNofLight[1, 1] = (double)buttonOfColor.BackColor.G * ((double)trackBarFon.Value / 5) / 255;
            iNofLight[1, 2] = (double)buttonOfColor.BackColor.B * ((double)trackBarFon.Value / 5) / 255;

            iNofLight[2, 0] = (double)buttonOfColor.BackColor.R * ((double)trackBarMir.Value / 5) / 255;
            iNofLight[2, 1] = (double)buttonOfColor.BackColor.G * ((double)trackBarMir.Value / 5) / 255;
            iNofLight[2, 2] = (double)buttonOfColor.BackColor.B * ((double)trackBarMir.Value / 5) / 255;


            Pen pen = new Pen(Color.Black, 1.0f);
            e.Graphics.DrawRectangle(new Pen(Color.Red, 3.0f), (int)light.x, -(int)light.y, 1, 1);

            // задание масштаба
            double zoom_level = (scale + mashtabK) / 1000;
            double coeff = Math.Max(e.ClipRectangle.Width, e.ClipRectangle.Height) * zoom_level;

            // матрица сдвига
            ShiftMatrix sh = new ShiftMatrix(e.ClipRectangle.Width / 2, e.ClipRectangle.Height / 2, 0);

            // матрица масштабирования
            ScalingMatrix sc = new ScalingMatrix(coeff, coeff, coeff);

            // создаём две матрицы поворота
            RotationMatrix rtx = new RotationMatrix('X', my * Math.PI / 180.0);
            RotationMatrix rty = new RotationMatrix('Y', -mx * Math.PI / 180.0);

            // матрица преобразования
            Matrix preobr = sh * rtx * rty * sc;

            // построение сектора эллипсоида
            ellipsoid.draw(preobr, pen, e.Graphics, light, iNofLight, kMat);
        }

        // передвижение мышкой
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                int delta_x = e.X - cx;
                int delta_y = e.Y - cy;
                mx += delta_x;
                my += delta_y;
                cx = e.X;
                cy = e.Y;

                Refresh();
            }
        }

        // остальные функции формы
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            cx = e.X;
            cy = e.Y;
        }
        private void checkBoxOfIlluminate_CheckedChanged(object sender, EventArgs e)
        {
            ellipsoid.illuminate = checkBoxOfIlluminate.Checked;
            panelOfLight.Enabled = checkBoxOfIlluminate.Checked;
            //panelOfLightSource.Enabled = checkBoxOfIlluminate.Checked;

            Refresh();
        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            Refresh();
        }
        private void buttonOfApply1_Click(object sender, EventArgs e)
        {
            ellipsoid.calcEllipsoid(int.Parse(textBoxOfNumberOfCircles.Text), int.Parse(textBoxOfNumberOfParabols.Text));

            Refresh();
        }
        private void buttonRight_Click(object sender, EventArgs e)
        {
            light.x += 10;

            Refresh();
        }
        private void buttonLeft_Click(object sender, EventArgs e)
        {
            light.x -= 10;

            Refresh();
        }
        private void buttonUp_Click(object sender, EventArgs e)
        {
            light.y += 10;

            Refresh();
        }
        private void buttonDown_Click(object sender, EventArgs e)
        {
            light.y -= 10;

            Refresh();
        }
        private void buttonNaNas_Click(object sender, EventArgs e)
        {
            light.z -= 10;

            Refresh();
        }
        private void buttonOtNas_Click(object sender, EventArgs e)
        {
            light.z += 10;

            Refresh();
        }
        private void buttonOfColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                buttonOfColor.BackColor = colorDialog1.Color; // выбор цвета диалога
                Refresh();
            }
        }
        private void trackBarFon_ValueChanged(object sender, EventArgs e)
        {
            Refresh();
        }
        private void trackBarRas_ValueChanged(object sender, EventArgs e)
        {
            Refresh();
        }
        private void trackBarMir_ValueChanged(object sender, EventArgs e)
        {
            Refresh();
        }
        private void buttonOfMatApply_Click(object sender, EventArgs e)
        {
            Refresh();
        }
        private void mashtabMinusButton_Click(object sender, EventArgs e)
        {
            mashtabK += 5;

            Refresh();
        }
        private void mashtabPlusButton_Click(object sender, EventArgs e)
        {
            mashtabK += 5;

            Refresh();}}}
