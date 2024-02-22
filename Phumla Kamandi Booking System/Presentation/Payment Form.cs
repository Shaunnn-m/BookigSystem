using Phumla_Kamandi_Booking_System.Business;
using Phumla_Kamandi_Booking_System.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Phumla_Kamandi_Booking_System.Presentation
{
    public partial class Payment_Form : Form
    {
        protected Payment payment;
    
        Booking_Listing_Form booking_Listing_Form;
        private PaymentController PaymentController;
        private BookingController bookingController;
        private Booking_Confirmation_Form confirmation_Form;
        private Booking selectedBooking;
        private Menu_Form menu_form;

        public Payment_Form()
        {
            InitializeComponent();
            bookingController = new BookingController();
            PaymentController = new PaymentController();
        }

        public Payment_Form(Booking booking )
        {
            InitializeComponent();
            selectedBooking = booking;
            bookingController = new BookingController();
            PaymentController = new PaymentController();
        }

        private void Populate()
        {
            payment = new Payment();
            payment.PaymentDate = DateTime.Now;
            payment.CreditCard = Convert.ToString(Payment_TextBox);
            payment.GetBooking = Booking_Form.booking;
            payment.PaymentAmount = Convert.ToDouble(amount_textBox.Text);
           
        }
        private void Payment_Form_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void payment_form_submit_button_Click(object sender, EventArgs e)
        {
            Populate();            
            Booking_Form.booking.Payments.Add(payment);
            Booking_Form.booking.CheckDepositPaid(payment.PaymentAmount);
            payment.ProcessPayment();      
            Payment_TextBox.Clear();
            confirmation_Form = new Booking_Confirmation_Form();
            this.Hide();
            confirmation_Form.Show();
        }

        private void payment_form_back_button_Click(object sender, EventArgs e)
        {

            menu_form = new Menu_Form();
            menu_form.Show();
            this.Close();

        }

        private void Payment_TextBox_Enter(object sender, EventArgs e)
        {
            Payment_TextBox.Text = "";
        }

        private void Payment_TextBox_Leave(object sender, EventArgs e)
        {
            string creditCardNumber = Payment_TextBox.Text;

          
            creditCardNumber = creditCardNumber.Replace(" ", "");

            if (string.IsNullOrWhiteSpace(creditCardNumber))
            {
                MessageBox.Show("Credit card number cannot be empty. Please enter a valid number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Payment_TextBox.Focus();
                return;
            }

            if (creditCardNumber.Length != 10 || !creditCardNumber.All(char.IsDigit))
            {
                MessageBox.Show("Invalid credit card number. Please enter a 10-digit number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Payment_TextBox.Focus();
            }

        }

        private void amount_textBox_TextChanged(object sender, EventArgs e)
        {

        }
        

        private void amount_textBox_Leave(object sender, EventArgs e)
        {                     

            if (string.IsNullOrWhiteSpace(amount_textBox.Text))
            {
                MessageBox.Show("Credit card number cannot be empty. Please enter a valid number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Payment_TextBox.Focus();
                return;
            }

            if (!amount_textBox.Text.All(char.IsDigit))
            {
                MessageBox.Show("Invalid credit card number. Please enter a 10-digit number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Payment_TextBox.Focus();
            }



        }

        private void amount_textBox_Enter(object sender, EventArgs e)
        {
            amount_textBox.Text = "";
            amount_textBox.ForeColor = SystemColors.WindowText; // Restore the default text color
        }
    }
}
