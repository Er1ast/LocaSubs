namespace LocaSubs.External.YClients.Common;

public static class Endpoints
{
    public static string Companies() 
        => "https://api.yclients.com/api/v1/companies";
    public static string BookStaff(long companyId) 
        => $"https://api.yclients.com/api/v1/book_staff/{companyId}";
    public static string BookStaffSeances(long companyId, long staffId) 
        => $"https://api.yclients.com/api/v1/book_staff_seances/{companyId}/{staffId}";
}