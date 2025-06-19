using UnityEngine;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
namespace GenerativeAI.Unity
{

    public class GTTS : MonoBehaviour
{
        public string apiKey
        {
            get
            {
                return GGenerativeAIManager.APIKey;
            }
        }
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

        public async void Speak()
        {
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash-preview-tts:generateContent?key={apiKey}";

            var payload = new
            {
                contents = new[]
                {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            },
                generationConfig = new
                {
                    responseModalities = new[] { "AUDIO" },
                    speechConfig = new
                    {
                        voiceConfig = new
                        {
                            prebuiltVoiceConfig = new
                            {
                                voiceName = voiceName
                                
                            }
                        }
                    }
                },
                model = ModelName
            };

            var json = JsonUtility.ToJson(payload).Replace("\"prebuiltVoiceConfig\"", "\"prebuiltVoiceConfig\""); // workaround for camelCase issue

            try
            {
                var response = await httpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
                var responseText = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Debug.LogError($"Gemini TTS failed: {response.StatusCode} - {responseText}");
                    return;
                }

                // Extract base64 audio
                var base64 = JObject.Parse(responseText)["candidates"]?[0]?["content"]?["parts"]?[0]?["inlineData"]?["data"]?.ToString();

                if (string.IsNullOrEmpty(base64))
                {
                    Debug.LogError("No audio data found in response.");
                    return;
                }

                var pcmBytes = Convert.FromBase64String(base64);
                var clip = ConvertPCMToAudioClip(pcmBytes, 24000, 1, "GeminiTTS");
                AudioSource.PlayClipAtPoint(clip, Vector3.zero);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error speaking from Gemini: " + ex);
            }
        }

        private AudioClip ConvertPCMToAudioClip(byte[] pcmData, int sampleRate, int channels, string name)
        {
            int sampleCount = pcmData.Length / 2; // 2 bytes per sample (16-bit)
            float[] samples = new float[sampleCount];

            for (int i = 0; i < sampleCount; i++)
            {
                short sample = BitConverter.ToInt16(pcmData, i * 2);
                samples[i] = sample / 32768f; // normalize to [-1, 1]
            }

            var clip = AudioClip.Create(name, sampleCount / channels, channels, sampleRate, false);
            clip.SetData(samples, 0);
            return clip;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
}
