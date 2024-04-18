using Azure;
using Azure.AI.OpenAI;

namespace ProfileSummaryDemo
{
    public class AiUtil
    {
        //The OpenAI model to use for processing
        public const string DEFAULT_OPENAI_MODEL = "gpt-4-turbo-preview";

        private OpenAIClient _client;

        public AiUtil(IConfiguration config)
        {
            _client = new OpenAIClient(config["OpenAIKey"]);
        }

        //Summarise the content of a string into a single paragraph and return as HTML
        public async Task<string> SummariseContent(string content)
        {
            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                DeploymentName = DEFAULT_OPENAI_MODEL,
                Messages =
                {
                    new ChatRequestSystemMessage("You are a helpful assistant, all responses will be in HTML format."),
                    new ChatRequestUserMessage("Summarise the following into single paragraph: " + Environment.NewLine + content),
                }
            };

            Response<ChatCompletions> response = _client.GetChatCompletions(chatCompletionsOptions);
            var result = response.Value.Choices[0].Message.Content;

            //Because we have asked for it in HTML content, OpenAI will add in some additional markup that we need to remove
            result = result.Replace("```html", "").Replace("```", "");

            return result;
        }

        //Answer a question about some content and return as HTML
        public async Task<string> FollowUp(string question, string originalContent)
        {
            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                DeploymentName = DEFAULT_OPENAI_MODEL,
                Messages =
                {
                    new ChatRequestSystemMessage("You are a helpful assistant, all responses will be in HTML format."),
                    new ChatRequestUserMessage("Regarding the following content, answer the question '"+question+"': " + Environment.NewLine + originalContent),
                }
            };

            Response<ChatCompletions> response = _client.GetChatCompletions(chatCompletionsOptions);
            var result = response.Value.Choices[0].Message.Content;

            //Because we have asked for it in HTML content, OpenAI will add in some additional markup that we need to remove
            result = result.Replace("```html", "").Replace("```", "");

            return result;
        }
    }
}
