using UnityEngine;

public class BoidEvadeState : State
{
    private readonly BoidSteering _boid;

    public BoidEvadeState(StateMachine stateMachine, BoidSteering Boid) : base(stateMachine)
    {
        _boid = Boid;
    }

    public override void Enter()
    {
        _boid.UpdateStateText("Evade");
    }
    public override void Update()
    {
        if (_boid.IsDown) { stateMachine.ChangeState(BoidStateType.Dead); return; }
        if (!_boid.IsThreatDetected(true)) { stateMachine.ChangeState(BoidStateType.Flocking); return; }

        _boid.Move(_boid.EvadeSteering());
    }
}