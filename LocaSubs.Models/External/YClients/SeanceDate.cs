using Newtonsoft.Json;

namespace LocaSubs.Models.External.YClients;

public class SeanceDate
{
    [JsonProperty("seance_date")]
    public DateOnly Date { get; set; }

    [JsonProperty("seances")]
    public IReadOnlyCollection<Seance>? Seances { get; set; }
}
