using System.Text;

namespace Salmon.Extensions;

public static class StringBuilderExtensions
{
    public static StringBuilder AppendLineLf(this StringBuilder sb, string s)
    {
        ArgumentNullException.ThrowIfNull(sb);
        return sb.Append(s).Append('\n');
    }
}
