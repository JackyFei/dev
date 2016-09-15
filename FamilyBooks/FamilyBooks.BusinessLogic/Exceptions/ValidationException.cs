using System;
namespace FamilyBooks.BusinessLogic.Exceptions
{
    public class ValidationException: FamilyBooksException
    {
        public ValidationErrorCode ValidationErrorCode { get; private set; }

        public ValidationException(string message, ValidationErrorCode validationErrorCode) : base(message)
        {
            ValidationErrorCode = validationErrorCode;
        }
    }
}
