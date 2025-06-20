using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
namespace GenerativeAI.Unity
{
    public abstract class GPlugin<T> : MonoBehaviour, IGPlugin
{
        public T api;
        public string Model = "gemini-2.0-flash";

        public UnityEvent<object> onResponseReceived;
        DateTime debounceSend = DateTime.MinValue;
        public bool debounced
        {
            get
            {
                if (debounceSend < DateTime.Now)
                {
                    return true;
                }
                return false;
            }
        }

        public string apiKey
        {
            get
            {
                return GPluginManager.APIKey;
            }
        }
        public abstract string ID { get; }

        public void Register()
        {

            GPluginManager.Instance.Register(this);
        }

        private void Start()
        {

            Register();
        }

        public virtual void Dispose()
        {
           
        }

        public virtual void Init(object context)
        {
            
        }

        public virtual void Reset()
        {
            
        }

        public virtual void RunUpdate(float deltaTime)
        {
           
        }

        public virtual async Task<object> Run(object data)
        {

            return null;
            
        }


    }
}
