using Newtonsoft.Json;

namespace LocaSubs.Models.External.YClients;

public class Company
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("title")]
    public string? Title { get; set; }

    [JsonProperty("coordinate_lat")]
    public double CoordinateLat { get; set; }

    [JsonProperty("coordinate_lon")]
    public double CoordinateLon { get; set; }
}