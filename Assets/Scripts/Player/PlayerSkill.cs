using System.ComponentModel.Design;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    Animator anim;
    PlayerMove pm;

    private void Start() {
        anim = GetComponent<Animator>();
        pm = GetComponent<PlayerMove>();
    }

    private void OnAttack(){
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && GameManager.Instance.activeSkill){
            if(GameManager.Instance.activeSkill){
                GameManager.Instance.Skill1Set();
                anim.SetTrigger("Attack");
                GameManager.Instance.Skill1Set();
            }
        }
    }

    private void OnTeleport(){
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && GameManager.Instance.activeSkill){
            if(GameManager.Instance.activeSkill){
                GameManager.Instance.Skill2Set();
                anim.SetTrigger("Attack");
                GameManager.Instance.Skill2Set();
            }
        }
    }

    private void OnTurnEnd(){
        if(GameManager.Instance.playerTurn){
            pm.moveStop = true;
            GameManager.Instance.activeSkill = true;
            //GameManager.Instance.playerTurn = false;
        }
    }
}
