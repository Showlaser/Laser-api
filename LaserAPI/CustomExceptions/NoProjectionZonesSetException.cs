using System;

namespace LaserAPI.CustomExceptions
{
    public class NoProjectionZonesSetException : Exception
    {
        public NoProjectionZonesSetException()
        {

        }

        public NoProjectionZonesSetException(string message) : base(message)
        {

        }

        public NoProjectionZonesSetException(string message, Exception inner)
        : base(message, inner)
        {

        }
    }
}
