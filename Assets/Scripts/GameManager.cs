using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _hasGasMask = false;

    private static GameManager _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static GameManager GetInstance()
    {
        return _instance;
    }

    public bool CheckHasGasMask()
    {
        return _hasGasMask;
    }

    public void PickUpGasMask()
    {
        _hasGasMask = true;
    }
}
