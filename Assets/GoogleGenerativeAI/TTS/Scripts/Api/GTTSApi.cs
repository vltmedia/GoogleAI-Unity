
using GenerativeAI.Unity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace GenerativeAI
{
    public class GTTSApi: GApi
    {



        public virtual async Task<byte[]> SendText(string text)
        {
            Content data = new Content
            {
                parts = new List<Part>
                {
                    new Part
                    {
                        text = text
                    }
                },
                role = "user"
            };

            if(contents.Count == 0)
            {
                contents.Add(data);
            }
            else
            {
                contents[0] = data;
            }
            Uri BaseAddress = new Uri("https://generativelanguage.googleapis.com/v1beta/");
            var relativeUrl = $"models/{Model}:generateContent?key={ApiKey}";
            var response = await SendRequest(BaseAddress, relativeUrl, payloadJson);
            if (response != "")
            {
                GResponse gResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<GResponse>(response);
                if (gResponse != null && gResponse.candidates != null && gResponse.candidates.Count > 0)
                {
                    var base64String = gResponse.candidates[0].content.parts[0].inlineData.data;
                    if(string.IsNullOrEmpty(base64String))
                    {
                        return null;
                    }
                    return Convert.FromBase64String(base64String);
                }
                else
                {
                    response = null;
                }
            }
            return null;
        }

    }
}
