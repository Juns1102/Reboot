using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private Vector2 targetPos;
    private Animator anim;
    private Rigidbody2D body;
    private SpriteRenderer spriter;
    public bool moveStop;

    public AudioSource audioSource;
    public AudioClip walkSound;
    public AudioClip attackSound;
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        moveStop = true;
        anim.SetBool("Walk", false);
    }

    private void FixedUpdate() {
        Debug.DrawRay(transform.position, Vector2.right * 1f, Color.red);
        Debug.DrawRay(transform.position, Vector2.left * 1f, Color.red);
        Debug.DrawRay(transform.position, Vector2.up * 1f, Color.red);
        Debug.DrawRay(transform.position, Vector2.down * 1f, Color.red);
    }

    private void OnMove(InputValue value) {
        if (InventoryManager.Instance.CheckCapacity() && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") &&
         !UIManager.Instance.activeMap && !UIManager.Instance.startMenu && !UIManager.Instance.activeMenu && GameManager.Instance.playerTurn) {
            if (Mathf.Abs(value.Get<Vector2>().x) == 1 || 
                Mathf.Abs(value.Get<Vector2>().y) == 1) {
                if(value.Get<Vector2>().x == 1){
                    spriter.flipX = false;
                }
                else if(value.Get<Vector2>().x == -1){
                    spriter.flipX = true;
                }
                RaycastHit2D hit = Physics2D.Raycast(body.position, value.Get<Vector2>(), 1f, LayerMask.GetMask("Platform", "Enemy"));
                if (moveStop && hit.collider == null) {
                    anim.SetBool("Walk", true);
                    if(SceneManager.GetActiveScene().name != "Lobby"){
                        GameManager.Instance.moveCount--;
                        UIManager.Instance.SetMoveCount();
                    }
                    moveStop = false;
                    GameManager.Instance.SetTp(moveStop);
                    targetPos = value.Get<Vector2>();
                    transform.DOMove((Vector2)transform.position + targetPos, 0.7f).SetEase(Ease.OutQuad).OnComplete(() => 
                    {SetMove(); GameManager.Instance.SetTp(moveStop); GameManager.Instance.CheckEnemies(); 
                    UIManager.Instance.ActiveSkill(); if(GameManager.Instance.moveCount <= 0)anim.SetTrigger("Die");});
                }
            }
        }
    }

    public void SetMove(){
        anim.SetBool("Walk", false);
        if(!GameManager.Instance.enemyZone){
            moveStop = true;
        }
        GameManager.Instance.activeSkill = true;
        if(!GameManager.Instance.enemyZone){
            GameManager.Instance.SkillCoolDown();
        }
        UIManager.Instance.SetCoolTime();
    }
    public void PlayWalkSound()
    {
        audioSource.PlayOneShot(walkSound);
    }


    // ���� �Ҹ�
    public void PlayAttackSound()
    {
        audioSource.PlayOneShot(attackSound);
    }

    public void Die(){
        UIManager.Instance.Menu();
    }
}
