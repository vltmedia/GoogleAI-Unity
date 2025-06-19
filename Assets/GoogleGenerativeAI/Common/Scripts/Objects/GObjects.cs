using Newtonsoft.Json;
using System.Collections.Generic;
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
    public class PrebuiltVoiceConfig
    {
        public string voiceName;
    }

    [System.Serializable]
    public class GenerationConfiguration
    {
        public List<string> responseModalities;
        public SpeechConfig speechConfig;
    }

    [System.Serializable]
    public class SpeechConfig
    {
        public VoiceConfig voiceConfig;
    }

    [System.Serializable]
    public class VoiceConfig
    {
        public PrebuiltVoiceConfig prebuiltVoiceConfig;
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
        }
        public GPayloadBase(List<T> contents)
        {
            this.contents = contents;
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




}
