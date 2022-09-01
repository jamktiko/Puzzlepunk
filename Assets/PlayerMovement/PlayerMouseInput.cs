using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseInput : MonoBehaviour
{
    Camera cam;
    private void Awake()
    {
        cam = GetComponent<Camera>();
    }
    private void Update()
    {
        if (DialogueUIController.main.IsCutscenePlaying())
            return;
        if (Input.GetMouseButtonDown(0))
        {
            MovePlayer();
        }
    }
    void MovePlayer()
    {
        Vector2 point = cam.ScreenToWorldPoint(Input.mousePosition);
        foreach (RaycastHit2D hit in Physics2D.CircleCastAll(point,.1f,Vector2.zero))
        {
            if (hit.transform.TryGetComponent(out InteractableBase interactable))
            {
                if (interactable.RequiresMoveInRange)
                {
                    PlayerMovement.main.IssueMoveOrder(interactable);
                    return;
                }
                else
                    interactable.OnInteract();
            }
        }
        PlayerMovement.main.IssueMoveOrder(point);
    }
}
