using System;
using System.Collections.Generic;
using System.Drawing;
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
        bool gameOver = false;

        public Form1()
        {
            InitializeComponent();
            CreateBoard();
            SetupPieces();

            this.Text = "Red's Turn";
            this.ClientSize = new Size(520, 520);
        }

        void CreateBoard()
        {
            int size = 60;
            int offsetX = 20;
            int offsetY = 20;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    P[i, j] = new PictureBox();
                    P[i, j].Width = size;
                    P[i, j].Height = size;
                    P[i, j].Left = j * size + offsetX;
                    P[i, j].Top = i * size + offsetY;

                    P[i, j].BackColor =
                        (i + j) % 2 == 0
                        ? Color.White
                        : Color.Black;

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
                    board[i, j] = 0;

                    if ((i + j) % 2 == 1)
                    {
                        if (i < 3)
                        {
                            board[i, j] = 2;
                            P[i, j].Image =
                                MakeTransparent(
                                    new Bitmap(Properties.Resources.B));
                        }
                        else if (i > 4)
                        {
                            board[i, j] = 1;
                            P[i, j].Image =
                                MakeTransparent(
                                    new Bitmap(Properties.Resources.R));
                        }
                    }
                }
            }
        }

        void Cell_Click(object sender, EventArgs e)
        {
            if (gameOver)
                return;

            PictureBox clicked = sender as PictureBox;

            int row = (int)clicked.Tag / 8;
            int col = (int)clicked.Tag % 8;

            if (selectedRow == -1)
            {
                if (board[row, col] == 0)
                    return;

                if (board[row, col] == (isRedTurn ? 1 : 2) ||
                    board[row, col] == (isRedTurn ? 3 : 4))
                {
                    selectedRow = row;
                    selectedCol = col;

                    ResetBoardColors();
                    P[row, col].BackColor = Color.Yellow;
                }
            }
            else
            {
                TryMove(selectedRow, selectedCol, row, col);

                selectedRow = -1;
                selectedCol = -1;

                ResetBoardColors();
            }
        }

        public void TryMove(int sr, int sc, int dr, int dc, bool computerMove = false)
        {
            if (gameOver)
                return;

            if (!IsValidMove(sr, sc, dr, dc))
            {
                MessageBox.Show("Invalid move");
                return;
            }

            int piece = board[sr, sc];

            board[dr, dc] = piece;
            board[sr, sc] = 0;

            P[dr, dc].Image = P[sr, sc].Image;
            P[sr, sc].Image = null;

            // capture
            if (Math.Abs(dr - sr) == 2)
            {
                int midRow = (sr + dr) / 2;
                int midCol = (sc + dc) / 2;

                board[midRow, midCol] = 0;
                P[midRow, midCol].Image = null;
            }

            // king promotion
            if (dr == 0 && piece == 1)
            {
                board[dr, dc] = 3;
                P[dr, dc].Image =
                    MakeTransparent(
                        new Bitmap(Properties.Resources.RK));
            }

            if (dr == 7 && piece == 2)
            {
                board[dr, dc] = 4;
                P[dr, dc].Image =
                    MakeTransparent(
                        new Bitmap(Properties.Resources.BK));
            }

            // winner check
            CheckWinner();

            if (gameOver)
                return;

            isRedTurn = !isRedTurn;

            this.Text =
                isRedTurn ? "Red's Turn" : "Black's Turn";

            if (!isRedTurn && !computerMove)
            {
                MakeComputerMove();
            }
        }

        void CheckWinner()
        {
            bool redExists = false;
            bool blackExists = false;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j] == 1 || board[i, j] == 3)
                        redExists = true;

                    if (board[i, j] == 2 || board[i, j] == 4)
                        blackExists = true;
                }
            }

            if (!redExists)
            {
                gameOver = true;
                MessageBox.Show("Black wins!");
            }

            else if (!blackExists)
            {
                gameOver = true;
                MessageBox.Show("Red wins!");
            }
        }

        bool IsValidMove(int sr, int sc, int dr, int dc)
        {
            if (dr < 0 || dr >= 8 || dc < 0 || dc >= 8)
                return false;

            if (board[dr, dc] != 0)
                return false;

            int piece = board[sr, sc];
            int direction = (piece == 1) ? -1 : 1;

            if (piece == 3 || piece == 4)
                direction = 0;

            if (Math.Abs(dr - sr) == 1 &&
                Math.Abs(dc - sc) == 1)
            {
                if (piece == 3 || piece == 4)
                    return true;

                if (dr == sr + direction)
                    return true;
            }

            if (Math.Abs(dr - sr) == 2 &&
                Math.Abs(dc - sc) == 2)
            {
                int midRow = (sr + dr) / 2;
                int midCol = (sc + dc) / 2;

                int midPiece = board[midRow, midCol];

                if (midPiece != 0 &&
                    midPiece != piece &&
                    !IsSameTeam(piece, midPiece))
                {
                    if (piece == 3 || piece == 4)
                        return true;

                    if (dr == sr + 2 * direction)
                        return true;
                }
            }

            return false;
        }

        bool IsSameTeam(int a, int b)
        {
            if ((a == 1 || a == 3) &&
                (b == 1 || b == 3))
                return true;

            if ((a == 2 || a == 4) &&
                (b == 2 || b == 4))
                return true;

            return false;
        }

        void ResetBoardColors()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    P[i, j].BackColor =
                        (i + j) % 2 == 0
                        ? Color.White
                        : Color.Black;
                }
            }
        }

        Bitmap MakeTransparent(Bitmap bmp)
        {
            bmp.MakeTransparent(Color.White);
            return bmp;
        }

        Random rand = new Random();

        public List<(int sr, int sc, int dr, int dc)> GetAllValidMoves(bool forRed)
        {
            var moves = new List<(int, int, int, int)>();

            for (int sr = 0; sr < 8; sr++)
            {
                for (int sc = 0; sc < 8; sc++)
                {
                    int piece = board[sr, sc];

                    if (piece == 0)
                        continue;

                    bool isRedPiece =
                        (piece == 1 || piece == 3);

                    if (isRedPiece != forRed)
                        continue;

                    for (int dr = 0; dr < 8; dr++)
                    {
                        for (int dc = 0; dc < 8; dc++)
                        {
                            if (IsValidMove(sr, sc, dr, dc))
                            {
                                moves.Add((sr, sc, dr, dc));
                            }
                        }
                    }
                }
            }

            return moves;
        }

        public void MakeComputerMove()
        {
            var moves = GetAllValidMoves(false);

            if (moves.Count == 0)
            {
                gameOver = true;
                MessageBox.Show("Red wins!");
                return;
            }

            var move = moves[rand.Next(moves.Count)];

            TryMove(
                move.sr,
                move.sc,
                move.dr,
                move.dc,
                true);
        }
    }
}