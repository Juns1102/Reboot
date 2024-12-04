using System.ComponentModel.Design;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PlayerSkill : MonoBehaviour
{
    Animator anim;
    PlayerMove pm;
    CircleCollider2D ccd;
    Vector2 tpPos;
    GameObject tpStone;
    public GameObject tpStonePrefab;
    public BoxCollider2D box;
    public AudioSource audioSource;
    public AudioClip Teleport;

    private void Start() {
        anim = GetComponent<Animator>();
        pm = GetComponent<PlayerMove>();
        ccd = GetComponent<CircleCollider2D>();
    }

    private void OnAttack(){
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && GameManager.Instance.activeSkill && !UIManager.Instance.startMenu && 
        !UIManager.Instance.activeMenu && SceneManager.GetActiveScene().name != "Lobby"){
            if(GameManager.Instance.skill1CoolTime == 0){
                GameManager.Instance.Skill1Set();
                UIManager.Instance.SetCoolTime();
                anim.SetTrigger("Attack");
            }
        }
    }

    private void OnTeleport(){
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && GameManager.Instance.activeSkill && !UIManager.Instance.startMenu &&
         !UIManager.Instance.activeMenu && SceneManager.GetActiveScene().name != "Lobby"){
            if(GameManager.Instance.skill2CoolTime == 0){
                if(!GameManager.Instance.equipTp){
                    tpStone = Instantiate(tpStonePrefab, gameObject.transform);
                    tpStone.transform.SetParent(null);
                    GameManager.Instance.equipTp = true;
                    UIManager.Instance.EquipTp();
                    tpPos = new Vector2(transform.position.x, transform.position.y);
                }
                else{
                    TpStep1();
                    UIManager.Instance.UseTp();
                    GameManager.Instance.Skill2Set();
                    UIManager.Instance.SetCoolTime();
                    GameManager.Instance.equipTp = false;
                    audioSource.PlayOneShot(Teleport);

                }
            }
        }
    }

    private void TpStep1(){
        ccd.enabled = false;
        transform.DOMove((Vector2)transform.position + new Vector2(0, 0.5f), 0.5f).SetEase(Ease.OutQuad).OnComplete(() => TpStep2());
    }

    private void TpStep2(){
        transform.DOMove(new Vector2(tpPos.x, tpPos.y + 0.5f), 0.5f).SetEase(Ease.OutQuad).OnComplete(() => TpStep3());

    }

    private void TpStep3(){
        transform.DOMove((Vector2)transform.position + new Vector2(0, -0.5f), 0.5f).SetEase(Ease.OutQuad).OnComplete(() => {ccd.enabled = true; Destroy(tpStone);});
    }

    private void OnTurnEnd(){
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && GameManager.Instance.playerTurn && SceneManager.GetActiveScene().name != "Lobby" && GameManager.Instance.enemyZone){
            pm.moveStop = true;
            GameManager.Instance.activeSkill = true;
            GameManager.Instance.SkillCoolDown();
            UIManager.Instance.SetCoolTime();
            GameManager.Instance.EnemiesMove();
            //GameManager.Instance.playerTurn = false;
        }
    }

    private void SetAttack(){
        box.gameObject.SetActive(true);
    }

    private void EndAttack(){
        box.gameObject.SetActive(false);
    }
}
