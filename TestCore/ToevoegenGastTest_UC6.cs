using Camping.Core.Services;
using Camping.Core.Interfaces.Repositories;
using Camping.Core.Models;

namespace TestCore
{
    //Unit tests voor UC6 – Toevoegen gasten aan reservering
    public class ToevoegenGastTest_UC6
    {
        private ToevoegenGastService _service;

        [SetUp]
        public void Setup()
        {
            _service = new ToevoegenGastService();
        }

        // TC6-24 – Leeftijd is 120 of hoger
        [Test]
        public void ValidateGeboortedatum_ReturnsError_WhenAgeAbove120()
        {
            // Arrange
            var geboortedatum = DateTime.Today.AddYears(-121);

            // Act
            var result = _service.ValidateGeboortedatum(geboortedatum);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(
                "Het is erg onwaarschijnlijk dat de gast zo oud is.",
                result.Error
            );
        }

        // TC6-25 – Geboortedatum in de toekomst
        [Test]
        public void ValidateGeboortedatum_ReturnsError_WhenDateIsInFuture()
        {
            // Arrange
            var geboortedatum = DateTime.Today.AddDays(1);

            // Act
            var result = _service.ValidateGeboortedatum(geboortedatum);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(
                "De gast kan niet onder de 0 zijn.",
                result.Error
            );
        }

        [Test]
        public void ValidateGeboortedatum_ValidAge_ReturnsValid()
        {
            // Arrange
            var geboortedatum = DateTime.Today.AddYears(-30);

            // Act
            var result = _service.ValidateGeboortedatum(geboortedatum);

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(string.Empty, result.Error);
        }


        // TC6-02 – Max aantal gasten bereikt
        [Test]
        public void ValidateMaxGuests_ReturnsFalse_WhenMaxReached()
        {
            // Arrange
            int maxGasten = 4;
            int huidigeGasten = 4;

            // Act
            var result = _service.ValidateMaxGuests(maxGasten, huidigeGasten);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void ValidateMaxGuests_ReturnsTrue_WhenBelowMax()
        {
            // Arrange
            int maxGasten = 4;
            int huidigeGasten = 3;

            // Act
            var result = _service.ValidateMaxGuests(maxGasten, huidigeGasten);

            // Assert
            Assert.IsTrue(result);
        }
    }
}

    }
}
