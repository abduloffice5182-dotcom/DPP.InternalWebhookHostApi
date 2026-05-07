using DPP.PartnerPaymentIntegration.Application.Operations.Fortellis.Queries.Requests;
using DPP.PartnerPaymentIntegration.Domain.Common.ExceptionHandling;
using DPP.PartnerPaymentIntegration.Domain.Entities;
using DPP.PartnerPaymentIntegration.Infrastructure.Interfaces;
using DPP.PartnerPaymentIntegration.Infrastructure.Repositories;
using DPP.Security.MultiAuth.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPP.PartnerPaymentIntegration.Application.Operations.Fortellis.Queries.Handlers
{
    /// <summary>
    /// Handles the retrieval of invoice amount from the DPP by invoice ID.
    /// </summary>
    public class GetInvoiceAmountByInvoiceIdHandler : IRequestHandler<GetInvoiceAmountRequest, GetInvoiceAmountResponse>
    {
        private readonly ILogger<GetInvoiceAmountByInvoiceIdHandler> _logger;
        private readonly IFortellisRepositories _fortellisRepositories;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IMerchantRepository _merchantRepository;
        private readonly IDepartmentRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetInvoiceAmountByInvoiceIdHandler"/> class.
        /// </summary>
        /// <param name="logger">Logger instance for logging operations and errors.</param>
        /// <param name="fortellisRepositories">Fortellis repositories interface.</param>
        public GetInvoiceAmountByInvoiceIdHandler(ILogger<GetInvoiceAmountByInvoiceIdHandler> logger,IFortellisRepositories fortellisRepositories, ICurrentUserContext currentUserContext, IMerchantRepository merchantRepository, IDepartmentRepository repository)
        {
            _logger = logger;
            _fortellisRepositories = fortellisRepositories;
            _currentUserContext = currentUserContext;
            _merchantRepository = merchantRepository;
            _repository = repository;
        }

        /// <summary>
        /// Handles the request to get the amount by invoice ID.
        /// </summary>
        /// <param name="request">The query containing InvoiceId.</param>
        /// <param name="cancellationToken">Cancellation token for the async operation.</param>
        /// <returns>The response containing the invoice amount in DPP.</returns>
        public async Task<GetInvoiceAmountResponse> Handle(GetInvoiceAmountRequest request, CancellationToken cancellationToken)
        {
            try
            {
                
                Guid.TryParse(_currentUserContext?.PartnerToken, out Guid partnerToken);
                var accountId = await _merchantRepository.GetAccountIdByOktaIDAsync(partnerToken, cancellationToken);

                var departmentId = await _repository.GetDepartmentIdByCodeAsync(accountId,request.DepartmentCode, cancellationToken);
                
                if (departmentId == 0)
                {
                    throw new BadRequestCustomException([$"Invalid department code {request.DepartmentCode} for this merchant"]);
                }

                var response = await _fortellisRepositories.GetInvoiceAmountByInvoiceId(request.InvoiceId, departmentId, cancellationToken);

                return new GetInvoiceAmountResponse
                {
                    InvoiceAmount = response.InvoiceAmount,
                    Status = "success"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception thrown in GetInvoiceAmountByInvoiceIdHandler.Handle for request: {request}", JsonConvert.SerializeObject(request));
                throw;
            }
        }
    }
}
