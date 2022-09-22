using GeoApp.Application.Model;

namespace GeoApp.Infrastructure.Common;

public static class BiSearch
{
    public static int Find(int[] array, int value)
    {
        var hi = array.Length - 1;
        var lo = 0;
        while (hi >= lo)
        {
            var cu = lo + (hi - lo) / 2;
            var comp = CompareTo(value, array[cu]);
            if (comp == 0)
                return cu;

            if (comp > 0)
                hi = cu - 1;
            if (comp < 0)
                lo = cu + 1;
        }
        return -1;
    }

    private static int CompareTo(int value, int other)
    {
        if (value < other)
            return 1;
        if (value > other)
            return -1;

        return 0;
    }
}