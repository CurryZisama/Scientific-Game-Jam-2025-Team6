using UnityEngine;
using UnityEngine.SceneManagement;
public class FADEScript : MonoBehaviour
{
    public string sceneName = "GameScene";
    public GameObject targetObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAnimation()
    {
        if (targetObject != null) 
        {
            targetObject.SetActive(!targetObject.activeSelf);
        }
        else
        {
            Debug.LogError("targetObjectがインスペクターで設定されていません！");
        }
    
    }

    public void PlayFade()
    {
        SceneManager.LoadScene(sceneName);
    }
    public void QuitApplication()
    {
        Application.Quit();
    }
}
