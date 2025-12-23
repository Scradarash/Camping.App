using NUnit.Framework;
using Camping.Core.Services; // Zorg dat deze namespace klopt met waar je Service staat
using System;

namespace TestCore
{
    // Een hoop unittests voor de validatie logica in ReservatieDataService
    public class ReservatieValidatieTests
    {
        private ReservatieDataService _service;

        [SetUp]
        public void Setup()
        {
            _service = new ReservatieDataService();
        }

        // Testen of validatie van verplicht invullen werkt
        [Test]
        public void ValidateInput_ReturnsError_WhenDatesAreNull()
        {
            // Arrange
            DateTime? start = null;
            DateTime? end = null;

            // Act
            string result = _service.ValidateInput(start, end);

            // Assert
            Assert.AreEqual("Selecteer alstublieft een start- en einddatum.", result);
        }

        // Testen of vertrekdatum na aankomstdatum validatie werkt
        [Test]
        public void ValidateInput_ReturnsError_WhenEndDateIsBeforeStartDate()
        {
            // Arrange
            int currentYear = DateTime.Now.Year;
            DateTime start = new DateTime(currentYear, 8, 10);
            DateTime end = new DateTime(currentYear, 8, 5);

            // Act
            string result = _service.ValidateInput(start, end);

            // Assert
            Assert.AreEqual("De vertrekdatum moet na de aankomstdatum liggen.", result);
        }

        // Testen of hij ook blokkeert als vertrekdatum gelijk is aan aankomstdatum
        [Test]
        public void ValidateInput_ReturnsError_WhenEndDateIsSameAsStartDate()
        {
            // Arrange
            int currentYear = DateTime.Now.Year;
            DateTime start = new DateTime(currentYear, 8, 10);
            DateTime end = new DateTime(currentYear, 8, 10);

            // Act
            string result = _service.ValidateInput(start, end);

            // Assert
            Assert.AreEqual("De vertrekdatum moet na de aankomstdatum liggen.", result);
        }

        // Testen of validatie voor verkeerd jaar werkt
        [Test]
        public void ValidateInput_ReturnsError_WhenDateIsNextYear()
        {
            // Arrange
            int nextYear = DateTime.Now.Year + 1;
            DateTime start = new DateTime(nextYear, 1, 1);
            DateTime end = new DateTime(nextYear, 1, 5);

            // Act
            string result = _service.ValidateInput(start, end);

            // Assert
            Assert.AreEqual("Reserveren is alleen mogelijk binnen het huidige kalenderjaar.", result);
        }

        // Ook ff testen voor vorig jaar
        [Test]
        public void ValidateInput_ReturnsError_WhenDateIsLastYear()
        {
            // Arrange
            int lastYear = DateTime.Now.Year - 1;
            DateTime start = new DateTime(lastYear, 12, 25);
            DateTime end = new DateTime(lastYear, 12, 30);

            // Act
            string result = _service.ValidateInput(start, end);

            // Assert
            Assert.AreEqual("Reserveren is alleen mogelijk binnen het huidige kalenderjaar.", result);
        }

        // Nog even een happy path test voor geldige data
        [Test]
        public void ValidateInput_ReturnsEmptyString_WhenInputIsValid()
        {
            // Arrange
            int currentYear = DateTime.Now.Year;
            DateTime start = new DateTime(currentYear, 7, 15);
            DateTime end = new DateTime(currentYear, 7, 20); // Later dan start, zelfde jaar

            // Act
            string result = _service.ValidateInput(start, end);

            // Assert
            Assert.IsEmpty(result);
        }
    }
}