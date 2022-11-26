using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region Variables
    public Transform[] pointsObjects;
    public int speedWalking;
    public GameObject acornPrefab;

    [Header("Attack Player")]
    public float distanceToPlayer;  // Limit distance for agroo lose
    public GameObject player;       
    public float speedAttack;       // Attack speed
    public float speedAnimation;    // Walking animation speed
    public float deathTrhust;       // Thrust use when the player kills the enemy

    Vector2[] points;               // Path positions extracted form pointsObjects
    Vector3 posToGo;                // Next path that the object has to follow
    int i;
    float speed;

    SpriteRenderer spriteRenderer;
    Animator animator;
    #endregion

    #region Life Cycle
    // Start is called before the first frame update
    void Start()
    {
        speed = speedWalking;
        points = new Vector2[pointsObjects.Length];
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Set the path points for better code view
        for (int i = 0; i < pointsObjects.Length; i++)
            points[i] = pointsObjects[i].position;

        // Intialitate path
        posToGo = points[0];
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, player.transform.position, Color.red);

        if(Vector2.Distance(transform.position, player.transform.position) <= distanceToPlayer)
        {
            // Attack
            PlayerDetected();
        }
        else
        {
            // It will update the next pos
            NextPathPos();

            // Speed check
            if (speed != speedAttack) ResetAnimSpeed();
        }            

        // It will follow the path
        FollowPath();

        // Animations
        Flip();
    }
    #endregion

    #region Path
    // It will advance to the next position
    void FollowPath()
    {
        transform.position = Vector2.MoveTowards(transform.position, posToGo, speed * Time.deltaTime);
    }

    // Update and set the next path
    void NextPathPos()
    {
        // It will update path pos
        if(transform.position == posToGo)
        {
            i = i == points.Length - 1 ? 0 : i + 1;
            posToGo = points[i];
            Debug.Log(posToGo);
        }
    }

    // It will increase the speed and change the path to aim the player
    void PlayerDetected()
    {
        Debug.DrawLine(transform.position, player.transform.position, Color.green);

        // Speed update
        speed = speedAttack * 1.5f;
        animator.speed = speedAnimation * 1.5f;

        // New path
        posToGo = new Vector2(player.transform.position.x, transform.position.y);
    }
    #endregion

    #region Animations
    // It will get the movement dir and flipping the player sprite in consecuence
    void Flip()
    {
        if (posToGo.x > transform.position.x) spriteRenderer.flipX = true;
        if (posToGo.x < transform.position.x) spriteRenderer.flipX = false;
    }

    // It will reset speed if player has been followed
    void ResetAnimSpeed()
    {
        speed = speedAttack;
        animator.speed = speedAnimation;
    }
    #endregion

    #region collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Player collision
        if (collision.collider.CompareTag("Player")) OnPlayerCollide(collision);
    }
    #endregion

    #region Others

    void OnPlayerCollide(Collision2D collision)
    {
        GameObject _player = collision.gameObject;
        if (!_player.GetComponent<PlayerController>().isGrounded)
        {
            // Feedback and destroy
            _player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * deathTrhust);
            Destroy(gameObject);
        }
        else
        {
            //destroy
            Destroy(_player);
        }
    }

    #endregion
}
