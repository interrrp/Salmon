using Chess;
using Salmon;

var board = new ChessBoard();
var engine = new Engine(board);
var errored = false;

while (!board.IsEndGame)
{
    if (board.Turn == PieceColor.Black)
    {
        engine.Move();
        continue;
    }

    Console.Clear();

    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine(board.ToPgn() + "\n");

    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.Write($"eval ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine(board.Evaluate() + "\n");

    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine(board.ToAscii());

    Console.ForegroundColor = errored ? ConsoleColor.Red : ConsoleColor.Blue;
    Console.Write("san → ");
    Console.ForegroundColor = ConsoleColor.White;
    try
    {
        board.Move(Console.ReadLine()!);
        errored = false;
    }
    catch (Exception)
    {
        errored = true;
    }
}

