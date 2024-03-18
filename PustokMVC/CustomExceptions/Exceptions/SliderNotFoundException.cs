namespace PustokMVC.CustomExceptions.Exceptions
{
    public class SliderNotFoundException: Exception
    {
        public SliderNotFoundException()
        {
        }

        public SliderNotFoundException(string? message) : base(message)
        {
        }
    }
}
