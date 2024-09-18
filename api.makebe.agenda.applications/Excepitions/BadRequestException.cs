namespace api.makebe.agenda.applications.Exceptions;
public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
    }
}
