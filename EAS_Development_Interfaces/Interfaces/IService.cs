namespace EAS_Development_Interfaces
{
    public interface IService
    {
        bool IsRunning { get; }
        void Start();
        void Stop();
    }
}
