using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoxyQL.BoxPlatform.Service;
using Microsoft.AspNetCore.Mvc;

namespace BoxyQL.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IBoxPlatformService _boxService;

        public ValuesController(IBoxPlatformService boxService)
        {
            this._boxService = boxService;
        }
        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            var boxAdminClient = this._boxService.AdminClient();
            var currentUser = await boxAdminClient.UsersManager.GetCurrentUserInformationAsync();
            System.Console.WriteLine("Current User's name...");
            System.Console.WriteLine(currentUser.Name);
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
