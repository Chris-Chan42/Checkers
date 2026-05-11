public class CheckersGame
{
    public int[,] board = new int[8, 8];
    public bool isRedTurn = true;

    public CheckersGame()
    {
        SetupPieces();
    }

    // places all starting pieces on the board
    public void SetupPieces()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                board[i, j] = 0;

                if ((i + j) % 2 == 1)
                {
                    if (i < 3)
                        board[i, j] = 2;
                    else if (i > 4)
                        board[i, j] = 1;
                }
            }
        }
    }

    // attempts to move a piece if the move is valid
    public void TryMove(int sr, int sc, int dr, int dc)
    {
        if (!IsValidMove(sr, sc, dr, dc))
            return;

        int piece = board[sr, sc];

        board[dr, dc] = piece;
        board[sr, sc] = 0;

        if (System.Math.Abs(dr - sr) == 2)
        {
            int midRow = (sr + dr) / 2;
            int midCol = (sc + dc) / 2;
            board[midRow, midCol] = 0;
        }

        if (dr == 0 && piece == 1)
            board[dr, dc] = 3;

        if (dr == 7 && piece == 2)
            board[dr, dc] = 4;

        isRedTurn = !isRedTurn;
    }

    // checks whether a move follows checkers rules
    public bool IsValidMove(int sr, int sc, int dr, int dc)
    {
        if (dr < 0 || dr >= 8 || dc < 0 || dc >= 8)
            return false;

        if (board[dr, dc] != 0)
            return false;

        int piece = board[sr, sc];
        int direction = (piece == 1) ? -1 : 1;

        if (piece == 3 || piece == 4)
            direction = 0;

        if (System.Math.Abs(dr - sr) == 1 && System.Math.Abs(dc - sc) == 1)
        {
            if (piece == 3 || piece == 4)
                return true;

            if (dr == sr + direction)
                return true;
        }

        if (System.Math.Abs(dr - sr) == 2 && System.Math.Abs(dc - sc) == 2)
        {
            int midRow = (sr + dr) / 2;
            int midCol = (sc + dc) / 2;
            int midPiece = board[midRow, midCol];

            if (midPiece != 0 && !IsSameTeam(piece, midPiece))
            {
                if (piece == 3 || piece == 4)
                    return true;

                if (dr == sr + 2 * direction)
                    return true;
            }
        }

        return false;
    }

    // returns true if both pieces belong to the same player
    private bool IsSameTeam(int a, int b)
    {
        if ((a == 1 || a == 3) && (b == 1 || b == 3))
            return true;

        if ((a == 2 || a == 4) && (b == 2 || b == 4))
            return true;

        return false;
    }
}