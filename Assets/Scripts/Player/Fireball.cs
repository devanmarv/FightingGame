using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public PlayerController owner;
    public int damage;
    public GameObject hitEffect;
    public AudioSource audio;
    public AudioClip hitsnd;
    public float lifetime;
    public float speed;
    private Rigidbody2D rig;
    public float dir;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(Instantiate(hitEffect, transform.position, Quaternion.identity), 1.0f);
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().TakeDamage(damage, owner);
            
        }
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CompareTag("Ground") || )
    }

    public void onSpawn(float damage, float speed, PlayerController owner, float dir)
    {
        setOwner(owner);
        setDamage(damage);
        setSpeed(speed);
        rig.velocity = new Vector2(dir * speed, 0);
    }

    public void setOwner(PlayerController owner_new)
    {
        owner = owner_new;
    }
    public void setDamage(float damage)
    {
        this.damage = (int)damage;
    }
    public void setDamage(int damage)
    {
        this.damage = damage;
    }
    public void setSpeed(float speed)
    {
        this.speed = speed;
    }
}
