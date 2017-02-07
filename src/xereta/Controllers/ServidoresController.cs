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
        IRepository<PublicWorker> _publicWorkersRepository;
        IRepository<Salary> _salariesRepository;

        public ServidoresController(IDataParser dataParser, IDataRetriever dataRetriever, 
        IRepository<PublicWorker> publicWorkersRepository, IRepository<Salary> salariesRepository)
        {
            this._dataParser = dataParser;
            this._dataRetriever = dataRetriever;
            this._publicWorkersRepository = publicWorkersRepository;
            this._salariesRepository = salariesRepository;
        }

        // GET api/servidores/id
        // TODO: Implement UserExists filter
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id, int number = 5)
        {
            try
            {
                // tries to retrieve it from the database
                PublicWorker publicWorker = await LoadPublicWorker(id);

                // if there's no entry in the db, retrieves it from the web and saves it
                if (publicWorker != null)
                    return Ok(publicWorker);
                else
                {
                    publicWorker = await RetrievePublicWorker(id, number);
                    if (publicWorker != null)
                    {
                        await AddPublicWorker(publicWorker);
                        return Ok(publicWorker);
                    }
                    else
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

        private async Task<PublicWorker> RetrievePublicWorker(string id, int number)
        {
            PublicWorker publicWorker = null;
            string profileInfo = await _dataRetriever.GetProfileAsync(id);
            if (!profileInfo.Equals(string.Empty))
            {
                IEnumerable<string> profileSalary = await _dataRetriever.GetProfileSalaryAsync(id, number);
                publicWorker = _dataParser.Parse(id, profileInfo, profileSalary);
            }
            return publicWorker;
        }

        private async Task AddPublicWorker(PublicWorker publicWorker)
        {
            if (publicWorker != null)
            {
                publicWorker.LastUpdate = DateTime.Now;
                await _publicWorkersRepository.AddAsync(publicWorker);
                await _salariesRepository.AddRangeAsync(publicWorker.Salaries);
            }
        }

        private async Task<PublicWorker> LoadPublicWorker(string id)
        {
            PublicWorker worker = await _publicWorkersRepository.GetAsync(id);
            if (worker != null)
            {
                if (ValidateIfIsOutdated(worker))
                {
                    worker = await RetrievePublicWorker(id, 5);
                    await UpdatePublicWorker(worker);
                }
                else
                    worker.Salaries = await _salariesRepository.GetAllWithIdAsync(id);
            }

            return worker;
        }

        private async Task UpdatePublicWorker(PublicWorker publicWorker)
        {
            if (publicWorker != null)
            {
                publicWorker.LastUpdate = DateTime.Now;
                await _publicWorkersRepository.UpdateAsync(publicWorker);
            }
        }

        /// <summary>
        /// Validates if the public worker info is outdated
        /// </summary>
        /// <returns>If it's outdated or not</returns>
        private bool ValidateIfIsOutdated(PublicWorker publicWorker)
        {
            DateTime diff = DateTime.Now.AddTicks(-publicWorker.LastUpdate.Ticks);
            return diff.Month > 1;
        }
    }
}
