using System;
using System.Runtime.Serialization;

namespace KillDNS.CaptchaSolver.Core.Exceptions;

public class CaptchaException : Exception
{
    public CaptchaException()
    {
    }

    protected CaptchaException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public CaptchaException(string message) : base(message)
    {
    }

    public CaptchaException(string message, Exception innerException) : base(message, innerException)
    {
    }
}