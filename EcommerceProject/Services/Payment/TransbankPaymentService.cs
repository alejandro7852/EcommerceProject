using Microsoft.Extensions.Options;
using Transbank.Webpay.WebpayPlus;
using EcommerceProject.Models.Configuration;
using Transbank.Webpay.WebpayPlus.Responses;

namespace EcommerceProject.Services.Payment
{
    public class TransbankPaymentService : PaymentService
    {
        private readonly TransbankSettings _settings;

        public TransbankPaymentService(
            IOptions<TransbankSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<(string Url, string Token)> CreateTransactionAsync(
            string buyOrder,
            string sessionId,
            decimal amount,
            string returnUrl)
        {
            var transaction = Transaction.buildForIntegration(
                _settings.CommerceCode,
                _settings.ApiKey);

            var response = transaction.Create(
                buyOrder,
                sessionId,
                amount,
                returnUrl);

            return (response.Url, response.Token);
        }

        public CommitResponse CommitTransaction(string token)
        {
            var transaction = Transaction.buildForIntegration(
                _settings.CommerceCode,
                _settings.ApiKey);

            var response = transaction.Commit(token);

            return response;
        }
    }
}