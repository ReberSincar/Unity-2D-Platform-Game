using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private float characterSpeedX;
    public float speed;
    public float jumpPower;
    private Rigidbody2D charBody;
    private Vector3 defaultLocaleScale;
    public bool isOnFloor;
    private bool canDoubleJump;
    public GameObject arrow;
    public bool isAttacked;
    public float currentAttackTimer;
    public float defaultAttackTimer;
    private Animator animator;
    public GameObject winPanel, losePanel;
    public int arrowCount;
    public Text arrowCountText;
    public AudioClip dieMusic;
    void Start()
    {
        charBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        defaultLocaleScale = transform.localScale;
        isOnFloor = true;
        isAttacked = false;
        arrowCountText.text = arrowCount.ToString();
    }

    void Update()
    {
        MoveXDirection();
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            JumpControl();
        }

        if (Input.GetMouseButtonDown(0) && arrowCount > 0)
        {
            if (!isAttacked)
            {
                isAttacked = true;
                animator.SetTrigger("Attack");
                Invoke("ThrowArrow", 0.5f);
            }
        }

        //Start Timer if attacked
        if (isAttacked)
        {
            currentAttackTimer -= Time.deltaTime;
        }
        //Reset the timer
        else
        {
            currentAttackTimer = defaultAttackTimer;
        }

        //When time is up
        if (currentAttackTimer < 0)
        {
            isAttacked = false;
            animator.SetBool("Attack", false);
        }
    }

    void ThrowArrow()
    {
        bool charDirection = transform.localScale.x > 0;
        float arrowPositionX = charDirection ? transform.position.x + 1 : transform.position.x - 1;
        GameObject cloneArrow = Instantiate(arrow, new Vector3(arrowPositionX, transform.position.y), Quaternion.identity);
        cloneArrow.transform.parent = GameObject.Find("Arrows").transform;
        if (charDirection)
        {
            cloneArrow.GetComponent<Rigidbody2D>().velocity = new Vector2(20f, 0f);
        }
        else
        {
            cloneArrow.transform.localScale = new Vector3(-cloneArrow.transform.localScale.x, cloneArrow.transform.localScale.y, cloneArrow.transform.localScale.z);
            cloneArrow.GetComponent<Rigidbody2D>().velocity = new Vector2(-20f, 0f);
        }
        arrowCount--;
        arrowCountText.text = arrowCount.ToString();
    }

    void MoveXDirection() {
        characterSpeedX = Input.GetAxis("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(characterSpeedX));
        charBody.velocity = new Vector2(characterSpeedX * speed, charBody.velocity.y);
        ChangeCharDirection();
    }

    void ChangeCharDirection()
    {
        if (characterSpeedX > 0)
        {
            transform.localScale = defaultLocaleScale;
        }
        else if (characterSpeedX < 0)
        {
            transform.localScale = new Vector3(-defaultLocaleScale.x, defaultLocaleScale.y, defaultLocaleScale.z);
        }
    }

    void JumpControl()
    {
        if (isOnFloor)
        {
            Jump();
            canDoubleJump = true;
        }
        else
        {
            if (canDoubleJump)
            {
                Jump();
                canDoubleJump = false;
            }
        }
    }

    void Jump()
    {
        charBody.velocity = new Vector2(charBody.velocity.x, jumpPower);
        animator.SetTrigger("Jump");
    }

    public void Die()
    {
        AudioSource audioSource = GameObject.Find("SoundController").GetComponent<AudioSource>();
        audioSource.clip = null;
        audioSource.PlayOneShot(dieMusic);
        animator.SetFloat("Speed", 0f);
        animator.SetTrigger("Die");
        charBody.constraints = RigidbodyConstraints2D.FreezeAll;
        enabled = false;
        StartCoroutine(Wait(false, 1f));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Thorn")
        {
            GetComponent<TimeController>().enabled = false;
            Die();
        }
        else if (collision.gameObject.CompareTag("Finish")){
            StartCoroutine(Wait(true, 0.5f));
            Destroy(collision.gameObject);
        }
    }

    IEnumerator Wait(bool win, float time)
    {
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 0;
        if (win)
        {
            winPanel.SetActive(true);
        } else
        {
            losePanel.SetActive(true);
        }
    }
}
