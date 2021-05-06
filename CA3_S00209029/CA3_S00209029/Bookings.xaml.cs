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
using System.Windows.Shapes;

namespace CA3_S00209029
{
    /// <summary>
    /// Interaction logic for Bookings.xaml
    /// </summary>
    public partial class Bookings : Window
    {
        CarsAndBookingsEntities db = new CarsAndBookingsEntities();
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            MainWindow main = Owner as MainWindow;

            Car selectedCar = main.lsbxCars.SelectedItem as Car;
            
            txblBookingConfirmation.Text = String.Format("Booking confirmation:\n\nCar Id: {0}\nMake: {1}\nModel: {2}\nType: {3}\nRental Date: {4}\nReturn Date: {5}", selectedCar.Id, selectedCar.Make, selectedCar.Model, selectedCar.Size, main.dtpStartDate.SelectedDate.Value.ToShortDateString(), main.dtpEndDate.SelectedDate.Value.ToShortDateString());

            
        }


        public Bookings()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
