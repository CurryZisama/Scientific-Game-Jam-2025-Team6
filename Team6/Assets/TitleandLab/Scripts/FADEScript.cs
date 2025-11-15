using UnityEngine;

public class FADEScript : MonoBehaviour
{
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
}
