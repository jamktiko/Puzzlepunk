using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueScriptSO", menuName = "Dialogue/DialogueCharacter")]
public class DialogueCharacterSO : ScriptableObject
{

    public string CharacterName;

    public Sprite Head;
    public Sprite[] Faces;
}
