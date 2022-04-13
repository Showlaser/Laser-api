using System;

namespace LaserAPI.CustomExceptions
{
    public class NoClientSoftwareFoundException : Exception
    {
        public NoClientSoftwareFoundException()
        {

        }

        public NoClientSoftwareFoundException(string message) : base(message)
        {

        }

        public NoClientSoftwareFoundException(string message, Exception inner)
        : base(message, inner)
        {

        }
    }
}
