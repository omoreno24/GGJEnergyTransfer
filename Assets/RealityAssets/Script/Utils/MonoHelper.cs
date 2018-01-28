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

    public CameeraShake shaker
    {
        get
        {
            if (_shaker == null)
                _shaker = FindObjectOfType<CameeraShake>();
            return _shaker;
        }
    }
    private static CameeraShake _shaker;

    public UIManager uiManager
    {
        get
        {
            if (_uiManager == null)
                _uiManager = FindObjectOfType<UIManager>();
            return _uiManager;
        }
    }
    private static UIManager _uiManager;

}
