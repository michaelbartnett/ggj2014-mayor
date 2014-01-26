using UnityEngine;
using System.Collections;

public class Mayor : MonoBehaviour
{
    public PartyGoer[] guards;

    void OnPlayerActivate(Player player)
    {
        MayorMiniGame.Instance.BeginGame();
    }
}