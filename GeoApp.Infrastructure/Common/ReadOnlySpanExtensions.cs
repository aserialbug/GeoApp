namespace GeoApp.Infrastructure.Common;

public static class ReadOnlySpanExtensions
{
    public static ReadOnlySpan<byte> TrimNullBytes(this ReadOnlySpan<byte> span)
    {
        int idx = 0;
        while (idx < span.Length && span[idx] != 0x00)
        {
            idx++;
        }

        return idx == span.Length ? span : span.Slice(0, idx);
    }
}