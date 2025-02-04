using CommunityToolkit.WinForms.AI;
using CommunityToolkit.WinForms.AI.ConverterLogic;
using CommunityToolkit.WinForms.BasicTests.Helper;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace CommunityToolkit.WinForms.BasicTests.SemanticKernel;

public class SemanticKernelTest
{
    [Fact]
    public async Task ReturnTokenParserTest()
    {
        // Arrange
        var mockList = new List<StreamingChatMessageContent>()
        {
            new(AuthorRole.User, "Hello {meta} World"),
            new(AuthorRole.User, "This is a test")
        };

        var mockData = mockList.ToIAsyncEnumerable();

        var receivedMetaData = new List<string>();
        var receivedParagraphs = new List<string>();

        void OnReceivedMetaData(AsyncReceivedInlineMetaDataEventArgs args)
        {
            receivedMetaData.Add(args.MetaData);
        }

        void OnReceivedNextParagraph(AsyncReceivedNextParagraphEventArgs args)
        {
            receivedParagraphs.Add(args.Paragraph);
        }

        // Act
        var result = new List<string>();
        await foreach (var token in ReturnTokenParser.ProcessTokens(mockData, OnReceivedMetaData, OnReceivedNextParagraph))
        {
            result.Add(token);
        }

        // Assert
        Assert.Equal(["Hello", "World", "This", "is", "a", "test"], result);
        Assert.Equal(["meta"], receivedMetaData);
        Assert.Equal(["Hello World", "This is a test"], receivedParagraphs);
    }
}
