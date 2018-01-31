using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class TTTForm : Form
    {
        public TTTForm()
        {
            InitializeComponent();
        }
        // constant files
        const string USER_SYMBOL = "X";
        const string COMPUTER_SYMBOL = "O";
        const string EMPTY = "";

        const int SIZE = 5;


        // constants for the 2 diagonals
        const int TOP_LEFT_TO_BOTTOM_RIGHT = 1;
        const int TOP_RIGHT_TO_BOTTOM_LEFT = 2;

        // constants for IsWinner
        const int NONE = -1;
        const int ROW = 1;
        const int COLUMN = 2;
        const int DIAGONAL = 3;

        string[,] board = new string[SIZE, SIZE];

        // create a method to fill the board with empty values
        public void FillBoard()
        {
            for (int row = 0; row < SIZE; row++)
            {
                for (int col = 0; col < SIZE; col++)
                {
                    board[row, col] = EMPTY;
                }
            }
        }

        // This method takes a row and column as parameters and 
        // returns a reference to a label on the form in that position
        private Label GetSquare(int row, int column)
        {
            int labelNumber = row * SIZE + column + 1;
            return (Label)(this.Controls["label" + labelNumber.ToString()]);
        }

        // This method does the "reverse" process of GetSquare
        // It takes a label on the form as it's parameter and
        // returns the row and column of that square as output parameters
        private void GetRowAndColumn(Label l, out int row, out int column)
        {
            int position = int.Parse(l.Name.Substring(5));
            row = (position - 1) / SIZE;
            column = (position - 1) % SIZE;
        }

        // This method takes a row (in the range of 0 - 4) and returns true if 
        // the row on the form contains 5 Xs or 5 Os.
        // Use it as a model for writing IsColumnWinner
        // CHNAGE 
        private bool IsRowWinner(int row)
        {
            string symbol = board[row, 0];
            for (int col = 1; col < SIZE; col++)
            {
                if (symbol == EMPTY || board[row, col] != symbol)
                    return false;
            }
            return true;
        }

        //* TODO:  finish all of these that return true
        // Checks to see if any row is the winner by interating through each row.
        // CHANGE
        private bool IsAnyRowWinner()
        {
            for (int row = 1; row < SIZE; row++)
                if (IsRowWinner(row))
                {
                    return true;
                }
            return false;
        }

        // Checks to see if a column is the winner by interating through each row.
        // CHANGE
        private bool IsColumnWinner(int col)
        {
            string symbol = board[0, col];
            for (int row = 1; row < SIZE; row++)
            {
                if (symbol == EMPTY || board[row, col] != symbol)
                    return false;
            }
            return true;
        }

        // Checks to see if any column is the winner by interating through each column.
        //CHANGE
        private bool IsAnyColumnWinner()
        {
            for (int col = 1; col < SIZE; col++)
            {
                if (IsColumnWinner(col))
                {
                    return true;
                }
            }
            return false;            
        }
        //CHANGE
        private bool IsDiagonal1Winner()
        {
            string symbol = board[0, 0];
            for (int diag = 1; diag < SIZE; diag ++)
            {
                if (symbol == EMPTY || board[diag,diag] != symbol)
                    return false;
            }
            return true;
        }

        //CHANGE
        private bool IsDiagonal2Winner()
        {
            string symbol = board[0, (SIZE-1)];
            for (int row = 1, col = SIZE - 2; row < SIZE; row++, col--)
            {
                if (symbol == EMPTY || board[row, col] != symbol)
                    return false;
            }
            return true;
        }
        
        //CHANGE
        private bool IsAnyDiagonalWinner()
        {
            //Label square = GetSquare(0, (SIZE - 1));
            string symbol = board[0, (SIZE - 1)];
            for (int row = 1, col = SIZE-2; row < SIZE; row++, col--)
            {
                //square = GetSquare(row, col);
                if (symbol == EMPTY || board[row, col] != symbol)
                    return false;
            }
            return true;
        }
        //CHANGE
        private bool IsFull()
        {
            string symbol = board[0, 0];
            for (int row = 0; row < SIZE; row++)
            {
                for (int col = 0; col < SIZE; col++)
                {
                    if (board[row, col] == EMPTY)
                    return false;
                }
            }
 
            return true;
        }

        // This method determines if any row, column or diagonal on the board is a winner.
        // It returns true or false and the output parameters will contain appropriate values
        // when the method returns true.  See constant definitions at top of form.
        private bool IsWinner(out int whichDimension, out int whichOne)
        {
            // rows
            for (int row = 0; row < SIZE; row++)
            {
                if (IsRowWinner(row))
                {
                    whichDimension = ROW;
                    whichOne = row;
                    return true;
                }
            }
            // columns
            for (int column = 0; column < SIZE; column++)
            {
                if (IsColumnWinner(column))
                {
                    whichDimension = COLUMN;
                    whichOne = column;
                    return true;
                }
            }
            // diagonals
            if (IsDiagonal1Winner())
            {
                whichDimension = DIAGONAL;
                whichOne = TOP_LEFT_TO_BOTTOM_RIGHT;
                return true;
            }
            if (IsDiagonal2Winner())
            {
                whichDimension = DIAGONAL;
                whichOne = TOP_RIGHT_TO_BOTTOM_LEFT;
                return true;
            }
            
            whichDimension = NONE;
            whichOne = NONE;
            return false;
        }

        // I wrote this method to show you how to call IsWinner
        private bool IsTie()
        {
            int winningDimension, winningValue;
            return (IsFull() && !IsWinner(out winningDimension, out winningValue));
        }

        // This method takes an integer in the range 0 - 4 that represents a column
        // as it's parameter and changes the font color of that cell to red.
        private void HighlightColumn(int col)
        {
            for (int row = 0; row < SIZE; row++)
            {
                Label square = GetSquare(row, col);
                square.Enabled = true;
                square.ForeColor = Color.Red;
            }
        }

        // This method changes the font color of the top right to bottom left diagonal to red
        // I did this diagonal because it's harder than the other one
        private void HighlightDiagonal2()
        {
            for (int row = 0, col = SIZE - 1; row < SIZE; row++, col--)
            {
                Label square = GetSquare(row, col);
                square.Enabled = true;
                square.ForeColor = Color.Red;
            }
        }

        // This method will highlight either diagonal, depending on the parameter that you pass
        private void HighlightDiagonal(int whichDiagonal)
        {
            if (whichDiagonal == TOP_LEFT_TO_BOTTOM_RIGHT)
                HighlightDiagonal1();
            else
                HighlightDiagonal2();

        }

        //* TODO:  finish these 2
        private void HighlightRow(int row)
        {
            for (int col = 0; col < SIZE; col++)
            {
                Label square = GetSquare(row, col);
                square.Enabled = true;
                square.ForeColor = Color.Red;
            }
        }

        private void HighlightDiagonal1()
        {
            for (int row = 0, col = 0; row < SIZE; row++, col++)
            {
                Label l = GetSquare(row, col);
                l.Enabled = true;
                l.ForeColor = Color.Red;
            }
        }

        //* TODO:  finish this
        private void HighlightWinner(string player, int winningDimension, int winningValue)
        {
            switch (winningDimension)
            {
                case ROW:
                    HighlightRow(winningValue);
                    resultLabel.Text = (player + " wins!");
                    break;
                case COLUMN:
                    HighlightColumn(winningValue);
                    resultLabel.Text = (player + " wins!");
                    break;
                case DIAGONAL:
                    HighlightDiagonal(winningValue);
                    resultLabel.Text = (player + " wins!");
                    break;
            }
        }

        //* TODO:  finish these 2
        private void ResetSquares()
        {
            for (int row = 0, col = 0; row < SIZE; row++, col++)
            {
                Label l = GetSquare(row, col);
                l.Text = EMPTY;
                l.Enabled = true;
                l.ForeColor = Color.Black;
            }
        }
        //CHANGE
        private void MakeComputerMove()
        {
            Random randomNumberGenerator = new Random();
            int row, col;
            do
            {
                row = randomNumberGenerator.Next(0, SIZE);
                col = randomNumberGenerator.Next(0, SIZE);

            } while (board[row, col] != EMPTY);

            board[row, col] = COMPUTER_SYMBOL;
            //DisableSquare(square);

        }

        // Setting the enabled property changes the look and feel of the cell.
        // Instead, this code removes the event handler from each square.
        // Use it when someone wins or the board is full to prevent clicking a square.
        private void DisableAllSquares()
        {
            for (int row = 0; row < SIZE; row++)
            {
                for (int col = 0; col < SIZE; col++)
                {
                    Label square = GetSquare(row, col);
                    DisableSquare(square);
                }
            }
        }

        // Inside the click event handler you have a reference to the label that was clicked
        // Use this method (and pass that label as a parameter) to disable just that one square
        private void DisableSquare(Label square)
        {
            square.Click -= new System.EventHandler(this.label_Click);
        }

        // You'll need this method to allow the user to start a new game
        private void EnableAllSquares()
        {
            for (int row = 0; row < SIZE; row++)
            {
                for (int col = 0; col < SIZE; col++)
                {
                    Label square = GetSquare(row, col);
                    square.Click += new System.EventHandler(this.label_Click);
                }
            }
        }
        // sync the board and the UI
        public void SyncArrayAndUI()
        {
            for(int row = 0; row < board.GetLength(0); row++)
            {
                for(int col = 0; col < board.GetLength(1); col++)
                {
                    GetSquare(row, col).Text = board[row, col];
                    GetSquare(row, col).ForeColor = Color.Black;
                    if (GetSquare(row, col).Text != EMPTY)
                        DisableSquare(GetSquare(row, col));
                }
            }
        }


        //* TODO:  finish the event handlers
        private void label_Click(object sender, EventArgs e)
        {
            int winningDimension = NONE;
            int winningValue = NONE;
            int row, col;

            //CHANGE
            Label clickedLabel = (Label)sender;
            //clickedLabel.Text = USER_SYMBOL;

            // put the x in the array
            GetRowAndColumn(clickedLabel, out row, out col);
            board[row, col] = USER_SYMBOL;
            SyncArrayAndUI();
            //DisableSquare(clickedLabel);

            if (IsWinner(out winningDimension, out winningValue) == false)
            {
                if (!IsFull())
                {
                    MakeComputerMove();
                    SyncArrayAndUI();
                    if (IsWinner(out winningDimension, out winningValue))
                    {
                        HighlightWinner("Computer", winningDimension, winningValue);
                        DisableAllSquares();
                    }
                }
                else
                {
                    MessageBox.Show("It's a Tie!");
                }
            }
            else
            {
                HighlightWinner("User", winningDimension, winningValue);
                DisableAllSquares();
            }
            
        }

        private void newGameButton_Click(object sender, EventArgs e)
        {
            //Application.Restart();
            //ResetSquares();
            
            EnableAllSquares();
            FillBoard();
            SyncArrayAndUI();
            resultLabel.Text = "";

        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TTTForm_Load(object sender, EventArgs e)
        {
            FillBoard();
            SyncArrayAndUI();
        }
    }
}
