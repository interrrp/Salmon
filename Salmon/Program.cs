using Chess;
using Salmon;

var board = new ChessBoard();
var engine = new Engine(board);
Uci.Run(ref board, ref engine);
