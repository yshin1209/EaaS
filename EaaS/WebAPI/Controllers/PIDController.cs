using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using PID.Interfaces;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/PID")]
    public class PIDController : Controller
    {
        [HttpGet]
        [Route("{id}/{reset}/{measuredValue}/{desiredValue}/{Kp}/{Ki}/{Kd}")]
        public async Task<string> Get(int id, bool reset, double measuredValue, double desiredValue, double Kp, double Ki, double Kd)
        {
            ActorId actorId = new ActorId(id);
            var actor = ActorProxy.Create<IPID>(actorId, new Uri("fabric:/Application/PIDActorService"));
            double response = await actor.RunPIDAsync(reset, measuredValue, desiredValue, Kp, Ki, Kd);
            return response.ToString();
        }
    }
}