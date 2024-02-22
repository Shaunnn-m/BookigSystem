using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using Phumla_Kamandi_Booking_System.Business;
using Phumla_Kamandi_Booking_System.Data;


namespace Phumla_Kamandi_Booking_System.Data
{
    public class PaymentDB : DB
    {
        #region Data Members

        private string table = "Payment";
        private string sqlLocal = "SELECT * FROM Payment";
        private BookingController bookingController;
        private Collection<Payment> payments;        

        #endregion

        #region Property Method: Collection

        public Collection<Payment> AllPayments
        {
            get { return payments; }
        }

        #endregion

        #region Constructor

        public PaymentDB() : base()
        {
            payments = new Collection<Payment>();
            bookingController = new BookingController();
            FillDataSet(sqlLocal, table);
            Add2Collection();
        }

        #endregion

        #region Utility Methods

        private int FindRow(Payment payment)
        {
            int rowIndex = 0;
            DataRow myRow = null;
            int returnValue = -1;

            foreach (DataRow myRow_loopVariable in dsMain.Tables[table].Rows)
            {
                myRow = myRow_loopVariable;

                if (myRow.RowState != DataRowState.Deleted)
                {
                    if (payment.GetBooking.BookingID == Convert.ToString(dsMain.Tables[table].Rows[rowIndex]["BookingID"]))
                    {
                        returnValue = rowIndex;
                    }
                }

                rowIndex++;
            }

            return returnValue;
        }

        private void Add2Collection()
        {
            DataRow myRow = null;
            Payment aPayment;

            foreach (DataRow myRow_loopVariable in dsMain.Tables[table].Rows)
            {
                myRow = myRow_loopVariable;
                if (!(myRow.RowState == DataRowState.Deleted))
                {
                    aPayment = new Payment();

                    aPayment.GetBooking = bookingController.Find(Convert.ToString(myRow["BookingID"]));
                    aPayment.PaymentAmount = Convert.ToDouble(myRow["Amount"]);
                    aPayment.PaymentDate = Convert.ToDateTime(myRow["PaymentDate"]);
                    aPayment.CreditCard = Convert.ToString(myRow["CreditCardNumber"]);

                    payments.Add(aPayment);
                }
            }
        }
        

        private void FillRow(DataRow aRow, Payment payment)
        {
            aRow["BookingID"] = payment.GetBooking.BookingID;
            aRow["Amount"] = payment.PaymentAmount;
            aRow["PaymentDate"] = payment.PaymentDate;
            aRow["CreditCardNumber"] = payment.CreditCard;
        }

        #endregion

        #region Database Operations (CRUD)

        public void DataSetChange(Payment payment)
        {
            DataRow aRow = null;

            aRow = dsMain.Tables[table].NewRow();
            FillRow(aRow, payment);
            dsMain.Tables[table].Rows.Add(aRow);

        }

        public bool UpdateDataSource(Payment payment)
        {
            Create_INSERT_Command(payment);
            
            return UpdateDataSource(sqlLocal, table);
        }

        

        #endregion

        #region Build Parameters, Create Commands & Update database


        private void Build_INSERT_Parameters(Payment aPayment)
        {
            SqlParameter param = default(SqlParameter);

            param = new SqlParameter("@Amount", SqlDbType.Money, 15, "Amount");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@BookingID", SqlDbType.NVarChar, 50, "BookingID");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@PaymentDate", SqlDbType.Date, 3, "PaymentDate");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@CreditCardNumber", SqlDbType.NVarChar, 20, "CreditCardNumber");
            daMain.InsertCommand.Parameters.Add(param);
        }

        private void Create_INSERT_Command(Payment aPayment)
        {
            daMain.InsertCommand = new SqlCommand("INSERT INTO Payment (Amount, BookingID, PaymentDate, CreditCardNumber) VALUES (@Amount, @BookingID, @PaymentDate, @CreditCardNumber)", cnMain);
            Build_INSERT_Parameters(aPayment);
        }


        #endregion
    }
}
