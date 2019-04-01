using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3dTicTacToe
{
    public partial class Form1 : Form
    {
        readonly int sideLength = 50;
        //initializes the sidelength used for each of the squares on the board
        int player = 1;
        //initializes the starting player
        int[,,] values = new int[4,4,4];
        //defines the 3d array used to store square values (player 1 or player 2)
        Bitmap board;
        //defines the current board
        Bitmap defaultBoard;
        //defines the initial board
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            board = new Bitmap(pictureBox1.ClientSize.Width, pictureBox1.ClientSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(board);
            //creates graphic from board
            Pen myPen = new Pen(Color.Black);
            Pen myPen2 = new Pen(Color.Black, 4);
            for (int z = 0; z < 4; z++)
            {
                for (int x = 0; x < 4; x++)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        g.DrawRectangle(myPen, z * sideLength * 4 + x * sideLength, y * sideLength, sideLength, sideLength);
                        //draws all the squares on the board
                    }
                }
            }
            for (int z = 1; z < 4; z++)
            {
                g.DrawLine(myPen2, z * sideLength * 4, 0, z * sideLength * 4, sideLength * 4);
                //draws thick lines to indicate a seperation on the z plane
            }
            pictureBox1.Invalidate();
            defaultBoard = new Bitmap(board);
            //saves the initial board for future use
            gameReset();
        }

        private void gameReset()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int i2 = 0; i2 < 4; i2++)
                {
                    for (int i3 = 0; i3 < 4; i3++)
                    {
                        values[i, i2, i3] = 0;
                        //resets array values
                    }
                }
            }
            board = new Bitmap(defaultBoard);
            //resets board
            player = 1;
            //resets player value to first player
            pictureBox1.Invalidate();
        }

        private void drawX(int x, int y)
        {
            Graphics g = Graphics.FromImage(board);
            Pen myPen = new Pen(Color.Black);
            g.DrawLine(myPen, x, y, x + sideLength, y + sideLength);
            g.DrawLine(myPen, x + sideLength, y, x, y + sideLength);
        }

        private Boolean hasTie()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int i2 = 0; i2 < 4; i2++)
                {
                    for (int i3 = 0; i3 < 4; i3++)
                    {
                        if (values[i, i2, i3] == 0)
                            return false;
                        //if any square is still open, then the game is not yet a tie
                    }
                }
            }
            return true;
        }

        private Boolean hasWon()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int direction = 0; direction < 3; direction++)
                {
                    //the direction value determines across which dimension the one dimensional win is checked (e.g. across the z dimension with procedural x and y values being used)
                    for (int i2 = 0; i2 < 4; i2++)
                    {
                        if (oneDWin(i, i2, direction))
                            return true;
                        //checks every possible winning condition across one dimension
                    }
                    if (twoDWin(i,direction, 1, 0) || twoDWin(i, direction, -1, 3))
                        return true;
                    //checks every possible winning condition across two dimensions
                }
                if (threeDWin(i))
                    return true;
                //checks one of 4 possible three dimensional winning conditions
            }
    return false;
        }

        private Boolean oneDWin(int i, int i2, int direction)
        {
                    for (int i3 = 0; i3 < 4; i3++)
                    {
                //i3 is the value that changes in order to check the line, it is put somewhere different depending on the direction
                        switch (direction)
                        {
                            case 0:
                                if (values[i, i2, i3] != player)
                                    return false;
                                //returns false if any part of the winning condition isn't true
                                break;
                            case 1:
                                if (values[i, i3, i2] != player)
                                    return false;
                                break;
                            case 2:
                                if (values[i3, i2, i] != player)
                                    return false;
                                break;
                        }
                    }
            return true; 
        }

        private Boolean twoDWin(int i, int direction, int otherWay1, int otherWay2)
        {
            for (int i3 = 0; i3 < 4; i3++)
            {
                switch (direction)
                {
                    case 0:
                        if (values[i, i3, i3 * otherWay1 + otherWay2] != player)
                            return false;
                        //the otherway variables allow me to reuse this code to also check a reversed version of the winning condition (e.g. an upside down version of a diaganoal)
                        //this is by making otherWay1 negative one, and otherWay2 three
                        //it isn't very intuitive, but I thought this was more efficient than making an entirely new method or making three more cases
                        //returns false if any part of the winning condition isn't true
                        break;
                    case 1:
                        if (values[i3 * otherWay1 + otherWay2, i, i3] != player)
                            return false;
                        break;
                    case 2:
                        if (values[i3, i3 * otherWay1 + otherWay2, i] != player)
                            return false;
                        break;
                }
            }
            return true;
        }

        private Boolean threeDWin(int direction)
        {
            //for three dimensional wins, I chose not to use otherWay variable like in twoDWin because the code for threeDWin is shorter and already very complicated and unintuitive
            //I hardcoded all 4 possibilites even if two of them are just upside-down version of the other two
            for (int i = 0; i < 4; i++)
            {
                switch (direction)
                {
                    case 0:
                        if (values[i, i, i] != player)
                            return false;
                        //returns false if any part of the winning condition isn't true
                        break;
                    case 1:
                        if (values[i, 3-i, i] != player)
                            return false;
                        break;
                    case 2:
                        if (values[i, i, 3 - i] != player)
                            return false;
                        break;
                    case 3:
                        if (values[i, 3 - i, 3 - i] != player)
                            return false;
                        break;
                }
            }
            return true;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(board, 0, 0, board.Width, board.Height);
            //draws the board whenever the picture box invalidates
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Pen myPen = new Pen(Color.Black);
            Graphics g = Graphics.FromImage(board);
            int x = e.X - e.X % sideLength;
            int y = e.Y - e.Y % sideLength;
            //rounds the x and y values to the top left corner of each square
            if (x < sideLength * 16 && y < sideLength * 4)
            {
                //only runs code if mouse was clicked on the board
                int arrayX = (x % (sideLength * 4)) / sideLength;
                int arrayY = (y % (sideLength * 4)) / sideLength;
                int arrayZ = (x - x % (sideLength * 4)) / (sideLength * 4);
                //converts coordinate into location in array
                if (values[arrayX, arrayY, arrayZ] == 0)
                {
                    //only runs code if current square isn't already taken
                    if (player == 1)
                    {
                        drawX(x, y);
                        pictureBox1.Invalidate();
                        values[arrayX, arrayY, arrayZ] = 1;
                        //draws the players symbol and fills the array location with their value
                        if (hasWon())
                        {
                            string message = "Player " + player.ToString() + " has won";
                            MessageBox.Show(message);
                            gameReset();
                            //shows message to winning player and resets board
                        }
                        else if (hasTie())
                        {
                            string message = "It is a tie";
                            MessageBox.Show(message);
                            gameReset();
                            //shows tie message and resets board
                        }
                        else
                        {
                            player = 2;
                            //switches player
                        }
                    }
                    else
                    {
                        g.DrawEllipse(myPen, x, y, sideLength, sideLength);
                        pictureBox1.Invalidate();
                        values[arrayX, arrayY, arrayZ] = 2;
                        // draws the players symbol and fills the array location with their value
                        if (hasWon())
                        {
                            string message = "Player " + player.ToString() + " has won";
                            MessageBox.Show(message);
                            gameReset();
                            //shows message to winning player and resets board
                        }
                        else if (hasTie())
                        {
                            string message = "It is a tie";
                            MessageBox.Show(message);
                            gameReset();
                            //shows tie message and resets board
                        }
                        else
                        {
                            player = 1;
                            //switches player
                        }
                    }
                }
            }

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            gameReset();
        }
    }
}
