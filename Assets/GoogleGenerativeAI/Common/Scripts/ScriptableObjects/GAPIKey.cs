using UnityEngine;

namespace GenerativeAI.Unity {

    [CreateAssetMenu(fileName = "GAPIKey", menuName = "Google/GenerativeAI/GAPIKey")]
    public class GAPIKey : ScriptableObject
{
        public string ApiKey;
}
}
