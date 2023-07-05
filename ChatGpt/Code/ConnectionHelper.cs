using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;

namespace ChatGpt.Code
{
    public interface IOpenAIProxy
    {
        Task<ChatCompletionMessage[]> SendChatMessage(string message, int MaxTokens = 800);
    }
}
