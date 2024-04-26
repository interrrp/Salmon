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
            foreach (var entry in engine.Options)
            {
                var name = entry.Key;
                var defaultVal = entry.Value;

                response.Append($"option name {name} ");

                switch (defaultVal)
                {
                    case int:
                        response.AppendLineLf($"type spin default {defaultVal} min {int.MinValue} max {int.MaxValue}");
                        break;
                    case bool:
                        // We have to make a special case for booleans since ToString on them
                        // returns True/False and not true/false which is needed for UCI
                        response.AppendLineLf($"type check default {defaultVal.ToString()!.ToLower()}");
                        break;
                    case string:
                        response.AppendLineLf($"type string default {defaultVal}");
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            response.AppendLineLf("uciok");
        }
        else if (command == "isready")
        {
            response.AppendLineLf("readyok");
        }
        else if (command.StartsWith("setoption") && parts.Length == 5)
        {
            var name = parts[2];
            var newValue = parts[4];
            if (engine.Options.TryGetValue(name, out var oldValue))
            {
                switch (oldValue)
                {
                    case int:
                        engine.Options[name] = int.Parse(newValue);
                        break;
                    case bool:
                        engine.Options[name] = bool.Parse(newValue);
                        break;
                    default:
                        engine.Options[name] = newValue;
                        break;
                }
            }
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
