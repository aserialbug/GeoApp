using System.Runtime.InteropServices;
using System.Text;

namespace GeoApp.Infrastructure.Model;

internal record DatabaseHeader(
    int Version,
    string Name,
    DateTime CreatedAt,
    int RecordCount,
    uint RangesArrayOffset,
    uint LocationsIndexOffset,
    uint LocationArrayOffset);