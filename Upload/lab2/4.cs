using KG1;
using System;
using System.Drawing;
using System.Windows.Forms;
namespace CG2 {
    public partial class MainForm : Form {
        int a = 50; // ребро пятиугольника
        int h = 70; // высота призмы
        Pen axispen = new Pen(Color.Black);
        Pen penX = new Pen(Color.Brown, 2);
        Pen penY = new Pen(Color.DarkGreen, 2);
        Pen penZ = new Pen(Color.Blue, 2);
        Vertex[] vertices = new Vertex[10];
        Edge[] edges = new Edge[15];
        Face[] faces = new Face[7];

        public MainForm() {
            InitializeComponent();
            Init();
        }
        private void Init() {
            // инициализация вершин
            vertices[0] = new Vertex(a/2 / Math.Sin(0.8), 0, -h/2, 1);
            vertices[5] = new Vertex(a/2 / Math.Sin(0.8), 0, h/2, 1);
            double[,] tmpforMatrix = { { Math.Cos(1.26), -Math.Sin(1.26), 0, 0},
                                       { Math.Sin(1.26), Math.Cos(1.26), 0, 0},
                                       { 0, 0, 1, 0},
                                       { 0, 0, 0, 1} };
            Matrix matrix = new Matrix(4, 4, tmpforMatrix);
            // вершины нижней грани
            for (int i = 1; i < 5; i++) {
                vertices[i] = new Vertex(matrix * vertices[i - 1].Data);
            }
            // вершины верхней грани
            for (int i = 6; i < 10; i++) {
                vertices[i] = new Vertex(matrix * vertices[i - 1].Data);
            }
            // инициализация рёбер
            for (int i = 0; i < 4; i++) edges[i] = new Edge(vertices[i], vertices[i + 1]);
            edges[4] = new Edge(vertices[4], vertices[0]);
            for (int i = 5; i < 9; i++) edges[i] = new Edge(vertices[i], vertices[i + 1]);
            edges[9] = new Edge(vertices[9], vertices[5]);
            for (int i = 0; i < 5; i++) edges[i + 10] = new Edge(vertices[i], vertices[i + 5]);
            // инициализация граней
            for (int i = 0; i < 4; i++) {
                Edge[] tmp = { edges[i], edges[i + 10], edges[i + 5], edges[i + 11] };
                faces[i] = new Face(tmp);
            }
            Edge[] face4 = { edges[4], edges[10], edges[9], edges[14] };
            faces[4] = new Face(face4);
            Edge[] face5 = { edges[0], edges[4], edges[3], edges[2], edges[1] };
            faces[5] = new Face(face5);
            Edge[] face6 = { edges[5], edges[6], edges[7], edges[8], edges[9] };
            faces[6] = new Face(face6);
        }
// проверка видимости грани
        private bool IsFaceVisible(Face face, Vertex normal) { // normal - нормаль проекции
            double tmp = 0;
            Vertex vertex1 = new Vertex(face[0][1].Data - face[0][0].Data);
            Vertex vertex2 = new Vertex(face[1][1].Data - face[1][0].Data);
            // вектор-нормаль грани
            Vertex face_normal = new Vertex( vertex1[1]*vertex2[2] - vertex1[2]*vertex2[1],
                                             vertex1[2]*vertex2[0] - vertex1[0]*vertex2[2],
                                             vertex1[0]*vertex2[1] - vertex1[1]*vertex2[0],
                                             1 );
            // скалярное произведение
            for (int i = 0; i < 3; i++) {
                tmp += face_normal[i] * normal[i];
            }
            if (tmp > 0)
                return true;
            else return false;
        }
        // перерисовка многогранника в проекции z = 0
        private void pictureBox2_Paint(object sender, PaintEventArgs e) {
            float dx = pictureBox2.Width / 2;
            float dy = pictureBox2.Height / 2;
            float min = Math.Min(dx, dy);
            // коэффициенты масштабирования
            double kx = pictureBox2.Width / (1.2*Math.Max(1.8*a, h));
            double ky = pictureBox2.Height / (1.2*Math.Max(1.8*a, h));
            double kmin = Math.Min(kx, ky);

            // соблюдать ли пропорцию между осями
            if (radioButton1.Checked == true) {
                dx = min;
                dy = min;
                kx = kmin;
                ky = kmin;
            }
            // отрисовка осей
            e.Graphics.DrawLine(axispen, 0, dy, pictureBox2.Width, dy);
            e.Graphics.DrawLine(axispen, dx, 0, dx, pictureBox2.Height);
            e.Graphics.DrawString("X", new Font("Arial", 14), Brushes.Black, dx - 20, pictureBox2.Height - 25);
            e.Graphics.DrawString("Y", new Font("Arial", 14), Brushes.Black, pictureBox2.Width - 20, dy + 5);
            // отрисовка видимых граней
            foreach (Face face in faces) {
                if (IsFaceVisible(face, new Vertex(0, 0, 1, 1)) == true) {
                    for (int i = 0; i < face.Edges_count; i++) {
                        e.Graphics.DrawLine(penZ, dx + (float)(face[i][0][1]*kx), dy + (float)(face[i][0][0]*ky),
                                                dx + (float)(face[i][1][1]*kx), dy + (float)(face[i][1][0]*ky));
                    }
                }
            }
        }
        // Обработка поворотов мышью
        private bool click2 = false;
        Point mousePos2;
        private void pictureBox2_MouseDown(object sender, MouseEventArgs e) {
            click2 = true;
            mousePos2 = e.Location;
        }
        private void pictureBox2_MouseUp(object sender, MouseEventArgs e) {
            if (click2 == true) {
                float dx = mousePos2.X - e.Location.X;
                float dy = mousePos2.Y - e.Location.Y;
                TurnPolyhedron(dx/200, dy/200, 0);
            }
            click2 = false;
        }
        //
        //
        // перерисовка многогранника в проекции x = 0
        private void pictureBox3_Paint(object sender, PaintEventArgs e) {
            float dx = pictureBox3.Width/2;
            float dy = pictureBox3.Height/2;
            float min = Math.Min(dx, dy);
            // коэффициенты масштабирования
            double kx = pictureBox3.Width / (1.2*Math.Max(1.8*a, h));
            double ky = pictureBox3.Height / (1.2 * Math.Max(1.8*a, h));
            double kmin = Math.Min(kx, ky);

            // соблюдать ли пропорцию между осями
            if (radioButton1.Checked == true) {
                dx = min;
                dy = min;
                kx = kmin;
                ky = kmin;
            }
            // отрисовка осей
            e.Graphics.DrawLine(axispen, 0, dy, pictureBox3.Width, dy);
            e.Graphics.DrawLine(axispen, dx, 0, dx, pictureBox3.Height);
            e.Graphics.DrawString("Y", new Font("Arial", 14), Brushes.Black, pictureBox2.Width - 20, dy + 5);
            e.Graphics.DrawString("Z", new Font("Arial", 14), Brushes.Black, dx - 20, 3);
            // отрисовка видимых граней
            foreach (Face face in faces) {
                if (IsFaceVisible(face, new Vertex(1, 0, 0, 1)) == true) {
                    for (int i = 0; i < face.Edges_count; i++) {
                        e.Graphics.DrawLine(penX, dx + (float)(face[i][0][1]*kx), dy + -(float)(face[i][0][2]*ky),
                                                dx + (float)(face[i][1][1]*kx), dy + -(float)(face[i][1][2]*ky));
                    }
                }
            }
        }
        // Обработка поворотов мышью
        private bool click3 = false;
        Point mousePos3;
        private void pictureBox3_MouseDown(object sender, MouseEventArgs e) {
            click3 = true;
            mousePos3 = e.Location;
        }
        private void pictureBox3_MouseUp(object sender, MouseEventArgs e) {
            if (click3 == true) {
                float dz = e.Location.X - mousePos3.X;
                float dy = mousePos3.Y - e.Location.Y;
                TurnPolyhedron(0, dy/200, dz/200);
            }
            click3 = false;
        }
        //
        //
        // перерисовка многогранника в проекции y = 0
        private void pictureBox4_Paint(object sender, PaintEventArgs e) {
            float dx = pictureBox4.Width / 2;
            float dy = pictureBox4.Height / 2;
            float min = Math.Min(dx, dy);
            // коэффициенты масштабирования
            double kx = pictureBox4.Width / (1.2*Math.Max(1.8*a, h));
            double ky = pictureBox4.Height / (1.2*Math.Max(1.8*a, h));
            double kmin = Math.Min(kx, ky);

            // соблюдать ли пропорцию между осями
            if (radioButton1.Checked == true) {
                dx = min;
                dy = min;
                kx = kmin;
                ky = kmin;
            }
            // отрисовка осей
            e.Graphics.DrawLine(axispen, 0, dy, pictureBox4.Width, dy);
            e.Graphics.DrawLine(axispen, dx, 0, dx, pictureBox4.Height);
            e.Graphics.DrawString("X", new Font("Arial", 14), Brushes.Black, 3, dy + 5);
            e.Graphics.DrawString("Z", new Font("Arial", 14), Brushes.Black, dx - 20, 3);
            // отрисовка видимых граней
            foreach (Face face in faces) {
                if (IsFaceVisible(face, new Vertex(0, 1, 0, 1)) == true) {
                    for (int i = 0; i < face.Edges_count; i++) {
                        e.Graphics.DrawLine(penY, dx + -(float)(face[i][0][0]*kx), dy + -(float)(face[i][0][2]*ky),
                                                dx + -(float)(face[i][1][0]*kx), dy + -(float)(face[i][1][2]*ky));
                    }
                }
            }
        }
        // Обработка поворотов мышью
        private bool click4 = false;
        Point mousePos4;
        private void pictureBox4_MouseDown(object sender, MouseEventArgs e) {
            click4 = true;
            mousePos4 = e.Location;
        }
        private void pictureBox4_MouseUp(object sender, MouseEventArgs e) {
            if (click4 == true) {
                float dz = e.Location.X - mousePos4.X;
                float dx = mousePos4.Y - e.Location.Y;
                TurnPolyhedron(dx/200, 0, dz/200);
            }
            click4 = false;
        }
        // перерисовка всех проекций при изменении формы окна
        private void MainForm_SizeChanged(object sender, EventArgs e) {
            pictureBox2.Invalidate();
            pictureBox3.Invalidate();
            pictureBox4.Invalidate();
        }
        // повернуть многогранник
        private void TurnPolyhedron(float phiX, float phiY, float phiZ) {
            double[,] turnX = { { 1, 0, 0, 0},
                                { 0, Math.Cos(phiX), -Math.Sin(phiX), 0},
                                { 0, Math.Sin(phiX), Math.Cos(phiX), 0},
                                { 0, 0, 0, 1} };
            Matrix turn_matrixX = new Matrix(4, 4, turnX);
            double[,] turnY = { { Math.Cos(phiY), 0, -Math.Sin(phiY), 0},
                                { 0, 1, 0, 0},
                                { Math.Sin(phiY), 0, Math.Cos(phiY), 0},
                                { 0, 0, 0, 1} };
            Matrix turn_matrixY = new Matrix(4, 4, turnY);
            double[,] turnZ = { { Math.Cos(phiZ), -Math.Sin(phiZ), 0, 0},
                                { Math.Sin(phiZ), Math.Cos(phiZ), 0, 0},
                                { 0, 0, 1, 0},
                                { 0, 0, 0, 1} };
            Matrix turn_matrixZ = new Matrix(4, 4, turnZ);
            foreach (Vertex v in vertices) {
                v.Transform(turn_matrixX);
                v.Transform(turn_matrixY);
                v.Transform(turn_matrixZ);
            }
            pictureBox2.Invalidate();
            pictureBox3.Invalidate();
            pictureBox4.Invalidate();
        }
        // Изменить параметры
        private void button1_Click(object sender, EventArgs e) {
            if (Int32.TryParse(textBox1.Text, out a) == false)
                a = 50;
            else if (a <= 1) a = Math.Abs(a);
            if (Int32.TryParse(textBox2.Text, out h) == false)
                h = 70;
            else if (h <= 1) h = Math.Abs(h);
            Init();
            pictureBox2.Invalidate();
            pictureBox3.Invalidate();
            pictureBox4.Invalidate();
        }
    }
}

namespace CG2 { // Грань
    public class Face {
        private int edges_count = 0;
        public int Edges_count { get { return edges_count; } }
        private Edge[] edges;
        
        public Face(Edge[] data) {
            edges_count = data.Length;
            edges = data;
        }
        public Edge this[int index] {
            get {
                if (edges_count != 0)
                    return edges[index];
                else return null;
            }
        }
    }
}

using System;
namespace CG2 { // Ребро
    public class Edge { 
        private Vertex[] data = new Vertex[2];

        public Edge(Vertex vertex1, Vertex vertex2) {
            data[0] = vertex1;
            data[1] = vertex2;
        }
        public Vertex this[int index] {
            get {
                if (data[0] != null && data[1] != null) {
                    switch (index) {
                        case 0: return data[0];
                        case 1: return data[1];
                        default: throw new IndexOutOfRangeException();
                    }
                }
                else throw new NullReferenceException();
            }
        }
    }
}
Vertex.cs
using KG1;
namespace CG2 { // Вершина
    public class Vertex {
        private Matrix data;
        public Matrix Data { get { return data.Copy(); } }
        private int dim = 0;
        public int Dim { get { return dim; } }
        private double[,] tmp;

        public Vertex(double[] data) {
            this.dim = data.Length;
            int i = 0;
            tmp = new double[dim, 1];
            foreach (double val in data) {
                tmp[i, 0] = val;
                i++;
            }
            this.data = new Matrix(dim, 1, tmp);
        }

        public Vertex(Matrix matrix) {
            if (matrix.GetCountOfColumns() == 1) {
                this.dim = matrix.GetCountOfRows();
                data = matrix;
            } else {
                throw new MyExceptions.VertexCreationException();
            }
        }
        public Vertex(double x, double y, double z, double h) {
            dim = 4;
            tmp = new double[4, 1];
            tmp[0, 0] = x;
            tmp[1, 0] = y;
            tmp[2, 0] = z;
            tmp[3, 0] = h;
            data = new Matrix(4, 1, tmp);
        }
        public double this[int index] { 
            get {
                if (dim != 0 && index < dim)
                    return data[index, 0];
                else return 0;
            }
        }
        // преобразование вершины
        public void Transform(Matrix matrix) { 
            if (matrix.GetCountOfColumns() == dim &&
                matrix.GetCountOfRows() == dim) {
                data = matrix * data;
            }
        }
    }
}

using System;
namespace KG1 {
    public class Matrix {
        // класс, реализующий объекты - матрицы
        private int rows_count = 0;
        private int columns_count = 0;
        // data - массив элементов матрицы
        private double[,] data;
        public double[,] Data {
            get {
                if (rows_count > 0 && columns_count > 0) {
                    return CopyData();
                } else return null;
            }
        }
        public Matrix(int m, int n, double[,] data) {
            // нет проверки размерности data
            rows_count = m;
            columns_count = n;
            this.data = new double[m, n];
            for (int i = 0; i < m; i++) {
                for (int j = 0; j < n; j++) {
                    this.data[i, j] = data[i, j];
                }
            }
        }
        public double this[int index1, int index2] { 
            get {
                if (index1 >= 0 && index1 < rows_count &&
                    index2 >= 0 && index2 < columns_count)
                {
                    return data[index1, index2];
                }
                else throw new IndexOutOfRangeException();
            }
        }

        public int GetCountOfRows() { return rows_count; }
        public int GetCountOfColumns() { return columns_count; }

        public void PrintMatrix() {
            // распечатка матрицы
            Console.WriteLine("\nМатрица: ");
            for (int i = 0; i < rows_count; i++) {
                for (int j = 0; j < columns_count; j++) {
                    Console.Write("{0} ", data[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        // копия содержимого матрицы
        private double[,] CopyData() {
            double[,] tmp = new double[rows_count, columns_count];
            for (int i = 0; i < rows_count; i++)
                for (int j = 0; j < columns_count; j++)
                    tmp[i, j] = data[i, j];
            return tmp;
        }
        // копия матрицы
        public Matrix Copy() {
            return new Matrix(rows_count, columns_count, CopyData());
        }
        // перегрузка оператора сложения
        public static Matrix operator +(Matrix l, Matrix r) {
            if (l.rows_count != r.rows_count ||
                l.columns_count != r.columns_count) {
                throw new MyExceptions.MatrixSumException();
            }
            else {
                double[,] tmp = new double[l.rows_count, l.columns_count];
                for (int i = 0; i < l.rows_count; i++) {
                    for (int j = 0; j < l.columns_count; j++) {
                        tmp[i, j] = l.data[i, j] + r.data[i, j];
                    }
                }
                Matrix matrix = new Matrix(l.rows_count, l.columns_count, tmp);
                return matrix;
            }
        }
        // перегрузка оператора вычитания
        public static Matrix operator -(Matrix l, Matrix r) {
            if (l.rows_count != r.rows_count ||
                l.columns_count != r.columns_count){
                throw new MyExceptions.MatrixSumException();
            }
            else {
                double[,] tmp = new double[l.rows_count, l.columns_count];
                for (int i = 0; i < l.rows_count; i++) {
                    for (int j = 0; j < l.columns_count; j++) {
                        tmp[i, j] = l.data[i, j] - r.data[i, j];
                    }
                }
                Matrix matrix = new Matrix(l.rows_count, l.columns_count, tmp);
                return matrix;
            }
        }
        // перегрузка оператора умножения
        public static Matrix operator *(Matrix l, Matrix r) {
            if (l.columns_count != r.rows_count) throw new MyExceptions.MatrixMultException();
            else {
                double[,] tmp = new double[l.rows_count, r.columns_count];
                for (int i = 0; i < l.rows_count; i++)
                    for (int j = 0; j < r.columns_count; j++) {
                        tmp[i, j] = 0;
                        for (int k = 0; k < l.columns_count; k++)
                            tmp[i, j] += l.data[i, k] * r.data[k, j];
                    }
                Matrix matrix = new Matrix(l.rows_count, r.columns_count, tmp);
                return matrix;
            }
        }
    }
}

using System;
namespace CG2.MyExceptions {
    class VertexCreationException : Exception {
        public VertexCreationException() : base("Невозможно создать вершину:" +
                                                " она должна представлять собой" +
                                                " столбец Nx1.") { }
    }
}

using System;
namespace KG1.MyExceptions {
    // исключение, вызываемое при невозможности сложить матрицы
    public class MatrixSumException: Exception {
        public MatrixSumException() : base("Матрицы невозможно сложить," +
                                             " проверьте их размерности.") { }
    }
}

using System;
namespace KG1.MyExceptions {
    // исключение, вызываемое при невозможности перемножения матриц
    public class MatrixMultException : Exception {
        public MatrixMultException() : base("Матрицы невозможно перемножить," +
                                             " проверьте их размерности.") { }
    }
}
