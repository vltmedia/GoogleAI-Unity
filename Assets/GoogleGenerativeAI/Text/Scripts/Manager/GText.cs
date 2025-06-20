using UnityEngine;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine.Events;
namespace GenerativeAI.Unity
{

    public class GText : GPlugin<GTextApi>
{
        public override string ID => "google.text.generate";

     
        public static GText Instance;


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







        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
    {
            if (api == null)
            {
                api = new GTextApi();
            }
            Register();
            api.SetModel(Model);
            api.OnResponseReceived += OnResponseReceived;
            api.OnErrorReceived += OnErrorReceived;
            api.OnLog += (data)=>
            {
                Debug.Log(data);
            };
        }
        private void OnResponseReceived(string response)
        {
            onResponseReceived.Invoke(response);
        }
        private void OnErrorReceived(string error)
        {
            Debug.LogError("Error: " + error);

        }

        public async Task<string> SendText(string text)
        {
            if (debounced == false)
            {
                return "";
            }
            var response = await api.SendText(text);
            return response;
        }

        public async void SendHello()
        {
            var response = await SendText("Hello how are you today?");

        }

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

        // Update is called once per frame
        void Update()
    {
            

        }
}
}
