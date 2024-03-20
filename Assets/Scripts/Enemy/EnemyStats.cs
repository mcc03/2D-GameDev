using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class EnemyStats : MonoBehaviour
{
    //reference for enemy scriptable object
    public EnemyScriptableObject enemyData;

    //variables for enemy scriptable object
    //current stats
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;

    public float despawnDistance = 20f;
    Transform player;

    [Header("Damage Feedback")]
    public Color damageColor = new Color(1,0,0,1);// what the color of the dmage flash should be.
    public float damageFlashDuration = 0.2f; // how long the flash should last
    public float deathFadeTime = 0.6f; // howw much time it takes for the enemy to fade.
    Color originalColor;
    SpriteRenderer sr;
    EnemyMovement movement;

    void Awake()
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
    }

    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        movement = GetComponent<EnemyMovement>();
    }

   /*  void update()
    {
        if(Vector2.Distance(transform.position, player.position) >= despawnDistance)
        {

        }
    } */

    //This function always needs at least 2 values, the amount dealth <dmg> as welll as where the damage us
    // coming from. whoch is passed as <sourePostion>. The <sourcePosition> is necessary because it is used to calculate 
    // the direction of the knockback
    public void TakeDamage(float dmg, Vector2 sourcePosition,float knockbackForce = 5f, float knockbackDuration = 0.2f)
    {
        currentHealth -= dmg;
        StartCoroutine(DamageFlash());
         // apply knockback if it is not zero
        if(knockbackForce > 0)
        {
            // gets the direction of knockbacks
            Vector2 dir = (Vector2)transform.position - sourcePosition;
            movement.knockback(dir.normalized * knockbackForce, knockbackDuration);
        }

        if(currentHealth <= 0)
        {
            Kill();
        }
    }

    // This is a Coroutine function that makes the enemy flash when taking damage.
    IEnumerator DamageFlash()
    {
        sr.color = damageColor;
        yield return new WaitForSeconds(damageFlashDuration);
        sr.color = originalColor;
    }

    public void Kill()
    {
        StartCoroutine(KillFade());
       // Destroy(gameObject);
    }

    IEnumerator KillFade()
    {
        //waits for a single frame
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float t = 0, origaAlpha = sr.color.a;

        // This is a loop that fires every frame.
        while(t < deathFadeTime) {
            yield return w;
            t += Time.deltaTime;

            //set the color for theis frame 
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, (1- t/ deathFadeTime) + origaAlpha);
        }

        Destroy(gameObject);
    }

    //enemy will deal damager to player when their collider is touching the players' collider
    private void OnCollisionStay2D(Collision2D col) 
    {
        //reference script from the collided collider and deal damage using TakeDamage method
        if(col.gameObject.CompareTag("Player"))
        {
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage);
        }
    }
}
