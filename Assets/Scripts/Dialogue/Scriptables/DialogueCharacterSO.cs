using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Dialogue/Character")]
public class DialogueCharacterSO : ScriptableObject
{

    public string CharacterName;

    public Sprite Head;
    public CharacterFace[] Faces;
    [System.Serializable]
    public class CharacterFace
    {
        public DialogueLineSO.CharacterEmotion Emotion;
        public Sprite Reaction;
    } 
}
