using UnityEngine;
using System.Collections;

public class Monohelper : MonoBehaviour
{
    public PlayerController player
    {
        get
        {
            if (_player == null)
                _player = FindObjectOfType<PlayerController>();
            return _player;
        }
    }
    private static PlayerController _player;
}
