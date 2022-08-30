using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerMovement))]

public class PlayerController : MonoBehaviour
{
    bool Selected = false;
    public PlayerMovement parent;
    void Awake()
    {
        parent = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Selected)
            HandleControls();
    }
    void HandleControls()
    {
        parent.Move(Input.GetAxis("Horizontal"));
        parent.TryJump();
    }
    public void SetSelected()
    {

    }
}
