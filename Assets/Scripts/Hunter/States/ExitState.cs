using UnityEngine;

public class ExitState : State
{
    private readonly FSM_Hunter hunter;

    public ExitState(FSM_Hunter hunter, StateMachine stateMachine): base(stateMachine)
    {
        this.hunter = hunter;
    }

    private bool hasFinished;
    public override void Enter()
    {
        hasFinished = false;
        base.Enter();

        hunter.ChangeColor(Color.green);

        hunter.Navigation.CalculatePathTo( hunter.ExitNode,true);
    }

    public override void Update()
    {
        if (hasFinished)
            return;

        hunter.Navigation.FollowPath(hunter.MaxSpeed);

        if (hunter.Navigation.PathFollower.HasReachedDestination)
        {
            hasFinished = true;            
            hunter.RestartCycle();
            return;
        }
    }
}