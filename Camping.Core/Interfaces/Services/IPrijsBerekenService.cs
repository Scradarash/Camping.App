using Camping.Core.Models;

namespace Camping.Core.Interfaces.Services
{
    public interface IPrijsBerekenService
    {
        decimal StroomPrijsPerNacht { get; }
        decimal WaterPrijsPerNacht { get; }

        int BerekenNachten(DateTime start, DateTime end);

        (decimal Totaal, List<PrijsInfo> Regels) Bereken(
            DateTime start,
            DateTime end,
            decimal staanplaatsPrijsPerNacht,
            decimal accommodatiePrijsPerNacht,
            bool kiestStroom,
            bool stroomMogelijk,
            bool kiestWater,
            bool waterMogelijk,
            int hoeveelheidGasten);
    }
}
