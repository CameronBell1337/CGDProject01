using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBehaviour : MonoBehaviour
{
   [SerializeField]
   private Transform player;

   private Vector3 player_pos;
   
   [SerializeField]
   private GameObject bullet;

   [SerializeField]
   private float action_timer = 1f;

   enum action_state
   {
       IDLE,
       SHOOTING,
       CHASING,
       FLEEING
   }

   private action_state state = action_state.IDLE;

   // Start is called before the first frame update
    void Start()
    {
        player_pos = player.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case(action_state.IDLE):
                break;
            case(action_state.SHOOTING):
                doShooting();
                break;
            case(action_state.CHASING):
                doChasing();
                break;
            case(action_state.FLEEING):
                doFleeing();
                break;
        }

        if (hasPlayerLOS())
        {
            switch (timerCheck())
            {
                case(0):
                    action_timer -= Time.deltaTime;
                    break;
                case (1):
                    state = action_state.SHOOTING;
                    break;
                case (2):
                    state = action_state.CHASING;
                    action_timer = 3f;
                    break;
                case (3):
                    state = action_state.FLEEING;
                    action_timer = 3f;
                    break;
            }
        }
        else state = action_state.IDLE;
    }

    bool hasPlayerLOS()
    {
        Debug.Log("checking player LOS");
        //set layermask to enemy layer
        int layerMask = 1 << 8;
        //set mask to check for everything but enemies
        layerMask = ~layerMask;
        RaycastHit hit;
        if (Physics.Linecast(transform.position, player_pos, out hit, layerMask))
        {
            Debug.DrawLine(transform.position, hit.point, Color.white, 2.5f);
            if (hit.collider.CompareTag("Player")) 
            {
                return true;
            }
        }
        
        return false;
    }

    void doShooting()
    {
        Debug.Log("i want to shoot");
        //Instantiate(bullet, transform.position, transform.rotation);
    }

    void doChasing()
    {
        // Check if close enough to player
        if (Vector3.Distance(transform.position, player_pos) > 1f)
        {
            //Move toward player
            transform.position = Vector3.MoveTowards(transform.position, player_pos, 1 * Time.deltaTime);
        }
    }

    void doFleeing()
    {
        Vector3 opposite_player = transform.position - player_pos;
        //Move away from player
        transform.position = Vector3.MoveTowards(transform.position, opposite_player, 1 * Time.deltaTime);
    }

    /*
     * returns a random between 1 and 4 if action timer is 0 or less
     * returns 0 if action timer is above 0 and reduces action_timer by time
     */
    int timerCheck()
    {
        if (action_timer <= 0)
        {
            return Random.Range(1, 4);
        }
        return 0;
    }
}
