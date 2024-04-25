using Chess;

namespace Salmon;

public static class PieceColorExtensions
{
    public static int BestScore(this PieceColor? color)
    {
        return color == PieceColor.White ? int.MaxValue : int.MinValue;
    }

    public static int WorstScore(this PieceColor? color)
    {
        return color == PieceColor.White ? int.MinValue : int.MaxValue;
    }
}
