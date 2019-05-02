using StateStuff;

public class IdleState : State<AI>
{
    private static IdleState _instance;

    private IdleState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static IdleState Instance
    {
        get
        {
            if (_instance == null)
            {
                new IdleState();
            }
            return _instance;
        }

    }
    public override void EnterState(AI _owner)
    {
        
        _owner.navMeshAgent.isStopped = false;
    }
    public override void ExitState(AI _owner)
    {
        
        
    }
    public override void UpdateState(AI _owner)
    {
        _owner.Patrol();
        
        if (_owner.shouldStare == true && _owner.Search() == false)
        {
           
            _owner.aiState = AiStates.Watch;
            _owner.stateMachine.ChangeState(WatchState.Instance);
            
        }
        if (_owner.shouldStare == false)
        {
            _owner.Timer();            
        }
    }
}