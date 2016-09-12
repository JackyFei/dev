using System;

namespace FamilyBooks.BusinessLogic.Exceptions
{
    [Serializable]
    public class FamilyBooksException : ApplicationException
    {
        public FamilyBooksException(string message) : base(message)
        {
        }

        public FamilyBooksException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}