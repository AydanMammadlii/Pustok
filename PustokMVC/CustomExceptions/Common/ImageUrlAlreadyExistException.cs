namespace PustokMVC.CustomExceptions.Common;

public class ImageUrlAlreadyExistException: Exception
{
    public string PropertyName { get; set; }
    public ImageUrlAlreadyExistException() { }

    public ImageUrlAlreadyExistException(string? message) : base(message) { }

    public ImageUrlAlreadyExistException(string? propertyName, string? message) : base(message)
    {
        PropertyName = propertyName;
    }
}
