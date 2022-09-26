using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Room Change", menuName = "Dialogue/Room Change")]
public class ScriptableDialogueRoomTransition : ScriptableDialogue
{
    public string RoomName = "";
    public Vector2 RoomPoint = Vector2.zero;
    public override IEnumerator Run(DialogueUIController DC)
    {
        RoomTransitioner.main.TransitionRoom(RoomName, RoomPoint);
        yield return base.Run(DC);
    }
}
