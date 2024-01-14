using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

static class OpenAiClient
{
    private static readonly HttpClient HttpClient = new HttpClient();

    // This key has a very limited amount of credit. Using it will only work for a few requests.
    private const string ApiKey = "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";


    static OpenAiClient()
    {
        HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");
    }

    public static async Task<string> GetChatGptResponse(string prompt)
    {
        var requestContent = new
        {
            model = "gpt-4-1106-preview",
            // model = "gpt-3.5-turbo-1106",
            messages = new[]
            {
            new { role = "system", content = "You are a chatbot." },
            new { role = "user", content = prompt }
            }
        };

        var jsonContent = JsonConvert.SerializeObject(requestContent);
        var httpResponse = await HttpClient.PostAsync(
            "https://api.openai.com/v1/chat/completions",
            new StringContent(jsonContent, Encoding.UTF8, "application/json"));

        httpResponse.EnsureSuccessStatusCode();

        var responseContent = await httpResponse.Content.ReadAsStringAsync();
        dynamic responseObject = JsonConvert.DeserializeObject(responseContent);

        string responseMessage = responseObject.choices[0].message.content;

        return responseMessage;
    }
}

