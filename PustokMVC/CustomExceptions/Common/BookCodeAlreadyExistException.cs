namespace PustokMVC.CustomExceptions.Common
{
    public class BookCodeAlreadyExistException: Exception
    {
        public string PropertyName { get; set; }
        public BookCodeAlreadyExistException() { }

        public BookCodeAlreadyExistException(string? message) : base(message) { }

        public BookCodeAlreadyExistException(string? propertyName, string? message) : base(message)
        {
            PropertyName = propertyName;
        }
    }
}
