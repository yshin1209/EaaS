using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using LA.Interfaces;
using ClassLibrary;
using Newtonsoft.Json;

namespace LA
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    /// 


    [StatePersistence(StatePersistence.Persisted)]
    internal class LA : Actor, ILA
    {
        /// <summary>
        /// Initializes a new instance of LA
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public LA(ActorService actorService, ActorId actorId)
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

            return this.StateManager.TryAddStateAsync("count", 0);
        }

        Task<double> ILA.VecVecMultiply(double[] vector1, double[] vector2)
        {
            var n = vector1.Length;
            double product = 0;
            for (int i = 0; i < n; i++)
            {
                product = product + vector1[i] * vector2[i];
            }
            return Task.FromResult<double>(product);
        }

        Task<double[]> ILA.MatVecMultiply(string json)
        {
            MatVecMulClass input = JsonConvert.DeserializeObject<MatVecMulClass>(json);

            double[,] matrix = input.Matrix;
            double[] vector = input.Vector;
            var m = matrix.GetLength(0);
            var n = matrix.GetLength(1);
            double[] product = new double[m];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    product[i] = product[i] + matrix[i,j] * vector[j];
                }
            }

            return Task.FromResult<double[]>(product);
        }

        Task<double[][]> ILA.MatMatMultiply(double[][] matrix1, double[][] matrix2)
        {
            var m = matrix1.GetLength(0);
            var r = matrix1.GetLength(1);
            var n = matrix2.GetLength(1);
            double[][] product = new double[m][];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; i < n; j++)
                {
                    for (int k = 0; i < r; k++)
                    {
                        product[i][j] = product[i][j] + matrix1[i][k] * matrix2[k][j];
                    }
                }
            }
            return Task.FromResult<double[][]>(product);
        }

    }
}
