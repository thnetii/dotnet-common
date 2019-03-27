using System.Buffers;
using THNETII.Common.Buffers;

namespace THNETII.Common.IO
{
    internal class CharBufferPipelineExtensions
    {
        internal static readonly ArrayMemoryPool<char> CharBufferPool =
            ArrayMemoryPool<char>.Shared;

        internal const int MinimumBufferSize = 4096;
    }
}
