using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopsMovement : MonoBehaviour
{
    private float aggroRadius;
    private float movementSpeed;
    private Transform player;
    private Rigidbody rb;
    private Animator animator;
    private bool chaseStarted;

    void Start()
    {
        chaseStarted = false;
        aggroRadius = 4f;
        movementSpeed = 1f;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        float playerDistance = Vector3.Distance(transform.position, player.position);

        if (playerDistance <= aggroRadius || chaseStarted)
        {
            chaseStarted = true;
            animator.SetBool("PlayerInRange", true);
            
            // Oyuncu range içinde, hareket etmeye başla
            Vector3 direction = (player.position - transform.position).normalized;

            // Y eksenindeki hareketi engelle
            direction.y = 0;

            // Rigidbody ile hareket
            if (rb != null)
            {
                rb.MovePosition(transform.position + direction * movementSpeed * Time.deltaTime);
            }

            // Yüzünü karaktere dönsün
            RotateTowardsPlayer(direction);
        }
        else
        {
            animator.SetBool("PlayerInRange", false);
        }
    }

    // Yüzü her zaman karaktere döndürmek için fonksiyon
    void RotateTowardsPlayer(Vector3 direction)
    {
        // Hedef yönün rotasyonunu hesapla
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Modelinizin yanlış eksenini düzeltmek için ekstra rotasyon ekleyin
        Quaternion fixRotation = Quaternion.Euler(0, 180f, 0); // Y ekseninde 180 derece döndür
        targetRotation *= fixRotation;

        // Mevcut rotasyondan hedef rotasyona yumuşak geçiş
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }
}
