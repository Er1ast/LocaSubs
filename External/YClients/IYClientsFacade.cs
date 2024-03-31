using LocaSubs.Models.External.YClients;
using LocaSubs.Models;

namespace LocaSubs.External.YClients
{
    public interface IYClientsFacade
    {
        Task<IReadOnlyCollection<Company>> GetCompanies(ServiceType serviceType);
        Task<IReadOnlyCollection<StaffMember>> GetStaff(long companyId);
        Task<SeanceDate> GetSeanceDate(long companyId, long staffId);
    }
}
