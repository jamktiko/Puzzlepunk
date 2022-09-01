using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IdeaButtonManager : MonoBehaviour
{
    public Image ideaImage;
    private void Awake()
    {
        if (ideaImage==null)
        {
            ideaImage = transform.GetChild(1).GetComponent<Image>();
        }
    }
    private void Start()
    {
        ShowNewIdea(false);
    }
public void ShowNewIdea(bool value)
    {
        ideaImage.gameObject.SetActive(value);
    }
}
