using Chess;
using Salmon.Engine;
using Salmon.Uci;

namespace Salmon.Tests.Uci;

public static class Utils
{
    public static void AssertUciResponse(
        ref ChessBoard board, ref SalmonEngine engine,
        string command, params string[] expectedLines)
    {
        var response = UciInterface.Respond(ref board, ref engine, command);

        if (expectedLines.Length <= 0)
        {
            // Should be no response
            Assert.Empty(response);
            return;
        }

        var expected = string.Join("\n", expectedLines) + "\n";
        Assert.Equal(expected, response);
    }
}
