using CommunityToolkit.WinForms.AI;
using CommunityToolkit.WinForms.Controls.Blazor;

namespace Chatty.DataEntities;

[StructuredReturnData(
    Prompt = ClassPrompt, 
    ProvideDate = true, 
    ProvideTime = true)]
public class ChatMetaData
{
    private const string ClassPrompt = """
        You are an assistant in an LLM Chat-Bot client software. 
        You're purpose is to provide certain meta information 
        like chat summaries or brief headlines for a chat.
        """;

    private const string ChatTitlePrompt = """
        The title of the chat. 
        Make sure that the title is no longer than 40 characters
        """;

    private const string ChatSummaryPrompt = """
        A brief summary of the chat. 
        Make sure that the summary is no longer than 2000 characters
        """;

    private const string ChatKeywordsPrompt = """
        5 Keywords that are relevant to the chat, and separated by semicolon. 
        Try to find the keywords in such a way that they become increasingly 
        specific/concrete from original rather generalized.
        """;

    private const string CountTokensPrompt = """
        The number of tokens used in the chat so far.
        """;

    public const string MemoryFactListPrompt = """
        A list of phrased out facts worth rembering long terms from the chat.
        Important is, that it needs to be facts that are
        stated by the user about the user or the user's circumstances.

        For example, if the user mentions in the chat, 
        "I have a dog named Fido and he is a golden retriever. I learned that 
        golden retrievers always eat what they can get, is that true?" then we
        derive that the user has a dog named Fido and that Fido is a golden
        retriever. We do not derive that golden retrievers always eat what they
        can get, since that was not stated as a fact but was rather a question.
        Begin every new memory fact in a new line and start with '* '.

        If it is not a personal fact about the user, we're not capturing it.
        """;

    [StructuredReturnDataProperty(ChatTitlePrompt)]
    public string? ChatTitle { get; set; }

    [StructuredReturnDataProperty(ChatSummaryPrompt)]
    public string? ChatSummary { get; set; }

    [StructuredReturnDataProperty(ChatKeywordsPrompt)]
    public string? ChatKeywords { get; set; }

    [StructuredReturnDataProperty(CountTokensPrompt)]
    public int TokenCount { get; set; }

    [StructuredReturnDataProperty(MemoryFactListPrompt)]
    public string? MemoryFactList { get; set; }

    public void Merge(Conversation conversation)
    {
        conversation.Title = ChatTitle!;
        conversation.Summary = ChatSummary!;
        conversation.Keywords = ChatKeywords!;
        conversation.TokenCount = TokenCount;
        conversation.DateLastEdited = DateTimeOffset.Now;
    }
}
