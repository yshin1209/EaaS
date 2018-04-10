using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using Actors.Interfaces;

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

        Task IActors.AddFieldAsync(string fieldName)
        {
            this.StateManager.TryAddStateAsync(fieldName, "");
            return Task.FromResult(0);
        }

        Task IActors.RemoveFieldAsync(string fieldName)
        {
            this.StateManager.TryRemoveStateAsync(fieldName);
            return Task.FromResult(0);
        }

        Task<string> IActors.GetFieldValueAsync(string fieldName)
        {
            var fieldValue = this.StateManager.GetStateAsync<string>(fieldName);
            return fieldValue;
        }

        Task IActors.SetFieldValueAsync(string fieldName, string fieldValue)
        {

            this.StateManager.SetStateAsync(fieldName, fieldValue);
            return Task.FromResult(0);
        }
    }
}
