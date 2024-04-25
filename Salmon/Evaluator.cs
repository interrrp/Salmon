using Chess;

namespace Salmon;

public static class Evaluator
{
    public static int Evaluate(this ChessBoard board)
    {
        ArgumentNullException.ThrowIfNull(board);

        if (board.EndGame != null)
        {
            // Game is finished, favor the winning side
            return board.EndGame.WonSide.BestScore();
        }

        var evaluation = 0;
        foreach (var piece in board)
        {
            evaluation += EvaluatePiece(piece);
        }
        return evaluation;
    }

    private static int EvaluatePiece(Piece piece)
    {
        // 1 for white, -1 for black
        var sign = piece.Color == PieceColor.White ? 1 : -1;
        return GetPieceMaterial(piece.Type) * sign;
    }

    private static int GetPieceMaterial(PieceType type) => type.Name switch
    {
        "King" => 0,
        "Pawn" => 1,
        "Knight" or "Bishop" => 3,
        "Rook" => 5,
        "Queen" => 9,
        _ => 0,
    };
}
