using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace Actors.Interfaces
{
    /// <summary>
    /// This interface defines the methods exposed by an actor.
    /// Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface IActors : IActor
    {
        Task CreateActorAsync();
        Task AddFieldAsync(string fieldName);
        Task RemoveFieldAsync(string fieldName);
        Task<string> GetFieldValueAsync(string fieldName);
        Task SetFieldValueAsync(string fieldName, string fieldValue);
    }
}
