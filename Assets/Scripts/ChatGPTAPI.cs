using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

public class ChatGPTAPI : MonoBehaviour
{
    private const string API_URL = "https://api.openai.com/v1/chat/completions";
    private const string API_KEY = "YOUR_OPENAI_API_KEY"; // Replace with your API key

    [Serializable]
    public class ChatGPTRequest
    {
        public string model = "gpt-4";
        public List<Message> messages;
        public float temperature = 0.7f;

        public ChatGPTRequest(List<Message> messages)
        {
            this.messages = messages;
        }
    }

    [Serializable]
    public class Message
    {
        public string role;
        public string content;

        public Message(string role, string content)
        {
            this.role = role;
            this.content = content;
        }
    }

    [Serializable]
    public class ChatGPTResponse
    {
        public List<Choice> choices;
    }

    [Serializable]
    public class Choice
    {
        public Message message;
    }

    private static readonly HttpClient client = new HttpClient();

    public async Task<string> SendMessageToChatGPT(string userMessage)
    {
        List<Message> messages = new List<Message>
        {
            new Message("system", "You are a helpful assistant."),
            new Message("user", userMessage)
        };

        ChatGPTRequest request = new ChatGPTRequest(messages);
        string jsonRequest = JsonConvert.SerializeObject(request);

        using (HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, API_URL))
        {
            httpRequest.Headers.Add("Authorization", $"Bearer {API_KEY}");
            httpRequest.Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.SendAsync(httpRequest);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                ChatGPTResponse chatResponse = JsonConvert.DeserializeObject<ChatGPTResponse>(jsonResponse);
                return chatResponse?.choices[0].message.content ?? "No response from ChatGPT.";
            }
            catch (Exception ex)
            {
                Debug.LogError("Error calling ChatGPT API: " + ex.Message);
                return "Error: Unable to connect to ChatGPT.";
            }
        }
    }

    // Example call in Unity
    public async void TestChatGPT()
    {
        string response = await SendMessageToChatGPT("Hello, how are you?");
        Debug.Log("ChatGPT Response: " + response);
    }
}
