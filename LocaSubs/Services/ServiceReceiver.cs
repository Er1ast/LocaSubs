using Geolocation;
using LocaSubs.External.YClients;
using LocaSubs.Contracts;
using LocaSubs.Models;
using LocaSubs.Models.External.YClients;
using LocaSubs.Models.ServiceReceiver;

namespace LocaSubs.Services;

public class ServiceReceiver : IServiceReceiver
{
    private readonly IYClientsFacade _yClientsFacade;

    public ServiceReceiver(IYClientsFacade yClientsFacade)
    {
        _yClientsFacade = yClientsFacade;
    }

    public async Task<IReadOnlyCollection<Company>> GetCompaniesAsync(ServiceType serviceType)
    {
        var companies = await _yClientsFacade.GetCompanies(serviceType);
        return companies;
    }

    public async Task<IReadOnlyCollection<CompanyDistance>> GetNearbyCompaniesAsync(
        double coordinateLat,
        double coordinateLon,
        long radius,
        ServiceType serviceType)
    {
        var companies = await _yClientsFacade.GetCompanies(serviceType);

        var userLocation = new Coordinate(coordinateLat, coordinateLon);

        List<CompanyDistance> companyDistances = new();
        foreach (var company in companies)
        {
            var companyLocation = new Coordinate(company.CoordinateLat, company.CoordinateLon);
            var distance = GeoCalculator.GetDistance(userLocation, companyLocation, 0, distanceUnit: DistanceUnit.Meters);
            companyDistances.Add(new CompanyDistance(company, distance));
        }

        var result = companyDistances.Where(companyDistance => companyDistance.Distance <= radius);
        return result.ToList();
    }

    public async Task<IReadOnlyCollection<StaffMember>> GetStaffAsync(long companyId)
    {
        var staff = await _yClientsFacade.GetStaff(companyId);
        return staff;
    }

    public async Task<IReadOnlyCollection<NearbySeance>> GetNextSessionAsync(
        double coordinateLat,
        double coordinateLon,
        long radius,
        ServiceType serviceType)
    {
        var companies = await _yClientsFacade.GetCompanies(serviceType);

        var userLocation = new Coordinate(coordinateLat, coordinateLon);

        List<CompanyDistance> companyDistances = new();
        foreach (var company in companies)
        {
            var companyLocation = new Coordinate(company.CoordinateLat, company.CoordinateLon);
            var distance = GeoCalculator.GetDistance(userLocation, companyLocation, 0, distanceUnit: DistanceUnit.Meters);
            companyDistances.Add(new CompanyDistance(company, distance));
        }

        var nearbyCompanies = companyDistances.Where(companyDistance => companyDistance.Distance <= radius);

        List<NearbySeance> nearbySeances = new();
        foreach (var company in nearbyCompanies)
        {
            var staff = await _yClientsFacade.GetStaff(company.Company.Id);

            NearbySeance nearbySeance = null;
            foreach (var staffMember in staff)
            {
                var seance = await _yClientsFacade.GetSeanceDate(company.Company.Id, staffMember.Id);

                if (seance.Seances is null ||
                    !seance.Seances.Any()) continue;

                var seanceTime = seance.Seances.OrderBy(seance => seance.Time).FirstOrDefault()?.Time ?? "";

                if (nearbySeance is null ||
                    seance.Date <= nearbySeance.Date)
                {
                    nearbySeance = new NearbySeance
                        (
                            company.Company,
                            company.Distance,
                            staffMember,
                            seance.Date,
                            seance.Seances.OrderBy(seance => seance.Time).FirstOrDefault()?.Time ?? ""
                        );
                } 
            }
            if (nearbySeance is not null) nearbySeances.Add(nearbySeance);
        }

        return nearbySeances;
    }
}
