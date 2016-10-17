using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using Newtonsoft.Json;

namespace DatabaseTest.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public string Get()
        {
            BaseRepository br = new BaseRepository("Server=localhost;Database=sakila;Uid=root;Pwd=admin", String.Empty);

            using (var connection = br.GetConnection())
            {
                try
                {
                    MySqlHasBool model = connection.Query<dynamic>(@"select actor_id, first_name, last_name, last_update from actor where actor_id = @Id", new { Id = 1 }).FirstOrDefault();

                    if (model != null)
                    {
                        return JsonConvert.SerializeObject(model);
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
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
