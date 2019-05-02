using UnityEngine;
using StateStuff;

public class AttackState : State<AI>
{
    private static AttackState _instance;

    private AttackState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;        
    }
    public static AttackState Instance
    {
        get
        {
            if (_instance == null)
            {
                new AttackState();
            }
            return _instance;
        }
    }

    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering first State.");
        //_owner.takeOutWeapon = true;
    }
    public override void ExitState(AI _owner)
    {
        //_owner.takeOutWeapon = false;
        Debug.Log("Exiting first State.");
    }


    public override void UpdateState(AI _owner)
    {


    }
}
