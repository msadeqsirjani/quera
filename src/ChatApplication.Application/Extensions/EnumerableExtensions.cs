using System.Collections;

namespace ChatApplication.Application.Extensions;

public static class EnumerableExtensions
{
    public static bool Any(this IEnumerable @this)
    {
        var enumerator = @this.GetEnumerator();

        while (enumerator.MoveNext())
            return true;

        return false;
    }
}