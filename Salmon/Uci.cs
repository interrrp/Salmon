using System.Diagnostics;
using System.Text;
using Chess;

namespace Salmon;

public static class Uci
{
    public static void Run(ref ChessBoard board, ref Engine engine)
    {
        ArgumentNullException.ThrowIfNull(board);

        while (true)
        {
            var command = Console.ReadLine();
            if (command == null)
                break;
            var response = Respond(ref board, ref engine, command);
            Console.Write(response);
        }
    }

    public static string Respond(ref ChessBoard board, ref Engine engine, string command)
    {
        ArgumentNullException.ThrowIfNull(board);
        ArgumentNullException.ThrowIfNull(engine);
        ArgumentNullException.ThrowIfNull(command);

        var parts = command.Split();

        var response = new StringBuilder();

        if (command == "uci")
        {
            response.AppendLineLf($"id name {Engine.Name}");
            response.AppendLineLf($"id author {Engine.Author}");
            response.AppendLineLf("uciok");
        }
        else if (command == "isready")
        {
            response.AppendLineLf("readyok");
        }
        else if (command.StartsWith("position") && parts.Length > 1)
        {
            if (parts[1] == "startpos")
            {
                board.Clear();
            }
            else
            {
                var fen = string.Join(' ', parts.Skip(1).TakeWhile(part => part != "moves"));
                board = ChessBoard.LoadFromFen(fen);
            }

            var moves = parts.SkipWhile(part => part != "moves").Skip(1);
            foreach (var move in moves)
                board.MoveLan(move);

        }
        else if (command.StartsWith("go"))
        {
            var move = engine.Move();
            response.AppendLineLf($"bestmove {move.San}");
        }
        else if (command == "quit")
        {
            Environment.Exit(0);
        }

        return response.ToString();
    }
}
