using UnityEngine;

public class LoadScene : MonoBehaviour
{
    [SerializeField] public string sceneName;
    public void loadScene(string sceneName)
    {
        SceneLoader.LoadSceneByName(sceneName);
    }
}
