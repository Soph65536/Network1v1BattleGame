using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAttack : NetworkBehaviour
{
    public NetworkVariable<bool> canAttack = new NetworkVariable<bool>(
        value: true,
        readPerm: NetworkVariableReadPermission.Everyone,
        writePerm: NetworkVariableWritePermission.Owner
        );

    private Animator animator;

    public int damageMultipler;

    [SerializeField] private bool lightUpperRunning;
    [SerializeField] private int lightUpperComboNumber;
    const int maxLightUpperCombo = 3;

    private void Awake()
    {
        enabled = false; //this will only be enabled by the owning player

        canAttack.Value = true;

        //get animator
        animator = GetComponentInChildren<Animator>();

        damageMultipler = 1;

        lightUpperRunning = false;
        lightUpperComboNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner) return;

        if (canAttack.Value)
        {
            if (Input.GetMouseButtonDown(0) && !lightUpperRunning)
            {
                StartCoroutine("WaitForLightUpperCombo");
                LightUpperAttackServerRpc(lightUpperComboNumber);
            }
        }
    }

    private IEnumerator WaitForLightUpperCombo()
    {
        lightUpperRunning = true;

        lightUpperComboNumber++;

        if(lightUpperComboNumber > maxLightUpperCombo)
        {
            //if combo has been completed it is set back to the first attack of the combo
            lightUpperComboNumber = 1;
        }

        //store combo current state
        int currentComboNumber = lightUpperComboNumber;

        //wait for duration of animation to run then stop light upper running
        yield return new WaitForSeconds(0.5f);
        lightUpperRunning = false;

        //wait for the max time between combos
        yield return new WaitForSeconds(0.3f);

        //if combo state has stayed the same then light upper hasn't been pressed
        //so reset the combo
        if(currentComboNumber == lightUpperComboNumber)
        {
            lightUpperComboNumber = 0;
        }
    }

    [Rpc(SendTo.Server)]
    private void LightUpperAttackServerRpc(int comboNumber)
    {
        switch (comboNumber)
        {
            case 2:
                animator.SetTrigger("Light Upper 2");
                break;
            case 3:
                animator.SetTrigger("Light Upper 3");
                break;
            default:
                animator.SetTrigger("Light Upper 1");
                break;
        }
    }
}
