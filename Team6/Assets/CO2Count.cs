using UnityEngine;

public class CO2Count : MonoBehaviour
{
    public static int InstanceCount = 0;

    void OnEnable()
    {
        // Scene 上に出現したらカウントアップ
        InstanceCount++;
    }

    void OnDisable()
    {
        // Scene 上から消えたらカウントダウン
        InstanceCount--;
    }
}
