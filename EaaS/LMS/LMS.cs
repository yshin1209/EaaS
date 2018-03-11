using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using LMS.Interfaces;

namespace LMS
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
    internal class LMS : Actor, ILMS
    {
        double[] result = new double[] { 0, 0, 0 };
        /// <summary>
        /// Initializes a new instance of LMS
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public LMS(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }
        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");

            // The StateManager is this actor's private state store.
            // Data stored in the StateManager will be replicated for high-availability for actors that use volatile or persisted state storage.
            // Any serializable object can be saved in the StateManager.
            // For more information, see https://aka.ms/servicefabricactorsstateserialization
            ActorEventSource.Current.ActorMessage(this, "LMS Actor activated.");
            this.StateManager.TryAddStateAsync<int>("timeDelay", 0);
            this.StateManager.TryAddStateAsync<int>("counter", 0);
            this.StateManager.TryAddStateAsync<List<double>>("inputValues", new List<double>());
            this.StateManager.TryAddStateAsync<double>("estimatedParameter", 0);
            return base.OnActivateAsync();
        }

        // Initialize the actor by resetting the counter to zero and by providing the timeDealy value
        async Task<int> ILMS.StartLMSAsync(int timeDelay)
        {
            await this.StateManager.SetStateAsync<int>("counter", 0);
            await this.StateManager.SetStateAsync<int>("timeDelay", timeDelay);
            await this.StateManager.SetStateAsync<List<double>>("inputValues", new List<double>());
            await this.StateManager.SetStateAsync<double>("estimatedParameter", 0);
            return 0;
        }

        async Task<double[]> ILMS.RunLMSAsync(double stepSize, double inputValue, double outputValue)
        {
            // Retrive the stored counter value
            var counter = await this.StateManager.GetStateAsync<int>("counter");
            // Increase the counter value by 1
            counter = counter + 1;
            // Store the increased counter value
            await this.StateManager.SetStateAsync<int>("counter", counter);

            // Retrive the stored input value list
            var inputValues = await this.StateManager.GetStateAsync<List<double>>("inputValues");
            // Add the new input value to the list
            inputValues.Add(inputValue);
            // Store the list
            await this.StateManager.SetStateAsync<List<double>>("inputValues", inputValues);

            // Retrive stored timeDelay and estimatedParameter values
            var timeDelay = await this.StateManager.GetStateAsync<int>("timeDelay");
            var estimatedParameter = await this.StateManager.GetStateAsync<double>("estimatedParameter");

            // LMS algorithm
            if (counter > timeDelay)
            { 
                var estimatedOutput = estimatedParameter * inputValues[counter-timeDelay-1];
                var estimationError = outputValue - estimatedOutput;
                estimatedParameter = estimatedParameter + stepSize * inputValues[counter - timeDelay - 1] * estimationError;
                result[0] = estimatedOutput;
                result[1] = estimationError;
                result[2] = estimatedParameter;
                await this.StateManager.SetStateAsync<double>("estimatedParameter", estimatedParameter);
            }
            else
            {
                result[0] = 0;
                result[1] = outputValue;
                result[2] = estimatedParameter;
            }
            return result;
        }
    }
}
