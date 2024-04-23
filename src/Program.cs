using Chess;

var board = new ChessBoard();
var errored = false;

while (!board.IsEndGame)
{
    Console.Clear();

    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine(board.ToPgn() + "\n");

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

