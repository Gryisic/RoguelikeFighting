namespace Core.Interfaces
{
    public interface IServicesHandler
    {
        IInputService InputService { get; }

        T GetSubService<T>() where T: IService;
    }
}