using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class coyoteMovement : MonoBehaviour
{
    Animator animator;
    public PlayerMovement obi;
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    //idle
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    //states
    public float sightRange;
    public bool playerInSightRange;
    float attackTime = 0f;
    float timer = 25f;
    bool notStop = true;
    float respawnTime = 0f;
    bool attacking = false;
    float wait;
    Vector3 first;
    Vector3 second;

    public GameObject coyoteMesh;
    public GameObject coyoteFaintMesh;
    int isIdleHash;


    private void Awake()
    {
        player = GameObject.Find("influence").transform;
        agent = GetComponent<NavMeshAgent>();
        wait = 0f;
        second=transform.position;
        animator = coyoteMesh.GetComponent<Animator>();
        isIdleHash = Animator.StringToHash("isIdle");

    }
    private void FixedUpdate()
    {
        bool isIdle = animator.GetBool(isIdleHash);
        if(wait>=0.1f)
        {
            first=transform.position;
            if(first.x==second.x && first.z==second.z)
            {
                animator.SetBool(isIdleHash, true);
            }
            else{
                animator.SetBool(isIdleHash, false);
            }
            wait=0;
        }
        else{
            wait+=Time.deltaTime;
        }
        if(wait==0)
        {
            second=transform.position;
        }
        if(!notStop)
        {
            if(!obi.attacked)
            {
                if(respawnTime>0)
                {
                    respawnTime -= Time.deltaTime;
                    coyoteFaintMesh.SetActive(true);
                    coyoteFaintMesh.transform.eulerAngles = new Vector3(
                        coyoteFaintMesh.transform.eulerAngles.x * 1,
                        coyoteFaintMesh.transform.eulerAngles.y * 1,
                        coyoteFaintMesh.transform.eulerAngles.z * 1
                    );
                }
                else{
                    coyoteMesh.SetActive(true);
                    GetComponent<BoxCollider>().enabled = true;
                    GetComponent<NavMeshAgent>().enabled = true;
                    notStop = true;
                }
            }
        }
        if(notStop)
        {
            coyoteFaintMesh.SetActive(false);
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            
            if(!playerInSightRange) 
            {
                Idle();
                GetComponent<NavMeshAgent>().speed = 15; 
                if(walkPointSet)
                {
                    if(timer>0)
                    {
                        timer -= Time.deltaTime;
                    }
                    else{
                        walkPointSet = false;

                        timer = 25f;
                    }
                }
                else{
                    timer = 25f;
                }
            }
            if(playerInSightRange)
            {
                if(!obi.attacked)
                {
                    GetComponent<NavMeshAgent>().speed = 30; 
                    Run();
                }
            }
            if(attackTime > 0)
            {
                attackTime -= Time.deltaTime;
                agent.SetDestination(player.position);
            }
            if(obi.attacked)
            {
                Idle();
            }
        }
    }

    private void Idle()
    {
        if (!walkPointSet) 
            SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < .1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }
    private void Run()
    {
        attackTime = 2f;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(transform.position,sightRange);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!obi.attacked)
        {
            if (other.gameObject.name == "obi")
            {
                GetComponent<BoxCollider>().enabled = false;
                GetComponent<NavMeshAgent>().enabled = false;
                coyoteMesh.SetActive(false);
                obi.attacked = true;
                notStop = false;
                respawnTime = 20f;
            }
        }
    }

}
