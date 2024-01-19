
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

internal static class Logic
{
    public static async Task<string[]> RewriteParagraphWithoutBannedTerms(string[] paragraphs)
    {
        paragraphs = await Task.WhenAll(paragraphs.Select(async paragraph =>
        {
            if (paragraph.Contains(".") && paragraph.Split(".").Length > 2 && paragraph.Length > 30)
            {
                return await OpenAiClient.GetChatGptResponse(Prompts.ConstructBannedVocabularyParagrapPrompt(paragraph));
            }
            return paragraph;
        }));
        return paragraphs;
    }

    public static async Task<string[]> RewriteParagraphForSimplicity(string[] paragraphs)
    {
        paragraphs = await Task.WhenAll(paragraphs.Select(async paragraph =>
        {
            if (paragraph.Contains(".") && paragraph.Split(".").Length > 2 && paragraph.Length > 6)
            {
                return await OpenAiClient.GetChatGptResponse(Prompts.ConstructSimpleParagrapPrompt(paragraph));
            }
            return paragraph;
        }));
        return paragraphs;
    }


    public static async Task<string> Paraphrase(string text)
    {
        string[] paragraphs = text.Split("\n\n");

        paragraphs = await RewriteParagraphForSimplicity(paragraphs);
        paragraphs = await RewriteParagraphWithoutBannedTerms(paragraphs);

        // print text
        foreach (string paragraph in paragraphs)
        {
            Console.WriteLine("\n\n\n\n" + paragraph);
        }
        Console.WriteLine("\n\n\n\n");

        List<Task<string>> paragraphTasks = new List<Task<string>>();

        foreach (string paragraph in paragraphs)
        {
            if (!paragraph.Contains(".") || paragraph.Split(".").Length <= 2)
            {
                paragraphTasks.Add(Task.FromResult(paragraph + "\n\n"));
                continue;
            }

            paragraphTasks.Add(RewriteParagraph(paragraph));
        }

        string[] newParagraphs = await Task.WhenAll(paragraphTasks);
        newParagraphs = newParagraphs.Where(x => x.Length > 0).ToArray();
        string newText = string.Join("\n\n", newParagraphs);

        return newText;
    }

    private static async Task<string> RewriteParagraph(string paragraph)
    {
        string[] sentences = paragraph.Split(new char[] { '.', '?', '!' }, StringSplitOptions.RemoveEmptyEntries);
        List<Task<string>> sentenceTasks = sentences.Select(RewriteSentence).ToList();

        string[] newSentences = await Task.WhenAll(sentenceTasks);
        return string.Join(" ", newSentences);
    }

    private static async Task<string> RewriteSentence(string sentence)
    {
        sentence = await ReplaceWords(Prompts.WordsToReplacePrompt(sentence, "verb", sentence), sentence);
        sentence = await ReplaceWords(Prompts.WordsToReplacePrompt(sentence, "adverb", sentence), sentence);
        sentence = await ReplaceWords(Prompts.WordsToReplacePrompt(sentence, "adjective", sentence), sentence);
        return sentence;
    }


    public static async Task<string> ReplaceWords(string prompt, string sentence)
    {
        if (sentence.Length < 12 || sentence.Split(" ").Length < 5)
        {
            return sentence;
        }
        string response = await OpenAiClient.GetChatGptResponse(prompt);

        Console.WriteLine("\n\n\n\nResponse: " + response);
        // Console.WriteLine("\n\nPrompt: " + prompt);

        return ExtractResponse(response);
    }

    public static string GetWords(string path = "banned_words.json")
    {
        string output = "[";
        string jsonData = File.ReadAllText(path);
        var json = JObject.Parse(jsonData);
        string[] words = json["words"].ToObject<string[]>();
        foreach (string word in words)
        {
            output += "\"" + word + "\", ";
        }
        output += "]";
        return output;
    }
    
    public static string ExtractResponse(string response)
    {
        int lastStartIndex = -1;
        int lastEndIndex = -1;
        int startIndex = 0;

        while ((startIndex = response.IndexOf("{", startIndex)) != -1)
        {
            int endIndex = response.IndexOf("}", startIndex);
            if (endIndex != -1)
            {
                lastStartIndex = startIndex;
                lastEndIndex = endIndex;
                startIndex = endIndex + 1; // Move past the current end index for the next iteration
            }
            else
            {
                break;
            }
        }

        if (lastStartIndex != -1 && lastEndIndex != -1 && lastEndIndex > lastStartIndex)
        {
            string newSentence = response.Substring(lastStartIndex + 1, lastEndIndex - lastStartIndex - 1).Trim();

            if (!newSentence.Contains("."))
            {
                newSentence += ".";
            }
            newSentence = newSentence.Replace("{", "").Replace("}", "");
            return newSentence;
        }
        else
        {
            return "Error: No sentence found within double curly braces.";
        }
    }
}
    

