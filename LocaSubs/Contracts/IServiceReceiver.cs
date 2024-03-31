using LocaSubs.Models;
using LocaSubs.Models.External.YClients;
using LocaSubs.Models.ServiceReceiver;

namespace LocaSubs.Contracts;

public interface IServiceReceiver
{
    Task<IReadOnlyCollection<Company>> GetCompaniesAsync(ServiceType serviceType);
    Task<IReadOnlyCollection<CompanyDistance>> GetNearbyCompaniesAsync(
        double coordinateLat, 
        double coordinateLon, 
        long radius,
        ServiceType serviceType);
    Task<IReadOnlyCollection<StaffMember>> GetStaffAsync(long companyId);
    Task<IReadOnlyCollection<NearbySeance>> GetNextSessionAsync(
        double coordinateLat, 
        double coordinateLon, 
        long radius,
        ServiceType serviceType);
}
