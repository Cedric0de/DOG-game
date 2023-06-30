using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class bunnyMovement : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    //idle
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //states
    public float sightRange;
    public bool playerInSightRange;


    private void Awake()
    {
        player = GameObject.Find("influence").transform;
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        
        if(!playerInSightRange) Idle();
        if(playerInSightRange) Run();
    }

    private void Idle()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            StartCoroutine (IdleSpot ());
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
        agent.SetDestination(player.position);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(transform.position,sightRange);
    }
    IEnumerator IdleSpot()
    {
        yield return new WaitForSeconds(Random.Range(2,10));
    }
}
