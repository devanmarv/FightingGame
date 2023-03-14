using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public int curHp;
    public int maxHp;
    public int score;
    public float moveSpeed;
    public float jumpForce;
    public int maxJumps;


    private int jumpsAvalable;
    private float curMoveInput;

    private PlayerController curAttacker;

    private List<GameObject> curStandingGameObjects = new List<GameObject>();


    [Header("Components")]
    public Rigidbody2D rig;
    public Animator anim;
    public Transform muzzle;
    public playerContainerUi containerUI;
    public AudioSource audio;
    public AudioClip laugh;
    public AudioClip death;

    [Header("combat")]
    public float attackRate;
    private float lastAttackTime;
    public float projectileSpeed;
    public GameObject projectilePrefab;
    

    public void spawn(Vector3 spawnPos)
    {
        transform.position = spawnPos;
        curHp = maxHp;
        containerUI.updateHealthBarFill(curHp, maxHp);
        curAttacker = null;
    }

    private void FixedUpdate()
    {
        move();
        // Animation update
        if(curMoveInput == 0.0f && IsGrounded())
        {
            anim.SetBool("is_moving", false);
            anim.SetBool("is_Jumping", false);
        }
        else if (curMoveInput != 0.0f && IsGrounded())
        {
            anim.SetBool("is_moving", true);
            anim.SetBool("is_Jumping", false);
        }
        else if (!IsGrounded())
        {
            anim.SetBool("is_moving", false);
            anim.SetBool("is_Jumping",true);
        }

        if (curMoveInput != 0)
        {
            float x = curMoveInput > 0 ? 1 : -1;
            transform.localScale = new Vector3(x, 1, 1);
        }

    }
    private void move()
    {
        rig.velocity = new Vector2(curMoveInput * moveSpeed, rig.velocity.y);
    }

    private void jump()
    {
        rig.velocity = new Vector2(rig.velocity.x, 0);
        rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

    }


    public void OnMoveInput(InputAction.CallbackContext context)
    {
        curMoveInput = context.ReadValue<float>();

    }
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            if(jumpsAvalable > 0)
            {
                jumpsAvalable--;
                jump();
            }
            

        }
    }
    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && Time.time - lastAttackTime > attackRate)
        {
            lastAttackTime = Time.time;
            spawnProjectile();
        }
    }

    void spawnProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, muzzle.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * projectileSpeed, 0);
        projectile.GetComponent<Fireball>().setOwner(this);
    }

    public void OnTauntInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            
            audio.PlayOneShot(laugh);
        }
    }
    bool IsGrounded()
    {
        return curStandingGameObjects.Count > 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].point.y < transform.position.y)
        {
            jumpsAvalable = maxJumps;

            if (!curStandingGameObjects.Contains(collision.gameObject))
            {
                curStandingGameObjects.Add(collision.gameObject);
            }

        }

       
    }

    
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        
        if (curStandingGameObjects.Contains(collision.gameObject))
        {
            curStandingGameObjects.Remove(collision.gameObject);
        }
    }
    

    public void TakeDamage(int damage, PlayerController attacker)
    {
        curAttacker = attacker;
        curHp -= damage;

        if (curHp <= 0)
        {
            die();
        }
        // update UI and health bar
        containerUI.updateHealthBarFill(curHp, maxHp);
    }

    void die()
    {
        audio.PlayOneShot(death);
        PlayerManager.instance.OnPlayerDeath(this, curAttacker);
    }
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -10)
        {
            die();
        }
    }
}
