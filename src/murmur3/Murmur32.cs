using System;


namespace murmur
{
    public class Murmur32 : MurmurBase
    {
        private const uint C1 = 0xcc9e2d51;
        private const uint C2 = 0x1b873593;


        public Murmur32(uint seed = 0) : base(seed)
        {
        }


        private uint H1 { get; set; }

        public override int HashSize => 32;


        protected override void Reset()
        {
            H1 = Seed;
            Length = 0;
        }


        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            Length += cbSize;
            var remainder = cbSize & 3;
            var alignedLength = ibStart + (cbSize - remainder);

            for (var i = ibStart; i < alignedLength; i += 4)
            {
                H1 = H1 ^ (RotateLeft(ToUInt32(array, i) * C1, 15) * C2);
                H1 = RotateLeft(H1, 13) * 5 + 0xe6546b64;
            }

            if (remainder > 0)
            {
                Tail(array, alignedLength, remainder);
            }
        }


        private void Tail(byte[] tail, int position, int remainder)
        {
            // create our keys and initialize to 0
            uint k1 = 0;

            // determine how many bytes we have left to work with based on length
            switch (remainder)
            {
                case 3:
                    k1 ^= (uint) tail[position + 2] << 16;
                    goto case 2;
                case 2:
                    k1 ^= (uint) tail[position + 1] << 8;
                    goto case 1;
                case 1:
                    k1 ^= tail[position];

                    break;
            }

            H1 ^= RotateLeft(k1 * C1, 15) * C2;
        }


        protected override byte[] HashFinal()
        {
            H1 = FMix(H1 ^ (uint) Length);

            return BitConverter.GetBytes(H1);
        }


        private static uint ToUInt32(byte[] data, int start)
        {
            return BitConverter.IsLittleEndian
                ? (uint) (data[start] | (data[start + 1] << 8) | (data[start + 2] << 16) | (data[start + 3] << 24))
                : (uint) ((data[start] << 24) | (data[start + 1] << 16) | (data[start + 2] << 8) | data[start + 3]);
        }


        private static uint RotateLeft(uint x, byte r)
        {
            return (x << r) | (x >> (32 - r));
        }


        private static uint FMix(uint h)
        {
            h = (h ^ (h >> 16)) * 0x85ebca6b;
            h = (h ^ (h >> 13)) * 0xc2b2ae35;

            return h ^ (h >> 16);
        }
    }
}
