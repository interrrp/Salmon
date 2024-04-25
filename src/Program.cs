using Chess;
using Salmon;

var board = new ChessBoard();
var engine = new Engine(board);
var errored = false;

Console.Clear();
Console.WriteLine("The engine plays as...");
Console.ForegroundColor = ConsoleColor.Magenta;
Console.Write("↑ ");
Console.ForegroundColor = ConsoleColor.White;
Console.Write("Black    ");
Console.ForegroundColor = ConsoleColor.Yellow;
Console.Write("↓ ");
Console.ForegroundColor = ConsoleColor.White;
Console.Write("White");
var engineColor = Console.ReadKey().Key switch
{
    ConsoleKey.UpArrow => PieceColor.Black,
    ConsoleKey.DownArrow => PieceColor.White,
    _ => PieceColor.Black,
};

while (!board.IsEndGame)
{
    if (board.Turn == engineColor)
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
