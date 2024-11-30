using System.ComponentModel.Design;
using UnityEngine;
using DG.Tweening;

public class PlayerSkill : MonoBehaviour
{
    Animator anim;
    PlayerMove pm;
    CircleCollider2D ccd;
    Vector2 tpPos;
    GameObject tpStone;
    public GameObject tpStonePrefab;

    private void Start() {
        anim = GetComponent<Animator>();
        pm = GetComponent<PlayerMove>();
        ccd = GetComponent<CircleCollider2D>();
    }

    private void OnAttack(){
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && GameManager.Instance.activeSkill){
            if(GameManager.Instance.skill1CoolTime == 0){
                GameManager.Instance.Skill1Set();
                UIManager.Instance.SetCoolTime();
                anim.SetTrigger("Attack");
            }
        }
    }

    private void OnTeleport(){
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && GameManager.Instance.activeSkill){
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
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && GameManager.Instance.playerTurn){
            pm.moveStop = true;
            GameManager.Instance.activeSkill = true;
            GameManager.Instance.SkillCoolDown();
            UIManager.Instance.SetCoolTime();
            //GameManager.Instance.playerTurn = false;
        }
    }
}
