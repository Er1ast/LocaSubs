using LocaSubs.Models.External.YClients;

namespace LocaSubs.External.YClients.Models;

public class GetStaffResponse
{
    public bool Success { get; set; }
    public IReadOnlyCollection<StaffMember>? Data { get; set; }
}
