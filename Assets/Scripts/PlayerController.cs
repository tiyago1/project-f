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
    [SerializeField] private bool isIdle;
    [SerializeField] private bool isMove;

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
    public Projectile pr;
    bool ix;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ix = !ix;
            pr.enabled =ix;
        }

        direction = (mainCamera.ScreenToWorldPoint(Input.mousePosition) - this.transform.position);
        direction.Normalize();

        renderer.flipX = direction.x < 0;

        if (Input.anyKey)
        {
            bool isMoveInputDetected = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D));

            if (isHeadAnimationCompleted && 
                !isHead &&
                (isMoveInputDetected && Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(0))
                )
            {
                isHeadAnimationCompleted = false;
                SetupAnimationBool(false, false, true);
            }
            else
            {
                SetupAnimationBool(isMove: true, isIdle: false, isHead: false);
                if (Input.GetKey(KeyCode.W))
                    dir += new Vector2(0, 1);

                if (Input.GetKey(KeyCode.A))
                    dir -= new Vector2(1, 0);

                if (Input.GetKey(KeyCode.S))
                    dir -= new Vector2(0, 1);

                if (Input.GetKey(KeyCode.D))
                    dir += new Vector2(1, 0);

                rigidbody.MovePosition(new Vector2(this.transform.position.x, this.transform.position.y) + (dir.normalized * speed));
                dir = Vector2.zero;
            }
        }
        else
        {
            isMove = false;
            if (!isMove && !isHead)
            {
                SetupAnimationBool(isMove: false, isIdle: true, isHead: false);
            }
        }
    }

    private void SetupAnimationBool(bool isMove, bool isIdle, bool isHead)
    {
        if (isMove && !isHeadAnimationCompleted)
        {
            return;
        }

        this.isMove = isMove;
        this.isIdle = isIdle;
        this.isHead = isHead;

        animator.SetBool("Move", isMove);
        animator.SetBool("Idle", isIdle);
        animator.SetBool("Head", isHead);
    }

    private IEnumerator _HeadAttackKey()
    {
        StopCoroutine(_HeadAttackKey());
        isHeadAnimationCompleted = false;
        StopCoroutine(_HeadAttackKey());
        head.Force(direction, force);
        yield return new WaitForEndOfFrame();
    }

    private IEnumerator _HeadFinishKey()
    {
        StopCoroutine(_HeadFinishKey());
        yield return new WaitForSeconds(.5f);
        isHead = false;
        head.ResetSetup();
        isHeadAnimationCompleted = true;
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
