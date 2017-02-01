using System.Collections.Generic;
using Moq;
using xereta.Helpers;
using xereta.Models;
using Xunit;

namespace xereta.Tests
{
    public class HTMLParserTests : TestBase
    {
        Mock<IDataParser> _dataParser;
        IEnumerable<SearchResult> searchResults; 
        PublicWorker publicWorker;

        public HTMLParserTests()
        {
            _dataParser = new Mock<IDataParser>();

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
        public void ParseSearch_CorrectQuery_ReturnsSearchResult()
        {
            _dataParser.Setup(parser => parser.ParseSearch("")).Returns(searchResults);
            var result = _dataParser.Object.ParseSearch("");
            Assert.Equal(searchResults, result);
        }

        [Fact]
        public void Parse_CorrectId_ReturnsPublicWorker()
        {
            _dataParser.Setup(parser => parser.Parse("",new List<string>())).Returns(publicWorker);
            var result = _dataParser.Object.Parse("", new List<string>());
            Assert.Equal(publicWorker, result);
        }
    }
}