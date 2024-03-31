using LocaSubs.External.YClients.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.WebUtilities;
using LocaSubs.Models;
using LocaSubs.Models.External.YClients;
using LocaSubs.External.YClients.Common;

namespace LocaSubs.External.YClients;

public class YClientsFacade : IYClientsFacade
{
    public async Task<IReadOnlyCollection<Company>> GetCompanies(ServiceType serviceType)
    {
        using var client = new YClientHttpClientFactory().CreateClient();
 
        string uri = Endpoints.Companies();
        Dictionary<string, string?> queryString = new()
        {
            { "active", "1" },
            { "business_type_id", ((int)serviceType).ToString() },
            { "moderated", "1" },
            { "count", "1000" },
            { "city_id", "1" }
        };
        var requestUri = QueryHelpers.AddQueryString(uri, queryString);

        HttpResponseMessage response = await client.GetAsync(requestUri);

        GetCompaniesResponse getCompaniesResponse = null!;
        if (response.IsSuccessStatusCode)
        {
            string data = await response.Content.ReadAsStringAsync();
            getCompaniesResponse = JsonConvert.DeserializeObject<GetCompaniesResponse>(data)!;
        }
        return getCompaniesResponse?.Data ?? Array.Empty<Company>();
    }

    public async Task<IReadOnlyCollection<StaffMember>> GetStaff(long companyId)
    {
        using var client = new YClientHttpClientFactory().CreateClient();

        HttpResponseMessage response = await client.GetAsync(Endpoints.BookStaff(companyId));

        GetStaffResponse getStaffResponse = null!;
        if (response.IsSuccessStatusCode)
        {
            string data = await response.Content.ReadAsStringAsync();
            getStaffResponse = JsonConvert.DeserializeObject<GetStaffResponse>(data)!;
        }
        return getStaffResponse?.Data ?? Array.Empty<StaffMember>();
    }

    public async Task<SeanceDate> GetSeanceDate(long companyId, long staffId)
    {
        using var client = new YClientHttpClientFactory().CreateClient();

        HttpResponseMessage response = await client.GetAsync(Endpoints.BookStaffSeances(companyId, staffId));

        GetStaffSeancesResponse getStaffSeancesResponse = null!;
        if (response.IsSuccessStatusCode)
        {
            string data = await response.Content.ReadAsStringAsync();
            getStaffSeancesResponse = JsonConvert.DeserializeObject<GetStaffSeancesResponse>(data)!;
        }
        return getStaffSeancesResponse?.Data ?? new SeanceDate { Date = default, Seances = Array.Empty<Seance>() };
    }
}
