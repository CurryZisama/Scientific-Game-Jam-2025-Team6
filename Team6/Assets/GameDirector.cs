using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{

    public int GameOverCount = 20;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(CO2Count.InstanceCount >= GameOverCount)
        {
            SceneManager.LoadScene("EndScene");
        }
    }
}
