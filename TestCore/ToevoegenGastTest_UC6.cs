using System;
using NUnit.Framework;
using Camping.Core.Services;

namespace TestCore
{
    // Unit tests voor UC6 – Toevoegen gasten aan reservering
    [TestFixture]
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
            var geboortedatum = DateTime.Today.AddYears(-121);

            var result = _service.ValidateGeboortedatum(geboortedatum);

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
            var geboortedatum = DateTime.Today.AddDays(1);

            var result = _service.ValidateGeboortedatum(geboortedatum);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(
                "De gast kan niet onder de 0 zijn.",
                result.Error
            );
        }

        [Test]
        public void ValidateGeboortedatum_ValidAge_ReturnsValid()
        {
            var geboortedatum = DateTime.Today.AddYears(-30);

            var result = _service.ValidateGeboortedatum(geboortedatum);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(string.Empty, result.Error);
        }

        // TC6-02 – Max aantal gasten bereikt
        [Test]
        public void ValidateMaxGuests_ReturnsFalse_WhenMaxReached()
        {
            int maxGasten = 4;
            int huidigeGasten = 4;

            var result = _service.ValidateMaxGuests(maxGasten, huidigeGasten);

            Assert.IsFalse(result);
        }

        [Test]
        public void ValidateMaxGuests_ReturnsTrue_WhenBelowMax()
        {
            int maxGasten = 4;
            int huidigeGasten = 3;

            var result = _service.ValidateMaxGuests(maxGasten, huidigeGasten);

            Assert.IsTrue(result);
        }
    }
}
