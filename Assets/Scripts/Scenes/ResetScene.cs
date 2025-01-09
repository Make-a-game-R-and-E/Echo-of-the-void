using UnityEngine;

public class ResetScene : MonoBehaviour
{
    void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneLoader.ReloadCurrentScene();
        }
    }
}
