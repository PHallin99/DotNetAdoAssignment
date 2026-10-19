namespace StateMachine;

public abstract class Context {
    protected IState? State;

    public abstract void StartContext();

    public void SetState(IState newState) {
        if (State == null) {
            State = newState;
            return;
        }

        State.ExitState();
        State = newState;
        State.EnterState();
        State.RunState();
    }
}