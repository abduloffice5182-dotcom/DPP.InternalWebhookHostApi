namespace DPP.PartnerPaymentIntegration.Infrastructure.Interfaces
{
    public interface IMerchantRepository
    {
        Task<long> GetAccountIdByOktaIDAsync(Guid partnerToken, CancellationToken cancellationToken);
        Task<GetMerchantDetailResponse> GetMerchantDetailAsync(long merchantConfigPaymentMethodID, CancellationToken cancellationToken);

    }
}
