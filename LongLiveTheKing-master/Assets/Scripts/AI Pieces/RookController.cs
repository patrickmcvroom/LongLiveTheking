using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

public class RookController : MonoBehaviour
{
    //the state of the AI
    public enum AIState { WANDERING, CHASING, ATTACKING, DEAD };

    private AICharacterControl rookControl;
    private GameObject[] allWaypoints;
    private int currentWaypoint = 0;
    private ThirdPersonCharacter tpCharacter;
    private AIState state = AIState.WANDERING;
    //private Animator animator;

    public int hits = 5;

    // UNCOMMENT THIS ONE // private Image AIHealthBar;
    
    [SerializeField]
    private float deathTimeout;

    public ThirdPersonCharacter TpCharacter { get => tpCharacter; set => tpCharacter = value; }

    public AIState State()
    {
        return state;
    }

    // Start is called before the first frame update
    void Start()
    {
        deathTimeout = 1;
        //animator = GetComponent<Animator>();
        

        // UNCOMMENT THIS ONE // AIHealthBar.fillAmount = health / (float)startingHealth;

        //store the controllers in variables for easy access later
        rookControl = GetComponent<AICharacterControl>();
        TpCharacter = GetComponent<ThirdPersonCharacter>();

        //store all the waypoints too
        allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        //shuffle array to make unique wandering path
        System.Random rnd = new System.Random(System.DateTime.Now.Millisecond);
        allWaypoints = allWaypoints.OrderBy(x => rnd.Next()).ToArray();
    }

    protected bool CanSeePlayer()
    {
        //function to determine whether the AI character can see the player

        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position; // get player position

        //we only want to look ahead so we check if the player in a 90 degree arc. (Vec3.angle returns an absolute value, so 45 degrees either way means <=45)
        if (Vector3.Distance(transform.position, playerPosition) <= 40f)
        {
            return true;
        }//or if any of these tests failed, i can't see them.
        return false;
    }

    // Heal method
    // UNCOMMENT THIS ONE // public void Heal()
    // UNCOMMENT THIS ONE // {
    // UNCOMMENT THIS ONE // health = startingHealth;
    // UNCOMMENT THIS ONE // }

    // Update is called once per frame
    void Update()
    {
        // UNCOMMENT THIS ONE // AIHealthBar.fillAmount = Mathf.Lerp(AIHealthBar.fillAmount, health / (float)startingHealth, Time.deltaTime * 2);

        Debug.Log(state, this);

        switch (state)
        {
            case AIState.WANDERING:
                rookControl.SetTarget(allWaypoints[currentWaypoint].transform);
                //Debug.Log(currentWaypoint);
                //rookControl.GetComponentInChildren<Animator>().SetBool("IsAttacking", false);
                //rookControl.GetComponentInChildren<Animator>().SetBool("IsChasing", false);
                if ((Vector3.Distance(rookControl.target.transform.position, transform.position) < 2.0f))
                {
                    //...make me target the next one
                    currentWaypoint++;
                    //make sure that we don't fall off the end of the array but lop back round
                    currentWaypoint %= allWaypoints.Length;
                }
                //can I see the player? if so, the chase is on!
                if (CanSeePlayer())
                {
                    state = AIState.CHASING;
                    //Debug.Log(this.name + " CHASING");
                }
                break;
            case AIState.CHASING:
                rookControl.SetTarget(GameObject.FindGameObjectWithTag("Player").transform);
                //rookControl.GetComponentInChildren<Animator>().SetBool("IsChasing", true);
                if (!CanSeePlayer())
                {
                    //i can't see him, so back to wandering...
                    state = AIState.WANDERING;
                    return;
                }
                if (Vector3.Distance(rookControl.target.transform.position, transform.position) <= GetComponent<SphereCollider>().radius)
                {
                    state = AIState.ATTACKING;
                }
                break;
            case AIState.ATTACKING:
                if (Vector3.Distance(rookControl.target.transform.position, transform.position) <= GetComponent<SphereCollider>().radius)
                {
                    //rookControl.GetComponentInChildren<Animator>().SetBool("IsAttacking", true);
                    //Debug.Log(this.name + " I AM ATTACKING");
                } else
                {
                    state = AIState.CHASING;
                    //Debug.Log(this.name + " CHASING");
                }
                break;
            case AIState.DEAD:
                Destroy(gameObject, deathTimeout); // remove myself from the game after timeout
                break;
        }
    }

    public void Die()
    {
        hits -= 1;
        if (hits <= 0)
        {
            Destroy(this.gameObject);
        }
        Debug.Log(hits);
    }
}