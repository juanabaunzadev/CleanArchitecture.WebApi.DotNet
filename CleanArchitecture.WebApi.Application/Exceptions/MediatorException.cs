namespace CleanArchitecture.WebApi.Application.Exceptions;

public class MediatorException : Exception
{
    public MediatorException(string message) : base(message) { }
}