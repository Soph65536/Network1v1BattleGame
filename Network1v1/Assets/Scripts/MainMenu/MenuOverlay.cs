using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOverlay : MonoBehaviour
{
    public void CloseMenuOverlay()
    {
        gameObject.SetActive(false);
    }
}
