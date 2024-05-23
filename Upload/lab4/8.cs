using Autofac;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using OpenTK.Wpf;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;

namespace Lab
{
    /*
     * Хашимов Амир Азизович
     * М8О-307Б-20
     * Создать графическое приложение с использованием OpenGL. Используя результаты Л.Р.No3, изобразить заданное тело (то же, что и в л.р. No3) 
     * с использованием средств OpenGL 2.1. Использовать буфер вершин. Точность аппроксимации тела задается пользователем. Обеспечить возможность 
     * вращения и масштабирования многогранника и удаление невидимых линий и поверхностей. Реализовать простую модель освещения на GLSL.
     * Параметры освещения и отражающие свойства материала задаются пользователем в диалоговом режиме.
     * Вариант 3. Шар
     */
    public partial class MainWindow : Window
    {
        public static MainWindow Value;
        private static readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        private ViewModel VM { get; set; }

        public MainWindow()
        {
            Value = this;
            VM = App.Container.Resolve<ViewModel>();
            DataContext = VM;
            InitializeComponent();
            var mainSettings = new GLWpfControlSettings { MajorVersion = 2, MinorVersion = 1 };
            OpenTkControl.Start(mainSettings);
            VM.OnReady.Invoke();
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            VM.MouseMoved?.Invoke(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        private void OpenTkControl_OnRender(TimeSpan obj)
        {
            VM.OnRender?.Invoke();
        }
        private bool isCaptured = false;


        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isCaptured && e.LeftButton == MouseButtonState.Pressed)
            {
                if (sender == bulb)
                {
                    VM.LightPositionX = (int)(e.GetPosition(field).X - bulb.ActualWidth / 2);
                    VM.LightPositionY = (int)(e.GetPosition(field).Y - bulb.ActualHeight / 2);
                    VM.OnPropertyChanged(nameof(VM.LightPositionX));
                    VM.OnPropertyChanged(nameof(VM.LightPositionY));
                }
                else
                {
                    VM.MouseMoved(e);
                }
            }

            isCaptured = e.LeftButton == MouseButtonState.Pressed;
        }

        private void bulb_MouseLeave(object sender, MouseEventArgs e)
        {
            if (isCaptured && e.LeftButton == MouseButtonState.Pressed)
            {
                Canvas.SetLeft(bulb, e.GetPosition(field).X - bulb.Width / 2);
                Canvas.SetTop(bulb, e.GetPosition(field).Y - bulb.Height / 2);
            }
        }

        private void bulb_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                var dialog = new System.Windows.Forms.ColorDialog();
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    VM.LightColor = dialog.Color;
                    VM.OnPropertyChanged(nameof(VM.LightColor));
                }
            }
            isCaptured = true;
        }

        private void field_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            VM.FieldWidth = field.ActualWidth;
            VM.FieldHeight = field.ActualHeight;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.SaveFileDialog())
            {
                dialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
                dialog.DefaultExt = "xml";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(ViewModel));

                    using (FileStream fs = new FileStream(dialog.FileName, FileMode.OpenOrCreate))
                    {
                        xmlSerializer.Serialize(fs, VM);
                    }
                }
            }
        }
        
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                dialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
                dialog.DefaultExt = "xml";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(ViewModel));

                    using (FileStream fs = new FileStream(dialog.FileName, FileMode.OpenOrCreate))
                    {
                        ViewModel fase = xmlSerializer.Deserialize(fs) as ViewModel;
                        if (fase != null)
                        {
                            fase.LightPositionX = (int)(fase.LightPositionX * field.ActualWidth / fase.FieldWidth);
                            fase.LightPositionY = (int)(fase.LightPositionY * field.ActualHeight / fase.FieldHeight);
                            VM.Constuct(fase);
                        }
                    }

                }               
            }
        }
    }
}
