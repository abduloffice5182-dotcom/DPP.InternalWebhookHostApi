using AutoMapper;
using DPP.PartnerPaymentIntegration.Application.Operations.Fortellis.Queries.Requests;
using DPP.PartnerPaymentIntegration.Domain.Common.ExceptionHandling;
using DPP.PartnerPaymentIntegration.Domain.Entities;
using DPP.PartnerPaymentIntegration.Infrastructure.ExternalAPIs.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DPP.PartnerPaymentIntegration.Application.Operations.Fortellis.Queries.Handlers
{
    /// <summary>
    /// Handles the retrieval of invoice amount from the Fortellis API by invoice ID and department code.
    /// </summary>
    public class GetAmountByInvoiceIdHandler : IRequestHandler<GetAmountByInvoiceIdQuery, GetAmountByInvoiceIdResponse>
    {
        private readonly IFortellisIntegration _fortellisIntegration;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAmountByInvoiceIdHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAmountByInvoiceIdHandler"/> class.
        /// </summary>
        /// <param name="configuration">Application configuration instance.</param>
        /// <param name="fortellisIntegration">Fortellis integration service instance.</param>
        /// <param name="mapper">AutoMapper instance for mapping responses.</param>
        /// <param name="logger">Logger instance for logging operations and errors.</param>
        public GetAmountByInvoiceIdHandler(
            IConfiguration configuration,
            IFortellisIntegration fortellisIntegration,
            IMapper mapper,
            ILogger<GetAmountByInvoiceIdHandler> logger)
        {
            _configuration = configuration;
            _fortellisIntegration = fortellisIntegration;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Handles the request to get the amount by invoice ID.
        /// </summary>
        /// <param name="request">The query containing DepartmentCode and InvoiceId.</param>
        /// <param name="cancellationToken">Cancellation token for the async operation.</param>
        /// <returns>The response containing the invoice amount.</returns>
        /// <exception cref="BadRequestCustomException">Thrown when the Fortellis API returns an error or invalid response.</exception>
        public async Task<GetAmountByInvoiceIdResponse> Handle(GetAmountByInvoiceIdQuery request, CancellationToken cancellationToken)
        {
            // Manually invoke FluentValidation for the query
            var validator = new Validators.GetAmountByInvoiceIdQueryValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                throw new BadRequestCustomException(errors);
            }

            var response = await _fortellisIntegration.RetrieveInvoiceAmountAsync(request.DepartmentCode, request.InvoiceId, cancellationToken);
            var mappedResponse = _mapper.Map<GetAmountByInvoiceIdResponse>(response);
            return mappedResponse;
        }
    }
}
