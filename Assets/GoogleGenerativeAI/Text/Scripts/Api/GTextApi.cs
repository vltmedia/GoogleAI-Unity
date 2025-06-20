
using GenerativeAI.Unity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
namespace GenerativeAI
{
    [System.Serializable]
    public class GTextApi: GApi
    {
        public virtual async Task<string> SendText(string text)
        {
            contents.Add(new Content
            {
                parts = new List<Part>
                {
                    new Part
                    {
                        text = text
                    }
                },
                role = "user"
            });
            Uri BaseAddress = new Uri("https://generativelanguage.googleapis.com/v1beta/");
            var relativeUrl = $"models/{Model}:generateContent?key={ApiKey}";
            var response = await  SendRequest(BaseAddress, relativeUrl, payloadJson);
            if(response != "")
            {
                GResponse gResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<GResponse>(response);
                if (gResponse != null && gResponse.candidates != null && gResponse.candidates.Count > 0)
                {
                    response = gResponse.candidates[0].content.parts[0].text;
                    contents.Add(gResponse.candidates[0].content);
                }
                else
                {
                    response = "No response received.";
                }
            }
            return response;
        }
       
    }
}
