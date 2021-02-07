using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum DirectionType
{
    Left,
    Right,
    Up,
    Down
}

public class PlayerController : MonoBehaviour
{
    private const float MOVE_STEP_VALUE = 0.2f;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private float speed;
    [SerializeField] private Head head;
    [SerializeField] private Vector2 direction;
    [SerializeField] private float force;

    private DirectionType directionType;
    private Animator animator;
    private SpriteRenderer renderer;
    private Rigidbody2D rigidbody;
    private Vector2 dir;

    private int health;

    [SerializeField] private bool isHead;
    [SerializeField] private bool isHeadlessMove;
    [SerializeField] private bool isIdle;
    [SerializeField] private bool isMove;

    private bool isHeadless;

    [SerializeField] private bool isHeadAnimationCompleted;

    void Start()
    {
        animator = this.GetComponent<Animator>();
        renderer = this.GetComponent<SpriteRenderer>();
        rigidbody = this.GetComponent<Rigidbody2D>();
        isMove = false;
        isHead = false;
        isIdle = true;
        isHeadAnimationCompleted = true;
    }

    private void ResetAllTrigger()
    {
        animator.ResetTrigger("Move");
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Head");
        animator.ResetTrigger("HeadToBack");
        animator.ResetTrigger("Prepare");
    }

    private void Update()
    {
        direction = (mainCamera.ScreenToWorldPoint(Input.mousePosition) - this.transform.position);
        direction.Normalize();
        renderer.flipX = direction.x < 0;

        if (Input.anyKey)
        {
            bool isMoveInputDetected = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D));

            if (!isHeadAnimationCompleted && isMoveInputDetected && Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(0))
            {
                isHead = true;
                ResetAllTrigger();
                animator.SetTrigger("Head");

            }
            else if (isHeadless && (isMoveInputDetected && Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(1)))
            {
                ResetAllTrigger();
                animator.SetTrigger("Prepare");
                head.GetHeadToMe(GetHeadToMeCompleted);
            }
            else if (Input.GetMouseButtonUp(1))
            {
                ResetAllTrigger();
            }
            else
            {
                if (Input.GetKey(KeyCode.W))
                    SetDirection(new Vector2(0, 1));

                if (Input.GetKey(KeyCode.A))
                    SetDirection(new Vector2(-1, 0));

                if (Input.GetKey(KeyCode.S))
                    SetDirection(new Vector2(0, -1));

                if (Input.GetKey(KeyCode.D))
                    SetDirection(new Vector2(1, 0));

                rigidbody.MovePosition(new Vector2(this.transform.position.x, this.transform.position.y) + (dir.normalized * speed));
                dir = Vector2.zero;
            }
        }
    }

    private void GetHeadToMeCompleted()
    {
        isHeadless = false;
        ResetAllTrigger();
        animator.SetTrigger("HeadToBack");
    }

    private void LateUpdate()
    {
        if (!Input.anyKey)
        {
            if (!isHead && isHeadAnimationCompleted)
            {
                ResetAllTrigger();
                animator.SetTrigger("Idle");
            }
        }
    }

    private void SetDirection(Vector2 dir)
    {
        this.dir += dir;
        bool xxx = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("chaHead");
        bool yyy = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("chaPrepareToRewind");
        if (!xxx && !yyy)
        {
            ResetAllTrigger();
            animator.SetTrigger("Move");
        }
    }

    private IEnumerator _HeadAttackKey()
    {
        isHeadless = true;
        isHeadAnimationCompleted = false;
        head.Force(direction, force);
        yield return new WaitForEndOfFrame();
    }

    private IEnumerator _HeadFinishKey()
    {
        isHead = false;
        isHeadlessMove = true;
        isHeadAnimationCompleted = true;
        ResetAllTrigger();
        animator.SetTrigger("Idle");
        yield return new WaitForSeconds(.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("BossBullet"))
        {
            health++;
            mainCamera.DOShakePosition(.1f, .2f, 4);
            renderer.DOBlendableColor(new Color(.5f, .5f, .5f), .2f).OnComplete(() => renderer.DOBlendableColor(Color.white, .1f));
        }
    }
}
