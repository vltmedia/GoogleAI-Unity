using UnityEngine;
using System.Collections.Generic;
using GenerativeAI;
using System.Threading.Tasks;
namespace GenerativeAI.Unity
{
    public class GPluginManager : MonoBehaviour
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

        public object context;
        public Dictionary<string, IGPlugin> plugins = new();

    public static GPluginManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
            GApi.SetStaticApiKey(ApiKey);

        }

        public void Register(IGPlugin plugin)
    {

            if (plugins == null)
            {
                plugins = new Dictionary<string, IGPlugin>();
            }
            if (!plugins.ContainsKey(plugin.ID))
        {
            plugin.Init(context);
            plugins.Add(plugin.ID,plugin);
        }
    }

    private void Update()
    {
        float delta = Time.deltaTime;
        foreach (var plugin in plugins.Values)
        {
            plugin.RunUpdate(delta);
        }
    }

    private void OnDestroy()
    {
        foreach (var plugin in plugins.Values)
        {
            plugin.Dispose();
        }
        plugins.Clear();
    }

        public async Task<object> RunFunction(string id, object data)
        {
            
            if (plugins.TryGetValue(id, out var plugin))
            {

                var response = await    plugin.Run(data);
                return response;
            }
            else
            {
                Debug.LogWarning($"Plugin with ID '{id}' not found.");
                return null;
            }
        }


    }
}
