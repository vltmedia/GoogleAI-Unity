using UnityEngine;
using GenerativeAI;
namespace GenerativeAI.Unity
{
    public class GGenerativeAIManager : MonoBehaviour
{
        public GAPIKey GAPIKey;
        public string ApiKey
        {
            get
            {
                return GAPIKey == null ? "" : GAPIKey.ApiKey;
            }
        }
        
        public static string APIKey
        {
            get
            {
                return Instance == null ? "" : Instance.ApiKey;
            }
        }


        public static GGenerativeAIManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                GApi.SetStaticApiKey(ApiKey);
            }
            else
            {
                Destroy(this);
            }
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
