using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace xereta.Helpers
{
    public class HTMLDataRetriever : IDataRetriever
    {

        readonly string searchURL = "http://www.portaldatransparencia.gov.br/servidores/Servidor-ListaServidores.asp?bogus=1&Pagina=1&TextoPesquisa=";
        readonly string profileURL = "http://www.portaldatransparencia.gov.br/servidores/Servidor-DetalhaServidor.asp?IdServidor=";
        readonly string profileSalaryURL = "http://www.portaldatransparencia.gov.br/servidores/Servidor-DetalhaRemuneracao.asp?Op=1&IdServidor=";

        public async Task<string> SearchAsync(string query)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(searchURL + query);
                    response.EnsureSuccessStatusCode();
                    string searchResult = await response.Content.ReadAsStringAsync();

                   return searchResult;
                }
                catch(HttpRequestException e)
                {
                    Console.WriteLine($"Request exception: {e.Message}");
                }

            }
            return "";
        }

        public async Task<string> GetProfileAsync(string id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(profileURL + id);
                    response.EnsureSuccessStatusCode();
                    string profileResponse = await response.Content.ReadAsStringAsync();
                    return profileResponse;
                }
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine($"Request exception: {e.Message}");
            }
            return "";
            
        }

        public async Task<string> GetProfileSalaryAsync(string id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(profileSalaryURL + id);
                    response.EnsureSuccessStatusCode();
                    string profileResponse = await response.Content.ReadAsStringAsync();
                    return profileResponse;
                }
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine($"Request exception: {e.Message}");
            }
            return "";
        }
    }
}