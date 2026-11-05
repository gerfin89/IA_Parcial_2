using UnityEngine;

public class HunterDeadState : State
{
    private readonly FSM_Hunter hunter;
    private const float RespawnDelay = 3f;
    private float respawnTime;

    public HunterDeadState(FSM_Hunter hunter, StateMachine stateMachine): base(stateMachine)
    {
        this.hunter = hunter;
    }

    public override void Enter()
    {
        base.Enter();

        respawnTime = Time.time + RespawnDelay;
        hunter.ChangeColor(Color.red);
        SetHunterVisible(false);
              
    }

    public override void Update()
    {
        if (Time.time < respawnTime)
            return;

        RespawnHunter();
    }

    private void RespawnHunter()
    {
        HunterHealth health = hunter.GetComponent<HunterHealth>();

        if (health == null)
            return;

        health.Respawn();
        SetHunterVisible(true);

        stateMachine.ChangeState(FarmerState.Patrol);
    }

    private void SetHunterVisible(bool visible)
    {
        foreach (Renderer renderer in hunter.GetComponentsInChildren<Renderer>())
            renderer.enabled = visible;
    }
}