using NUnit.Framework;
using Camping.Core.Services;
using System;

namespace TestCore
{
    // Unit tests voor UC5 – Invoeren gegevens reserveringshouder
    public class ReserveringshouderValidatieTests
    {
        private ReserveringshouderValidatieService _service;

        [SetUp]
        public void Setup()
        {
            _service = new ReserveringshouderValidatieService();
        }

        // UC5.1 - TC5-13 – Naam verplicht
        [Test]
        public void ValidateNaam_ReturnsError_WhenNaamIsEmpty()
        {
            var result = _service.ValidateNaam("");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Naam is verplicht.", result.Error);
        }

        // UC5.1 - TC5-14 – Naam moet minimaal 2 tekens bevatten
        [Test]
        public void ValidateNaam_ReturnsError_WhenNaamIsTooShort()
        {
            var result = _service.ValidateNaam("A");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Naam moet minimaal 2 tekens bevatten.", result.Error);
        }

        // UC5.1 - TC5-15 – Naam mag maximaal 25 tekens bevatten
        [Test]
        public void ValidateNaam_ReturnsError_WhenNaamIsTooLong()
        {
            var langeNaam = new string('A', 26);

            var result = _service.ValidateNaam(langeNaam);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Naam mag maximaal 25 tekens bevatten.", result.Error);
        }

        // UC5.1 - TC5-16 – Naam bevat ongeldige tekens
        [Test]
        public void ValidateNaam_ReturnsError_WhenNaamContainsInvalidCharacters()
        {
            var result = _service.ValidateNaam("Jacob123");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Naam bevat ongeldige tekens.", result.Error);
        }

        // UC5.2 - TC5-17 – Geboortedatum verplicht
        [Test]
        public void ValidateGeboortedatum_ReturnsError_WhenNull()
        {
            var result = _service.ValidateGeboortedatum(null);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Geboortedatum is verplicht.", result.Error);
        }

        // UC5.2 - TC5-18 – Leeftijd minimaal 18
        [Test]
        public void ValidateGeboortedatum_ReturnsError_WhenUnder18()
        {
            var geboortedatum = DateTime.Today.AddYears(-17);

            var result = _service.ValidateGeboortedatum(geboortedatum);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("De hoofdboeker moet minimaal 18 jaar zijn.", result.Error);
        }

        // UC5.2 - TC5-19 – Leeftijd mag niet hoger zijn dan 120 jaar
        [Test]
        public void ValidateGeboortedatum_ReturnsError_WhenAgeIsAbove120()
        {
            var teOud = DateTime.Today.AddYears(-121);

            var result = _service.ValidateGeboortedatum(teOud);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Leeftijd boven 120 jaar is niet toegestaan.", result.Error);
        }

        // UC5.3 - TC5-20 – Email verplicht
        [Test]
        public void ValidateEmail_ReturnsError_WhenEmpty()
        {
            var result = _service.ValidateEmailadres("");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("E-mailadres is verplicht.", result.Error);
        }

        // UC5.3 - TC5-21 – Email formaat
        [Test]
        public void ValidateEmail_ReturnsError_WhenInvalidFormat()
        {
            var result = _service.ValidateEmailadres("geenemail");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("E-mailadres structuur klopt niet.", result.Error);
        }

        // UC5.3 - TC5-22 – E-mailadres bevat niet toegestane karakters
        [Test]
        public void ValidateEmail_ReturnsError_WhenEmailContainsInvalidCharacters()
        {
            var result = _service.ValidateEmailadres("test@te$st.nl");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("E-mailadres bevat niet toegestane karakters.", result.Error);
        }

        // UC5.4 - TC5-23 – Telefoon verplicht
        [Test]
        public void ValidateTelefoon_ReturnsError_WhenEmpty()
        {
            var result = _service.ValidateTelefoonnummer("");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Telefoonnummer is verplicht.", result.Error);
        }

        // UC5.4 - TC5-24 – Telefoonnummer bevat niet toegestane karakters
        [Test]
        public void ValidateTelefoon_ReturnsError_WhenContainsInvalidCharacters()
        {
            var result = _service.ValidateTelefoonnummer("06-12345678");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Alleen cijfers, spaties en '+' zijn toegestaan.", result.Error);
        }

        // UC5.4 - TC5-25 – Telefoon lengte
        [Test]
        public void ValidateTelefoon_ReturnsError_WhenTooShort()
        {
            var result = _service.ValidateTelefoonnummer("123");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Telefoonnummer moet tussen 8 en 15 cijfers bevatten.", result.Error);
        }

        // UC5 - TC5-26 – Happy path
        [Test]
        public void Validate_AllFieldsValid_ReturnsValid()
        {
            var naam = _service.ValidateNaam("Jacob Arie Romkes");
            var geboortedatum = _service.ValidateGeboortedatum(new DateTime(2006, 5, 10));
            var email = _service.ValidateEmailadres("jacobarie@test.com");
            var telefoon = _service.ValidateTelefoonnummer("0612345678");

            Assert.IsTrue(naam.IsValid);
            Assert.IsTrue(geboortedatum.IsValid);
            Assert.IsTrue(email.IsValid);
            Assert.IsTrue(telefoon.IsValid);
        }
    }
}