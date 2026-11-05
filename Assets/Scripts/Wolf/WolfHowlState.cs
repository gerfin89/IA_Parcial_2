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
        AlertNearbyWolves();
    }

    public override void Update()
    {
        if (howlFinished)
            return;

        timer += Time.deltaTime;

        if (timer >= HowlDuration)
        {
            howlFinished = true;
            
            stateMachine.ChangeState(WolfState.Chase);
            return;
        }
    }

    private void AlertNearbyWolves()
    {
        WolfController[] wolves =
            UnityEngine.Object.FindObjectsByType<WolfController>(
                FindObjectsSortMode.None
            );

        foreach (WolfController otherWolf in wolves)
        {
            if (otherWolf == wolf)
                continue;

            otherWolf.ReceiveHowlAlert();
        }
    }
}