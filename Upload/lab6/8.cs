using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Wpf;
using System.Diagnostics;

namespace Lab
{
    /*
     * Хашимов Амир Азизович
     * М8О-307Б-20
     * Создать графическое приложение с использованием OpenGL. Используя результаты Л.Р.No5, изобразить заданное тело (то же, что и в л.р. No3) 
     * с использованием средств OpenGL 2.1. Использовать буфер вершин. Точность аппроксимации тела задается пользователем. Обеспечить возможность 
     * вращения и масштабирования многогранника и удаление невидимых линий и поверхностей. Реализовать простую модель освещения на GLSL.
     * Параметры освещения и отражающие свойства материала задаются пользователем в диалоговом режиме.
С помощью шейдеров реализовать анимацию вращение
     * Вариант 14. Вращение вокруг оси  OX
     */
    public partial class MainWindow : Window
    {
        public static MainWindow Value;
        private static readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        public MainWindow()
        {
            Value = this;
            DataContext = App.Container.Resolve<ViewModel>();
            InitializeComponent();
            var mainSettings = new GLWpfControlSettings { MajorVersion = 2, MinorVersion = 1 };
            OpenTkControl.Start(mainSettings);
            (DataContext as ViewModel).OnReady.Invoke();
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            (DataContext as ViewModel).MouseMoved?.Invoke(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        private void OpenTkControl_OnRender(TimeSpan obj)
        {
            (DataContext as ViewModel).OnRender?.Invoke(_stopwatch.Elapsed);
        }
    }
}
