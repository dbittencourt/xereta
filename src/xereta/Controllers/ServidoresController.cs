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
        IRepository<PublicWorker> _repository;

        public ServidoresController(IDataParser dataParser, IDataRetriever dataRetriever, IRepository<PublicWorker> _repository)
        {
            this._dataParser = dataParser;
            this._dataRetriever = dataRetriever;
        }

        // GET api/servidores/id
        // TODO: Implement UserExists filter
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id, int number = 5)
        {
            try
            {
                string profileInfo = await _dataRetriever.GetProfileAsync(id);
                if (!profileInfo.Equals(string.Empty))
                {
                    IEnumerable<string> profileSalary = await _dataRetriever.GetProfileSalaryAsync(id, number);
                    PublicWorker publicWorker = _dataParser.Parse(profileInfo, profileSalary);
                    publicWorker.Id = id;
                    return Ok(publicWorker);
                }
                else
                {
                    return NotFound(id);
                }
            } 
            catch(HttpRequestException e)
            {
                Console.WriteLine($"Request exception: {e.Message}");
            }
            
            return NotFound(id);
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
