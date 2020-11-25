using System;
using System.IO;
using Dbca.Municipalities.Abstractions.Factories;
using Dbca.Municipalities.Abstractions.Model;
using Dbca.Municipalities.Abstractions.Model.TaxRates;
using Dbca.Municipalities.Abstractions.Repositories;
using Dbca.Municipalities.Abstractions.Services;
using Dbca.Municipalities.Factories;
using Dbca.Municipalities.Services;
using Moq;
using Xunit;

namespace Dbca.Municipalities.Tests.Services
{
    public class MunicipalityServiceTests
    {
        private readonly Mock<IMunicipalityRepository> _municipalityRepositoryMock;
        private readonly IScheduledTaxRateFactory _schedulTaxRateFactory;
        private readonly IMunicipalityService _service;

        public MunicipalityServiceTests()
        {
            _municipalityRepositoryMock = new Mock<IMunicipalityRepository>();
            _schedulTaxRateFactory = new ScheduledTaxRateFactory();

            _service = new MunicipalityService(_municipalityRepositoryMock.Object, _schedulTaxRateFactory);
        }

        public class Constructor : MunicipalityServiceTests
        {
            [Fact]
            public void GivenMunicialityRepository_WhenNull_ThrowsException()
            {
                Assert.Throws<ArgumentNullException>(() => new MunicipalityService(null, _schedulTaxRateFactory));
            }

            [Fact]
            public void GivenScheduledTaxRate_WhenNull_ThrowsException()
            {
                Assert.Throws<ArgumentNullException>(() => new MunicipalityService(_municipalityRepositoryMock.Object, null));
            }
        }

        public class ImportScheduledTaxRates : MunicipalityServiceTests
        {
            private const string resourcesPath = "../../../Services/Resources";

            [Fact]
            public void GivenFilePath_WhenEmpty_ResponseIsFalse()
            {
                var response = _service.ImportScheduledTaxRates(string.Empty);

                Assert.False(response.Success);
            }

            [Fact]
            public void GivenFilePath_WhenNotCsvFormat_ResponseIsFalse()
            {
                var response = _service.ImportScheduledTaxRates("drive:\\filepath\\filename.pdf");

                Assert.False(response.Success);
            }

            [Fact]
            public void GivenFilePath_WhenFileDoesNotExist_ResponseIsFalse()
            {
                var response = _service.ImportScheduledTaxRates(@"c:\file.csv");

                Assert.False(response.Success);
            }

            [Fact]
            public void GivenValidFilePath_ParsesLines()
            {
                var filePath = Path.Combine(resourcesPath, "TestImportFile.csv");

                var response = _service.ImportScheduledTaxRates(filePath);

                Assert.True(response.Success);
                _municipalityRepositoryMock.Verify(m => m.SaveScheduledTaxRate(It.IsAny<IScheduledTaxRate>()), Times.Exactly(5));
            }

            [Fact]
            public void GivenValidFilePath_WhenDataIsInvalid_ResponseIsFalse()
            {
                var filePath = Path.Combine(resourcesPath, "TestImportFileInvalidData.csv");

                var response = _service.ImportScheduledTaxRates(filePath);

                Assert.False(response.Success);
                _municipalityRepositoryMock.Verify(m => m.SaveScheduledTaxRate(It.IsAny<IScheduledTaxRate>()), Times.Never);
            }
        }

        public class GetScheduledTaxRate : MunicipalityServiceTests
        {
            private const decimal rate01012020 = 0.1m;
            private const decimal rate02052020 = 0.4m;

            public GetScheduledTaxRate()
                : base()
            {
                var municipalityMock = new Mock<IMunicipality>();
                municipalityMock.Setup(m => m.GetTaxRate(It.IsAny<DateTime>())).Returns(0m);
                municipalityMock.Setup(m => m.GetTaxRate(new DateTime(2020, 1, 1).Date)).Returns(rate01012020);
                municipalityMock.Setup(m => m.GetTaxRate(new DateTime(2020, 5, 2).Date)).Returns(rate02052020);

                _municipalityRepositoryMock.Setup(m => m.GetMunicipality(It.IsAny<string>())).Returns(municipalityMock.Object);
            }
        }
    }
}