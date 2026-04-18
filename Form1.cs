using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checkers
{
    public partial class Form1 : Form
    {

        private void Form1_Load(object sender, EventArgs e)
        { }

        PictureBox[,] P = new PictureBox[8, 8];
        int[,] board = new int[8, 8];

        int selectedRow = -1;
        int selectedCol = -1;

        bool isRedTurn = true;

        public Form1()
        {
            InitializeComponent();
            CreateBoard();
            SetupPieces();
        }

        void CreateBoard()
        {
            int size = 60;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    P[i, j] = new PictureBox();
                    P[i, j].Width = size;
                    P[i, j].Height = size;
                    P[i, j].Left = j * size;
                    P[i, j].Top = i * size;

                    P[i, j].BackColor = (i + j) % 2 == 0 ? Color.White : Color.Black;
                    P[i, j].SizeMode = PictureBoxSizeMode.Zoom;

                    P[i, j].Tag = i * 8 + j;
                    P[i, j].Click += Cell_Click;

                    this.Controls.Add(P[i, j]);
                }
            }
        }

        void SetupPieces()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((i + j) % 2 == 1)
                    {
                        if (i < 3)
                        {
                            board[i, j] = 2; // black
                            P[i, j].Image = MakeTransparent(new Bitmap(Properties.Resources.B));
                        }
                        else if (i > 4)
                        {
                            board[i, j] = 1; // red
                            P[i, j].Image = MakeTransparent(new Bitmap(Properties.Resources.R));
                        }
                    }
                }
            }
        }

        void Cell_Click(object sender, EventArgs e)
        {
            PictureBox clicked = sender as PictureBox;

            int row = (int)clicked.Tag / 8;
            int col = (int)clicked.Tag % 8;

            if (selectedRow == -1)
            {
                if (board[row, col] == (isRedTurn ? 1 : 2) ||
                    board[row, col] == (isRedTurn ? 3 : 4))
                {
                    selectedRow = row;
                    selectedCol = col;
                }
            }
            else
            {
                TryMove(selectedRow, selectedCol, row, col);
                selectedRow = -1;
                selectedCol = -1;
            }
        }

        void TryMove(int sr, int sc, int dr, int dc)
        {
            

            int piece = board[sr, sc];

            // Move piece
            board[dr, dc] = piece;
            board[sr, sc] = 0;

            P[dr, dc].Image = P[sr, sc].Image;
            P[sr, sc].Image = null;

            // Capture
            if (Math.Abs(dr - sr) == 2)
            {
                int midRow = (sr + dr) / 2;
                int midCol = (sc + dc) / 2;

                board[midRow, midCol] = 0;
                P[midRow, midCol].Image = null;
            }

            // King promotion
            if (dr == 0 && piece == 1)
                board[dr, dc] = 3;

            if (dr == 7 && piece == 2)
                board[dr, dc] = 4;

            isRedTurn = !isRedTurn;
        }

        Bitmap MakeTransparent(Bitmap bmp)
        {
            bmp.MakeTransparent(Color.White);
            return bmp;
        }
    }


}


