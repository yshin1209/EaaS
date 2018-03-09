using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using PID.Interfaces;

namespace PID
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
    internal class PID : Actor, IPID
    {
       
        public PID(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "PID Actor activated.");
            this.StateManager.TryAddStateAsync<double>("oldError", 0);
            this.StateManager.TryAddStateAsync<double>("sumError", 0);
            return base.OnActivateAsync();
        }

        Task<double> IPID.RunPIDAsync(bool reset, double actualValue, double desiredValue, double Kp, double Ki, double Kd)
        {
            if (reset == true)
            {
                this.StateManager.SetStateAsync<double>("oldError", 0);
                this.StateManager.SetStateAsync<double>("sumError", 0);
            }

            //Retrieve values stored in the previous call
            var oldErrorTask = this.StateManager.GetStateAsync<double>("oldError");
            var sumErrorTask = this.StateManager.GetStateAsync<double>("sumError");
            var oldError = oldErrorTask.Result;
            var sumError = sumErrorTask.Result;



            //PID control
            var newError = desiredValue - actualValue;
            var diffError = newError - oldError;
            var control = Kp * newError + Ki * sumError + Kd * diffError;
            sumError = sumError + newError;

            //Store values for the next call
            this.StateManager.SetStateAsync<double>("oldError", newError);
            this.StateManager.SetStateAsync<double>("sumError", sumError);

            //Return control value to the client
            return Task.FromResult<double>(control);
        }
    }
}
