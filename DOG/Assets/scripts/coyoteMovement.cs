using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class coyoteMovement : MonoBehaviour
{
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
    int mash = 0;
    bool attacking = false;


    private void Awake()
    {
        player = GameObject.Find("influence").transform;
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        if(!notStop)
        {
            if(respawnTime>0)
            {
                respawnTime -= Time.deltaTime;
            }
            else{
                GetComponent<MeshRenderer>().enabled = true;
                GetComponent<BoxCollider>().enabled = true;
                GetComponent<NavMeshAgent>().enabled = true;
                notStop = true;
            }
        }
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
            if(playerInSightRange)
            {
                GetComponent<NavMeshAgent>().speed = 25; 
                Run();
            }
            if(attackTime > 0)
            {
                attackTime -= Time.deltaTime;
                agent.SetDestination(player.position);
            }
        }
        if(attacking)
        {
            if(mash<=0)
            {
                mash += Random.Range(8,15);
            }
            if(mash>0)
            {
                if(Input.GetKeyDown("joystick button 1") || Input.GetKeyDown("space"))
                {
                    mash-=1;
                }
            }
            if(mash<=0)
            {
                obi.attacked = false;
                attacking = false;
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

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround) && (walkPoint.x<172 && walkPoint.x>-170) && (walkPoint.y<192 && walkPoint.y>-195))
            walkPointSet = true;
    }
    private void Run()
    {
        attackTime = 3f;
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
            obi.attacked = true;
            notStop = false;
            respawnTime = 30f;
            attacking =true;
        }
    }

}
