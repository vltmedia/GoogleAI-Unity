# Google AI - Unity

This is a Unity project that that shows the usage of Google AI services in Unity.

# Features

- [X] Text Generation
- [ ] Image Generation
- [ ] Text to Speech Generation
- [X] Speech to Text Generation
- [ ] Translation
- [ ] Live API
- [ ] Sound Generation

# Examples

Example scenes can be found in each AI Process folder. Each scene demonstrates how to use the respective AI service in Unity.

# Plugin System

This project uses a plugin system to allow for easy integration of different AI services. Each service is implemented as a separate plugin, which can be easily added or removed from the project.
The plugins automatically register themselves with the `GPluginManager` when they are started. This allows for easy management of the plugins and their APIs.

## Plugin Structure

Each plugin should have the following structure:

```csharp
    public abstract class GPlugin<T> : MonoBehaviour, IGPlugin
{
        public T api;
        public UnityEvent<object> onResponseReceived;

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
```

Example Plugin: [Text Generation Plugin](Assets/GoogleGenerativeAI/Text/Scripts/Manager/GText.cs)

## Running a Plugin

To run a plugin, you call the `RunFunction` method on the `GPluginManager` with the plugin ID and the data to be sent to the plugin. The plugin will then return an `object` which should be parsed by your script.

```csharp
var response =   await GPluginManager.Instance.RunFunction("google.text.generate", "Hi there!");
if(response != null && response is string)
{
	// Handle the response
	var text = response.ToString();
}
```
