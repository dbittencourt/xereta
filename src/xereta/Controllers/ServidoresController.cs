using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using xereta.Helpers;
using xereta.Models;

namespace xereta.Controllers
{
    [Route("api/[controller]")]
    public class ServidoresController : Controller
    {

        IDataParser _dataParser;
        IDataRetriever _dataRetriever;
        

        public ServidoresController(IDataParser dataParser, IDataRetriever dataRetriever)
        {
            this._dataParser = dataParser;
            this._dataRetriever = dataRetriever;
        }

        // GET api/servidores/id
        // TODO: Implement UserExists filter
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                string profileInfo = await _dataRetriever.GetProfileAsync(id);
                string profileSalary = await _dataRetriever.GetProfileSalaryAsync(id);

                PublicWorker publicWorker = _dataParser.Parse(profileInfo, profileSalary);
                return Ok(publicWorker);
            } 
            catch(HttpRequestException e)
            {
                Console.WriteLine($"Request exception: {e.Message}");
            }
            
            return NoContent();
        }

        // GET api/servidores?q=query
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery]string q)
        {
            try
            {
                string searchTextResult = await _dataRetriever.SearchAsync(q);
                var searchResult = _dataParser.ParseSearch(searchTextResult);
                return Ok(searchResult);
            } 
            catch(HttpRequestException e)
            {
                Console.WriteLine($"Request exception: {e.Message}");
            }
            
            return NoContent();
        }
       
       /*
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            Dictionary<string, string> test = new Dictionary<string, string>();
            test["option1"] = "teste 1";
            test["option2"] = "teste 2";
            return new string[] {$"option1 = {test["option1"]}", $"option2 = {test["option2"]}"};

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

        */
    }
}
