using System;

namespace Services
{
    public interface IPromptable<T>
    {
        Action<T> Success { get; set; }

        Action Cancel { get; set; }

        Action<Exception> Error { get; set; }
    }
}