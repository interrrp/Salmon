using Chess;
using Salmon.Engine;
using Salmon.Uci;

var board = new ChessBoard();
var engine = new SalmonEngine(board);
UciInterface.Run(ref board, ref engine);
