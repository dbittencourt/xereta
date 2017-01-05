using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using xereta.Helpers;

namespace xereta.Controllers
{
    [Route("api/[controller]")]
    public class ServidoresController : Controller
    {

        IHTMLParser _htmlParser;
        readonly string urlPortalTransparencia = "http://www.portaldatransparencia.gov.br/servidores/Servidor-ListaServidores.asp?bogus=1&Pagina=1&TextoPesquisa=";
        
        
        public ServidoresController(IHTMLParser htmlParser)
        {
            this._htmlParser = htmlParser;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<string> Get(string id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(urlPortalTransparencia + id);
                    response.EnsureSuccessStatusCode();
                    string stringResponse = await response.Content.ReadAsStringAsync();

                    var result = _htmlParser.Parse(stringResponse);
                    return result[0];
                }
                catch(HttpRequestException e)
                {
                    Console.WriteLine($"Request exception: {e.Message}");
                }

            }
            return "Something not expected happened";
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
