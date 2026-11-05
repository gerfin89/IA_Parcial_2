using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    private readonly FSM_Hunter hunter;
    private readonly PatrolData data;
    private readonly float visionRadius;
    private readonly float gatherVisionRadius;

    private readonly List<GameObject> spawnedBait = new();

    private GraphNode destinationNode;
    private float baitTimer;
    private float searchTimer;

    public PatrolState(
        FSM_Hunter hunter,
        PatrolData data,
        float visionRadius,
        float gatherVisionRadius,
        StateMachine stateMachine) : base(stateMachine)
    {
        this.hunter = hunter;
        this.data = data;
        this.visionRadius = visionRadius;
        this.gatherVisionRadius = gatherVisionRadius;
    }

    public override void Enter()
    {
        base.Enter();

        hunter.ChangeColor(Color.blue);

        baitTimer = data.baitSpawnTime;
        searchTimer = 10f;
        destinationNode = null;
    }

    public override void Exit()
    {
        base.Exit();
        destinationNode = null;
    }

    public override void Update()
    {
        if (CheckBoids())
            return;

        SpawnBait();

        if (CheckSearch())
            return;

        PatrolLoop();
    }

    private bool CheckBoids()
    {
        Transform fallenBoid =
            hunter.Perception.FallendBoidInVision(gatherVisionRadius);

        if (fallenBoid != null)
        {
            stateMachine.ChangeState(FarmerState.Gather);
            return true;
        }

        Transform boid =
            hunter.Perception.BoidInVision(visionRadius);

        if (boid != null && hunter.IsTbaReady)
        {
            stateMachine.ChangeState(FarmerState.Attack);
            return true;
        }

        return false;
    }

    private void SpawnBait()
    {
        baitTimer -= Time.deltaTime;

        if (baitTimer > 0f)
            return;

        baitTimer = data.baitSpawnTime;

        spawnedBait.RemoveAll(bait => bait == null);

        if (spawnedBait.Count >= data.maxBaits || data.baitPrefab == null)
            return;

        GameObject bait = Object.Instantiate(data.baitPrefab,hunter.transform.position, Quaternion.identity);

        spawnedBait.Add(bait);
    }

    private void PatrolLoop()
    {
        if (destinationNode == null || hunter.Navigation.PathFollower.HasReachedDestination)
        {
            SelectDestination();
        }

        if (destinationNode == null)
            return;

        hunter.Navigation.FollowPath(hunter.MaxSpeed);
    }

    private void SelectDestination()
    {
        destinationNode = null;

        var nodes = hunter.Navigation.GraphNodes;

        if (nodes == null || nodes.Count == 0)
            return;

       
        for (int i = 0; i < nodes.Count; i++)
        {
            GraphNode candidate =
                nodes[Random.Range(0, nodes.Count)];

            if (candidate == null || candidate.IsSafeZone)
                continue;

            if (Vector3.Distance(
                hunter.transform.position,
                candidate.Position) < 2f)
                continue;

            hunter.Navigation.CalculatePathTo(candidate, true);

            destinationNode = candidate;
            return;
        }
    }

    private bool CheckSearch()
    {
        searchTimer -= Time.deltaTime;

        if (searchTimer > 0f)
            return false;

        stateMachine.ChangeState(FarmerState.Search);
        return true;
    }

    [System.Serializable]
    public class PatrolData
    {
        [Header("Bait")]
        public GameObject baitPrefab;
        public float baitSpawnTime = 5f;
        public int maxBaits = 5;
    }
}