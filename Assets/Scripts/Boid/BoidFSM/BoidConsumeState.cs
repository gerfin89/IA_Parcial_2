using UnityEngine;

public class BoidConsumeState : State
{
    private readonly BoidSteering _boid;
    private Transform _bait;
    private float _consumeTimer;
    

    public BoidConsumeState(StateMachine stateMachine, BoidSteering boid)
        : base(stateMachine)
    {
        _boid = boid;
    }

    public override void Enter()
    {
        _boid.UpdateStateText("Consume");
        _consumeTimer = 0f;
        _boid.TryGetBait(out _bait);
        _boid.StopMovement();
    }

    public override void Update()
    {
        if (_boid.IsDown)
        {
            stateMachine.ChangeState(BoidStateType.Dead);
            return;
        }

        if (_boid.IsThreatDetected(false))
        {
            stateMachine.ChangeState(BoidStateType.Evade);
            return;
        }

        if (_bait == null)
        {
            stateMachine.ChangeState(BoidStateType.Flocking);
            return;
        }

        if (!_boid.IsAtBait(_bait))
        {
            stateMachine.ChangeState(BoidStateType.Arrive);
            return;
        }

        _consumeTimer += Time.deltaTime;

        if (_consumeTimer >= _boid.ConsumeInterval)
        {
            _boid.EatBait(_bait);
            _consumeTimer = 0f;

            if (_bait == null)
            {
                stateMachine.ChangeState(BoidStateType.Flocking);
                return;
            }
        }
    }
}