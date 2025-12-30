using Camping.Core.Services;
using Camping.Core.Interfaces.Repositories;
using Camping.Core.Models;

namespace TestCore
{
    //Unit tests voor UC7 – Tonen prijs van reservering
    public class PrijsBerekenServiceTests_UC7
    {
        private PrijsBerekenService _service;
        private FakeVoorzieningRepository _repo;

        [SetUp]
        public void Setup()
        {
            _repo = new FakeVoorzieningRepository();

            //Denkbeeldig prijzen voor voorzieningen
            _repo.SetPrijs("Stroom", 1.50m);
            _repo.SetPrijs("Water", 2.00m);

            _service = new PrijsBerekenService(_repo);
        }

        //UC7 - TC7-06 – Correcte totaalprijs met staanplaats + accommodatie + voorzieningen
        [Test]
        public void TC7_06_Bereken_ReturnsCorrectTotaalprijs_WhenAllComponentsSelected()
        {
            //Arrange
            DateTime start = new DateTime(2026, 1, 10);
            DateTime end = new DateTime(2026, 1, 13); //3 nachten

            //Denkbeeldig staanplaats en accommodatie prijzen
            decimal staanplaatsPrijsPerNacht = 10.00m;
            decimal accommodatiePrijsPerNacht = 5.00m;

            bool kiestStroom = true;
            bool stroomMogelijk = true;
            bool kiestWater = true;
            bool waterMogelijk = true;

            //Act
            var (totaal, regels) = _service.Bereken(
                start,
                end,
                staanplaatsPrijsPerNacht,
                accommodatiePrijsPerNacht,
                kiestStroom,
                stroomMogelijk,
                kiestWater,
                waterMogelijk);

            //Assert
            //3 * (10 + 5 + 1.50 + 2.00) = 55.50
            Assert.AreEqual(55.50m, totaal);

            Assert.AreEqual(4, regels.Count);
            Assert.IsTrue(regels.Any(r => r.Omschrijving.StartsWith("Staanplaats tarief")));
            Assert.IsTrue(regels.Any(r => r.Omschrijving.StartsWith("Accommodatie tarief")));
            Assert.IsTrue(regels.Any(r => r.Omschrijving.StartsWith("Voorziening: Stroom")));
            Assert.IsTrue(regels.Any(r => r.Omschrijving.StartsWith("Voorziening: Water")));

            var staanplaatsRegel = regels.Single(r => r.Omschrijving.StartsWith("Staanplaats tarief"));
            var accommodatieRegel = regels.Single(r => r.Omschrijving.StartsWith("Accommodatie tarief"));
            var stroomRegel = regels.Single(r => r.Omschrijving.StartsWith("Voorziening: Stroom"));
            var waterRegel = regels.Single(r => r.Omschrijving.StartsWith("Voorziening: Water"));

            Assert.AreEqual(30.00m, staanplaatsRegel.Bedrag);
            Assert.AreEqual(15.00m, accommodatieRegel.Bedrag);
            Assert.AreEqual(4.50m, stroomRegel.Bedrag);
            Assert.AreEqual(6.00m, waterRegel.Bedrag);
        }

        //UC7 - TC7-07 – Prijsopbouw regels met euro's en 2 decimalen
        [Test]
        public void TC7_07_Bereken_ReturnsPriceBreakdown_WithEuroAndTwoDecimals()
        {
            //Arrange
            DateTime start = new DateTime(2026, 2, 1);
            DateTime end = new DateTime(2026, 2, 4); //3 nachten

            decimal staanplaatsPrijsPerNacht = 12.34m;
            decimal accommodatiePrijsPerNacht = 0.00m;

            //Act
            var (_, regels) = _service.Bereken(
                start,
                end,
                staanplaatsPrijsPerNacht,
                accommodatiePrijsPerNacht,
                kiestStroom: false,
                stroomMogelijk: true,
                kiestWater: false,
                waterMogelijk: true);

            // Assert: minimaal 2 regels (staanplaats + accommodatie)
            Assert.AreEqual(2, regels.Count);

            foreach (var regel in regels)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(regel.Omschrijving));
                // NFR1 + NFR2 via BedragTekst in PrijsInfo model
                StringAssert.Contains("€", regel.BedragTekst);
                // Check 2 decimalen (laatste 3 tekens zijn  ",00")
                Assert.IsTrue(regel.BedragTekst.Length >= 3);
            }

            //Checken of de omschrijvingen correct aantal nachten bevat
            StringAssert.Contains("x 3 nacht(en)", regels[0].Omschrijving);
            StringAssert.Contains("x 3 nacht(en)", regels[1].Omschrijving);
        }

        //UC7 - TC7-08 – Voorziening telt alleen mee als gekozen
        [Test]
        public void TC7_08_Bereken_DoesNotIncludeVoorziening_WhenChosenButNotPossible()
        {
            // Arrange
            DateTime start = new DateTime(2026, 3, 10);
            DateTime end = new DateTime(2026, 3, 12); //2 nachten

            decimal staanplaatsPrijsPerNacht = 20.00m;
            decimal accommodatiePrijsPerNacht = 10.00m;

            // Act (stroom gekozen maar niet mogelijk)
            var (totaal, regels) = _service.Bereken(
                start,
                end,
                staanplaatsPrijsPerNacht,
                accommodatiePrijsPerNacht,
                kiestStroom: true,
                stroomMogelijk: false,
                kiestWater: false,
                waterMogelijk: true);

            //Assert
            //Alleen staanplaats + accommodatie: 2*(20 + 10) = 60
            Assert.AreEqual(60.00m, totaal);
            Assert.AreEqual(2, regels.Count);

            Assert.IsFalse(regels.Any(r => r.Omschrijving.StartsWith("Voorziening: Stroom")));
            Assert.IsFalse(regels.Any(r => r.Omschrijving.StartsWith("Voorziening: Water")));
        }

        //UC7 - TC7-09 – Als voorziening in DB ontbreekt, moet prijs 0 zijn
        [Test]
        public void TC7_09_Bereken_UsesZero_WhenVoorzieningPrijsNotInRepository()
        {
            //Arrange
            //Stroom helemaal weghalen
            _repo.Remove("Stroom");

            DateTime start = new DateTime(2026, 4, 1);
            DateTime end = new DateTime(2026, 4, 3); //2 nachten

            decimal staanplaatsPrijsPerNacht = 10.00m;
            decimal accommodatiePrijsPerNacht = 0.00m;

            //Act (stroom gekozen + mogelijk, maar prijs ontbreekt)
            var (totaal, regels) = _service.Bereken(
                start,
                end,
                staanplaatsPrijsPerNacht,
                accommodatiePrijsPerNacht,
                kiestStroom: true,
                stroomMogelijk: true,
                kiestWater: false,
                waterMogelijk: true);

            //Assert
            //totaal = 2 * 10 + 2 * 0 + 2 * 0 = 20
            Assert.AreEqual(20.00m, totaal);

            var stroomRegel = regels.Single(r => r.Omschrijving.StartsWith("Voorziening: Stroom"));
            Assert.AreEqual(0.00m, stroomRegel.Bedrag);
            StringAssert.Contains("€", stroomRegel.BedragTekst);
        }

        
        //Fake Repo voor voorzieningen
        //Dit is nodig want de voorzieningen prijs wordt niet meegegeven in de Bereken methode zoals die van staanplaats en accommodatie
        //in vorm van een decimaal, maar komt gelijk uit de repo gehaald. Dit kan verbeterd worden door de code achter voorzieningen aan te passen zodat ze
        //op hetzelfde wijze als staanplaats en accommodatie meegegeven worden, maar daarvoor moet de code achter voorzieningen veranderen.
        private class FakeVoorzieningRepository : IVoorzieningRepository
        {
            private readonly Dictionary<string, decimal> _prijzen = new(StringComparer.OrdinalIgnoreCase);

            public void SetPrijs(string naam, decimal prijs) => _prijzen[naam] = prijs;
            public void Remove(string naam) => _prijzen.Remove(naam);

            public IEnumerable<Voorziening> GetAll()
            {
                return _prijzen.Select(dictionaryItem => new Voorziening
                {
                    Naam = dictionaryItem.Key,
                    Prijs = dictionaryItem.Value
                });
            }

            public Voorziening? GetByNaam(string naam)
            {
                if (_prijzen.TryGetValue(naam, out var prijs))
                {
                    return new Voorziening
                    {
                        Naam = naam,
                        Prijs = prijs
                    };
                }

                return null;
            }
        }
    }
}
