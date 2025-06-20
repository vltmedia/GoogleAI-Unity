using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace GenerativeAI
{

    [System.Serializable]
    public class Candidate
    {
        public Content content;
        public string finishReason;
        public double avgLogprobs;
    }

    [System.Serializable]
    public class CandidatesTokensDetail
    {
        public string modality;
        public int tokenCount;
    }

    [System.Serializable]
    public class Content
    {
        public List<Part> parts;
        public string role;
    }

    [System.Serializable]
    public class Part
    {
        public string text;
        public InlineData inlineData;
    }

    [System.Serializable]
    public class InlineData
    {
        public string mimeType;
        public string data;
    }

    [System.Serializable]
    public class PromptTokensDetail
    {
        public string modality;
        public int tokenCount;
    }
    [System.Serializable]
    public class SetupPayload
    {
        public string model;
        public GenerationConfiguration generationConfig;
        public SetupPayload()
        {
            generationConfig = new GenerationConfiguration();
        }
    }
    
    [System.Serializable]
    public class Blob
    {
        /// <summary>
        /// The IANA standard MIME type of the source data. Examples:
        /// - image/png
        /// - image/jpeg
        /// If an unsupported MIME type is provided, an error will be returned.
        /// For a complete list of supported types, see
        /// <see href="https://ai.google.dev/gemini-api/docs/prompting_with_media#supported_file_formats">Supported file formats</see>.
        /// </summary>
        [JsonPropertyName("mimeType")]
        public string? MimeType { get; set; }

        /// <summary>
        /// Raw bytes for media formats.
        /// A base64-encoded string.
        /// </summary>
        [JsonPropertyName("data")]
        public string? Data { get; set; }
    }
    [System.Serializable]
    public class SetupPayloadContainer
    {
        public SetupPayload setup;
        public SetupPayloadContainer()
        {
            setup = new SetupPayload();
        }
    }

    [System.Serializable]
    public class PrebuiltVoiceConfig
    {
        public string voiceName;
    }

    [System.Serializable]
    public class GenerationConfiguration
    {
        public List<string> responseModalities;
        public SpeechConfig speechConfig;
        public GenerationConfiguration()
        {
            responseModalities = new List<string>();
            speechConfig = new SpeechConfig();
        }
    }

    [System.Serializable]
    public class SpeechConfig
    {
        public VoiceConfig voiceConfig;
        public SpeechConfig()
        {
            voiceConfig = new VoiceConfig();
        }
    }

    [System.Serializable]
    public class VoiceConfig
    {
        public PrebuiltVoiceConfig prebuiltVoiceConfig;
        public VoiceConfig()
        {
            prebuiltVoiceConfig = new PrebuiltVoiceConfig();
        }
    }

    [System.Serializable]
    public class GResponse
    {
        public List<Candidate> candidates;
        public UsageMetadata usageMetadata;
        public string modelVersion;
        public string responseId;
    }

    [System.Serializable]
    public class GPayloadBase<T>
    {
        public List<T> contents;
        public string model;
        public GenerationConfiguration generationConfig;
        public GPayloadBase()
        {
            contents = new List<T>();
            generationConfig = new GenerationConfiguration();
        }
        public GPayloadBase(List<T> contents)
        {
            this.contents = contents;
            generationConfig = new GenerationConfiguration();
        }
        public void SetModel(string model)
        {
            this.model = model;
        }
        public void SetGenerationConfig(GenerationConfiguration generationConfig)
        {
            this.generationConfig = generationConfig;
        }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    [System.Serializable]
    public class GPayload: GPayloadBase<Content>
    {
        public GPayload(): base()
        {
        }
        public GPayload(List<Content> contents) : base()
        {
        }


    }

    [System.Serializable]
    public class UsageMetadata
    {
        public int promptTokenCount;
        public int candidatesTokenCount;
        public int totalTokenCount;
        public List<PromptTokensDetail> promptTokensDetails;
        public List<CandidatesTokensDetail> candidatesTokensDetails;
    }

    [System.Serializable]
    public class BidiGenerateContentRealtimeInput
    {
        /// <summary>
        /// Inlined bytes data for media input.
        /// </summary>
        [JsonPropertyName("mediaChunks")]
        public Blob[]? MediaChunks { get; set; }
    }
    [System.Serializable]
    public class BidiGenerateContentClientContent
    {
        /// <summary>
        /// The content appended to the current conversation with the model.
        /// For single-turn queries, this is a single instance. For multi-turn queries, this is a repeated field that contains conversation history and the latest request.
        /// </summary>
        [JsonPropertyName("turns")]
        public Content[]? Turns { get; set; }

        /// <summary>
        /// If true, indicates that the server content generation should start with the currently accumulated prompt. Otherwise, the server awaits additional messages before starting generation.
        /// </summary>
        [JsonPropertyName("turnComplete")]
        public bool? TurnComplete { get; set; }
    }
}
