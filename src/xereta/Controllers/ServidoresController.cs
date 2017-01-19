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

        IDataParser _htmlParser;
        readonly string searchURL = "http://www.portaldatransparencia.gov.br/servidores/Servidor-ListaServidores.asp?bogus=1&Pagina=1&TextoPesquisa=";
        readonly string profileURL = "http://www.portaldatransparencia.gov.br/servidores/Servidor-DetalhaServidor.asp?IdServidor=";
        readonly string profileSalaryURL = "http://www.portaldatransparencia.gov.br/servidores/Servidor-DetalhaRemuneracao.asp?Op=1&IdServidor=";

        public ServidoresController(IDataParser htmlParser)
        {
            this._htmlParser = htmlParser;
        }

        // GET api/servidores/id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            using (var Client = new HttpClient())
            {
                try
                {
                    var response = await Client.GetAsync(profileURL + id);
                    response.EnsureSuccessStatusCode();
                    string profileResponse = await response.Content.ReadAsStringAsync();

                    response = await Client.GetAsync(profileSalaryURL + id);
                    response.EnsureSuccessStatusCode();
                    string profileSalaryResponse = await response.Content.ReadAsStringAsync();

                    var result = _htmlParser.Parse(profileResponse, profileSalaryResponse);

                    return Ok(result);
                }
                catch(HttpRequestException e)
                {
                    Console.WriteLine($"Request exception: {e.Message}");
                }
            }
            
            return NoContent();
        }

        // GET api/servidores?q=query
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery]string q)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(searchURL + q);
                    response.EnsureSuccessStatusCode();
                    string stringResponse = await response.Content.ReadAsStringAsync();

                    var result = _htmlParser.ParseSearch(stringResponse);
                    return Ok(result);
                }
                catch(HttpRequestException e)
                {
                    Console.WriteLine($"Request exception: {e.Message}");
                }

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
