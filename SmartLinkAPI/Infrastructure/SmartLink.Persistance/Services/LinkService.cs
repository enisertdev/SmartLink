using Azure;
using Azure.Core;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using NUglify;
using OpenAI.Chat;
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
            //  var apiKey = _configuration["ApiKey"];
            //  if (string.IsNullOrEmpty(apiKey))
            //  {
            //      throw new Exception("API key eksik.");
            //  }
            //   var prompt = plainText + " \n bu metnin içeriğini detaylıca açıkla.Bazı önemli bilgileri ayrıca vurgula.";
            ////  var prompt = plainText + " \nexplain this text in detail.";

            //  var googleAi = new GoogleAI(apiKey: apiKey);
            //  var model = googleAi.GenerativeModel(model: Model.Gemini15Flash);
            //  var response = await model.GenerateContent(prompt);
            //deepseek r1
            var endpoint = new Uri(_configuration["AzureAI:Endpoint"]);
            var model = _configuration["AzureAI:Model"];
            var deploymentName = _configuration["AzureAI:DeploymentName"];
            var apiKey = _configuration["AzureAI:ApiKey"];

            AzureOpenAIClient azureClient = new(
                endpoint,
                new AzureKeyCredential(apiKey));
            ChatClient chatClient = azureClient.GetChatClient(deploymentName);

            var requestOptions = new ChatCompletionOptions()
            {
                MaxOutputTokenCount = 4096,
                Temperature = 1.0f,
                TopP = 1.0f,
            };

            List<ChatMessage> messages = new List<ChatMessage>()
            {
                new SystemChatMessage("explain the given text in detail."),
                new UserChatMessage(plainText),
            };

            var response = chatClient.CompleteChat(messages, requestOptions);
            return response.Value.Content[0].Text;
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
