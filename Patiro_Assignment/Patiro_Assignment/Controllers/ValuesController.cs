using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Patiro_Assignment.Models;

namespace Patiro_Assignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            List<User> users = Storage.Instance.GetUsers().ToList();

            return users;
            //return new JsonResult(users);
        }

        [HttpGet]
        [Route("{username}")]
        public async Task<ActionResult<User>> Details([FromRoute]string username)
        {
            User user = Storage.Instance.GetUser(username);

            return user;
            //return new JsonResult(user);
        }

        [HttpGet]
        [Route("Clinic")]
        public async Task<ActionResult<Clinic>> GetClinic()
        {
            Clinic clinic = Storage.Instance.GetClinic();

            return clinic;
            //return new JsonResult(clinic);
        }

        [HttpPut]
        [Route("Clinic/Update")]
        public IActionResult UpdateClinic(string username, Clinic clinic)
        {
            User user;
            Clinic oldC;
            bool validUpdate = false;

            try
            {
                user = Storage.Instance.GetUser(username);
                oldC = Storage.Instance.GetClinic();

                validUpdate = Clinic.ValidateUpdate(clinic, oldC, user);
            }
            catch (Exception ex)
            {

                return BadRequest();
            }

            if (validUpdate)
            {
                // We can update.
                Storage.Instance.UpdateClinic(clinic);
                return Ok();
            }
            else
            {
                return NoContent();
            }
        }
    }
}
