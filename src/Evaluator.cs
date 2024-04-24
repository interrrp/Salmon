using Chess;

namespace Salmon;

public static class Evaluator
{
    public static float Evaluate(this ChessBoard board)
    {
        float evaluation = 0;
        foreach (var piece in board)
        {
            if (piece == null) continue;
            int sign = piece.Color == PieceColor.White ? 1 : -1;
            evaluation += piece.Type.Value * sign;
        }
        return evaluation;
    }
}
