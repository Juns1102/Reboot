using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private Vector2 targetPos;
    private bool moveStop;

    Rigidbody2D body;
    //Animator anim;
    //SpriteRenderer spriter;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        moveStop = true;
    }

    private void OnMove(InputValue value) {
        if (Mathf.Abs(value.Get<Vector2>().x) == 1 || 
            Mathf.Abs(value.Get<Vector2>().y) == 1) {
            if (moveStop) {
                moveStop = false;
                targetPos = value.Get<Vector2>();
                transform.DOMove((Vector2)transform.position + targetPos, 1.0f).SetEase(Ease.InOutQuint).OnComplete(() => moveStop = true);
            }
        }
    }
}
