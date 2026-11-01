using UnityEngine;

public class BoidRespawnState : State
{
    private readonly BoidSteering _boid;
    private float _timer;
    private Collider[] _colliders;

    public BoidRespawnState(StateMachine stateMachine, BoidSteering boid)
        : base(stateMachine)
    {
        _boid = boid;
        _colliders = _boid.GetComponentsInChildren<Collider>();
    }

    public override void Enter()
    {
        _boid.UpdateStateText("Respawn");
        _timer = _boid.RespawnDelay;
        _boid.Sleep();
        _boid.SetVisible(false);
        foreach (Collider col in _colliders)
        {
            if (col != null)
                col.enabled = false;
        }
    }

    public override void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer > 0f)
            return;

        _boid.Respawn();
        _boid.SetVisible(true);
        foreach (Collider col in _colliders)
        {
            if (col != null)
                col.enabled = true;
        }
        stateMachine.ChangeState(BoidStateType.Flocking);
    }
}