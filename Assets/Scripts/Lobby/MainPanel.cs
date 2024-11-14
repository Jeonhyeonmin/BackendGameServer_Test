using UnityEngine;

public class MainPanel : MonoBehaviour
{
    public void BtnClickGameStart()
    {
        Utils.LoadScene(SceneNames.Game);
    }
}
