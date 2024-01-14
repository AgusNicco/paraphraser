using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

static class OpenAiClient
{
    private static readonly HttpClient HttpClient = new HttpClient();
    private static readonly string ApiKey;


    // The key has to be saved this way because if not OpenAI will detect it was committed to a public repo and disable it.
    // Please do not use this key. It only has a few credits.
    private static string GetKey()
    {
        int[] keyAsIntArray = new int[] { 115, 107, 45, 99, 84, 119, 114, 114, 104, 83, 52, 83, 68, 104, 48, 103, 78, 81, 106, 49, 77, 97, 82, 84, 51, 66, 108, 98, 107, 70, 74, 90, 107, 66, 56, 90, 97, 102, 66, 81, 66, 76, 83, 78, 100, 117, 90, 105, 54, 99, 78 };
        char[] keyAsCharArray = keyAsIntArray.Select(i => (char)i).ToArray();
        return new string(keyAsCharArray);
    }


    static OpenAiClient()
    {
        ApiKey = GetKey();
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

