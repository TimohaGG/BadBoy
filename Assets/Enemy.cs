using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    
    public double hp;
    public double damage;


    GameObject player;
    NavMeshAgent agent;
    Animator animator;
    TextMeshProUGUI textObj;
    public LayerMask groundlayer, playerLayer;
    

    Vector3 destPoint;
    bool walkPointSet;
    bool canMove = true;
    public float walkingRange;

    public float sightRange, attackRange;
    bool playerInSight, playerInAttackRange;
    bool isKilled = false;
   

    public Enemy(double hp, double damage)
    {
        this.hp = hp;
        this.damage = damage;
    }

    void Start()
    {
        canMove = true;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        animator = GetComponent<Animator>();
        textObj = GetComponentInChildren<TextMeshProUGUI>();
        textObj.text = hp.ToSafeString();
    }

    // Update is called once per frame
    void Update()
    {
        textObj.transform.position = transform.position + new Vector3(0, 3);
        textObj.transform.LookAt(UnityEngine.Camera.main.transform);
        textObj.transform.Rotate(0, 180, 0);
        textObj.transform.rotation = Quaternion.Euler(0, textObj.transform.rotation.eulerAngles.y, 0);
        if (!canMove)
        {
            return;
        }

        playerInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);
        if (!playerInSight && !playerInAttackRange)
        {
            Patrol();
        }
       
        if (playerInSight && !playerInAttackRange)
        {
            agent.speed = 3;
            Chase();
        }

        if(playerInSight && playerInAttackRange)
        {
            Attack();
        }
     
     
    }

    void Chase()
    {
        animator.SetTrigger("NoAttack");
        if (agent.isActiveAndEnabled)
        {
            
            agent.SetDestination(player.transform.position);
        }
    }

    void Attack()
    {

    }
    void Patrol()
    {
        if (!walkPointSet)
        {
            SerachForDestination();
        }
        if (walkPointSet)
        {
            try
            {
                agent.SetDestination(destPoint);
            }
               
            catch(Exception ex)
            {
                Console.WriteLine("ASd");
            }
        }
        if(Vector3.Distance(transform.position,destPoint)<10) walkPointSet = false;
    }

    void SerachForDestination()
    {
        float z = UnityEngine.Random.Range(-walkingRange, walkingRange);
        float x = UnityEngine.Random.Range(-walkingRange, walkingRange);

        destPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
        if (Physics.Raycast(destPoint, Vector3.down, groundlayer))
        {
            walkPointSet = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Player")) {
            animator.SetTrigger("Attack");
            
        }
        else
        {
            animator.SetTrigger("NoAttack");
        }
    }

    public void AttackPlayer()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, attackRange, playerLayer);
        if (enemies.Length > 0)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if(enemies[i] != null && enemies[i].gameObject.CompareTag("Player"))
                {
                    Player player = enemies[i].gameObject.GetComponent<Player>();
                    player.GetDamage(damage);
                }
            }
        }
        
       
        Console.WriteLine("Player takes damage!");
    }

    public void GetDamage(double damage, Player player)
    {
        if(hp!=0)
            hp -= damage;
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = hp.ToSafeString();
        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isKilled)
            return;
        canMove = false;
        GameObject gameObj = GameObject.FindGameObjectWithTag("Respawn");
        if (gameObj != null)
        {
            Game game = gameObj.GetComponent<Game>();
            game.IncreaseKills();
        }
        isKilled = true;
        animator.SetTrigger("Death");
       
    }

    public void DestroyObj()
    {
        Destroy(gameObject);
    }
}
