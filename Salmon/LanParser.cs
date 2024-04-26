using System.Text.RegularExpressions;
using Chess;

namespace Salmon;

public static class LanParser
{
    // This exists because Gera.Chess does not parse long algebraic notation
    // correctly and fails on "g1f3" for instance

    // TODO: Open an issue for this at https://github.com/Geras1mleo/Chess

    public static Move ParseFromLan(this ChessBoard board, string lan)
    {
        ArgumentNullException.ThrowIfNull(board);
        ArgumentNullException.ThrowIfNull(lan);

        if (!Regex.IsMatch(lan, """([a-z]\d){2}"""))
        {
            // Gera.Chess can parse this fine
            // Notations specifying two positions, e.g. "g1f3", will fail
            return board.ParseFromSan(lan);
        }

        var fromPos = new Position(lan.Substring(0, 2));
        var toPos = new Position(lan.Substring(2, 2));

        return new Move(fromPos, toPos);
    }

    public static void MoveLan(this ChessBoard board, string lan)
    {
        ArgumentNullException.ThrowIfNull(board);
        board.Move(board.ParseFromLan(lan));
    }
}

