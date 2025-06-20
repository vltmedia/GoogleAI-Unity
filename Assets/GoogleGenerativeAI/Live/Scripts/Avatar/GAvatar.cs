
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
namespace GenerativeAI.Unity
{

    public class GAvatar : MonoBehaviour
{
    public string apiKey { get { 
        return GGenerativeAIManager.APIKey;
        } }

        private const string ApiKey = "AIzaSyA8M_5CO1VHlTr_DwdVOSWkU3Bhhe8z9YQ";
        private const string Model = "models/gemini-2.5-flash-preview-native-audio-dialog";

        public string SystemInstruction = "You are Miss Memory, the almighty keeper of memories in the Memory Booth in the Italian American Museum. You talk in a 28 year old Italian american accent and sound fun and witty.";
        public string Endpoint = "wss://generativelanguage.googleapis.com/ws/google.ai.generativelanguage.v1beta.GenerativeService.BidiGenerateContent";


        public ClientWebSocket _webSocket;
        public CancellationTokenSource _cts = new CancellationTokenSource();


    DateTime debounceSend = DateTime.MinValue;
    public bool debounced
        {
            get
            {
                if(debounceSend < DateTime.Now)
                {
                    return true;
                }
                return false;
            }
        }


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        async void Start()
        {
            //var googleAI = new GoogleAi(apiKey);

            //// 2) Create a GenerativeModel using the model name "gemini-1.5-flash"
            //var generativeModel = googleAI.CreateGenerativeModel("models/gemini-1.5-flash");

            //// 3) Start a chat session from the GenerativeModel
            //var chatSession = generativeModel.StartChat();

            //// 4) Send and receive messages
            //var firstResponse = await chatSession.GenerateContentAsync("Welcome to the Gemini 1.5 Flash chat!");
            //Debug.Log("First response: " + firstResponse.Text());

            //// Continue the conversation
            //var secondResponse = await chatSession.GenerateContentAsync("How can you help me with my AI development?");
            //Debug.Log("Second response: " + secondResponse.Text());
            await ConnectAndRun();
        }

        private async Task ConnectAndRun()
        {
            _webSocket = new ClientWebSocket();
            _webSocket.Options.AddSubProtocol("chat.googleapis.com");
            var fullUri = new Uri($"{Endpoint}?key={ApiKey}");

            try
            {
                Debug.Log(" Connecting...");
                await _webSocket.ConnectAsync(fullUri, _cts.Token);
                Debug.Log(" WebSocket connected");
                SetupPayloadContainer setupPayload = new SetupPayloadContainer();
                setupPayload.setup.model = Model;
                setupPayload.setup.generationConfig.responseModalities.Add("AUDIO");
                // Send setup message
                
                string setupJson = JsonUtility.ToJson(setupPayload);
                Debug.Log(setupJson);
                await SendTextAsync(setupJson);
                _ = ReceiveLoop(); // Start receiving in the background
            }
            catch (Exception ex)
            {
                Debug.LogError(" WebSocket connection failed: " + ex);
            }
        }

        private async Task SendTextAsync(string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            var buffer = new ArraySegment<byte>(bytes);
            await _webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, _cts.Token);
            Debug.Log(" Sent setup message");
        }

        private async Task ReceiveLoop()
        {
            var buffer = new byte[8192];

            while (_webSocket.State == WebSocketState.Open)
            {
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _cts.Token);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    Debug.Log($" WebSocket closed: {_webSocket.CloseStatus} - {_webSocket.CloseStatusDescription}");
                    break;
                }

                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Debug.Log(" Message received: " + message);
            }
        }

        private void OnApplicationQuit()
        {
            _cts.Cancel();
            _webSocket?.Dispose();
        }

    }
}
