using DPP.Security.MultiAuth.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace DPP.PartnerPaymentIntegration.Infrastructure.Persistence
{
    public class AuthDbConnectionFactory : IAuthDbConnectionFactory
    {
        private readonly string _connectionString;
        private readonly ILogger<AuthDbConnectionFactory> _logger;

        public AuthDbConnectionFactory(IConfiguration configuration, ILogger<AuthDbConnectionFactory> logger)
        {
            _connectionString = configuration.GetConnectionString("CoreMerchant") ?? "";
            _logger = logger;
        }

        public async Task<IDbConnection> GetOpenConnection(CancellationToken cancellationToken)
        {
            try
            {
                var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync(cancellationToken);
                return connection;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to open DB connection.");
                throw;
            }
        }
    }
}
