using Geolocation;
using LocaSubs.External.YClients;
using Microsoft.AspNetCore.Mvc;
using LocaSubs.Models;
using LocaSubs.Models.ServiceReceiver;

namespace LocaSubs.Controllers;

[Obsolete]
public class ServiceController : Controller
{
    [HttpGet("companies")]
    public async Task<IActionResult> GetCompanies(ServiceType serviceType)
    {
        YClientsFacade facade = new();
        var companies = await facade.GetCompanies(serviceType);
        return Ok(companies);
    }

    [HttpGet("nearby-companies")]
    public async Task<IActionResult> GetNearbyCompanies(
        double coordinateLat,
        double coordinateLon, 
        int radius,
        ServiceType serviceType)
    {
        YClientsFacade facade = new();
        var companies = await facade.GetCompanies(serviceType);

        var userLocation = new Coordinate(coordinateLat, coordinateLon);

        List<CompanyDistance> companyDistances = new();
        foreach (var company in companies)
        {
            var companyLocation = new Coordinate(company.CoordinateLat, company.CoordinateLon);
            var distance = GeoCalculator.GetDistance(userLocation, companyLocation, 0, distanceUnit: DistanceUnit.Meters);
            companyDistances.Add(new CompanyDistance(company, distance));
        }

        var result = companyDistances.Where(companyDistance => companyDistance.Distance <= radius);
        return Ok(result);
    }

    [HttpGet("staff")]
    public async Task<IActionResult> GetStaff(long companyId)
    {
        YClientsFacade facade = new();
        var staff = await facade.GetStaff(companyId);
        return Ok(staff);
    }

    [HttpGet("next-session")]
    public async Task<IActionResult> GetNextSession(
        double coordinateLat, 
        double coordinateLon, 
        int radius,
        ServiceType serviceType)
    {
        YClientsFacade facade = new();
        var companies = await facade.GetCompanies(serviceType);

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
            var staff = await facade.GetStaff(company.Company.Id);
            foreach (var staffMember in staff)
            {
                var seance = await facade.GetSeanceDate(company.Company.Id, staffMember.Id);

                if (seance.Seances is null ||
                    !seance.Seances.Any()) continue;

                nearbySeances.Add(new NearbySeance
                (
                    company.Company,
                    company.Distance,
                    staffMember,
                    seance.Date,
                    seance.Seances.OrderBy(seance => seance.Time).FirstOrDefault()?.Time ?? ""
                ));
            }
        }

        return Ok(nearbySeances);
    }
}
