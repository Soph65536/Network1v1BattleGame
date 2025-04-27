using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarSlider : NetworkBehaviour
{
    //ui reference to health slider
    public Slider healthSlider;

    [SerializeField] private CurrentPlayerCharacter.SideSpawned whichSide;

    PlayerHealth ownerPlayerHealth;

    private void Awake()
    {
        healthSlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        //try get owner player helth if not already gotten
        if (ownerPlayerHealth == null)
        {
            //check players for whichever one is this side
            var playerCharacters = FindObjectsOfType<CurrentPlayerCharacter>();
            foreach (var player in playerCharacters)
            {
                if (player.currentSide.Value == whichSide)
                {
                    //set player health to this player object's health script
                    ownerPlayerHealth = player.GetComponent<PlayerHealth>();

                    ownerPlayerHealth.health.OnValueChanged += UpdateHealthSliderServerRpc;
                    SetInitialValuesServerRpc();
                    return;
                }
            }
        }
    }

    [Rpc(SendTo.Everyone)]
    public void SetInitialValuesServerRpc()
    {
        healthSlider.maxValue = ownerPlayerHealth.health.Value;
        healthSlider.value = healthSlider.maxValue;
    }

    [Rpc(SendTo.Everyone)]
    private void UpdateHealthSliderServerRpc(int previousValue, int newValue)
    {
        healthSlider.value = ownerPlayerHealth.health.Value;
    }
}
