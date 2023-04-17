namespace Vendor.Services.Machines.Exceptions;

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