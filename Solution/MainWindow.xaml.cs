using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace Solution
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Point> Points = new List<Point>();
        public List<Path> Lines = new List<Path>();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainVM();
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var converter = TypeDescriptor.GetConverter(typeof(Geometry));
            POINT p = new POINT();
            Point pp = Mouse.GetPosition(e.Source as FrameworkElement);//WPF方法
            //Point ppp = (e.Source as FrameworkElement).PointToScreen(pp);//WPF方法
            if (GetCursorPos(out p))//API方法
            {
                //MessageBox.Show(string.Format("GetCursorPos {0},{1}  GetPosition {2},{3}\r\n {4},{5}", p.X, p.Y, pp.X, pp.Y, ppp.X, ppp.Y));

                if (Points.Count == 0)
                {
                    Path point = new Path();
                    point.Data = (Geometry)converter.ConvertFrom($"M {pp.X },{pp.Y} A 2,2 0 1 1 {pp.X },{pp.Y } Z");
                    point.Fill = Brushes.Blue;
                    //Grdrawing.Children.Add(point);
                    Points.Add(new Point(pp.X+2,pp.Y));
                }
                else
                {
                    Point last = Points.Last();
                    double XAbs = Math.Abs(last.X - pp.X);
                    double YAbs = Math.Abs(last.Y - pp.Y);
                    if (XAbs > YAbs)
                    {
                        Path point = new Path();
                        point.Data = (Geometry)converter.ConvertFrom($"M {pp.X },{last.Y} A 2,2 0 1 1 {pp.X },{last.Y } Z");
                        point.Fill = Brushes.Blue;
                        //Grdrawing.Children.Add(point);
                        Points.Add(new Point(pp.X,last.Y));

                        LineGeometry myLineGeometry = new LineGeometry();
                        myLineGeometry.StartPoint = last;
                        myLineGeometry.EndPoint = new Point(pp.X , last.Y);

                        Path line= new Path();
                        line.Data = myLineGeometry;
                        line.Stroke = Brushes.Black;
                        line.StrokeThickness = 1;
                        Grdrawing.Children.Add(line);
                        Lines.Add(line);
                    }
                    else
                    {
                        Path point = new Path();
                        point.Data = (Geometry)converter.ConvertFrom($"M {last.X },{pp.Y} A 2,2 0 1 1 {last.X },{pp.Y } Z");
                        point.Fill = Brushes.Blue;
                        //Grdrawing.Children.Add(point);
                        Points.Add(new Point(last.X,pp.Y));

                        LineGeometry myLineGeometry = new LineGeometry();
                        myLineGeometry.StartPoint = last;
                        myLineGeometry.EndPoint = new Point(last.X , pp.Y);

                        Path line = new Path();
                        line.Data = myLineGeometry;
                        line.Stroke = Brushes.Black;
                        line.StrokeThickness = 1;
                        Grdrawing.Children.Add(line);
                        Lines.Add(line);
                    }
                }

            }
        }

        ///<summary>   
        /// 设置鼠标的坐标   
        /// </summary>   
        /// <param name="x">横坐标</param>   
        /// <param name="y">纵坐标</param>   
        [DllImport("User32")]
        public extern static void SetCursorPos(int x, int y);
        public struct POINT
        {
            public int X;
            public int Y;
            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        /// <summary>   
        /// 获取鼠标的坐标   
        /// </summary>   
        /// <param name="lpPoint">传址参数，坐标point类型</param>   
        /// <returns>获取成功返回真</returns>   
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out POINT pt);
    }
}
