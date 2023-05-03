namespace Vendor.Services.Machines.Domain.Exceptions;

public class MachinesDomainException : Exception
{
    public MachinesDomainException()
    {
    }

    public MachinesDomainException(string? message) : base(message)
    {
    }

    public MachinesDomainException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}