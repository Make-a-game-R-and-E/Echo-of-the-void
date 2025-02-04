using UnityEngine;

public class CellPuzzleManager : MonoBehaviour
{
    [Header("List of sockets in this puzzle")]
    [SerializeField] private CellSocket[] sockets;

    [Header("Door to open when puzzle is solved")]
    [SerializeField] private GameObject finalDoor;

    private bool puzzleComplete = false;

    private void Update()
    {
        if (!puzzleComplete)
        {
            CheckPuzzleComplete();
        }
    }

    private void CheckPuzzleComplete()
    {
        // If *any* socket is missing the correct cell, puzzle is not complete
        foreach (var socket in sockets)
        {
            if (socket.placedCell == null)
            {
                return; // A socket is not yet filled
            }
        }

        // If we reach here, it means ALL sockets have a placed cell (of the correct color).
        puzzleComplete = true;
        Debug.Log("Puzzle solved! Opening final door.");

        finalDoor.SetActive(false);
    }
}
