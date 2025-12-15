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

        // UC5.1 – Naam verplicht
        [Test]
        public void ValidateNaam_ReturnsError_WhenNaamIsEmpty()
        {
            var result = _service.ValidateNaam("");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Naam is verplicht.", result.Error);
        }

        // UC5.1 – Naam bevat ongeldige tekens
        [Test]
        public void ValidateNaam_ReturnsError_WhenNaamContainsInvalidCharacters()
        {
            var result = _service.ValidateNaam("Jacob123");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Naam bevat ongeldige tekens.", result.Error);
        }

        // UC5.2 – Geboortedatum verplicht
        [Test]
        public void ValidateGeboortedatum_ReturnsError_WhenNull()
        {
            var result = _service.ValidateGeboortedatum(null);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Geboortedatum is verplicht.", result.Error);
        }

        // UC5.2 – Leeftijd minimaal 18
        [Test]
        public void ValidateGeboortedatum_ReturnsError_WhenUnder18()
        {
            var geboortedatum = DateTime.Today.AddYears(-17);

            var result = _service.ValidateGeboortedatum(geboortedatum);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("De hoofdboeker moet minimaal 18 jaar zijn.", result.Error);
        }

        // UC5.3 – Email verplicht
        [Test]
        public void ValidateEmail_ReturnsError_WhenEmpty()
        {
            var result = _service.ValidateEmailadres("");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("E-mailadres is verplicht.", result.Error);
        }

        // UC5.3 – Email formaat
        [Test]
        public void ValidateEmail_ReturnsError_WhenInvalidFormat()
        {
            var result = _service.ValidateEmailadres("geenemail");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("E-mailadres structuur klopt niet.", result.Error);
        }

        // UC5.4 – Telefoon verplicht
        [Test]
        public void ValidateTelefoon_ReturnsError_WhenEmpty()
        {
            var result = _service.ValidateTelefoonnummer("");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Telefoonnummer is verplicht.", result.Error);
        }

        // UC5.4 – Telefoon lengte
        [Test]
        public void ValidateTelefoon_ReturnsError_WhenTooShort()
        {
            var result = _service.ValidateTelefoonnummer("123");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Telefoonnummer moet tussen 8 en 15 cijfers bevatten.", result.Error);
        }

        // UC5 – Happy path
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