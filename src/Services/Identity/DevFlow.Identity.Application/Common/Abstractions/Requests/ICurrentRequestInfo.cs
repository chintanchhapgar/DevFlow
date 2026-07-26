using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFlow.Identity.Application.Common.Abstractions.Requests
{
    public interface ICurrentRequestInfo
    {
        string? DeviceName { get; }
        string? Browser { get; }
        string? OperatingSystem { get; }
        string? IpAddress { get; }
        string? UserAgent { get; }
    }
}
