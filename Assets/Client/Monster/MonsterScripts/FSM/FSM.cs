// 상태를 관리하는 머신
public class FSM
{
    public IMonsterState CurrentState { get; private set; } // 현재 상태 프로퍼티
    public FSM(Monster monster)
    {
        CurrentState = new IdleState(monster); // 이부분 문제 생기면 추상클래스로 만들어봐야 할듯
    }

    // 상태 변경
    public void SetState(IMonsterState state)
    {
        if (CurrentState == state) return;
        
        if(CurrentState != null)
            CurrentState.ExitState();
        CurrentState = state;
        CurrentState.EnterState();
    }
    // 상태 실행
    public void ExecuteState()
    {
        if(CurrentState != null)
            CurrentState.ExecuteState();
    } 

}