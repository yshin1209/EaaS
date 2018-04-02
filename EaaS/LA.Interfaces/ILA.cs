using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using ClassLibrary;

namespace LA.Interfaces
{

    /// <summary>
    /// This interface defines the methods exposed by an actor.
    /// Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface ILA : IActor
    {
        Task<double> VecVecMultiply(double[] vector1, double[] vector2);
        Task<double[]> MatVecMultiply(string json);
        Task<double[][]> MatMatMultiply(double[][] matrix1, double[][] matrix2);
    }
}
