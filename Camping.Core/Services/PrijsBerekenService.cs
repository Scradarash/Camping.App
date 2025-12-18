using Camping.Core.Interfaces.Repositories;
using Camping.Core.Models;

namespace Camping.Core.Services
{
    public class PrijsBerekenService
    {
        private readonly IVoorzieningRepository _voorzieningRepository;

        // Cache zodat je niet steeds opnieuw de database moet hitten tijdens binding/refresh in de UI
        private decimal? _stroomPrijsCache;
        private decimal? _waterPrijsCache;

        public PrijsBerekenService(IVoorzieningRepository voorzieningRepository)
        {
            _voorzieningRepository = voorzieningRepository;
        }

        // Prijzen definities (nu uit DB)
        public decimal StroomPrijsPerNacht => _stroomPrijsCache ??= GetVoorzieningPrijsOfDefault("Stroom");
        public decimal WaterPrijsPerNacht => _waterPrijsCache ??= GetVoorzieningPrijsOfDefault("Water");

        private decimal GetVoorzieningPrijsOfDefault(string naam)
        {
            Voorziening? v = _voorzieningRepository.GetByNaam(naam);

            // Fallback om crashes te voorkomen als de DB rij ontbreekt
            // (bijv. in een lege database of fout in seed data).
            return v?.Prijs ?? 0.00m;
        }

        public int BerekenNachten(DateTime start, DateTime end)
        {
            int nachten = (end.Date - start.Date).Days;
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

            decimal totaal = regels.Sum(r => r.Bedrag);
            return (totaal, regels);
        }
    }
}
