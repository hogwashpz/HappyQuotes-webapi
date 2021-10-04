using System;

namespace HappyQuotes.Application.Exceptions
{
    public class NoResultException : Exception
    {
        public NoResultException()
        {
        }

        public NoResultException(string? message) : base(message)
        {
        }

        public NoResultException(string? message, Exception? inner) : base(message, inner)
        {
        }
    }
}
