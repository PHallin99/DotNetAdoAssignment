namespace StateMachine;

public interface IState {
    void EnterState();
    void RunState();
    void ExitState();
    string GetName();
}