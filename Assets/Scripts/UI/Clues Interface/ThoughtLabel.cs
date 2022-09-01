using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoughtLabel : MonoBehaviour
{
    public bool Revealed = false;
    public string IdeaID = "-";

    private void Awake()
    {
        gameObject.SetActive(false);
    }
    public void Reveal()
    {
        Revealed = true;

    }
}
