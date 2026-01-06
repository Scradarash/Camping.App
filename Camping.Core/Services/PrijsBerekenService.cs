using Camping.Core.Interfaces.Repositories;
using Camping.Core.Interfaces.Services;
using Camping.Core.Models;

namespace Camping.Core.Services
{
    public class PrijsBerekenService : IPrijsBerekenService
    {
        private readonly IVoorzieningRepository _voorzieningRepository;

        //Tijdelijk opslag zodat niet elke keer opnieuw in de database moet kijken voor prijs recalculeren
        private decimal? _stroomPrijsCache;
        private decimal? _waterPrijsCache;

        public PrijsBerekenService(IVoorzieningRepository voorzieningRepository)
        {
            _voorzieningRepository = voorzieningRepository;
        }

        public decimal StroomPrijsPerNacht => _stroomPrijsCache ??= GetVoorzieningPrijsOfDefault("Stroom");
        public decimal WaterPrijsPerNacht => _waterPrijsCache ??= GetVoorzieningPrijsOfDefault("Water");

        private decimal GetVoorzieningPrijsOfDefault(string naam)
        {
            Voorziening? v = _voorzieningRepository.GetByNaam(naam);

            //Als voorziening prijs niet aanwezig is in DB, 0.00 van maken
            return v?.Prijs ?? 0.00m;
        }

        public int BerekenNachten(DateTime start, DateTime end)
        {
            int nachten = (end.Date - start.Date).Days;
            //Handig voor trekkersveld later, zet aantal nachten automatisch op 1 als de aantal kleiner is dan 1
            //Kan ook op andere manier opgelost worden en kan misschien later weg, 
            if (nachten < 1) nachten = 1;
            return nachten;
        }

        public (decimal Totaal, List<PrijsInfo> Regels) Bereken(
            DateTime start,
            DateTime end,
            decimal staanplaatsPrijsPerNacht,
            decimal accommodatiePrijsPerNacht,
            bool kiestStroom,
            bool stroomMogelijk,
            bool kiestWater,
            bool waterMogelijk)
        {
            int nachten = BerekenNachten(start, end);

            var regels = new List<PrijsInfo>();

            //Bepalen formaat per regel bij prijsinfo

            decimal staanplaatsTotaal = nachten * staanplaatsPrijsPerNacht;
            regels.Add(new PrijsInfo
            {
                Omschrijving = $"Staanplaats tarief (€ {staanplaatsPrijsPerNacht:F2}) x {nachten} nacht(en)",
                Bedrag = staanplaatsTotaal
            });

            decimal accommodatieTotaal = nachten * accommodatiePrijsPerNacht;
            regels.Add(new PrijsInfo
            {
                Omschrijving = $"Accommodatie tarief (€ {accommodatiePrijsPerNacht:F2}) x {nachten} nacht(en)",
                Bedrag = accommodatieTotaal
            });

            if (stroomMogelijk && kiestStroom)
            {
                decimal stroomTotaal = nachten * StroomPrijsPerNacht;
                regels.Add(new PrijsInfo
                {
                    Omschrijving = $"Voorziening: Stroom (€ {StroomPrijsPerNacht:F2}) x {nachten} nacht(en)",
                    Bedrag = stroomTotaal
                });
            }

            if (waterMogelijk && kiestWater)
            {
                decimal waterTotaal = nachten * WaterPrijsPerNacht;
                regels.Add(new PrijsInfo
                {
                    Omschrijving = $"Voorziening: Water (€ {WaterPrijsPerNacht:F2}) x {nachten} nacht(en)",
                    Bedrag = waterTotaal
                });
            }

            //Totaalprijs berekenen
            decimal totaal = regels.Sum(r => r.Bedrag);
            return (totaal, regels);
        }
    }
}
