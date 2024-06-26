using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;

public class ChatGPTManager
{
    private OpenAIApi openAI = new OpenAIApi(/*INSERT OPENAI_API_KEY HERE*/);
    //
    private List<ChatMessage> messages = new List<ChatMessage>();
    public string storedResponse="";

    public async void AskChatGPT(string newText)
    {
        ChatMessage newMessage = new ChatMessage();
        newMessage.Content = newText;
        newMessage.Role = "user";

        messages.Add(newMessage);
        
        CreateChatCompletionRequest request = new CreateChatCompletionRequest();
        request.Messages = messages;
        request.Model = "gpt-3.5-turbo";

        var response = await openAI.CreateChatCompletion(request);

        if (response.Choices != null && response.Choices.Count > 0)
        {
            var chatResponse = response.Choices[0].Message;
            messages.Add(chatResponse);

            storedResponse = chatResponse.Content;

            //Debug.Log(chatResponse.Content);
            //UnityEngine.Debug.Log(storedResponse);
        }
    }

}
