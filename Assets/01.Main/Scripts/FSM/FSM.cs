using UnityEngine;

public class FSM<T> : MonoBehaviour
{
    private T owner;
    private IFSMState<T> prevState = null;
    private IFSMState<T> currentState = null;

    public IFSMState<T> CurrentState { get { return currentState; } }
    public IFSMState<T> PrevState { get { return prevState; } }

    protected void InitState(T owner, IFSMState<T> initialState)
    {
        this.owner = owner;
        ChangeState(initialState);
    }

    protected void FSMUpdate()
    {
        if (currentState != null) currentState.OnUpdate(owner);
    }

    public void ChangeState(IFSMState<T> newState)
    {
        prevState = currentState;

        if (prevState != null) prevState.OnExit(owner);

        currentState = newState;

        if (currentState != null) currentState.OnEnter(owner);
    }

    public void ToPrevState()
    {
        if (prevState != null) ChangeState(prevState);
    }

    public override string ToString() { return currentState.ToString(); }
}
