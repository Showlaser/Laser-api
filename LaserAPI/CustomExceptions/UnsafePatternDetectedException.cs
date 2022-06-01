using System;

namespace LaserAPI.CustomExceptions
{
    public class UnsafePatternDetectedException : Exception
    {
        public UnsafePatternDetectedException()
        {

        }

        public UnsafePatternDetectedException(string message) : base(message)
        {

        }

        public UnsafePatternDetectedException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
