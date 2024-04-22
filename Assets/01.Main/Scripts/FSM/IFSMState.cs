public interface IFSMState<T>
{
    void OnEnter(T e);
    void OnUpdate(T e);
    void OnExit(T e);
}
