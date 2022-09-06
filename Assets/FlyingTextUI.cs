using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingTextUI : MonoBehaviour
{
    public float MoveSpeed = 1f;
    TMPro.TextMeshProUGUI Text;
    Vector3 endPosition;

    private void Awake()
    {
        endPosition = transform.position;
        if (Text == null)
        {
            Text = GetComponent<TMPro.TextMeshProUGUI>();
        }
    }

    public void RevealClue(string ClueName, Vector3 screenPosition)
    {
        gameObject.SetActive(true);
        Text.text = ClueName;
        transform.position = screenPosition;
        StartMoving();
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    Coroutine MoveTextCoroutine;
    void StartMoving()
    {
        if (MoveTextCoroutine!=null)
        {
            StopCoroutine(MoveTextCoroutine);
        }
        MoveTextCoroutine = StartCoroutine(SlideCoroutine());
    }
     IEnumerator SlideCoroutine()
    {
        Vector3 curPos = transform.position;

        float duration = 1f / ((curPos - endPosition).magnitude / MoveSpeed);

        float timeEplased = 0;

        while ((transform.position - endPosition).sqrMagnitude > MoveSpeed * MoveSpeed)
        {
            transform.position = Vector3.Lerp(curPos, endPosition, timeEplased);
            timeEplased += duration ;
            yield return new WaitForEndOfFrame();
        }
        MoveTextCoroutine = null;
        Hide();
    }
}
