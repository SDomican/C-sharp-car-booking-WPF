using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        CarsAndBookingsEntities db = new CarsAndBookingsEntities();
        List<String> carTypes = new List<String>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            carImage.Source = new BitmapImage(new Uri("/Images/vw.png", UriKind.Relative));


            /*var query = (from c in db.Cars
                         select new
                         {
                             Id = c.Id,
                             Make = c.Make,
                             Model = c.Model,
                             Size = c.Size
                         }).AsEnumerable().Select(item => new Car()
                         {
                             Make = item.Make,
                             Model = item.Model,
                             Size = item.Size
                         });
            
            */
            var query = from c in db.Cars
                        select c.ToString();
                        



            carTypes = (from c in db.Cars
                        select c.Size).ToList();
            carTypes.Add("All");
            carTypes = carTypes.OrderBy(type => type).ToList();

            //lsbxCars.ItemsSource = query.ToList().OrderBy(car => car.Make)
            //.ThenBy(car => car.Model);

            lsbxCars.ItemsSource = query.ToList();


            cmbCarTypes.ItemsSource = carTypes.ToList().Distinct();

        }

        private void cmbCarTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            
            string selectedType = cmbCarTypes.SelectedItem as String;

            if (selectedType.Equals("All"))
            {
                lsbxCars.ItemsSource = (from c in db.Cars
                                        select c).ToList()
                                        .OrderBy(car => car.Make)
                                        .ThenBy(car => car.Model);
            }

            else
            {
                lsbxCars.ItemsSource = (from c in db.Cars
                                        where c.Size.Equals(selectedType)
                                        select c).ToList()
                                        .OrderBy(car => car.Make)
                                        .ThenBy(car => car.Model);
            }
                                   
        }

        private void lsbxCars_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            MessageBox.Show(lsbxCars.SelectedItem.ToString());


            /*int selectedCarId = Convert.ToInt32(lsbxCars.SelectedItem);
               

            var query = from c in db.Cars
                       where c.Id == selectedCarId
                       select c;

            txblSelectedCar.Text = query.ToString();
            */
        }

        private void btnBook_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
