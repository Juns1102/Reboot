using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private Vector2 targetPos;
    private bool moveStop;
    private Animator anim;
    private Rigidbody2D body;
    //Animator anim;
    SpriteRenderer spriter;
    
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        moveStop = true;
    }

    private void FixedUpdate() {
        Debug.DrawRay(transform.position, Vector2.right * 1f, Color.red);
        Debug.DrawRay(transform.position, Vector2.left * 1f, Color.red);
        Debug.DrawRay(transform.position, Vector2.up * 1f, Color.red);
        Debug.DrawRay(transform.position, Vector2.down * 1f, Color.red);
    }

    private void OnMove(InputValue value) {
        if (InventoryManager.Instance.CheckCapacity() && anim.GetCurrentAnimatorStateInfo(0).IsName("Walk") != true) {
            if (Mathf.Abs(value.Get<Vector2>().x) == 1 || 
                Mathf.Abs(value.Get<Vector2>().y) == 1) {
                anim.SetBool("Walk", true);
                if(value.Get<Vector2>().x == 1){
                    spriter.flipX = false;
                }
                else if(value.Get<Vector2>().x == -1){
                    spriter.flipX = true;
                }
                RaycastHit2D hit = Physics2D.Raycast(body.position, value.Get<Vector2>(), 1f, LayerMask.GetMask("Platform"));
                if (moveStop && hit.collider == null) {
                    moveStop = false;
                    GameManager.Instance.SetTp(moveStop);
                    targetPos = value.Get<Vector2>();
                    transform.DOMove((Vector2)transform.position + targetPos, 0.7f).SetEase(Ease.OutQuad).OnComplete(() => {moveStop = true; GameManager.Instance.SetTp(moveStop);});
                }
            }
        }
    }

    private void SetMove(){
        anim.SetBool("Walk", false);
    }
}
