using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LaserAPI.Logic;
using LaserAPI.Models.Dto.LasershowSpotify;
using LaserAPI.Models.FromFrontend.LasershowSpotifyConnector;
using LaserAPI.Models.Helper;
using LaserAPI.Models.ToFrontend.LasershowSpotifyConnector;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace LaserAPI.Controllers
{
    [Route("spotify-connector")]
    [ApiController]
    public class LasershowSpotifyConnectorController : ControllerBase
    {
        private readonly LasershowSpotifyConnectorLogic _lasershowSpotifyConnectorLogic;
        private readonly ControllerResultHandler _controllerResultHandler;

        public LasershowSpotifyConnectorController(LasershowSpotifyConnectorLogic lasershowSpotifyConnectorLogic,
            ControllerResultHandler controllerResultHandler)
        {
            _lasershowSpotifyConnectorLogic = lasershowSpotifyConnectorLogic;
            _controllerResultHandler = controllerResultHandler;
        }

        [HttpPost]
        public async Task<ActionResult> AddOrUpdate([FromBody] LasershowSpotifyConnector connector)
        {
            async Task Action()
            {
                LasershowSpotifyConnectorDto connectorDto = connector.Adapt<LasershowSpotifyConnectorDto>();
                await _lasershowSpotifyConnectorLogic.AddOrUpdate(connectorDto);
            }

           return await _controllerResultHandler.Execute(Action());
        }

        [HttpGet]
        public async Task<ActionResult<List<LasershowSpotifyConnectorViewmodel>>> All()
        {
            async Task<List<LasershowSpotifyConnectorViewmodel>> Action()
            {
                List<LasershowSpotifyConnectorDto> connectors = await _lasershowSpotifyConnectorLogic.All();
                return connectors.Adapt<List<LasershowSpotifyConnectorViewmodel>>();
            }

            return await _controllerResultHandler.Execute(Action());
        }
    }
}
