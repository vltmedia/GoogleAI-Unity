using GenerativeAI.Types;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace GenerativeAI.Unity
{

    public class GTTS : GPlugin<GTTSApi>
    {
        public override string ID => "google.tts.generate";
        AudioClip audioClip = null;


        public static GTTS Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }
        public string voiceName = "Despina";
        public string ModelName = "gemini-2.5-flash-preview-tts";

        [TextArea(3, 10)] public string prompt = "Speak in an american Italian accent , funny and witty. ";

        private static readonly HttpClient httpClient = new HttpClient();

        public async override Task<object> Run(object data)
        {
            object response = null;
            if (data is string text)
            {
                response = await SendText(text);
                onResponseReceived.Invoke(response);
            }
            else
            {
                Debug.LogError("Data is not a string");
            }
            return response;
        }

        public async Task<AudioClip> SendText(string text)
        {
            if (debounced == false)
            {
                return null;
            }
            var response = await api.SendText(text);
            if(response != null)
            {
                try
                {
                    audioClip = LoadPCMFromBase64(response);
                    return audioClip;
                }
                catch
                {
                                        Debug.LogError("Error converting response to AudioClip");
                    return null;
                }
            }
            return null;
        }
        //private AudioClip ConvertPCMToAudioClip(byte[] pcmData, int sampleRate, int channels, string name)
        //{
        //    int sampleCount = pcmData.Length / 2; // 2 bytes per sample (16-bit)
        //    float[] samples = new float[sampleCount];

        //    for (int i = 0; i < sampleCount; i++)
        //    {
        //        short sample = BitConverter.ToInt16(pcmData, i * 2);
        //        samples[i] = sample / 32768f; // normalize to [-1, 1]
        //    }

        //    var clip = AudioClip.Create(name, sampleCount / channels, channels, sampleRate, false);
        //    clip.SetData(samples, 0);
        //    return clip;
        //}

        public AudioClip LoadPCMFromBase64(byte[] pcmData, int sampleRate = 24000, int channels = 1)
        {
            int sampleCount = pcmData.Length / 2; // 16-bit audio = 2 bytes per sample
            float[] audioData = new float[sampleCount];

            for (int i = 0; i < sampleCount; i++)
            {
                short value = BitConverter.ToInt16(pcmData, i * 2);
                audioData[i] = value / 32768f;
            }

            AudioClip clip = AudioClip.Create("GeminiTTS", sampleCount / channels, channels, sampleRate, false);
            clip.SetData(audioData, 0);
            return clip;
        }


        // Start is called once before the first execution of Update after the MonoBehaviour is created

        void Start()
    {
            Register();
            if(api == null)
            {
                api = new GTTSApi();
            }
            api.SetModel(ModelName);
            if(api.payload == null)
            {
                api.payload = new GPayload();
            }
            api.payload.generationConfig.responseModalities = new List<string> { "AUDIO" };
            api.payload.generationConfig.speechConfig.voiceConfig.prebuiltVoiceConfig.voiceName = voiceName;
        }

        // Update is called once per frame
        void Update()
    {
        
    }
}
}
