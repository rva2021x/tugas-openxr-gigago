using AssetVariable;
using Gameplay;
using UnityEngine;
using UnityEngine.AI;

public class MonsterBehaviour : Entity
{
    [SerializeField] private MonsterAnimator monsterAnimator;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform playerTransform;

    [SerializeField] private bool rangeAttack;
    [SerializeField] private float rangeSpeedAttack;
    [SerializeField] private GameObject projectileAttack;
    [SerializeField] private Transform attackPosition;
    private Vector3 target;
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

    [SerializeField] private AudioSource attackSound;

    //Attacking
    [SerializeField] private float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;


    private void Awake()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (waypoint[0] == null)
        {
            Transform wayTransform = GameObject.FindGameObjectWithTag("Waypoint").transform;
            waypoint = new Transform[wayTransform.childCount];
            for (int i = 0; i < wayTransform.childCount; i++)
            {
                waypoint[i] = wayTransform.GetChild(i);
            }
        }
        monsterAnimator = GetComponent<MonsterAnimator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private bool isPause;
    private float tempSpeed;
    private float animatorSpeed;
    public void Pause()
    {
        isPause = true;
        tempSpeed = agent.speed;
        agent.speed = 0;
        animatorSpeed = monsterAnimator.CharacterAnimator.speed;
        monsterAnimator.CharacterAnimator.speed = 0;
    }

    public void Resume()
    {
        isPause = false;
        agent.speed = tempSpeed;
        monsterAnimator.CharacterAnimator.speed = animatorSpeed;
    }

    private void Start()
    {
        GameOver go = GameObject.Find("Game Over")?.GetComponent<GameOver>();
        if (go != null)
        {
            go.gameOverEvent.AddListener(() =>
            {
                Pause();
            });
        }
    }

    private void Update()
    {
        if (isPause)
        {
            return;
        }
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

        if (Vector3.Distance(transform.position, target) < 1)
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
    private void AttackPlayer()
    {
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
                if (playerTransform.TryGetComponent(out PlayerBehaviour player))
                {
                    player.health -= 10;
                }
                if (attackSound)
                {
                    attackSound.Play();
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

    public void TakeDamage(float damage)
    {
        agent.speed = 0f;
        health -= damage;
        if (health <= 0)
        {
            if (vfxExplosion)
            {
                Instantiate(vfxExplosion, transform.position, Quaternion.identity);
            }
            GameObject gameOver = GameObject.FindWithTag("GameOver");
            gameOver.GetComponent<GameOver>().enemyKilled += 1;
            Destroy(this.gameObject);
        }
    }

    private void MonsterDie()
    {
        GameObject gameOver = GameObject.FindWithTag("GameOver");
        gameOver.GetComponent<GameOver>().enemyKilled += 1;
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
        if (waypointIndex == waypoint.Length)
        {
            waypointIndex = 0;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Magic")
        {
            TakeDamage(10);
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
