using Xunit;
using Moq;
using xereta.Controllers;
using xereta.Helpers;
using xereta.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;

namespace xereta.Tests
{
    public class ServidoresControllerTests : TestBase
    {
        Mock<IDataParser> _dataParser;
        Mock<IDataRetriever> _dataRetriever;
        Mock<IRepository<PublicWorker>> _publicWorkersRepository;
        Mock<IRepository<Salary>> _salariesRepository;
        ServidoresController _controller;
        IEnumerable<PublicWorker> searchResults; 
        PublicWorker publicWorker;

        public ServidoresControllerTests()
        {
            searchResults = new List<PublicWorker> {
                new PublicWorker { CPF = "123456789", Name = "Result1", Id = "id1", LastUpdate = DateTime.Now,
                OriginDepartment = "originDepartment", WorkingDepartment = "workingDepartment" },
                new PublicWorker { CPF = "987654321", Name = "Result2", Id = "id2", LastUpdate = DateTime.Now,
                OriginDepartment = "originDepartment", WorkingDepartment = "workingDepartment" },
                new PublicWorker { CPF = "012345678", Name = "Result3", Id = "id3", LastUpdate = DateTime.Now,
                OriginDepartment = "originDepartment", WorkingDepartment = "workingDepartment" }
            };

            publicWorker = new PublicWorker() {
                CPF = "123456789", Id = "id1", Name = "PublicWorker", LastUpdate = DateTime.Now,
                OriginDepartment = "OriginDepartment", WorkingDepartment = "WorkingDepartment",
                Salaries = new List<Salary>()
            };
            
            (publicWorker.Salaries as List<Salary>).Add(new Salary(){Id = "id1", Year = 2016, Month = 11, Income = 10000});
            (publicWorker.Salaries as List<Salary>).Add(new Salary(){Id = "id1", Year = 2016, Month = 10, Income = 10000});
            (publicWorker.Salaries as List<Salary>).Add(new Salary(){Id = "id1", Year = 2016, Month = 9, Income = 10000});
            (publicWorker.Salaries as List<Salary>).Add(new Salary(){Id = "id1", Year = 2016, Month = 8, Income = 10000});
            (publicWorker.Salaries as List<Salary>).Add(new Salary(){Id = "id1", Year = 2016, Month = 7, Income = 10000});
            
            _publicWorkersRepository = new Mock<IRepository<PublicWorker>>();
            _publicWorkersRepository.Setup(rep => rep.GetAsync("id1")).Returns(Task.FromResult(publicWorker));
            _salariesRepository = new Mock<IRepository<Salary>>();
            _salariesRepository.Setup(rep => rep.GetAllWithIdAsync("id1")).Returns(Task.FromResult(publicWorker.Salaries));

        }

        [Fact]
        public async Task Search_CorrectQuery_ReturnsListOfSearchResults() 
        {
            _dataRetriever = new Mock<IDataRetriever>();
            _dataRetriever.Setup(retriever => retriever.SearchAsync("")).Returns(Task.FromResult(""));

             _dataParser = new Mock<IDataParser>();
            _dataParser.Setup(parser => parser.ParseSearch("")).Returns(searchResults);

            _controller = new ServidoresController(_dataParser.Object, _dataRetriever.Object,
                                _publicWorkersRepository.Object, _salariesRepository.Object);
                                
            var result = await _controller.Search("");

            Assert.NotNull(result);
            var methodResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<PublicWorker>>(methodResult.Value);
            Assert.Equal(searchResults, model);
        } 

        [Fact]
        public async Task GetById_CorrectId_ReturnsPublicWorker()
        {
            
            IEnumerable<string> test = new List<string>{"test"};
            _dataRetriever = new Mock<IDataRetriever>();
            _dataRetriever.Setup(retriever => retriever.GetProfileAsync("1")).Returns(Task.FromResult("test"));
            _dataRetriever.Setup(retriever => retriever.GetProfileSalaryAsync("1", 5)).Returns(Task.FromResult(test));
            
            _dataParser = new Mock<IDataParser>();
            _dataParser.Setup(parser => parser.Parse("1", "test", test)).Returns(publicWorker);

            _controller = new ServidoresController(_dataParser.Object, _dataRetriever.Object, 
                                _publicWorkersRepository.Object, _salariesRepository.Object);

            var result = await _controller.GetById("1");

            Assert.NotNull(result);
            var methodResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<PublicWorker>(methodResult.Value);
        }

        [Fact]
        public async Task GetById_IncorrectId_ReturnsNotFound()
        {
            IEnumerable<string> test = new List<string>{"test"};
            _dataRetriever = new Mock<IDataRetriever>();
            _dataRetriever.Setup(retriever => retriever.GetProfileAsync("")).Returns(Task.FromResult(""));
            _dataRetriever.Setup(retriever => retriever.GetProfileSalaryAsync("", 5)).Returns(Task.FromResult(test));
            
            _dataParser = new Mock<IDataParser>();
            _dataParser.Setup(parser => parser.Parse("","", test)).Returns(publicWorker);

             _controller = new ServidoresController(_dataParser.Object, _dataRetriever.Object, 
                                _publicWorkersRepository.Object, _salariesRepository.Object);

            var result = await _controller.GetById("");

            Assert.NotNull(result);
            var methodResult = Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
