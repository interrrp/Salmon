using Chess;

namespace Salmon.Extensions;

public static class BoardExtensions
{
    public static IEnumerator<Piece> GetEnumerator(this ChessBoard board)
    {
        ArgumentNullException.ThrowIfNull(board);

        for (var row = 0; row < ChessBoard.MAX_ROWS; row++)
        {
            for (var col = 0; col < ChessBoard.MAX_COLS; col++)
            {
                var piece = board[row, col];
                if (piece != null) yield return piece;
            }
        }
    }
}
