// 상태를 관리하는 머신
public class FSM
{
    private IMonsterState currentState; // 현재 state
    public IMonsterState CurrentState { get { return currentState; } }
    // final 버전 변경
    public IdleState IdleState { get; private set; }
    public ChaseState ChaseState { get; private set; }
    public AttackState AttackState { get; private set; }
    public HitState HitState { get; private set; }
    public DeadState DeadState { get; private set; }
    public DefenseState DefenseState { get; private set; }
    public FSM(Monster monster)
    {
        // final 버전 변경
        // 상태 객체 초기화
        IdleState = new IdleState(monster);
        ChaseState = new ChaseState(monster);
        AttackState = new AttackState(monster);
        HitState = new HitState(monster);
        DeadState = new DeadState(monster);
        DefenseState = new DefenseState(monster);

        currentState = IdleState;
        //currentState = new IdleState(monster); // 이부분 문제 생기면 추상클래스로 만들어봐야 할듯
    }

    // 상태 변경
    public void SetState(IMonsterState state)
    {
        if (currentState == state) return;
        
        if(currentState != null)
            currentState.ExitState();
        currentState = state; // 현재 state 변경
        currentState.EnterState();
    }
    // called by Monster.cs Update()
    // 상태 실행
    public void ExecuteState()
    {
        if(currentState != null)
            currentState.ExecuteState();
    } 

}