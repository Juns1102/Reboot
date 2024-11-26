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
    //SpriteRenderer spriter;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        moveStop = true;
    }

    private void FixedUpdate(){
        Debug.DrawRay(body.position, Vector2. right, new Color(1, 0, 0));
    }

    private void OnMove(InputValue value) {
        if (InventoryManager.Instance.CheckCapacity()) {
            if (Mathf.Abs(value.Get<Vector2>().x) == 1 || 
                Mathf.Abs(value.Get<Vector2>().y) == 1) {
                anim.SetFloat("Speed", 1);
                RaycastHit2D hit = Physics2D.Raycast(body.position, value.Get<Vector2>(), 1, LayerMask.GetMask("Platform"));
                if (moveStop && hit.collider == null) {
                    moveStop = false;
                    targetPos = value.Get<Vector2>();
                    transform.DOMove((Vector2)transform.position + targetPos, 0.7f).SetEase(Ease.OutQuad).OnComplete(() => SetMove());
                }
            }
        }
    }

    private void SetMove(){
        moveStop = true;
        anim.SetFloat("Speed", 0);
    }
}
