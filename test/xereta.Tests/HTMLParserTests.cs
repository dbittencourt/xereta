using System;
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
        IEnumerable<PublicWorker> searchResults; 
        PublicWorker publicWorker;

        public HTMLParserTests()
        {
            _dataParser = new Mock<IDataParser>();

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
            _dataParser.Setup(parser => parser.Parse("1", "",new List<string>())).Returns(publicWorker);
            var result = _dataParser.Object.Parse("1", "", new List<string>());
            Assert.Equal(publicWorker, result);
        }
    }
}