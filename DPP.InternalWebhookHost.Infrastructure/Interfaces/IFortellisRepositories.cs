namespace DPP.PartnerPaymentIntegration.Infrastructure.Interfaces
{
    public interface IFortellisRepositories
    {
        public Task<FortellisTransactionResponse> GetFortellisTransactionAsync(Guid transactionId, string departmentCode, CancellationToken cancellationToken);
        public Task<long> InsertFortellisTransationAsync(FortellisPaymentInsertRequest request, CancellationToken cancellationToken);
        public Task<long> UpdateFortellisTransationAsync(Guid promiseId, long transactionRecordId, string status, CancellationToken cancellationToken);
        public Task<IEnumerable<GetFortellisPendingResponse>> GetFortellisPendingTransationAsync(CancellationToken cancellationToken);
        public Task<GetInvoiceAmountResponse> GetInvoiceAmountByInvoiceId(string invoiceId,long departmentId, CancellationToken cancellationToken); 
        public Task<GetTransactionDetailResponse> GetTransactionDetailAsync(Guid transactionId, CancellationToken cancellationToken);
        public Task<long> CheckDepartmentCode(string departmentCode, long accountId, CancellationToken cancellationToken);
        public Task<bool> CheckProductEnabled(string productCode, long accountId, CancellationToken cancellationToken);
    }
}
