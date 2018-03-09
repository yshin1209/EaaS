using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace PID.Interfaces
{
    /// <summary>
    /// This interface defines the methods exposed by the PID actor.
    /// Clients use this interface to interact with the PID actor that implements it.
    /// </summary>
    public interface IPID : IActor
    {
        Task<double> RunPIDAsync(bool reset, double actualValue, double desiredValue, double Kp, double Ki, double Kd);
    }
}
