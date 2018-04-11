using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Actors.Interfaces;
using ClassLibrary;
using RestSharp;

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

        Task IActors.AddVariableAsync(ActorVariable actorData )
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

        Task<ActorMethod> IActors.ExecuteMethodAsync(ActorMethod actorMethod)
        {

            var client = new RestClient(actorMethod.MethodUrl);
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest(Method.POST);
            // request.AddParameter("name", "value"); // adds to POST or URL querystring based on Method
            // request.AddUrlSegment("id", "123"); // replaces matching token in request.Resource

            // easily add HTTP Headers
            // request.AddHeader("header", "value");

            // execute the request
            IRestResponse response = client.Execute(request);
            actorMethod.Response = response.Content; // raw content as string
            return Task.FromResult<ActorMethod> (actorMethod);
        }
    }
}
