using UnityEngine;

public class WolfHowlState : State
{
    private readonly WolfController wolf;
    private float timer;
    private bool howlFinished;

    private const float HowlDuration = 1.5f;

    public WolfHowlState(WolfController wolf,StateMachine stateMachine) : base(stateMachine)
    {
        this.wolf = wolf;
    }

    public override void Enter()
    {
        base.Enter();

        timer = 0f;
        howlFinished = false;

        Debug.Log("¡Wolf está aullando!");
    }

    public override void Update()
    {
        if (howlFinished)
            return;

        timer += Time.deltaTime;

        if (timer >= HowlDuration)
        {
            howlFinished = true;
            Debug.Log("Wolf terminó de aullar");

            stateMachine.ChangeState(WolfState.Chase);
            return;
        }
    }
}