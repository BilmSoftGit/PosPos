using Microsoft.Extensions.Configuration;
using Pospos.AdminUI.Models;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pospos.AdminUI.Helpers
{
    public class RecaptchaValidator : IRecaptchaValidator
    {
        private const string GoogleRecaptchaAddress = "https://www.google.com/recaptcha/api/siteverify";

        public readonly IConfiguration Configuration;

        public RecaptchaValidator(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<bool> IsRecaptchaValid(string token)
        {
            using var client = new HttpClient();
            var response = await client.GetStringAsync($@"{GoogleRecaptchaAddress}?secret={Configuration["Google:RecaptchaV3SecretKey"]}&response={token}");
            var recaptchaResponse = JsonSerializer.Deserialize<RecaptchaResponse>(response);

            if (!recaptchaResponse.Success || recaptchaResponse.Score < Convert.ToDecimal(Configuration["Google:RecaptchaMinScore"]))
            {
                return false;
            }
            return true;
        }

    }
}
