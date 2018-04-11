using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using Actors.Interfaces;
using ClassLibrary;

namespace Actors
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    internal class Actors : Actor, IActors
    {
        /// <summary>
        /// Initializes a new instance of Actors
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public Actors(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }
        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actors Actor activated.");
            return base.OnActivateAsync();
        }

        Task IActors.CreateActorAsync()
        {
            return Task.FromResult(0);
        }

        Task IActors.AddVariableAsync(ActorData actorData )
        {
            this.StateManager.TryAddStateAsync(actorData.VariableName, "");
            this.StateManager.SetStateAsync(actorData.VariableName, actorData.VariableValue);
            return Task.FromResult(0);
        }

        Task IActors.RemoveVariableAsync(string variableName)
        {
            this.StateManager.TryRemoveStateAsync(variableName);
            return Task.FromResult(0);
        }

        Task<string> IActors.GetVariableValueAsync(string variableName)
        {
            var variableValue = this.StateManager.GetStateAsync<string>(variableName);
            return variableValue;
        }

        Task IActors.SetVariableValueAsync(string variableName, string variableValue)
        {
            this.StateManager.SetStateAsync(variableName, variableValue);
            return Task.FromResult(0);
        }
    }
}
