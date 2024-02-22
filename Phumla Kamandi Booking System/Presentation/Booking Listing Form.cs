using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Phumla_Kamandi_Booking_System.Data;
using Phumla_Kamandi_Booking_System.Business;
using static Phumla_Kamandi_Booking_System.Presentation.Booking_Listing_Form;
using static System.Windows.Forms.AxHost;

namespace Phumla_Kamandi_Booking_System.Presentation
{
    public partial class Booking_Listing_Form : Form
    {
        private Collection<Booking> bookings;
        private BookingController bookingController;
        private Payment_Form payment_Form;
        private Payment payment;
        private PaymentController PaymentController;

        private FormStates state;
        private Menu_Form menu_form;
        private DateTime start;
        private DateTime end;


        public enum FormStates
        {
            View = 0,
            Add = 1,
            Edit = 2,
            Delete = 3,
            Pay = 4,

        }
       
        public Booking_Listing_Form(BookingController controller)
        {
            InitializeComponent();
            bookingController = controller;
            PaymentController = new PaymentController();
            state = FormStates.View;

        }

        private void Booking_Listing_Form_Load(object sender, EventArgs e)
        {
            BookingListView.View = View.Details;
            ShowAll(false);
            UpdatePaymentControlsVisibility(false);
        }


        private void Booking_Listing_Form_Activated(object sender, EventArgs e)
        {
            BookingListView.View = View.Details;
            ShowAll(false);
            UpdatePaymentControlsVisibility(false);
            SetUpBookingListView();

        }
        private void ShowAll(bool value)
        {
            
            _bookingidlabel.Visible = value;
            booking_id_textbox.Visible = value;   
            _guestidlabel.Visible = value;
            guest_id_textbox.Visible  = value;
            _roomnumberlabel.Visible   = value;
            room_number_textbox.Visible = value;
            _checkinlabel.Visible = value;
            checkoutLabel.Visible = value;
            _amountduelabel.Visible = value;
            amount_due_textbox.Visible = value;
            booking_listing_form_check_in_dateTimePicker.Visible = value;
            booking_listing_form_check_out_dateTimePicker.Visible = value;
           


            if (state == FormStates.Delete)
            {
                booking_listing_back_button.Visible = true;
                submitButton.Visible = !value;
            }
            else
            {
                
                submitButton.Visible = value;
            }
            booking_listing_back_button.Visible = true;
            deleteButton.Visible = value;
            Pay_button.Visible = value;
            editButton.Visible = value;

        }
        private void UpdatePaymentControlsVisibility(bool showPaymentControls)
        {
            CardNumber_label.Visible = showPaymentControls;
            AmountPaid_label.Visible = showPaymentControls;
            Card_Number_TextBox.Visible = showPaymentControls;
            AmountPaid_Textbox.Visible = showPaymentControls;
            submitButton.Visible = showPaymentControls;


            if (!showPaymentControls)
            {
                Card_Number_TextBox.Text = "";
                AmountPaid_Textbox.Text = "";
            }
        }



        public void SetUpBookingListView()
        {

            ListViewItem bookingDetails;

            BookingListView.Clear();


            BookingListView.Columns.Insert(0, "Booking ID", 150, HorizontalAlignment.Left);
            BookingListView.Columns.Insert(1, "Guest Name", 150, HorizontalAlignment.Left);
            BookingListView.Columns.Insert(2, "Room Number", 100, HorizontalAlignment.Left);
            BookingListView.Columns.Insert(3, "Check-In Date", 120, HorizontalAlignment.Left);
            BookingListView.Columns.Insert(4, "Check-Out Date", 120, HorizontalAlignment.Left);
            BookingListView.Columns.Insert(5, "Total Amount", 100, HorizontalAlignment.Left);

            bookings = bookingController.AllBookings;


            foreach (Booking booking in bookings)
            {
                bookingDetails = new ListViewItem();
                bookingDetails.Text = booking.BookingID;               
                bookingDetails.SubItems.Add($"{booking.Guest.Name} {booking.Guest.LastName}");
                bookingDetails.SubItems.Add(booking.Room.RoomNumber.ToString());
                bookingDetails.SubItems.Add(booking.CheckInDate.ToShortDateString());
                bookingDetails.SubItems.Add(booking.CheckOutDate.ToShortDateString());
                bookingDetails.SubItems.Add(booking.TotalAmount.ToString("C"));
                BookingListView.Items.Add(bookingDetails);
            }

            BookingListView.Refresh();
            BookingListView.GridLines = true;
        }

        private void BookingListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableBookingFields(false);
            ShowAll(true);
            UpdatePaymentControlsVisibility(false);
            state = FormStates.View;

            
            if (BookingListView.SelectedItems.Count > 0)
            {

                ListViewItem selectedBooking = BookingListView.SelectedItems[0];


                string reservationRefNumber = selectedBooking.Text;

                PopulateBookingFields(bookingController.Find(reservationRefNumber));

            }
        }
        private void PopulateObject(Booking booking)
        {
            booking = new Booking();
            // Populate the booking object with data from the form fields
            booking.Guest.GuestID = booking_id_textbox.Text;
            booking.Guest.LastName = guest_id_textbox.Text;
            booking.Room.RoomNumber = int.Parse(room_number_textbox.Text);
            booking.CheckInDate = booking_listing_form_check_in_dateTimePicker.Value;
            booking.CheckOutDate = booking_listing_form_check_out_dateTimePicker.Value;
            booking.TotalAmount = double.Parse(amount_due_textbox.Text);
        }

        private void PopulateBookingFields(Booking booking)
        {
            
            booking_id_textbox.Text = booking.BookingID;
            guest_id_textbox.Text = $"{booking.Guest.Name}  {booking.Guest.LastName}";
            room_number_textbox.Text = booking.Room.RoomNumber.ToString();
            booking_listing_form_check_in_dateTimePicker.Value = booking.CheckInDate;
            booking_listing_form_check_out_dateTimePicker.Value = booking.CheckOutDate;
            amount_due_textbox.Text = booking.TotalAmount.ToString();


        }

        private void EnableBookingFields(bool isEnabled)
        {
            if (state == FormStates.Edit && isEnabled) {

                booking_id_textbox.Enabled = !isEnabled;
                guest_id_textbox.Enabled = !isEnabled;

               
            }
            else
            {
                booking_id_textbox.Enabled = isEnabled;
                guest_id_textbox.Enabled = isEnabled;
               
            }
            room_number_textbox.Enabled = false;
            amount_due_textbox.Enabled = false;

            booking_listing_form_check_in_dateTimePicker.Enabled = isEnabled;
            booking_listing_form_check_out_dateTimePicker.Enabled = isEnabled;
            booking_listing_back_button.Visible = true;

            if (state == FormStates.Delete)
            {
                booking_listing_back_button.Visible = !isEnabled;
                submitButton.Visible = !isEnabled;
            }
            else
            {

                submitButton.Visible = isEnabled;
            }
            if(state == FormStates.Pay) {
                Pay_button.Visible = !isEnabled;
            }else
            { 
                Pay_button.Visible = isEnabled; 
            }
        }

        private void ClearAll()
        {
            booking_id_textbox.Text = "";
            guest_id_textbox.Text = "";
            room_number_textbox.Text = "";
            booking_listing_form_check_in_dateTimePicker.Value = DateTime.Now; // Set to the current date
            booking_listing_form_check_out_dateTimePicker.Value = DateTime.Now; // Set to the current date
            amount_due_textbox.Text = "";
            
        }
       


        private void editButton_Click(object sender, EventArgs e)
        {
            if (BookingListView.SelectedItems.Count > 0)
            {
                state = FormStates.Edit;
                EnableBookingFields(true);
            }
        }

            private void deleteButton_Click(object sender, EventArgs e)
        {
            if (BookingListView.SelectedItems.Count > 0)
            {
                state = FormStates.Delete;
                EnableBookingFields(false);
            }
        }


        private void submitButton_Click(object sender, EventArgs e)
        {
            int selectedIndex = BookingListView.SelectedItems[0].Index;

            Booking selectedBooking = bookingController.AllBookings[selectedIndex];
            selectedBooking.checkStatus();
            PopulateObject(selectedBooking);

            if(state == FormStates.Pay)
            {
                Populate();
                payment.GetBooking = selectedBooking;
                payment.ProcessPayment(selectedBooking);
                selectedBooking.Payments.Add(payment);                             
                PaymentController.DataMaintenance(payment);
                PaymentController.FinalizeChanges(payment);
                selectedBooking.CheckDepositPaid(selectedBooking.GetTotalPayments());
                AmountPaid_Textbox.Clear();
                Card_Number_TextBox.Clear();


            }

            if (state == FormStates.Edit)
            {
                
                if (RoomController.IsRoomAvailable(selectedBooking.Room, start, end))
                {
                    int days = (end.Date.Subtract(start.Date)).Days;
                    double price = RoomController.GetRoomPrice(start) * days;
                    selectedBooking.TotalAmount = price;
                    selectedBooking.CheckInDate = start;
                    selectedBooking.CheckOutDate = end;
                    bookingController.DataMaintenance(selectedBooking, BookingDB.DBOperation.Edit);
                    bookingController.FinalizeChanges(selectedBooking);
                    ClearAll();
                    state = FormStates.View;
                    SetUpBookingListView();
                    ShowAll(false);
                }
                else
                {
                    MessageBox.Show("The selected dates are not available for this room. Please choose different dates.", "Room Availability", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            
            if (state == FormStates.Delete)

            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this booking?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    bookingController.DataMaintenance(selectedBooking, BookingDB.DBOperation.Delete);
                    bookingController.FinalizeChanges(selectedBooking);
                    ClearAll();
                    state = FormStates.View;
                    SetUpBookingListView();
                    ShowAll(false);
                }
                else { }
            }



        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void booking_listing_back_button_Click(object sender, EventArgs e)
        {
            menu_form = new Menu_Form();
            menu_form.Show();
            this.Close();
        }

        private void booking_listing_form_check_in_dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            start = booking_listing_form_check_in_dateTimePicker.Value;
        }

        private void booking_listing_form_check_out_dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            end = booking_listing_form_check_out_dateTimePicker.Value;
        }

        private void Populate()
        {
            payment = new Payment();
            payment.PaymentDate = DateTime.Now;
            payment.CreditCard = Convert.ToString(Card_Number_TextBox);            
            payment.PaymentAmount = Convert.ToDouble(AmountPaid_Textbox.Text);

        }

        private void Pay_button_Click(object sender, EventArgs e)
        {
            if (BookingListView.SelectedItems.Count > 0)
            {
                state = FormStates.Pay;
            ShowAll(false);
                UpdatePaymentControlsVisibility(true);
                }
            }

        private void status_textbox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}






 

