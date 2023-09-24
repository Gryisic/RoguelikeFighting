namespace Common.Units.Interfaces
{
    public interface IUnitStatesChanger
    {
        void ChangeState<T>() where T: IUnitState;
    }
}