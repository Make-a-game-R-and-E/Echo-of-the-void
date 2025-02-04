using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ObjectMatchForm : MonoBehaviour
{
    [SerializeField] int matchId;

    public int Get_ID()
    {
        return matchId;
    }

}
