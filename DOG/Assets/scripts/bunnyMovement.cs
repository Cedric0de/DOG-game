using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class bunnyMovement : MonoBehaviour
{
    Animator animator;
    public NavMeshAgent agent;

    public Transform player;
    public Transform playerReal;
    public rabbitCounter activator;

    public LayerMask whatIsGround, whatIsPlayer;

    //idle
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //states
    public float sightRange;
    public bool playerInSightRange;
    float panicTime = 0f;
    float timer = 25f;
    bool notStop = true;
    public GameObject rabbitMesh;
    float wait;
    Vector3 first;
    Vector3 second;
    int isIdleHash;
    int isIdleAltHash;
    int randomChance;
    //bool alt;


    private void Awake()
    {
        player = GameObject.Find("influence").transform;
        agent = GetComponent<NavMeshAgent>();
        second=transform.position;
        animator = rabbitMesh.GetComponent<Animator>();
        isIdleHash = Animator.StringToHash("isIdle");
        isIdleAltHash = Animator.StringToHash("isIdleAlt");
        //alt = false;
    }
    private void Update()
    {
        bool isIdle = animator.GetBool(isIdleHash);
        bool isIdleAlt = animator.GetBool(isIdleAltHash);
        if(wait>=0.01f)
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
        if(notStop)
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            
            if(!playerInSightRange) 
            {
                Idle();
                agent.speed = 15;
                if(walkPointSet)
                {
                    if(timer>0)
                    {
                        timer -= Time.deltaTime;
                    }
                    else{
                        walkPointSet = false;

                        timer = 10f;
                    }
                }
                else{
                    timer = 10f;
                }
            }
            if(playerInSightRange) {
                Run();
                agent.speed = 40; 
            }
            if(panicTime > 0)
            {
                panicTime -= Time.deltaTime;
                agent.SetDestination(transform.position + (transform.position - playerReal.position));
                //GetComponent<NavMeshAgent>().speed = 30;
            }
        }
    }

    private void Idle()
    {
        // if(randomChance==1)
        // {
        //     animator.SetBool(isIdleAltHash, true);
        //     alt = true;
        //     if(animator.GetCurrentAnimatorStateInfo(0).IsName("metarig_Idle Alt"))
        //     {
        //         animator.SetBool(isIdleAltHash, false);
        //         alt = false;
        //         randomChance = 2;
        //     }
        // }
        //else{
        if (!walkPointSet) {
            SearchWalkPoint();
        }

        if (walkPointSet)
            agent.SetDestination(walkPoint);
        //}
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
            //randomChance = Random.Range(1,4);
        }
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
        panicTime = 10;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(transform.position,sightRange);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "obi")
        {
            rabbitMesh.SetActive(false);
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            notStop = false;
            activator.rabbits+=1;
            FindObjectOfType<AudioManager>().Play("Rabbit Squeek");
        }
    }
}
