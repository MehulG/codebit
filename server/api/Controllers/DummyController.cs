using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class DummyController : ControllerBase
    {
        private readonly DummyService _eventService;
        

        public DummyController(DummyService eventService , IConfiguration configuration)
        {
            _eventService = eventService;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        [HttpGet]
        public ActionResult<List<Dummy>> Get() =>
          _eventService.Get();

        [HttpGet("{id:length(24)}", Name = "GetEvents")]
        public ActionResult<Dummy> Get(string id)
        {

            var events = _eventService.Get(id);

            if (events == null)
            {
                return NotFound();
            }

            return events;//dfsdfsdfsdf
        }
        [Authorize]
        [HttpPost]
        public ActionResult<Dummy> Create(Dummy events)
        {
            _eventService.Create(events);

            return CreatedAtRoute("GetEvents", new { id = events.id.ToString() }, events);

        }
        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Dummy eventsIn)
        {
            var events = _eventService.Get(id);

            if (events == null)
            {
                return NotFound();
            }

            _eventService.Update(id, eventsIn);

            return NoContent();
        }
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var events = _eventService.Get(id);

            if (events == null)
            {
                return NotFound();
            }

            _eventService.Remove(events.id);

            return NoContent();
        }


        [HttpPost("token")]
        public ActionResult GetToken()
        {
            //Security Key
            var securityKey = Configuration["securityKey"];
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            //signing credentials
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            //create token
            var token = new JwtSecurityToken(
                issuer: "admin",
                audience: "reader",
                expires: DateTime.Now.AddDays(1),
                signingCredentials: signingCredentials
                );
            //return token
            return Ok(new JwtSecurityTokenHandler().WriteToken(token));

        }
    }
}
