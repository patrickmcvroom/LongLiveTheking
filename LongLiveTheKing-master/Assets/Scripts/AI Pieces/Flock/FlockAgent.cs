using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{
    Vector3 velocity;
    Collider agentCollider;
    public Collider AgentCollider { get { return agentCollider; } }

    public bool IsAlive { get; internal set; } = true;

    public int hits = 1;
    public Flock flock;

    public void Die()
    {
        IsAlive = false;
        hits -= 1;
        if (hits <= 0)
        {
            if (flock != null)
            {
                flock.agents.Remove(this);
            }
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        agentCollider = GetComponent<Collider>();
    }

    public void Move(Vector3 velocity)
    {
        this.velocity = velocity;
        transform.forward = velocity;
        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    private void OnGUI()
    {
        //Debug.DrawLine(transform.position, velocity, Color.red);
    }
}
