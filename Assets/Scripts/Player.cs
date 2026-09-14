using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speedX;
    public float moveInputX;
    public float speedY;
    public float moveInputY;

    public float jumpForce;

    public LayerMask solidMask;
    public Transform groundCheck;
    public bool isOnGround;
    public bool isOnPlatform;
    public Transform ceilingCheck;
    public bool touchingCeiling;
    
    public GameObject attackObject;
    private GameObject _spawnedAttackObject;
    public Vector3 attackPosition;
    public float attackTimerDefault;
    private float _attackTimer = 0f;

    private bool _touchingLadder = false;
    public static bool usingLadder = false;
    public static bool dead = false;
    public static bool winLevel = false;

    public float fallingTime = 0;

    public AudioClip clipLose;
    public AudioClip clipWin;
    public AudioClip clipShine;
    public AudioClip clipPause;
    public AudioClip clipJump;
    public AudioClip clipScoreTick;

    public static int score = 0;
    public static int lives = 3;
    public static int stage = 1;
    public static int stageDisplay = 1;
    public static int time = 99;
    public static int givePointsChain = 1;
    public static int givePointsChainLinear = 0;
    public static int scoreForExtraLife = 8000;
    public static float sceneTime = 0;

    private RaycastHit2D _rayHit;
    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private AudioSource _audioSource;

    void Start()
    {
        Time.timeScale = 1.0f;

        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();

        _touchingLadder = false;
        usingLadder = false;
        dead = false;
        winLevel = false;
        fallingTime = 0;
        time = 99;
        sceneTime = 0;

        StartCoroutine(Timer());
    }

    void Update()
    {
        // Ground check.
        _rayHit = Physics2D.BoxCast(groundCheck.transform.position, new Vector2(0.3f, 0.2f), 0, -transform.up, -0.5f, solidMask);

        if (_rayHit.collider == null)
        {
            isOnGround = false;
            isOnPlatform = false;
        }
        else
        {
            isOnGround = true;

            if (_rayHit.collider.gameObject.tag == "Platform" && _rb.velocity.y >= 0)
            {
                if (_rayHit.collider.gameObject.GetComponent<Platform>().directionY < 0)
                {
                    isOnPlatform = true;
                }
                else
                {
                    isOnPlatform = false;
                }
            }
            else
            {
                isOnPlatform = false;
            }
        }

        if (isOnGround)
        {
            _animator.SetBool("MidAir", false);
            givePointsChain = 1;
            givePointsChainLinear = 0;

            if (fallingTime > 1 && dead == false)
            {
                Die();
            }
            else
            {
                fallingTime = 0;
            }
        }
        else
        {
            _animator.SetBool("MidAir", true);
        }

        // Ceiling check.
        _rayHit = Physics2D.BoxCast(ceilingCheck.transform.position, new Vector2(0.2f, 0.2f), 0, transform.up, 0.25f, solidMask);

        if (_rayHit.collider == null)
        {
            touchingCeiling = false;
        }
        else
        {
            touchingCeiling = true;
        }

        // Walk.
        if (!usingLadder)
        {
            if (isOnGround && _attackTimer <= 0 && dead == false)
            {
                moveInputX = Input.GetAxisRaw("Horizontal") * speedX * Time.deltaTime;
            }

            if (!isOnGround)
            {
                if (moveInputX > 0)
                {
                    moveInputX = speedX * Time.deltaTime;
                }
                else if (moveInputX < 0)
                {
                    moveInputX = -speedX * Time.deltaTime;
                }
            }
        }

        // Ladder.
        if (Input.GetAxisRaw("Vertical") != 0 && _touchingLadder && _attackTimer <= 0 && isOnGround && dead == false && winLevel == false)
        {
            usingLadder = true;
            _rb.velocity = new Vector2(0, 0);
            _animator.Play("PlayerClimbIdle");
        }

        if (usingLadder && dead == false)
        {
            _rb.gravityScale = 0.0f;
            fallingTime = 0;
            moveInputX = Input.GetAxisRaw("Horizontal") * (speedX / 2) * Time.deltaTime;
            moveInputY = Input.GetAxisRaw("Vertical") * speedY * Time.deltaTime;

            _animator.SetBool("Climb", true);
            _animator.SetBool("ClimbMoving", moveInputX != 0 || moveInputY != 0);

            if (((moveInputY > 0 && touchingCeiling == false) || (moveInputY < 0 && isOnGround == false)) && Time.timeScale != 0.0f)
            {
                transform.Translate(moveInputX, moveInputY, 0);
            }
            if (moveInputY < 0 && isOnGround)
            {
                usingLadder = false;
                moveInputY = 0;
            }
        }
        else
        {
            _animator.SetBool("Climb", false);
            _animator.SetBool("ClimbMoving", false);

            if (isOnPlatform == false)
            {
                _rb.gravityScale = 0.5f;
            }
            else
            {
                _rb.gravityScale = 2f;
            }
        }

        // Wall check.
        if (moveInputX > 0)
        {
            _rayHit = Physics2D.BoxCast(transform.position, new Vector2(0.1f, 0.1f), 0, transform.right, 0.2f, solidMask);
            if (_rayHit.collider != null)
            {
                moveInputX = 0;
            }
        }
        if (moveInputX < 0)
        {
            _rayHit = Physics2D.BoxCast(transform.position, new Vector2(0.1f, 0.1f), 0, -transform.right, 0.2f, solidMask);
            if (_rayHit.collider != null)
            {
                moveInputX = 0;
            }
        }

        if (dead == false && winLevel == false && Time.timeScale != 0.0f)
        {
            transform.Translate(moveInputX, 0, 0);
            _animator.SetFloat("Walking", Mathf.Clamp01(Mathf.Abs(moveInputX)));

            if (moveInputX > 0)
            {
                _spriteRenderer.flipX = false;
            }
            else if (moveInputX < 0)
            {
                _spriteRenderer.flipX = true;
            }
        }

        // Jump.
        if (Input.GetKeyDown(KeyCode.X) && isOnGround && _attackTimer <= 0 && usingLadder == false && dead == false && winLevel == false)
        {
            _rb.velocity = new Vector2(0, jumpForce);

            /*
            if (GlobalScript.debug)
            {
                Debug.Log("Jump");
            }*/

            _audioSource.PlayOneShot(clipJump);
        }

        // Attack.
        if (Input.GetKeyDown(KeyCode.Z) && isOnGround && _attackTimer <= 0 && usingLadder == false && dead == false && winLevel == false)
        {
            _spawnedAttackObject = Instantiate(attackObject, transform.position + new Vector3(attackPosition.x * FlipMultiplier(), attackPosition.y, 0), transform.rotation);
            _spawnedAttackObject.GetComponent<SpriteRenderer>().flipX = _spriteRenderer.flipX;
            _spawnedAttackObject.transform.parent = transform;
            _attackTimer = 0.75f;
            _animator.Play("PlayerIdle");

            _audioSource.PlayOneShot(clipShine);
        }

        if (_attackTimer > 0)
        {
            _attackTimer -= Time.deltaTime;
            moveInputX = 0;
        }
        if (_attackTimer < 0)
        {
            _attackTimer = 0;
        }

        // Score.
        if (score < 0)
        {
            score = 0;
        }
        if (score > 99999999)
        {
            score = 99999999;
        }

        // Extra life.
        if (score >= scoreForExtraLife)
        {
            lives++;
            scoreForExtraLife += 8000;
        }
        if (lives > 255)
        {
            lives = 255;
        }

        // Fall damage.
        if (_rb.velocity.y < 0)
        {
            fallingTime += Time.deltaTime;
        }

        // Bottomless pit.
        if (transform.position.y < -8)
        {
            Die();
        }

        // Pause.
        if (Input.GetKeyDown(KeyCode.Return) && dead == false && winLevel == false && sceneTime > 1f)
        {
            _audioSource.PlayOneShot(clipPause);

            if (Time.timeScale != 0.0f)
            {
                Time.timeScale = 0.0f;
            }
            else
            {
                Time.timeScale = 1.0f;
            }
        }

        if (Time.timeScale == 0.0f)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.LeftShift))
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
        else
        {
            // Scene time.
            sceneTime += Time.deltaTime;
        }

        // Debug.
        if (GlobalScript.debug)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                stageDisplay--;
                stage--;
                SceneManager.LoadScene("Stage" + stage);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                stageDisplay++;
                stage++;
                SceneManager.LoadScene("Stage" + stage);
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                GlobalScript.difficulty--;
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                GlobalScript.difficulty++;
            }
        }
    }

    private int FlipMultiplier()
    {
        if (_spriteRenderer.flipX == false)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }

    public void Die()
    {
        if (dead == false)
        {
            Time.timeScale = 1.0f;

            usingLadder = false;
            _touchingLadder = false;
            dead = true;

            if (_spawnedAttackObject != null)
            {
                Destroy(_spawnedAttackObject);
            }

            _audioSource.PlayOneShot(clipLose);

            _animator.Play("PlayerDead");

            lives--;

            DestroyAllObstacles();
            StartCoroutine(GlobalScript.Instance.MajorDeath(gameObject, _spriteRenderer, _rb, false));
            StartCoroutine(AfterDeath());
        }
    }

    private IEnumerator AfterDeath()
    {
        yield return new WaitForSeconds(4);

        SaveScore();

        if (lives > 0)
        {
            SceneManager.LoadScene("Stage" + stage.ToString());
        }
        else
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    public IEnumerator Win()
    {
        Time.timeScale = 1.0f;

        winLevel = true;
        DestroyAllObstacles();
        SaveScore();

        _audioSource.PlayOneShot(clipWin);

        yield return new WaitForSeconds(3);

        // Subtract timer to add score.
        while (time > 0)
        {
            time--;
            score += 10;
            _audioSource.PlayOneShot(clipScoreTick);

            yield return new WaitForSeconds(0.06f);
        }

        if (time < 0)
        {
            time = 0;
        }

        SaveScore();

        yield return new WaitForSeconds(2);

        // Next stage.
        stage++;
        stageDisplay++;

        if (GlobalScript.gameIsInfinite == false)
        {
            if (stage <= 5)
            {
                SceneManager.LoadScene("Stage" + stage.ToString());
            }
            else
            {
                SceneManager.LoadScene("Ending");
            }
        }
        else
        {
            if (stage > 5)
            {
                stage = 1;

                if (GlobalScript.difficulty < 9)
                {
                    GlobalScript.difficulty++;
                }
                if (GlobalScript.difficulty > 9)
                {
                    GlobalScript.difficulty = 9;
                }
            }

            SceneManager.LoadScene("Stage" + stage.ToString());
        }
    }

    private void DestroyAllObstacles()
    {
        GameObject[] everyObstacle = GameObject.FindGameObjectsWithTag("Kill");

        foreach (GameObject obstacle in everyObstacle)
        {
            Destroy(obstacle);
        }
    }

    private void SaveScore()
    {
        if (GlobalScript.gameIsInfinite == false)
        {
            if (score > GlobalScript.highScoreNormal)
            {
                GlobalScript.highScoreNormal = score;
            }
        }
        else
        {
            if (score > GlobalScript.highScoreInfinite)
            {
                GlobalScript.highScoreInfinite = score;
            }
        }

        PlayerPrefs.SetInt("highScore_Normal", GlobalScript.highScoreNormal);
        PlayerPrefs.SetInt("highScore_Infinite", GlobalScript.highScoreInfinite);
        PlayerPrefs.Save();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            _touchingLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            _touchingLadder = false;
            usingLadder = false;
        }
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);
        time--;

        if (dead == false && winLevel == false)
        {
            if (time <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(Timer());
            }
        }
    }
}
