using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using ClassLibrary;

namespace QuadraticProgramming.Interfaces
{
    /// <summary>
    /// This interface defines the methods exposed by an actor.
    /// Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface IQuadraticProgramming : IActor
    {
        Task<QuadraticProgrammingOutput> RunQPAsync(string jsonInput);
    }
}
