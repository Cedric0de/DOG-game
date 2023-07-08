using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class bunnyMovement : MonoBehaviour
{
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


    private void Awake()
    {
        player = GameObject.Find("influence").transform;
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        if(notStop)
        {
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
            if(playerInSightRange) {
                Run();
                GetComponent<NavMeshAgent>().speed = 30; 
            }
            if(panicTime > 0)
            {
                panicTime -= Time.deltaTime;
                agent.SetDestination(transform.position - ((playerReal.position - transform.position).normalized * 10));
                GetComponent<NavMeshAgent>().speed = 30;
            }
        }
    }

    private void Idle()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
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
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            notStop = false;
            activator.rabbits+=1;
        }
    }
}
