using LocaSubs.Models.External.YClients;

namespace LocaSubs.External.YClients.Models;

public class GetCompaniesResponse
{
    public bool Success { get; set; }
    public IReadOnlyCollection<Company>? Data { get; set; }
}
