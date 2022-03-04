using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Shooting variables")]
    [SerializeField] private GameObject m_Projectile;
    private bool isShooting;

    [Header("Movement variables")]
    [SerializeField] private float m_Speed;

    private Vector2 inputVector;
    private Vector2 directionVector;
    private Vector2 initialScale;

    private bool isMoving;

    [Header("Rigidbody variables")]
    [SerializeField] private float m_JumpForce;
    private Rigidbody2D m_Rigidbody;
    private bool isJumping;

    [Header("Ground Check variables")] 
    [SerializeField] private LayerMask m_WhatIsGround;
    private const float GROUND_CHECK_RADIUS = 0.01f;
    private ColliderCheck m_GroundCheck;
    private bool isGrounded;

    // Life variables
    public int lifeCounter { get; set; }
    public bool isHurt { get; set; }

    // Animation variables
    private Animator m_Animator;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_GroundCheck = transform.GetComponentInChildren<ColliderCheck>();
    }

    void Start()
    {
        isShooting = false;

        isJumping = false;

        isHurt = false;

        initialScale = transform.localScale;
        directionVector = new Vector2(transform.position.x, 0);
        isMoving = false;

        m_Animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        CheckGrounded();
        Movement();
    }

    void Update()
    {
        GameOver();
        HandleInput();
        Animate();
    }

    /// <summary>
    /// Logic associated to button presses.
    /// </summary>
    void HandleInput()
    {
        int horizontal = (int)Input.GetAxisRaw("Horizontal");
        isMoving = horizontal != 0;

        if (isMoving)
        {
            directionVector = new Vector2(horizontal, 0);
        }

        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
        }

        if (Input.GetButtonDown("Fire3"))
        {
            StartCoroutine(Shoot());
        }

        inputVector = new Vector2(horizontal, 0f);
    }

    /// <summary>
    /// Handles transform and/or rigidbody operations needed to move character.
    /// </summary>
    void Movement()
    {
        if (isGrounded && isJumping)
        {
            m_Rigidbody.AddForce(new Vector2(0f, m_JumpForce), ForceMode2D.Impulse);
            isJumping = false;
        }

        //transform.position += new Vector3(inputVector.x, 0f, 0f) * m_Speed * Time.deltaTime;
        transform.Translate(inputVector * m_Speed * Time.deltaTime);
        //m_Rigidbody.velocity = Vector2.SmoothDamp(m_Rigidbody.velocity, inputVector, ref smoothMoveVelocity, 0.15f);

        FaceDirection();
    }

    /// <summary>
    /// Adjusts player sprite's direction by rescaling.
    /// </summary>
    void FaceDirection()
    {
        if (isMoving)
        {
            transform.localScale = new Vector3(inputVector.x * initialScale.x, transform.localScale.y, 0);
        }
    }

    /// <summary>
    /// Performs the logic and returns a boolean - true if the player is on the ground, false otherwise.
    /// </summary>
    private void CheckGrounded()
    {
        if (m_GroundCheck.CheckCollision(GROUND_CHECK_RADIUS, m_WhatIsGround, gameObject))
        {
            isGrounded = true;
            return;
        }
        isGrounded = false;
    }

    private IEnumerator Shoot()
    {
        isShooting = true;
        yield return new WaitForSeconds(0.26f);
        GameObject projectile = Instantiate(m_Projectile, transform.position, m_Projectile.transform.rotation) as GameObject;
        projectile.GetComponent<Projectile>().Shoot(directionVector);
    }

    private void Animate()
    {
        m_Animator.SetBool("isWalking", isMoving);
        m_Animator.SetBool("isShooting", isShooting);
        isShooting = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "enemy")
        {
            Vector2 colliderVector = (transform.position - collision.collider.transform.position).normalized;
            m_Rigidbody.AddForce(colliderVector * m_JumpForce/2, ForceMode2D.Impulse);
            lifeCounter -= 1;
            isHurt = true;
            GameOver();
        }
    }

    private void GameOver()
    {
        if (lifeCounter <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}