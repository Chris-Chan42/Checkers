using NUnit.Framework;

[TestFixture]
public class CheckersTests
{
    [Test]
    public void ValidMove_MovesPieceCorrectly()
    {
        var game = new CheckersGame();

        for (int r = 0; r < 8; r++)
            for (int c = 0; c < 8; c++)
                game.board[r, c] = 0;

        game.board[5, 0] = 1;

        game.TryMove(5, 0, 4, 1);

        Assert.That(game.board[4, 1], Is.EqualTo(1));
        Assert.That(game.board[5, 0], Is.EqualTo(0));
    }

    [Test]
    public void InvalidMove_DoesNotMovePiece()
    {
        var game = new CheckersGame();

        for (int r = 0; r < 8; r++)
            for (int c = 0; c < 8; c++)
                game.board[r, c] = 0;

        game.board[5, 0] = 1;

        game.TryMove(5, 0, 5, 1);

        Assert.That(game.board[5, 0], Is.EqualTo(1));
        Assert.That(game.board[5, 1], Is.EqualTo(0));
    }

    [Test]
    public void Capture_RemovesEnemyPiece()
    {
        var game = new CheckersGame();

        for (int r = 0; r < 8; r++)
            for (int c = 0; c < 8; c++)
                game.board[r, c] = 0;

        game.board[5, 0] = 1;
        game.board[4, 1] = 2;

        game.TryMove(5, 0, 3, 2);

        Assert.That(game.board[3, 2], Is.EqualTo(1));
        Assert.That(game.board[4, 1], Is.EqualTo(0));
        Assert.That(game.board[5, 0], Is.EqualTo(0));
    }

    [Test]
    public void OutOfBoundsMove_DoesNotMovePiece()
    {
        var game = new CheckersGame();

        for (int r = 0; r < 8; r++)
            for (int c = 0; c < 8; c++)
                game.board[r, c] = 0;

        game.board[5, 0] = 1;

        game.TryMove(5, 0, 8, 1);

        Assert.That(game.board[5, 0], Is.EqualTo(1));
    }

    [Test]
    public void PieceBecomesKing_WhenReachingEnd()
    {
        var game = new CheckersGame();

        for (int r = 0; r < 8; r++)
            for (int c = 0; c < 8; c++)
                game.board[r, c] = 0;

        game.board[1, 2] = 1;

        game.TryMove(1, 2, 0, 3);

        Assert.That(game.board[0, 3], Is.EqualTo(3));
    }

    [Test]
    public void RegularPiece_CannotMoveBackwards()
    {
        var game = new CheckersGame();

        for (int r = 0; r < 8; r++)
            for (int c = 0; c < 8; c++)
                game.board[r, c] = 0;

        game.board[5, 0] = 1;

        game.TryMove(5, 0, 6, 1);

        Assert.That(game.board[5, 0], Is.EqualTo(1));
        Assert.That(game.board[6, 1], Is.EqualTo(0));
    }

    [Test]
    public void Piece_CannotMoveOntoOccupiedSpace()
    {
        var game = new CheckersGame();

        for (int r = 0; r < 8; r++)
            for (int c = 0; c < 8; c++)
                game.board[r, c] = 0;

        game.board[5, 0] = 1;
        game.board[4, 1] = 2;

        game.TryMove(5, 0, 4, 1);

        Assert.That(game.board[5, 0], Is.EqualTo(1));
        Assert.That(game.board[4, 1], Is.EqualTo(2));
    }

    [Test]
    public void King_CanMoveBackwards()
    {
        var game = new CheckersGame();

        for (int r = 0; r < 8; r++)
            for (int c = 0; c < 8; c++)
                game.board[r, c] = 0;

        game.board[3, 2] = 3;

        game.TryMove(3, 2, 4, 3);

        Assert.That(game.board[4, 3], Is.EqualTo(3));
        Assert.That(game.board[3, 2], Is.EqualTo(0));
    }

    [Test]
    public void Piece_CannotCaptureOwnTeam()
    {
        var game = new CheckersGame();

        for (int r = 0; r < 8; r++)
            for (int c = 0; c < 8; c++)
                game.board[r, c] = 0;

        game.board[5, 0] = 1;
        game.board[4, 1] = 1;

        game.TryMove(5, 0, 3, 2);

        Assert.That(game.board[5, 0], Is.EqualTo(1));
        Assert.That(game.board[4, 1], Is.EqualTo(1));
        Assert.That(game.board[3, 2], Is.EqualTo(0));
    }
}