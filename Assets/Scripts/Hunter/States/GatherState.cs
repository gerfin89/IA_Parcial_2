using UnityEngine;
using static PatrolState;

public class GatherState : State
{
    public FSM_Hunter _hunter;
    public GatherData _data;
    private float _timer;


    public GatherState(FSM_Hunter hunter, GatherData data, StateMachine stateMachine) : base(stateMachine)
    {
        _hunter = hunter;
        _data = data;
    }

    public override void Enter()
    {
        _timer = 0;
        _data.target = _data.target = _hunter.Perception.FallendBoidInVision(_data.visionRadius); 
        _hunter.ChangeColor(Color.cyan);
    }

    public override void Exit() 
    {
        _timer = 0;
        _data.target = null;
    }

    public override void Update()
    {
        if (!HasValidTarget())
        {
            ReturnToPatrol();
            return;
        }

        float distance = Vector3.Distance(_hunter.transform.position,_data.target.position);

        if (distance > _data.gatherDistance)
        {
            MoveToTarget();
            return;
        }

        GatherBoid();
    }

    private bool HasValidTarget()
    {
        if (_data.target == null)
            return false;

        BoidHealth health = _data.target.GetComponent<BoidHealth>();

        return health != null && health.IsDown;
    }

    private void MoveToTarget()
    {
        Vector3 direction = _data.target.position - _hunter.transform.position;
        direction.y = 0f;

        _hunter.transform.position += direction.normalized * _hunter.MaxSpeed * Time.deltaTime;
    }

    private void GatherBoid()
    {
        _timer += Time.deltaTime;

        if (_timer < _data.gatherTime)
            return;

        BoidSteering boid = _data.target.GetComponent<BoidSteering>();

        if (boid != null)
        {
            boid.SetBoidSpawner(_data.boidSpawner);
            boid.StartRespawn();
        }

        ReturnToPatrol();
    }

    private void ReturnToPatrol()
    {
        _data.target = null;
        stateMachine.ChangeState(FarmerState.Patrol);
    }



    [System.Serializable]
    public class GatherData
    {
        public Transform target;
        public float visionRadius;
        public float gatherDistance;
        public float gatherTime;
        public BoidSpawner boidSpawner;


    }
}

    