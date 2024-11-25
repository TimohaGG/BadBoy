using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{

    private double hp = 100;
    private double damage = 20;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float playerSpeed;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    private Animator animator;
    public float attackRange = 5;
    public LayerMask enemyLayer;


    public void SetDamage(double round)
    {
        this.damage = round * 20;
    }

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);
      
        animator.SetBool("inMoovement", transform.position != move);

        if (move != Vector3.zero) 
        {
            gameObject.transform.forward = move;
        }


        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        
        animator.SetFloat("Speed", move.magnitude);

        Debug.Log(move.magnitude);
        GameObject text = GameObject.FindGameObjectWithTag("HP").gameObject;
        TextMeshProUGUI mesh = text.GetComponent<TextMeshProUGUI>();
        mesh.text = "HP: " + hp;

        if (Input.GetMouseButtonDown(0)) {
            animator.SetTrigger("Attack");
           
        }
    }

    public void Attack()
    {
      
        Collider[] enemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
        if (enemies.Length > 0)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] != null && enemies[i].gameObject.CompareTag("Enemy"))
                {
                    transform.LookAt(enemies[i].transform);
                   Enemy enemy = enemies[0].gameObject.GetComponent<Enemy>();
                    Player player = this.gameObject.GetComponent<Player>();
                    enemy.GetDamage(damage, player);
                    Debug.Log("Attack!");
                }
            }


        }

    }

    public void GetDamage(double damage)
    {
        hp-=damage;
        if(hp <= 0)
        {
            Die();
        }
    }

    public void Heal()
    {
        hp = 100;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Enemy"))
        {
            //Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            //Player player = this.gameObject.GetComponent<Player>(); 
            //enemy.GetDamage(damage, player);
        }
    }

   
}
