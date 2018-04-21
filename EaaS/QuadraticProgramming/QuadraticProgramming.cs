using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using QuadraticProgramming.Interfaces;
using ClassLibrary;
using Newtonsoft.Json;
using Accord.Math.Optimization;

namespace QuadraticProgramming
{
    [StatePersistence(StatePersistence.Persisted)]
    internal class QuadraticProgramming : Actor, IQuadraticProgramming
    {
        public QuadraticProgramming(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");

            return this.StateManager.TryAddStateAsync("count", 0);
        }

        Task<QuadraticProgrammingOutput> IQuadraticProgramming.RunQPAsync(string jsonInput)
        {
            QuadraticProgrammingInput input = JsonConvert.DeserializeObject<QuadraticProgrammingInput>(jsonInput);
            double[,] Q = input.Q;
            double[] d = input.d;
            double[,] A = input.A;
            double[] b = input.b;
            int m = input.m;

            var solver = new GoldfarbIdnani(Q, d, A, b, m);
            solver.Minimize();

            QuadraticProgrammingOutput output = new QuadraticProgrammingOutput();
            output.MinimalCostValue = solver.Value;
            output.OptimalFirstVariableValue= solver.Solution[0];
            output.OptimalSecondVariableValue = solver.Solution[1];
            return Task.FromResult<QuadraticProgrammingOutput>(output);
        }
    }
}
