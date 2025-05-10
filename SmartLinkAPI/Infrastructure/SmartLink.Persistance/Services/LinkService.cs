using Azure;
using Azure.Core;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using Mscc.GenerativeAI;
using NUglify;
using OpenAI.Chat;
using SmartLink.Application.Services;
using ChatMessage = OpenAI.Chat.ChatMessage;

namespace SmartLink.Persistance.Services
{
    public class LinkService : ILinkService
    {

        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public LinkService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<string> GetAiSummary(string url)
        {

            var plainText = await GetPlainTextFromHtml(url);
            return await GetAzureReply("make a summary of the text given you with bullet points and make more details of those bullet points with examples if necessary.Ignore and html or site related text.", plainText);
        }

        public async Task<string> CreateSummaryTitle(string url)
        {
            var plainText = await GetPlainTextFromHtml(url);
            return await GetAzureReply("make a title of the context in 1 line", plainText);

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

        private async Task<string> GetAzureReply(string systemChatMessage, string userChatMessage)
        {

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
                new SystemChatMessage(userChatMessage),
                new SystemChatMessage(systemChatMessage),
                new UserChatMessage(userChatMessage),
            };

            var response = await chatClient.CompleteChatAsync(messages, requestOptions);
            return response.Value.Content[0].Text;
        }

        private async Task<string> GetDeepseekReply(string text, string systemPrompt)
        {
            var apiKey = _configuration["ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new Exception("API key eksik.");
            }
            var prompt = text + $"\n{systemPrompt}";
            var googleAi = new GoogleAI(apiKey: apiKey);
            var model = googleAi.GenerativeModel(model: Model.Gemini15Flash);
            var response = await model.GenerateContent(prompt);
            return response.Text;
        }
    }
}
