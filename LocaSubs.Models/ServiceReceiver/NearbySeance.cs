using LocaSubs.Models.External.YClients;

namespace LocaSubs.Models.ServiceReceiver;

public record NearbySeance
{
    public Company Company { get; init; }
    public double Distance { get; init; }
    public StaffMember StaffMember { get; init; }
    public DateOnly Date { get; init; }
    public string Time { get; init; }

    public NearbySeance(
        Company company, 
        double distance,
        StaffMember staffMember, 
        DateOnly date, 
        string time)
    {
        Company = company;
        Distance = distance;
        StaffMember = staffMember;
        Date = date;
        Time = time;
    }
}