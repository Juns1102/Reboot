using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {
    private static SceneChanger instance;

    public static SceneChanger Instance{
        get{
            if(null == instance){
                return null;
            }
            return instance;
        }
    }

    private void Awake() {
        if(instance == null){
            instance = this;
            if(transform.parent != null && transform.root != null){
                DontDestroyOnLoad(this.transform.root.gameObject);
            }
            else{
                DontDestroyOnLoad(this.gameObject);
            }
        }
        else{
            Destroy(this.gameObject);
        }
    }
    public void ChangeMap1(){
        SceneManager.LoadScene(0);
    }
    public void ChangeMap2(){
        SceneManager.LoadScene(1);
    }
    public void ChangeMap3(){
        SceneManager.LoadScene(2);
    }
    public void ChangeMap4(){
        SceneManager.LoadScene(3);
    }
}
