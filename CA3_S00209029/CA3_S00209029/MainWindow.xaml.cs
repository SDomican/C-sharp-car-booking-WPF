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

namespace CA3_S00209029
{
   

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CarsAndBookingsEntities db = new CarsAndBookingsEntities();


            List<CarObject> carList = new List<CarObject>();

            /*var query = from c in db.Cars
                        select c.Model;
            */
            var query = from c in db.Cars
                        select new
                        {
                            Id = c.Id,
                            Make = c.Make,
                            Model = c.Model,
                            Size = c.Size
                        };
            
            foreach(var item in query)
            {
                carList.Add(new CarObject() {
                    ID = item.Id,
                    Make = item.Make,
                    Model = item.Model,
                    Size = item.Size
                });

            }


            var carTypes = from c in db.Cars
                           select c.Size;

            lsbxCars.ItemsSource = carList.OrderBy(car => car.Make)
                .ThenBy(car => car.Model);

            cmbCarTypes.ItemsSource = carTypes.ToList();

        }

        private void cmbCarTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            
            string selectedType = cmbCarTypes.SelectedItem as String;




        }

        private void lsbxCars_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string selectedCar = lsbxCars.SelectedItem as String;


        }
    }
}
