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
            dtpStartDate.SelectedDate = DateTime.Now;
            dtpEndDate.SelectedDate = DateTime.Now;

            carImage.Source = new BitmapImage(new Uri("/Images/vw.png", UriKind.Relative));


            var query = from c in db.Cars
                        select c;
                        



            carTypes = (from c in db.Cars
                        select c.Size).ToList();
            carTypes.Add("All");
            carTypes = carTypes.OrderBy(type => type).ToList();



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

            UpdateSelectedCarTable();
            
        }

        private void btnBook_Click(object sender, RoutedEventArgs e)
        {
            Bookings bookingWindow = new Bookings();
            bookingWindow.Owner = this;
            bookingWindow.ShowDialog();
        }

        private void dtpStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

            if (txblSelectedCar.Text.Length > 0)
                UpdateSelectedCarTable();

        }

        private void dtpEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txblSelectedCar.Text.Length > 0)
                UpdateSelectedCarTable();
        }

        private void UpdateSelectedCarTable()
        {
            Car selectedCar = lsbxCars.SelectedItem as Car;

            if (selectedCar == null)
                return;

            int selectedCarId = Convert.ToInt32(selectedCar.Id);


            var query = from c in db.Cars
                        where c.Id == selectedCarId
                        select new
                        {
                            Id = c.Id,
                            Make = c.Make,
                            Model = c.Model,
                            Size = c.Size
                        };



            txblSelectedCar.Text = String.Format("Car Id: {0}\nMake: {1}\nModel: {2}\nSize: {3}\nRental Date: {4}\nReturn Date: {5}", query.FirstOrDefault().Id, query.FirstOrDefault().Make, query.FirstOrDefault().Model, query.FirstOrDefault().Size, dtpStartDate.SelectedDate.Value.ToShortDateString(), dtpEndDate.SelectedDate.Value.ToShortDateString());
        }
    }
}
