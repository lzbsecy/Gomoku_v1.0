using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserItem : MonoBehaviour
{
    [SerializeField]
    private Text playerName;

    public void SetPlayerInfo(string name)
    {
        playerName.text = name;
    }
}
