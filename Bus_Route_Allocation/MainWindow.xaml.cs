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
using System.Text.RegularExpressions;

namespace Bus_Route_Allocation
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       

        Dictionary<string, Station> Stations;
        Dictionary<string, double> Waiting_time;
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            Stations = new Dictionary<string, Station>();
            Waiting_time = new Dictionary<string, double>();

            string[] data = System.IO.File.ReadAllLines(@"C:\Users\Danila\source\repos\Bus_Route_Allocation\Routes.txt");
            foreach (var s in data)
            {
                var d = s.Split(',');
                if (Stations.ContainsKey(d[0]) == false)
                {
                    Station station = new Station();
                    station.Name = d[0];
                    station.neighbours = new List<(string, string, double)>();
                    station.neighbours.Add((d[1], d[2], double.Parse(d[3])));
                    Stations.Add(d[0], station);
                }
                else
                {
                    Stations[d[0]].neighbours.Add((d[1], d[2], double.Parse(d[3])));
                }               
            }

            data = System.IO.File.ReadAllLines(@"C:\Users\Danila\source\repos\Bus_Route_Allocation\Waiting time.txt");
            foreach (var s in data)
            {
                Console.WriteLine(s);
                var d = s.Split(',');
                Waiting_time.Add(d[0], double.Parse(d[1]));                                    
            }
        }

        public List<string> Common_bus(Station st1, Station st2)
        {
            var answ = new List<string>();
            foreach((string bus, string n, double time) in st1.neighbours)
            {
                foreach ((string bus2, string n2, double time2) in st2.neighbours)
                {
                    if ( (n == st2.Name) && (n2 ==st1.Name))
                    {
                        if (bus == bus2)
                        {
                            answ.Add(bus);
                        }
                    }
                }
            }
            return answ;
        }

        private void Button_click(object sender, RoutedEventArgs e)
        {
            string str1 = form.Text;
            string str2 = form2.Text;
            string str11 = str1 = Regex.Replace(str1.ToLower(), @"[^а-я]+", "");
            string str21 = str2 = Regex.Replace(str2.ToLower(), @"[^а-я]+", "");


            Regex ex1 = new Regex(".*" + str1 + ".*");
            Regex ex2 = new Regex(".*" + str2 + ".*");
            bool f1 = false;
            bool f2 = false;

            foreach (var k in Stations.Keys)
            {
                if (ex1.IsMatch(k.ToLower()))
                {
                    f1 = true;
                    str1 = k;
                }
                if (ex2.IsMatch(k.ToLower()))
                {
                    f2 = true;
                    str2 = k;
                }
            }
            if (f1 == false || f2 == false || str11.Length == 0 || str21.Length == 0)
            {
                TextBlock tx = new TextBlock()
                {
                    Text = "Таких станций не существует",
                    Foreground = Brushes.Gray,
                    FontSize = 15,
                };
                cv.Children.Add(tx);
                tx.SetValue(Canvas.LeftProperty, cv.Width / 2);
                tx.SetValue(Canvas.TopProperty, (double)6);
                return;
            }
            
            (var path, var time) = Find_path.Route(str1, str2,
                                                   Stations, Waiting_time);
            string prev = str1;
            Console.WriteLine(time);
            cv.Children.Clear();
            int ofset = 25;
            Ellipse cir = new Ellipse()
            {
                Width = 15,
                Height = 15,
                Fill = Brushes.Blue
            };
            cv.Children.Add(cir);
            cir.SetValue(Canvas.LeftProperty, (double)10);
            cir.SetValue(Canvas.TopProperty, (double)10);
            TextBlock t = new TextBlock()
            {
                Text = str1,
                Foreground = Brushes.Black,               
                FontSize = 15,
            };
            cv.Children.Add(t);
            t.SetValue(Canvas.LeftProperty, (double)10 + 30);
            t.SetValue(Canvas.TopProperty, (double)6 );

            foreach (var s  in path)
            {

                t = new TextBlock()
                {
                    Text = String.Join(", ", Common_bus(Stations[prev],Stations[s]).ToArray()),
                    Foreground = Brushes.Black,
                    FontSize = 15,
                };
                cv.Children.Add(t);
                t.SetValue(Canvas.LeftProperty, (double)13);
                t.SetValue(Canvas.TopProperty, (double)10 + ofset - 4);
                prev = s;
                ofset += 25;

                Ellipse cirl = new Ellipse()
                {
                    Width = 15,
                    Height = 15,
                    Fill = Brushes.Blue
                };
                cv.Children.Add(cirl);
                cirl.SetValue(Canvas.LeftProperty, (double)10);
                cirl.SetValue(Canvas.TopProperty, (double)10 + ofset);
                t = new TextBlock()
                {
                    Text = s,
                    Foreground = Brushes.Black,
                    FontSize = 15,
                };
                cv.Children.Add(t);
                t.SetValue(Canvas.LeftProperty, (double)10 + 30);
                t.SetValue(Canvas.TopProperty, (double)10 + ofset - 4);           

                ofset += 25;
            }
            t = new TextBlock()
            {
                Text = "В среднем " + time.ToString() + " минут",
                Foreground = Brushes.Gray,
                FontSize = 15,
            };
            cv.Children.Add(t);
            t.SetValue(Canvas.LeftProperty, (double)10);
            t.SetValue(Canvas.TopProperty, (double)10 + ofset - 4);
        }

         
    }
}
