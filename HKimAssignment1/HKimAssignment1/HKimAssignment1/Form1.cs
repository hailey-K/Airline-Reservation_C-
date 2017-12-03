/***************************************************************************
 *  NAME : HYERIM KIM
 *  STUDENT NUMBER : 7518301
 *  REVISION HISTORY : SEP 22ND 2017
 *  PROJECT : ASSIGNMENT 1 
 *  
 *  
 *  DOCUMENTATION COMMENT :
 *  THIS IS AIR PLANE RESERVATION PROGRAM. 
 *  YOU CAN BOOK AND CANCEL SEATS.
 *  IF THE SEATS ARE ALL RESERVED, YOU WILL BE PUT ON THE WAITING LIST.
 ***************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HKimAssignment1
{
    public partial class Form1 : Form
    {
        //ERROR MESSAGES 
        const string MESSAGE_SEAT_RESERVE_SUCCESS= "The seat is successfully booked.";
        const string MESSAGE_SEAT_ALREADY_BOOK = "Sorry, The seat is already booked !";
        const string MESSAGE_SEAT_CANCEL_SUCCESS = "The seat is successfully cancelled.";
        const string MESSAGE_SEAT_CANCEL_RESERVE_SUCCESS = "The seat is successfully cancelled and reserved by person from the waiting list.";
        const string MESSAGE_SEAT_CANCEL_FAIL = "Sorry, The selected seat hasn't booked.\n So we can't cancel it.";
        const string MESSAGE_ASK_FILL_NAME = "please fill your name.";
        const string MESSAGE_ASK_FILL_SEAT = "please choose the seat.";
        const string MESSAGE_AVAILABLE = "Available";
        const string MESSAGE_NON_AVAILABLE = "Not Available";
        const string MESSAGE_AVAILABLE_SEAT_EXIST = "Seats are available ! \nYou can't add to waiting list.";
        const string MESSAGE_AVAILABLE_SEAT_AND_WAITING_LIST_EXIST = "There are no seats available.\n But you are added on the waiting list.";
        const string MESSAGE_AVAILABLE_SEAT_AND_WAITING_LIST_NON_EXIST = "There are no seats available.\nAnd Waiting list is full.";
        
        //SEATS INFO
        string[,] seats = new string[,]
         {
             {"A0","A1","A2"},
             {"B0","B1","B2"},
             {"C0","C1","C2"},
             {"D0","D1","D2"},
             {"E0","E1","E2"}};

        //WAITINGLIST INFO
        string[] waitingList = new string[10];
        public Form1()
        {
            InitializeComponent();
        }

        //WHEN BOOK BUTTON CLICKED
        private void btnBook_Click(object sender, EventArgs e)
        {
            if (checkNameOrSeatEmpty("BOOK"))
            {
                int row= listSeat.SelectedIndex; ;
                int col= listSeatNumber.SelectedIndex;
  

                //if seat is available 
                if (!seats[row, col].Contains(","))
                {
                    seats[row, col] += ", " + txtName.Text;
                    MessageBox.Show(MESSAGE_SEAT_RESERVE_SUCCESS);
                }

                //if seat is not available
                else 
                {
                    // check if there are other available seats
                    // and if so,
                    if (checkSeatAvailable(seats))
                    {
                        MessageBox.Show(MESSAGE_SEAT_ALREADY_BOOK);
                    }
                    // if all seats are reserved, send the person to the waiting list 
                    else
                    {
                        btnAddToWaitingList_Click(sender, e);
                    }
                }
            }
        }

        //WHEN CANCEL BUTTON CLICKED
        private void btCancel_Click(object sender, EventArgs e)
        {
            if (checkNameOrSeatEmpty("CANCEL"))
            {
                int row = listSeat.SelectedIndex; ;
                int col = listSeatNumber.SelectedIndex;
                string cancelMsg = "";

                //if the seat hasn't booked,
                if (!seats[row, col].Contains(","))
                {
                    cancelMsg = MESSAGE_SEAT_CANCEL_FAIL;
                }

                //if the seat has booked,
                else
                {
                    //if there are people on the waiting list
                    if (waitingList[0]!="" && waitingList[0] != null)
                    {
                        //transfer the person from waiting list to the seating reserve
                        seats[row, col] = seats[row, col].Substring(0, seats[row, col].IndexOf(','));
                        seats[row, col] += ", "+waitingList[0];
                        //sort the waiting list array
                        waitingList = updateArray(waitingList);
                        cancelMsg = MESSAGE_SEAT_CANCEL_RESERVE_SUCCESS;
                    }

                    //if there are not people on the waiting list
                    else
                    {
                        seats[row, col] = seats[row, col].Substring(0, seats[row, col].IndexOf(','));
                        cancelMsg = MESSAGE_SEAT_CANCEL_SUCCESS;
                    }
                }
                MessageBox.Show(cancelMsg);
            }
            }

        //WHEN STATUS BUTTON CLICKED
        private void btnStatus_Click(object sender, EventArgs e)
        {
            if (checkNameOrSeatEmpty("STATUS")) { 
            int row = listSeat.SelectedIndex; ;
            int col = listSeatNumber.SelectedIndex;
            string statusMsg = "";

            //if seat is available 
            if (!seats[row, col].Contains(","))
            {
                statusMsg = "AVAILABLE";
            }
            else
            {
                statusMsg = "NOT AVAILABLE";
            }

            txtStatus.Text = statusMsg;
            }
        }

        //WHEN ADD_TO_WAITING_LIST BUTTON CLICKED
        private void btnAddToWaitingList_Click(object sender, EventArgs e)
        {
            string waitingListMsg = "";

            if(checkNameOrSeatEmpty("ADD_WAITINGLIST"))
            {

                //if there are available seats
                if (checkSeatAvailable(seats))
                {
                    waitingListMsg = MESSAGE_AVAILABLE_SEAT_EXIST;
                }

                //if no seats are available
                else
                {
                    int numberOfPeopleInTheWaitingList = peopleWaitingList();
                    //if there are less than 10 people waiting, put the person on the waiting list
                    if (numberOfPeopleInTheWaitingList < 10)
                    {
                        waitingList[numberOfPeopleInTheWaitingList] = txtName.Text;
                        waitingListMsg=MESSAGE_AVAILABLE_SEAT_AND_WAITING_LIST_EXIST;
                    }
                    else
                    {
                        waitingListMsg=MESSAGE_AVAILABLE_SEAT_AND_WAITING_LIST_NON_EXIST;
                    }
                }

                MessageBox.Show(waitingListMsg);
            }
        }

        //WHEN FILL ALL BUTTON CLICKED
        private void btnFillAll_Click(object sender, EventArgs e)
        {
            if (checkNameOrSeatEmpty("FILL_ALL"))
            {
                for(int i = 0; i < seats.GetLength(0); i++)
                    for(int j=0;j<seats.GetLength(1);j++)
                {
                        if (seats[i,j].Contains(","))
                        {
                            seats[i,j] = seats[i,j].Substring(0, seats[i,j].IndexOf(','));
                        }
                        seats[i, j] += ", " + txtName.Text; ;
                    }
                MessageBox.Show(MESSAGE_SEAT_RESERVE_SUCCESS);
            }
        }


        //WHEN SHOW ALL BUTTON CLICKED
        private void btnShowAll_Click(object sender, EventArgs e)
        {
            printSeatArray(txtShowAll, seats);
        }

        //WHEN SHOW WAITING LIST BUTTON CLICKED
        private void btnShowWaitingList_Click(object sender, EventArgs e)
        {
            printWaitingListArray(txtShowWaitingList, waitingList);
        }

        // UPDATE ARRAY : 
        // IF INDEX 0 HAS "" DATA, THE NEW ARRAY SAVE OLD ARRAY WITHOUT ""
        private string[] updateArray(string[] oldArray)
        {
             int oldArrayLength = oldArray.Length;
             string[] newArray = new string[oldArrayLength];

             for(int i=0;i< oldArray.Length-1; i++)
             {
                 if(oldArray[i + 1]!=null|| oldArray[i + 1] != "")
                {
                    newArray[i] = oldArray[i + 1];
                }
            }

             return newArray;
        }

        // CHECK IF NAME OR SEAT IS NOT SELCTED 
        private bool checkNameOrSeatEmpty(string option)
        {
            bool check = true;
            string msgBox="";
            if (txtName.Text == "" && (option == "BOOK" || option == "FILL_ALL" || option == "ADD_WAITINGLIST"))
            {
                check = false;
                msgBox = MESSAGE_ASK_FILL_NAME;
            }
            
            else if ((listSeat.SelectedIndex == -1 || listSeatNumber.SelectedIndex == -1) && option != "FILL_ALL" && option != "ADD_WAITINGLIST")
            {
                check = false;
                msgBox = MESSAGE_ASK_FILL_SEAT;
            }
            if (!check)
            {
                MessageBox.Show(msgBox);
            }
            return check;
        }
        
        //PRINT SEAT ARRAY 
        private void printSeatArray(RichTextBox richTxt, string[,] seatList)
        {
            richTxt.Text = "";
            for (int i = 0; i < seatList.GetLength(0); i++)
                for(int j=0; j < seatList.GetLength(1);j++)
            {
                string tmpPeopleList = seatList[i,j];
                richTxt.AppendText(tmpPeopleList + "\n");
            }
        }

        //PRINT WAITING LIST ARRAY 
        private void printWaitingListArray(RichTextBox richTxt, string[] waitingList)
        {
            richTxt.Text = "";
            for (int i = 0; i < waitingList.Length; i++)
            {
                  string tmpPeopleList = waitingList[i];
                  richTxt.AppendText(tmpPeopleList + "\n");
            }
        }

        //CHECK IF SEAT IS AVAILABLE
        //IF SO, RETURN TRUE
        //IF NOT, RETURN FALSE
        private bool checkSeatAvailable(string[,] seatList)
        {
            for (int i = 0; i < seatList.GetLength(0); i++)
                for (int j = 0; j < seatList.GetLength(1); j++)
                {
                    if (!seatList[i,j].Contains(","))
                    return true;
                 }
            return false;
        }

        //CHECK HOW MANY PEOPLE IN THE WAITING LIST
        private int peopleWaitingList()
        {
            int count = 0;
            for(int i = 0; i < waitingList.Length; i++)
            {
                if (waitingList[i]!="" && waitingList[i] != null)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
