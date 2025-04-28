using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HitCollider : NetworkBehaviour
{
    public int damage;

    [Rpc(SendTo.Everyone)]
    public void SetDamageServerRpc(int newDamage)
    {
        damage = newDamage;
    }
}
