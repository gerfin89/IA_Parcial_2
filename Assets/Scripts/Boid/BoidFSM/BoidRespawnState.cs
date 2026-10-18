using UnityEngine;

public class BoidRespawnState : State
{
    private readonly BoidSteering _boid;
    private float _timer;
    

    public BoidRespawnState(StateMachine stateMachine, BoidSteering boid)
        : base(stateMachine)
    {
        _boid = boid;
    }

    public override void Enter()
    {
        _timer = _boid.RespawnDelay;
        _boid.Sleep();
        _boid.SetVisible(false);
    }

    public override void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer > 0f)
            return;

        _boid.Respawn();
        _boid.SetVisible(true);
        stateMachine.ChangeState(BoidStateType.Flocking);
    }
}