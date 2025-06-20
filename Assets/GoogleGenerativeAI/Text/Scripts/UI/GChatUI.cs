using System;
using UnityEngine;
using UnityEngine.UI;
namespace GenerativeAI.Unity
{

    public class GChatUI : MonoBehaviour
{
        [Header("UI Elements")]
        public TMPro.TMP_InputField inputField;
        public Button sendButton;
        public TMPro.TMP_InputField chatView;
        public AudioSource audioSource;
        [Space(10)]

        [Header("Settings")]
        public string textPlugin = "google.text.generate";
        public string tttsPlugin = "google.tts.generate";
        public Color32 userColor;
        public string userColorHex { get { return ColorUtility.ToHtmlStringRGBA(userColor); } }
        public Color32 aIColor;
        public string aIColorHex { get { return ColorUtility.ToHtmlStringRGBA(aIColor); } }


        [Space(10)]

        [Header("Debug Info")]
        public AudioClip audioClip;
        public static GChatUI Instance;
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
            GText.Instance.onResponseReceived.AddListener(OnResponseReceived);
            sendButton.onClick.AddListener(OnSendButtonClicked);
            if(audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
        }
        public void AddUserText(string text)
        {
            if (chatView != null)
            {
                chatView.text += $"<color=#{userColorHex}>User: </color>{text}\n\n";
            }
        }

        public void AddResponseText(string text)
        {
            if (chatView != null)
            {
                chatView.text += $"<color=#{aIColorHex}>AI: </color>{text}\n\n";
            }
        }
        public async void SendInput()
        {
            if (!debounced)
            {
                Debug.LogWarning("Please wait before sending another message.");
                return;
            }
            debounceSend = DateTime.Now.AddSeconds(2); // Set debounce time to 2 seconds
            string inputText = inputField.text;
            inputField.text = string.Empty;

            if(!string.IsNullOrEmpty(inputText))
            {
                AddUserText(inputText);
              var response =   await GPluginManager.Instance.RunFunction(textPlugin, inputText);
                Debug.Log($"Response: {response}");
                //var response =   await GText.Instance.SendText(inputText);
                if (response is string responseText)
                {
               
                if (!string.IsNullOrEmpty(response.ToString()))
                {
                      var   ttsClip = await GPluginManager.Instance.RunFunction(tttsPlugin, responseText);
                        if(ttsClip != null)
                        {
                            if(audioClip is AudioClip) { 
                                audioClip = ttsClip as AudioClip;
                                if(audioSource != null)
                                {
                                    audioSource.Stop();
                                    audioSource.PlayOneShot(audioClip);
                                }
                            }
                            else
                            {
                               Debug.LogError("Audio clip is null");
                            }
                        }
                        AddResponseText(response.ToString());

                    }

                    else
                {
                    AddResponseText("No response received.");
                }
                }
                else
                {
                    AddResponseText("No response received.");
                }

            }
        }

        private void OnSendButtonClicked()
        {
            SendInput();
        }

        private void OnResponseReceived(object arg0)
        {
            
        }

        // Update is called once per frame
        void Update()
    {
            if(Input.GetKeyDown(KeyCode.Return) && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
            {
                SendInput();
            }

        }
}
}
