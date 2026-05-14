namespace DPP.InternalWebhookHost.Application.Common.Interfaces;
public interface IDateRange
{
    DateTime FromDate { get; set; }
    DateTime ToDate { get; set; }
}

public interface IPagingParameter
{
    int PageNumber { get; set; }
    int PageSize { get; set; }
}

public interface ICommonFilterRequest : IDateRange, IPagingParameter
{
    //string? SearchTerm { get; set; }
}