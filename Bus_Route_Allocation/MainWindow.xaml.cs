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

        private void Button_click(object sender, RoutedEventArgs e)
        {
           (var path, var time) = Find_path.Route("Южные ворота","Кампус ДВФУ",Stations,Waiting_time);
            Console.WriteLine(time);
        }
    }
}
