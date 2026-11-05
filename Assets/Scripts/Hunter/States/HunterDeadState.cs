using UnityEngine;

public class HunterDeadState : State
{
    private readonly FSM_Hunter hunter;
    private float respawnTimer;
    private const float RespawnDelay = 3f;

    public HunterDeadState(FSM_Hunter hunter,StateMachine stateMachine) : base(stateMachine)
    {
        this.hunter = hunter;
    }

    public override void Enter()
    {
        base.Enter();

        hunter.ChangeColor(Color.red);
        Debug.Log("Hunter entró en estado Dead");
        respawnTimer = RespawnDelay;
    }

    public override void Update()
    {
        respawnTimer -= Time.deltaTime;

        if (respawnTimer > 0f)
            return;

        RespawnHunter();
    }
    private void RespawnHunter()
    {
        HunterHealth health = hunter.GetComponent<HunterHealth>();

        if (health == null)
            return;

        health.Respawn();
        stateMachine.ChangeState(FarmerState.Patrol);
    }
}