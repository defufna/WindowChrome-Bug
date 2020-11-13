using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Shell;

namespace WindowChromeBug
{
    class Program
    {
        static Window CreateWindow(string name, ConsoleColor color)
        {
            Window w = new Window();
            w.Title = name;
            w.Foreground = (color == ConsoleColor.Red) ? Brushes.Red : Brushes.Green;

            w.Width = 400;
            w.Height = 300;

            Label label = new Label();
            label.Content = name;

            Canvas canvas = new Canvas();
            canvas.Children.Add(label);

            Ellipse elipse = new Ellipse();
            elipse.Width = 300;
            elipse.Height = 200;
            elipse.Fill = w.Foreground;

            elipse.MouseMove += ElipseMouseMove;
            canvas.Children.Add(elipse);
            Canvas.SetTop(elipse, 40);

            w.Content = canvas;

            WindowChrome.SetWindowChrome(w, new WindowChrome());

            w.MouseMove += (s, e) =>
            {
                Console.ForegroundColor = color;
                Console.WriteLine("{0} {1}: Mouse Move! {2}", name, DateTime.Now.ToString("HH:mm:ss:ff"), e.GetPosition(null));
                Console.ForegroundColor = ConsoleColor.Gray;
            };
            return w;
        }

        private static void ElipseMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Ellipse ellipse = (Ellipse)sender;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(ellipse, ellipse.Fill.ToString(), DragDropEffects.Copy);
            }
        }

        [System.STAThreadAttribute()]
        static void Main(string[] args)
        {
            Application app = new Application();

            Window w1 = CreateWindow("Window 1", ConsoleColor.Red);
            w1.Show();
            Window w2 = CreateWindow("Window 2", ConsoleColor.Green);
            w2.Show();

            app.Run();
        }
    }
}
