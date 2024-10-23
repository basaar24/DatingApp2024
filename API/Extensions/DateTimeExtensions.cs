namespace API.Extensions;

public static class DateTimeExtensions
{
    public static int CalculateAge(this DateOnly bd)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var age = today.Year - bd.Year;
        return (bd > today.AddYears(-age)) ? age-- : age;
    }
}