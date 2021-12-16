using AssetVariable;
using Gameplay;
using UnityEngine;
using UnityEngine.AI;

public class MonsterBehaviour : MonoBehaviour
{
    [SerializeField] private MonsterAnimator monsterAnimator;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform playerTransform;

    [SerializeField] private bool rangeAttack;
    [SerializeField] private float rangeSpeedAttack;
    [SerializeField] private GameObject projectileAttack;
    [SerializeField] private Transform attackPosition;
    private Vector3 target;
    [SerializeField] private float health;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private GameObject vfxExplosion;


    [SerializeField] private Transform[] waypoint;
    private int waypointIndex = 0;
    private float waypointDist;

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
        if (playerTransform == null) {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if(waypoint[0] == null) {
            Transform wayTransform = GameObject.FindGameObjectWithTag("Waypoint").transform;
            waypoint = new Transform[wayTransform.childCount];
            for(int i = 0; i < wayTransform.childCount; i++) {
                waypoint[i] = wayTransform.GetChild(i);
			}
		}
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
            
            Patrolling();

        }
        else if (playerInSightRange && !playerInAttackRange) 
            ChasePlayer();
        else if (playerInSightRange && playerInAttackRange) 
            AttackPlayer();
    }

    private void Idle()
    {
        monsterAnimator.CharacterStopWalk();
        agent.speed = 0f;
    }

    private void Patrolling()
    {
        agent.speed = walkSpeed;
        target = waypoint[waypointIndex].position;
        agent.SetDestination(target);

        if(Vector3.Distance(transform.position, target) < 1)
            IterateWaypointIndex();
        Debug.Log("Patrolling");
    }

    private void ChasePlayer()
    {
        agent.speed = runSpeed;

        agent.SetDestination(playerTransform.position);
        monsterAnimator.CharacterFoundEnemy();

        monsterAnimator.CharacterWalk();
        Debug.Log("Chase Player");

    }
    private void AttackPlayer() {
        agent.SetDestination(transform.position);
        transform.LookAt(playerTransform);
        Debug.Log("Attack Player");

        if (!alreadyAttacked)
        {
            monsterAnimator.CharacterAttackEnemy();

            //Attack Projectile
            if (rangeAttack)
            {
                Idle();
                Rigidbody rigidbody = Instantiate(projectileAttack, attackPosition.position, Quaternion.identity).GetComponent<Rigidbody>();
                rigidbody.AddForce(transform.forward * 32f, ForceMode.Impulse);
                Debug.Log("Attacking Player");
            }

            //Attack Melee
            else
            {
                if(playerTransform.TryGetComponent(out PlayerBehaviour player)) {
                    player.health -= 10;
				}
            }

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
        agent.speed = 0f;
        health -= damage;
        if(health < 0)
        {
            Invoke(nameof(TakeDamage), 0.5f);
        }
    }

    private void MonsterDie()
    {
        agent.speed = 0f;
        Destroy(gameObject);
    }

    private void DestroyProjectile(Rigidbody gb)
    {
        Destroy(gb);
    }

    private void IterateWaypointIndex()
    {
        waypointIndex++;
        if(waypointIndex == waypoint.Length)
        {
            waypointIndex = 0;
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Magic") {
            if (vfxExplosion) {
                Instantiate(vfxExplosion, transform.position, Quaternion.identity);
            }
            Destroy(this.gameObject);
        }
    }





    /*
    private void Wandering()
    {
        agent.speed = walkSpeed;

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
    */

}
