using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CyclopsAttack : MonoBehaviour
{
    public static CyclopsAttack instance;
    private Transform player;
    private Rigidbody rb;
    private Animator animator;
    private CyclopsMovement cyclopsMovement; // CyclopsMovement referansı

    private bool hasCollided = false; // Çarpışma gerçekleşti mi kontrol etmek için bayrak

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        cyclopsMovement = GetComponent<CyclopsMovement>(); // CyclopsMovement referansını al
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasCollided) return; // Eğer daha önce çarpıştıysa, methoddan çık

        if (collision.gameObject.CompareTag("Player"))
        {
            hasCollided = true; // Çarpışma gerçekleşti
            animator.SetBool("PlayerIsAttackable", true);
            if (cyclopsMovement != null)
            {
                cyclopsMovement.enabled = false; // CyclopsMovement'u devre dışı bırak
            }

            StartCoroutine(CooldownRoutine()); // Bekleme süresi başlat
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("PlayerIsAttackable", false);
            if (cyclopsMovement != null)
            {
                cyclopsMovement.enabled = true; // CyclopsMovement'u tekrar etkinleştir
            }
        }
    }

    IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(1.5f); // 1.5 saniye bekle
        hasCollided = false; // Çarpışmayı sıfırla, yeni çarpışmalara izin ver
    }
}