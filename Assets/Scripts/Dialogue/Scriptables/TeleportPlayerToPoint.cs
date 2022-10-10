using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Teleport Point", menuName = "Dialogue/Components/Teleport Point")]
public class TeleportPlayerToPoint : ScriptableDialogue
{
    public string PointName = "";
    public bool Instant = false;
    public override void OnSkipped(DialogueUIController DC)
    {
        NavPoint point = LevelController.main.GetPointByName(PointName);
        if (PointName != null)
        {
            PlayerTransitionController.main.TeleportToPoint(point, Instant);
        }
    }
    public override IEnumerator Run(DialogueUIController DC)
    {
        OnSkipped(DC);
        yield return base.Run(DC);
    }
}
