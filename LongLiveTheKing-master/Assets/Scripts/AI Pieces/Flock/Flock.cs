using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    public List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehaviour behaviour;

    [Range(1, 50)]
    public int startingCount = 1;
    const float AgentDensity = 1.7f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 10f)]
    public float neigbourRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    float squareMaxSpeed;
    float squareNeighbourRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    //the state of the Flock AI
    public enum AIState { WANDERING, CHASING, ATTACKING};

    private AIState state = AIState.WANDERING;

    private Shader originalShader;
    [SerializeField]
    private int chasingDistance = 50;

    public AIState State()
    {
        return state;
    }

    // Start is called before the first frame update
    private void Start()
    {
        state = AIState.WANDERING;

        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighbourRadius = neigbourRadius * neigbourRadius;
        squareAvoidanceRadius = squareNeighbourRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        Vector2 spawnCirc = UnityEngine.Random.insideUnitCircle;
        
        for(int i = 0; i < startingCount; i++)
        {
            spawnCirc = UnityEngine.Random.insideUnitCircle;

            FlockAgent newAgent = Instantiate(
                agentPrefab,
                transform.position + new Vector3(spawnCirc.x, 0, spawnCirc.y) * startingCount * AgentDensity,
                Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0f, 360f)),
                transform
                );
            newAgent.flock = this;
            newAgent.name = "Pawn Agent " + i;
            
            agents.Add(newAgent);
            // yield return new WaitForSeconds(1);
            // Debug.Log("position " + i + new Vector3(spawnCirc.x, 0, spawnCirc.y) * startingCount * AgentDensity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = GameObject.FindGameObjectWithTag("Player").transform.position;

        if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < chasingDistance)
        {
            state = AIState.CHASING;
        } else
        {
            state = AIState.WANDERING;
        }

        

        foreach (FlockAgent agent in agents)
        {
            // IF IN WANDER STATE:

            switch (state)
            {
                case AIState.WANDERING:

                    // Debug.Log("WANDERING!!!");

                    List<Transform> context = GetNearbyObjects(agent);
                    agent.GetComponentInChildren<Animator>().SetBool("IsChasing", false);
                    agent.GetComponentInChildren<Animator>().SetBool("IsAttacking", false);
                    Vector3 move = behaviour.CalculateMove(agent, context, this);
                    move *= driveFactor;
                    move.y = 0;
                    if (move.sqrMagnitude > squareMaxSpeed)
                    {
                        move = move.normalized * maxSpeed;
                    }
                    agent.Move(move);
                    break;
                case AIState.CHASING:
                    //agent.GetComponentInChildren<MeshRenderer>().material.color = Color.white;
                    agent.GetComponentInChildren<Animator>().SetBool("IsChasing", true);
                    Vector3 dir = (target - agent.transform.position).normalized;
                    // agent.GetComponentInChildren<MeshRenderer>().material.color = Color.white;
                    if (Vector3.Distance(target, agent.transform.position) <= agent.transform.GetComponent<SphereCollider>().radius)
                    {
                        state = AIState.ATTACKING;
                    }
                    else
                    {
                        agent.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
                        agent.transform.position = agent.transform.position + dir * (maxSpeed / 2) * Time.deltaTime;
                    }
                    break;
                case AIState.ATTACKING:

                    if (Vector3.Distance(target, agent.transform.position) <= agent.transform.GetComponent<SphereCollider>().radius)
                    {
                        // Debug.Log("attack attack attack!!!");
                        //agent.GetComponentInChildren<MeshRenderer>().material.color = Color.cyan;
                        agent.GetComponentInChildren<Animator>().SetBool("IsAttacking", true);
                    }
                    else
                    {
                        state = AIState.CHASING;
                        // agent.GetComponentInChildren<MeshRenderer>().material.color = Color.white;
                    }
                    break;
            }
        }
    }

    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neigbourRadius);

        foreach (Collider c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }
}
