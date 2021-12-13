using UnityEngine;
using UnityEngine.AI;

public class MonsterBehaviour : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private MonsterAnimator monsterAnimator;
    [SerializeField] private NavMeshAgent agent;
    [Range(0f, 1f)]
    [SerializeField] private int searching;
    [SerializeField] private Transform[] waypoint;
    [SerializeField] private int waypointIndex = 0;
    [SerializeField] private float waypointDist;

    [SerializeField] private string playerName;
    [SerializeField] private Transform playerTransform;
    private Vector3 target;

    [SerializeField] private LayerMask layerGround, layerPlayer;

    //Patrolling
    [SerializeField] private Vector3 walkPoint;
    bool walkPointSet;
    [SerializeField] private float walkPointRange;

    //Attacking
    [SerializeField] private float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;


    private void Awake()
    {
        playerTransform = GameObject.Find(playerName).transform;
        monsterAnimator = GetComponent<MonsterAnimator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, layerPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, layerPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            monsterAnimator.CharacterWalk();
            monsterAnimator.CharacterNotFoundEnemy();
            if (searching == 0)
            {
                Patrolling();
            }

            else
            {
                Wandering();
            }

        }
        else if (playerInSightRange && !playerInAttackRange) 
            ChasePlayer();
        else if (playerInSightRange && playerInAttackRange) 
            AttackPlayer();
    }

    private void Idle()
    {
        monsterAnimator.CharacterStopWalk();
    }

    private void Patrolling()
    {
        target = waypoint[waypointIndex].position;
        agent.SetDestination(target);

        if(Vector3.Distance(transform.position, target) < 1)
            IterateWaypointIndex();
    }

    private void Wandering()
    {
        if (!walkPointSet) 
            SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);
        

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint
        if(distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
    
        if(Physics.Raycast(walkPoint, -transform.up, 2f, layerGround))
        {
            walkPointSet = true;
        }
    }


    private void ChasePlayer()
    {
        agent.SetDestination(playerTransform.position);
        monsterAnimator.CharacterWalk();
        monsterAnimator.CharacterFoundEnemy();
    }
    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(playerTransform);

        if(!alreadyAttacked)
        {
            //Attack

            monsterAnimator.CharacterAttackEnemy();
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health < 0)
        {
            Invoke(nameof(TakeDamage), 0.5f);
        }
    }

    private void MonsterDie()
    {
        Destroy(gameObject);
    }

    private void IterateWaypointIndex()
    {
        waypointIndex++;
        if(waypointIndex == waypoint.Length)
        {
            waypointIndex = 0;
        }
    }
}
