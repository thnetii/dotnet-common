using System.Text;
using Microsoft.Extensions.ObjectPool;

namespace THNETII.Common.Buffers.Text
{
    internal class AsciiEncodingPool : IPooledObjectPolicy<Encoder>, IPooledObjectPolicy<Decoder>
    {
        private static readonly AsciiEncodingPool instance = new AsciiEncodingPool();
        private readonly ObjectPool<Encoder> encoderPool;
        private readonly ObjectPool<Decoder> decoderPool;

        public AsciiEncodingPool()
        {
            encoderPool = new DefaultObjectPool<Encoder>(this);
            decoderPool = new DefaultObjectPool<Decoder>(this);
        }

        public static ObjectPool<Encoder> EncoderPool => instance.encoderPool;

        public static ObjectPool<Decoder> DecoderPool => instance.decoderPool;

        Encoder IPooledObjectPolicy<Encoder>.Create() =>
            Encoding.ASCII.GetEncoder();

        Decoder IPooledObjectPolicy<Decoder>.Create() =>
            Encoding.ASCII.GetDecoder();

        bool IPooledObjectPolicy<Encoder>.Return(Encoder encoder)
        {
            if (encoder is null)
                return false;
            encoder.Reset();
            return true;
        }

        bool IPooledObjectPolicy<Decoder>.Return(Decoder decorder)
        {
            if (decorder is null)
                return false;
            decorder.Reset();
            return true;
        }
    }
}
