using System.Collections;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    private BlockController _blockController;

    private void Awake ()
    {
        _blockController = GetComponent<BlockController>();
    }

    private IEnumerator Start ()
    {
        _blockController.BlockStart();

        yield break;
    }
}