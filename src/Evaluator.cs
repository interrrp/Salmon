using Chess;

namespace Salmon;

public static class Evaluator
{
    public static int Evaluate(this ChessBoard board)
    {
        if (board.IsEndGame)
        {
            var winner = board.EndGame?.WonSide;
            if (winner == PieceColor.White)
                return int.MaxValue;
            else if (winner == PieceColor.Black)
                return int.MinValue;
        }

        int evaluation = 0;
        foreach (var piece in board)
        {
            evaluation += EvaluatePiece(piece);
        }
        return evaluation;
    }

    private static int EvaluatePiece(Piece piece)
    {
        // 1 for white, -1 for black
        int sign = piece.Color == PieceColor.White ? 1 : -1;
        return GetPieceValue(piece.Type) * sign;
    }

    private static int GetPieceValue(PieceType type) => type.Name switch
    {
        "King" => 0,
        "Pawn" => 1,
        "Knight" or "Bishop" => 3,
        "Rook" => 5,
        "Queen" => 9,
        _ => 0,
    };
}
