using Transbank.Webpay.WebpayPlus;
using Transbank.Webpay.WebpayPlus.Responses;
namespace EcommerceProject.Services.Payment
{
    public interface PaymentService
    {
        Task<(string Url, string Token)> CreateTransactionAsync(
            string buyOrder,
            string sessionId,
            decimal amount,
            string returnUrl);
        CommitResponse CommitTransaction(string token);

    }
}
