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
        }

        [Fact]
        public async Task Search_ReturnsOkObjectResult_WithAListOfSearchResults() 
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
        public async Task GetById_ReturnsOkObjectResult_WithAPublicWorker()
        {
            throw new NotImplementedException();
        }
    }
}
