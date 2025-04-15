using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    private Animator CharacterAnimator;

    // Start is called before the first frame update
    void Start()
    {
        CharacterAnimator = GetComponentInChildren<Animator>();
        UpdateCharacterImage();
    }

    // Update is called once per frame
    void UpdateCharacterImage()
    {
        CharacterAnimator.runtimeAnimatorController = CurrentPlayerCharacter.Instance.SetCharacterSelectImageAnimator();
    }

    //character select buttons
    public void SelectCharacterDaddyLongLegs()
    {
        CurrentPlayerCharacter.Instance.currentCharacter = CurrentPlayerCharacter.CharacterType.DaddyLongLegs;
        UpdateCharacterImage();
    }

    public void SelectCharacterMothEmperor()
    {
        CurrentPlayerCharacter.Instance.currentCharacter = CurrentPlayerCharacter.CharacterType.MothEmperor;
        UpdateCharacterImage();
    }
}
