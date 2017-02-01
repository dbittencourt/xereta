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
        ServidoresController _controller;
        IEnumerable<SearchResult> searchResults; 
        PublicWorker publicWorker;

        public ServidoresControllerTests()
        {
            searchResults = new List<SearchResult> {
                new SearchResult { CPF = "123456789", Name = "Result1", Id = "id1", 
                OriginDepartment = "originDepartment", WorkingDepartment = "workingDepartment" },
                new SearchResult { CPF = "987654321", Name = "Result2", Id = "id2", 
                OriginDepartment = "originDepartment", WorkingDepartment = "workingDepartment" },
                new SearchResult { CPF = "012345678", Name = "Result3", Id = "id3", 
                OriginDepartment = "originDepartment", WorkingDepartment = "workingDepartment" }
            };

            publicWorker = new PublicWorker() {
                CPF = "123456789", Id = "id1", Name = "PublicWorker", 
                OriginDepartment = "OriginDepartment", WorkingDepartment = "WorkingDepartment"
            };
            publicWorker.Salaries = new Dictionary<string, float>();
            publicWorker.Salaries.Add("2016-Novembro", 10000);
            publicWorker.Salaries.Add("2016-Outubro", 10000);
            publicWorker.Salaries.Add("2016-Setembro", 10000);
            publicWorker.Salaries.Add("2016-Agosto", 10000);
            publicWorker.Salaries.Add("2016-Julho", 10000);
        }

        [Fact]
        public async Task Search_CorrectQuery_ReturnsListOfSearchResults() 
        {
            _dataRetriever = new Mock<IDataRetriever>();
            _dataRetriever.Setup(retriever => retriever.SearchAsync("")).Returns(Task.FromResult(""));

             _dataParser = new Mock<IDataParser>();
            _dataParser.Setup(parser => parser.ParseSearch("")).Returns(searchResults);

            _controller = new ServidoresController(_dataParser.Object, _dataRetriever.Object);
            var result = await _controller.Search("");

            Assert.NotNull(result);
            var methodResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<SearchResult>>(methodResult.Value);
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
            _dataParser.Setup(parser => parser.Parse("test", test)).Returns(publicWorker);

            _controller = new ServidoresController(_dataParser.Object, _dataRetriever.Object);
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
            _dataParser.Setup(parser => parser.Parse("", test)).Returns(publicWorker);

            _controller = new ServidoresController(_dataParser.Object, _dataRetriever.Object);
            var result = await _controller.GetById("");

            Assert.NotNull(result);
            var methodResult = Assert.IsType<NotFoundResult>(result);
        }
    }
}
