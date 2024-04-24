using Chess;

namespace Salmon;

public static class Evaluator
{
    public static int Evaluate(this ChessBoard board)
    {
        int evaluation = 0;
        foreach (var piece in board)
        {
            if (piece == null) continue;
            int sign = piece.Color == PieceColor.White ? 1 : -1;
            evaluation += GetPieceValue(piece.Type) * sign;
        }
        return evaluation;
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
