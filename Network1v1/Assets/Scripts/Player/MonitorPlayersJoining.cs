using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MonitorPlayersJoining : NetworkBehaviour
{
    //players connected variables
    private NetworkVariable<bool> hasPlayer1 = new NetworkVariable<bool>(false, readPerm: NetworkVariableReadPermission.Everyone, writePerm: NetworkVariableWritePermission.Owner);
    private NetworkVariable<bool> hasPlayer2 = new NetworkVariable<bool>(false, readPerm: NetworkVariableReadPermission.Everyone, writePerm: NetworkVariableWritePermission.Owner);
    public bool hasPlayer1RO { get { return hasPlayer1.Value; } }
    public bool hasPlayer2RO { get { return hasPlayer2.Value; } }

    public void EnterAsHost()
    {
        if (IsServer)
        {
            Debug.Log("Has Player 1");
            hasPlayer1.Value = true;
        }
    }

    public void EnterAsClient()
    {
        if (IsServer)
        {
            Debug.Log("HasPlayer2");
            hasPlayer2.Value = true;
        }
    }
}
