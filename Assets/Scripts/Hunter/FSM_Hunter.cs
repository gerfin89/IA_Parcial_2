using UnityEngine;
using UnityEngine.UI;

public enum FarmerState
{
    Idle,
    Patrol,
    Attack,
    Gather,
    Rest

}

public class FSM_Hunter : Agent
{
      
    [SerializeField] private PatrolState.PatrolData dataPatrol;
    [SerializeField] private AttackState.AttackData dataAttack;
    [SerializeField] private GatherState.GatherData dataGather;    
    [SerializeField] private float tba;

    [Header("Energy Hunter")]
    [SerializeField] private float maxEnergy = 100f;
    [SerializeField] private float energyConsumption = 10f;
    [SerializeField] private float energyRecovery = 20f;

    [Header("UI Stamina")]
    [SerializeField] private Slider staminaBar;

    [Header("Límit scenary")]
    [SerializeField] private GoatPen goatPen;


        
    private HunterNavigation navigation;
    public HunterNavigation Navigation => navigation;

    private HunterPerception perception;
    public HunterPerception Perception => perception;

    public GoatPen GoatPen => goatPen;
    private float _currentEnergy;
    public bool IsEnergyEmpty => _currentEnergy <= 0f;
    public bool IsEnergyFull => _currentEnergy >= maxEnergy;
    private StateMachine stateMachine;
    private Vector3 _previousPosition;

    private float _tbaTimer;
    public bool IsTbaReady =>_tbaTimer <= 0;
    public void ResetTba() => _tbaTimer = tba;

    [Header("Boids recolectados")]
    [SerializeField] private int requiredBoids = 3;

    private int collectedBoids = 0;

    public int CollectedBoids => collectedBoids;
    public bool HasCollectedAllBoids => collectedBoids >= requiredBoids;

    public void RegisterCollectedBoid()
    {
        if (HasCollectedAllBoids)
            return;

        collectedBoids++;

        Debug.Log($"Hunter recolectó {collectedBoids}/{requiredBoids} Boids");
    }
    public void ConsumeEnergy()
    {
        _currentEnergy -= energyConsumption * Time.deltaTime;
        _currentEnergy = Mathf.Max(0f, _currentEnergy);

        if (staminaBar != null)
        {
            staminaBar.value = _currentEnergy;
        }
    }
    public void RecoverEnergy()
    {
        _currentEnergy += energyRecovery * Time.deltaTime;
        _currentEnergy = Mathf.Min(_currentEnergy, maxEnergy);

        if (staminaBar != null)
        {
            staminaBar.value = _currentEnergy;
        }
    }
    public void ChangeColor(Color color)
    {
        Renderer renderer = GetComponentInChildren<Renderer>();

        if (renderer != null)
            renderer.material.color = color;
    }
          
    private void Awake()
    {
        _previousPosition = transform.position;
        _currentEnergy = maxEnergy;
        navigation = GetComponent<HunterNavigation>();
        perception = GetComponent<HunterPerception>();

        if (staminaBar != null)
        {
            staminaBar.minValue = 0f;
            staminaBar.maxValue = maxEnergy;
            staminaBar.value = _currentEnergy;
        }
              
        stateMachine = new StateMachine();

        IdleState idleState = new IdleState(stateMachine);
        PatrolState patrolState = new PatrolState(this, dataPatrol,dataAttack.visionRadius,dataGather.visionRadius, stateMachine);
        AttackState attackState = new AttackState(this, dataAttack, stateMachine);
        GatherState gatherState = new GatherState(this, dataGather,stateMachine);
        RestState restState = new RestState(this, stateMachine);

        stateMachine.RegisterState(FarmerState.Idle,idleState);
        stateMachine.RegisterState(FarmerState.Patrol, patrolState);
        stateMachine.RegisterState(FarmerState.Attack, attackState);
        stateMachine.RegisterState(FarmerState.Gather, gatherState);
        stateMachine.RegisterState(FarmerState.Rest, restState);
        
        stateMachine.ChangeState(FarmerState.Patrol);
               
    }

    private void Update()
    {
        
        if (_tbaTimer > 0f)
        {
            _tbaTimer -= Time.deltaTime;
        }

        stateMachine.Update();

        if (Time.deltaTime > 0f)
        {
            _velocity = (transform.position - _previousPosition) / Time.deltaTime;
        }

        _previousPosition = transform.position;
    } 
      

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, dataAttack.visionRadius);

        Gizmos.color = new Color (1f,0.5f,0.5f);
        Gizmos.DrawWireSphere(transform.position, dataAttack.rangeAttackRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, dataAttack.meleeAtackRadius);

    }
}
