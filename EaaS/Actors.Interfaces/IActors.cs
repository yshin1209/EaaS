using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using ClassLibrary;

namespace Actors.Interfaces
{
    /// <summary>
    /// This interface defines the methods exposed by an actor.
    /// Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface IActors : IActor
    {
        Task CreateActorAsync();
        Task AddVariableAsync(ActorVariable actorData);
        Task RemoveVariableAsync(string fieldName);
        Task<string> GetVariableValueAsync(string fieldName);
        Task SetVariableValueAsync(string fieldName, string fieldValue);
        Task<ActorMethod> ExecuteMethodAsync(ActorMethod actorMethod);
    }
}
