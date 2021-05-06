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
        List<Car> carList = new List<Car>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dtpStartDate.SelectedDate = DateTime.Now;
            dtpEndDate.SelectedDate = DateTime.Now;

            List<Booking> bookingList = GetBookingList();

            carImage.Source = new BitmapImage(new Uri("/Images/vw.png", UriKind.Relative));

           
            carList = (from c in db.Cars.AsEnumerable()
                                 select new Car()
                                 {
                                     Make = c.Make,
                                     Model = c.Model,
                                     Size = c.Size,
                                     Id = c.Id
                                 }).OrderBy(car => car.Make)
                                 .ThenBy(car => car.Model)
                                 .ToList();

            //For each car, check its bookings and see if it's taken
            OuterLoop:
            foreach(Car car in carList)
            {

                InnerLoop:
                foreach(Booking booking in bookingList)
                {
                    DateTime datePickerStartTime = dtpStartDate.SelectedDate.Value.Date;
                    DateTime datePickerEndTime = dtpEndDate.SelectedDate.Value.Date;

                    String[] dateArray = booking.StartDate.Split('/');                   
                    
                    int bookingStartYear = Int32.Parse(dateArray[2]);
                    int bookingStartMonth = Int32.Parse(dateArray[1]);
                    int bookingStartDay = Int32.Parse(dateArray[0]);

                    DateTime bookingStartTime = new DateTime(bookingStartYear, bookingStartMonth, bookingStartDay);

                    String[] endDateArray = booking.StartDate.Split('/');

                    int bookingEndYear = Int32.Parse(endDateArray[2]);
                    int bookingEndMonth = Int32.Parse(endDateArray[1]);
                    int bookingEndDay = Int32.Parse(endDateArray[0]);

                    DateTime bookingEndTime = new DateTime(bookingEndYear, bookingEndMonth, bookingEndDay);


                    //TO CONTINUE - DATES STILL NEED TUNING
                    if (booking.CarId == car.Id && DateTime.Compare(datePickerStartTime, bookingStartTime) < 0 && DateTime.Compare(datePickerEndTime, bookingEndTime) > 1)
                        {

                        }


                }



            }

            


            //CarTypes list used to populate the car types combobox.
            carTypes = (from c in db.Cars
                        select c.Size).ToList();
            carTypes.Add("All");
            carTypes = carTypes.OrderBy(type => type).ToList();

            cmbCarTypes.ItemsSource = carTypes.ToList().Distinct();

            //lsbxCars.ItemsSource = query.ToList();
            lsbxCars.ItemsSource = carList;

            UpdateBookingList();

        }
        //Filters the list of cars shown depending on what car type is selected in the combo box.
        private void cmbCarTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
           

            string selectedType = cmbCarTypes.SelectedItem as String;

            if (selectedType.Equals("All"))
            {

               
                    lsbxCars.ItemsSource = (from c in db.Cars
                                            select c)
                                        .ToList()
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
        { UpdateSelectedCarTable(); }

        private void btnBook_Click(object sender, RoutedEventArgs e)
        {
            
            //Checks if a car has been selected when the booking button is clicked.
            if(lsbxCars.SelectedItem == null)
            {
                MessageBox.Show("Please select a car first!");
                return;
            }
            
            //Checks if a valid return date ahs been selected when booking button is clicked.
            if(dtpEndDate.SelectedDate < dtpStartDate.SelectedDate)
            {
                MessageBox.Show("Please enter a valid return date!");
                return;
            }

            string startDate = dtpStartDate.SelectedDate.Value.ToShortDateString();
            string endDate = dtpEndDate.SelectedDate.Value.ToShortDateString();
            
            Car selectedCar = lsbxCars.SelectedItem as Car;

            if (selectedCar == null)
                return;

            int selectedCarId = Convert.ToInt32(selectedCar.Id);


            Booking newBooking = new Booking() {
                StartDate = startDate,
                EndDate = endDate,
                Car = selectedCar,
                CarId = selectedCar.Id
            };

            db.Bookings.Add(newBooking);
            db.SaveChanges();

            UpdateBookingList();


            //Generates and displays the booking window when the "book" button is pressed.
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

        //Updates the txblSelectedCar text box with the details of the car selected in the lsbxCars list box.
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


            txblSelectedCar.Text = String.Format("Car Id: {0}\nMake: {1}\nModel: {2}\nSize: {3}\nRental Date: {4}\nReturn Date: {5}",
                query.FirstOrDefault().Id,
                query.FirstOrDefault().Make,
                query.FirstOrDefault().Model,
                query.FirstOrDefault().Size,
                dtpStartDate.SelectedDate.Value.ToShortDateString(),
                dtpEndDate.SelectedDate.Value.ToShortDateString());
        }

        private void UpdateBookingList()
        {
            var bookingData = from b in db.Bookings
                              select new {
                              Id = b.Id,
                              CarMake = b.Car.Make,
                              CarModel = b.Car.Model,
                              BookingCarId = b.CarId,
                              CarId = b.Car.Id,
                              Type = b.Car.Size,
                              StartDate = b.StartDate,
                              EndDate = b.EndDate
                              };

          

            lsbBookingsTest.ItemsSource = bookingData.ToList();
        }

        private List<Booking> GetBookingList()
        {
            List<Booking> bookingList = (from b in db.Bookings
                                         select b).ToList();
            return bookingList;
        }

    }
}
