using System;
using System.Security.Cryptography;


namespace murmur
{
    public abstract class MurmurBase : HashAlgorithm
    {

        public MurmurBase(uint seed = 0)
        {
            Seed = seed;
        }

        protected uint Seed { get; }

        protected int Length { get; set; }

        protected abstract void Reset();

        public override void Initialize()
        {
            Reset();
        }
    }
}
