using GenerativeAI;
using GenerativeAI.Live;
using GenerativeAI.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
namespace GenerativeAI.Unity
{

    public class GAvatar : MonoBehaviour
{
    public string apiKey { get { 
        return GGenerativeAIManager.APIKey;
        } }

    public string ModelName = "gemini-1.5-flash-exp";
    public string SystemInstruction = "You are a helpful assistant.";
    public List<Modality> ResponseModalities = new List<Modality> { Modality.TEXT, Modality.AUDIO };
    public List<SafetySetting> SafetySettings = null;
    public static MultiModalLiveClient Client;

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
            var googleAI = new GoogleAi(apiKey);

            // 2) Create a GenerativeModel using the model name "gemini-1.5-flash"
            var generativeModel = googleAI.CreateGenerativeModel("models/gemini-1.5-flash");

            // 3) Start a chat session from the GenerativeModel
            var chatSession = generativeModel.StartChat();

            // 4) Send and receive messages
            var firstResponse = await chatSession.GenerateContentAsync("Welcome to the Gemini 1.5 Flash chat!");
            Debug.Log("First response: " + firstResponse.Text());

            // Continue the conversation
            var secondResponse = await chatSession.GenerateContentAsync("How can you help me with my AI development?");
            Debug.Log("Second response: " + secondResponse.Text());
        }
        async void eStart()
    {

        if(SafetySettings != null)
        {

            if(SafetySettings.Count == 0)
            {
                SafetySettings = null;
            }
        }

            Client = new MultiModalLiveClient(
            platformAdapter: new GoogleAIPlatformAdapter(apiKey),
            modelName: ModelName,
            config: new GenerationConfig { ResponseModalities = ResponseModalities },
            safetySettings: null,
            systemInstruction: SystemInstruction
        );

        Client.Connected += Connected  ;
        Client.ErrorOccurred += ErrorOccurred  ;
        Client.Disconnected += Disconnected  ;
        Client.TextChunkReceived += TextChunkReceived;
        Client.AudioChunkReceived += AudioChunkReceived ;

            await Client.ConnectAsync();
            try
            {
                await Client.ConnectAsync();
                //  Wait a few frames (or delay) before sending
                await Task.Delay(1000); // Give the socket time to settle
                await Client.SentTextAsync("Hello Gemini!");
            }
            catch (Exception ex)
            {
                Debug.LogError("Start error: " + ex);
            }
        }

        private void Disconnected(object sender, EventArgs e)
        {
            Debug.Log("Disconnected!");


        }

        private void ErrorOccurred(object sender, ErrorEventArgs e)
        {
            Debug.LogError($"Error: {e.GetException()}");
        }

        public void AudioChunkReceived(object sender, AudioBufferReceivedEventArgs e)
    {
      Debug.Log($"Audio received: {e.Buffer.Length} bytes");
    }

    public async void Connected(object sender, EventArgs e)
    {
            Debug.Log("Connected!");
            //await Client.SentTextAsync("Hello, Gemini! What's the weather like?");

        }

        public void TextChunkReceived(object sender, TextChunkReceivedArgs e)
    {
        Debug.Log($"Text chunk: {e.Text}");
    }

        public async void SendText(string text)
        {
            if(debounced == false){
                return;
            }
            await Client.SentTextAsync(text);

        }

        // Update is called once per frame
        void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
                SendText("Hello, how are you?");
            }
        }
}
}
