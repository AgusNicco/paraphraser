internal static class Prompts
{
    
    public static string ConstructSimpleParagrapPrompt(string paragraph)
    {
        string prompt = @$"
You are going rewrite the following paragraph so it is more simplistic. Make sure to include all important information when rewriting. We want to change the style and tone, not the content. Make sure the new paragraph is as long as the original.
Rewrite it by presenting facts as they are, without any descriptive words such as adjectives, adverbs, etc, unless strictly necessary.

Paragraph to rewrite: 

'{paragraph}'

Respond to the message with the rewritten paragraph and no further comments.";

        return prompt;
    }


    public static string ConstructBannedVocabularyParagrapPrompt(string paragraph)
    {
        string prompt = @$"
You are going to help me rewrite the following paragraph. Make sure to include all important information when rewriting, we want to change the style and tone, not the content. Make sure the new paragraph is as long as the original.

Rewrite the paragraph without using any of the following words/terms (no exceptions):

{Logic.GetWords("banned_words.json")}

You are encouraged to use any of the following words/punctuation/concepts when appropriate:

{Logic.GetWords("encouraged_words.json")}

Also, if the paragraph has abreviations, remove the dots, example: Dr. becomes Dr, Mr. becomes Mr, etc.

Here is the paragraph:

'{paragraph}'

Respond to the message with the rewritten paragraph and no further comments.";

        return prompt;
    }

    
    public static string WordsToReplacePrompt(string sentence, string wordType, string paragraph = "")
    {

string diffPrompt = @$"Follow instructions to rewrite the following sentence:

Sentence = {sentence}

Instructions: 

1- Identify and list which words in the sentence are {wordType}s (if any).
2- Rewrite the sentence so each of these words is replaced by a {wordType} with a similar meaning that is more simplistic. Your choice should try to conserve the original meaning of the sentence, add prepositions if necessary, or make adjustments if necessary. If the word is irreplaceable, leave it as is.
3- Write anthe new version and asses that the new sentence conserves the original meaning. If it doesn't rewrite it again.
4- Once it has been simplified and the assesment has been made return the new sentence between braces {{}}. Example:" +"{This is the new sentence.}";

        return diffPrompt;
    }
}