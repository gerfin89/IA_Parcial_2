using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    public FSM_Hunter _hunter;
    public PatrolData _data;
    private int currentNode;
    private GraphNode currentDestinationNode;
    private bool isFollowingPath;
    private float _visionRadius;
    private float _gatherVisionRadius;
    private float baitTimer;
    private List<GameObject> spawnedBait = new List<GameObject>(); 
    public PatrolState(FSM_Hunter hunter, PatrolData data, float visionRadius, float gatherVisionRadius, StateMachine stateMachine) : base(stateMachine) 
    {
        _hunter = hunter;
        _data = data;
        _visionRadius = visionRadius;
        _gatherVisionRadius = gatherVisionRadius;
       
    }
    public override void Enter()
    {
        base.Enter();
        _hunter.ChangeColor(Color.blue);
        baitTimer = _data.baitSpawnTime;
                       
    }

    public override void Exit()
    {
        base.Exit();
        isFollowingPath = false;
        currentDestinationNode = null;
    }

    public override void Update()
    {

        Transform fallenBoid = _hunter.Perception.FallendBoidInVision(_gatherVisionRadius);


        if (fallenBoid != null)
        {
            stateMachine.ChangeState  (FarmerState.Gather);
            return;
        }

        Transform boidDetected = _hunter.Perception.BoidInVision(_visionRadius);

        if (boidDetected != null && _hunter.IsTbaReady)
        {
            stateMachine.ChangeState(FarmerState.Attack);
            return;
        }

        SpawnBait();

        PatrolLoop();

    }

    private void SpawnBait()
    {
        baitTimer -= Time.deltaTime;

        if (baitTimer > 0)
            return;

        spawnedBait.RemoveAll(x => x == null);

        if(spawnedBait.Count >= _data.maxBaits)
        {
            baitTimer = _data.baitSpawnTime;
            return; 
        }
              
        GameObject bait = Object.Instantiate(_data.baitPrefab,_hunter.transform.position,Quaternion.identity);
        spawnedBait.Add(bait);

        baitTimer = _data.baitSpawnTime;
    }


    private void PatrolLoop()
    {
        if (isFollowingPath)
        {
            _hunter.Navigation.FollowPath(_hunter.MaxSpeed);

            if (_hunter.Navigation.PathFollower.HasReachedDestination)
            {
                isFollowingPath = false;
                currentDestinationNode = null;
            }

            return;
        }
        var nextWaypoint = _data.wayPoints[currentNode];

       
        if (Vector3.Distance(nextWaypoint.position, _data.transform.position) <= _data.wayPointCheckDistance)
        {
            currentNode = currentNode + 1 < _data.wayPoints.Count? currentNode + 1 : 0;

            nextWaypoint = _data.wayPoints[currentNode];
        }

        if (!_hunter.Navigation.HasClearPathTo(nextWaypoint.position))
        {
            currentDestinationNode = _hunter.Navigation.GetNearestNode(nextWaypoint.position);

            if (currentDestinationNode == null)
                return;

            _hunter.Navigation.CalculatePathTo(currentDestinationNode, true);
            isFollowingPath = true;

           
            return;
        }

        var dir = nextWaypoint.position - _data.transform.position;

        _data.transform.position += dir.normalized * _hunter.MaxSpeed * Time.deltaTime;
    }

    [System.Serializable]
    public class PatrolData 
    {
        public List<Transform> wayPoints;
        public Transform transform;
        public float wayPointCheckDistance;

        [Header("Bait")]
        public GameObject baitPrefab;
        public float baitSpawnTime = 5f;
        public int maxBaits = 5;

        public Vector3 spawnAreaMin;
        public Vector3 spawnAreaMax;
    }



}
