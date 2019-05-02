using UnityEngine;
using StateStuff;

public class WatchState : State<AI>
{
    
    private static WatchState _instance;
    private WatchState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }
    public static WatchState Instance
    {
        get
        {
            if (_instance == null)
            {
                new WatchState();
            }
            return _instance;
        }
    }


    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering state Watch");
        
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting state Watch");     

    }

    //Update
    public override void UpdateState(AI _owner)
    {
        _owner.navMeshAgent.isStopped = true;
        _owner.Timer();
        _owner.Stare();
        if (_owner.shouldStare == false)
        {
            _owner.aiState = AiStates.Idle;
            _owner.stateMachine.ChangeState(IdleState.Instance);
        }
    }
}
