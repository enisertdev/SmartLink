using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI;
using NUglify;
using PuppeteerSharp;
using SmartLink.Application.Services;

namespace SmartLink.Persistance.Services
{
    public class LinkService : ILinkService
    {

        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient = new();

        public LinkService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetAiSummary(string url)
        {

            var plainText = await GetPlainTextFromHtml(url);
            var apiKey = _configuration["ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new Exception("API key eksik.");
            }
            // var prompt = plainText + " \n bu metnin içeriğini detaylıca açıkla.Bazı önemli bilgileri ayrıca vurgula.";
            var prompt = plainText + " \nexplain this text in detail.";

            var googleAi = new GoogleAI(apiKey: apiKey);
            var model = googleAi.GenerativeModel(model: Model.Gemini15Pro);
            var response = await model.GenerateContent(prompt);

            return response.Text;
        }

        private async Task<string> GetPlainTextFromHtml(string url)
        {
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var html = await response.Content.ReadAsStringAsync();
                var result = Uglify.HtmlToText(html);
                return result.Code;
            }

            return $"Failed to fetch HTML: {response.StatusCode}";


        }
    }
}
