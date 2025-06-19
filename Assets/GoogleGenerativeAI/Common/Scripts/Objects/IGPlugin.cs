using System.Threading.Tasks;

namespace GenerativeAI
{
    public interface IGPlugin
    {
        string ID { get; }
        void Init(object context);
        void Reset();
        void RunUpdate(float deltaTime);

        Task<object> Run(object data);
        void Dispose();
    }
}
