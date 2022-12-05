using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RobotMenu : MonoBehaviour
{
    public RobotPuzzleController myPuzzle;
     OrderUI orderMenu;
     IconUI iconMenu;
    public RobotInputListener input;

    private void Awake()
    {
        if (orderMenu == null)
            orderMenu = GetComponentInChildren<OrderUI>();
        if (iconMenu == null)
            iconMenu = GetComponentInChildren<IconUI>();
        if (input == null)
            input = GetComponentInChildren<RobotInputListener>();
        if (ErrorText != null)
            ErrorText.gameObject.SetActive(false);
    }
    public void InitPuzzle(RobotPuzzleController puzzle)
    {
        myPuzzle = puzzle;
        iconMenu.OnPuzzleStart();
    }
    public void OnReset(bool hard)
    {
        myPuzzle.StopSolution();
        myPuzzle.OnReset(hard);
        ResetOrderUI();
        ShowError(ErrorMessageID.reset);
    }
    public void OnBackSpace()
    {
        if (myPuzzle.IsPlaying())
        {
            OnReset(false);
        }
        else
        {
            myPuzzle.GetSelectedRobot().ClearLastOrder();
            ResetOrderUI();
        }
    }
    public bool IsPuzzleMode()
    {
        return gameObject.activeSelf && myPuzzle != null;
    }
    public void Close()
    {
        if (myPuzzle.TryShutDown())
        {
            myPuzzle = null;
            UIController.main.CloseWindow(true);
        }
    }
    public void OnSelectionChanged()
    {
        iconMenu.OnSelectionChanged();
        ResetOrderUI();
    }
    public void ResetOrderUI()
    {
        orderMenu.UpdateOrders();
        iconMenu.UpdateOrders();
    }
    public void PlaySolution()
    {
        myPuzzle.PlaySolution();
    }
    #region Errors

    public TextMeshProUGUI ErrorText;

    Coroutine errorCoroutine;
    public enum ErrorMessageID
    {
        robotOrders = 0,
        puzzlePlaying = 1,
        collision = 2,
        reset = 3,
        brokenrobot = 4,
    }
    public void ShowError (ErrorMessageID errorID)
    {
        if (errorCoroutine != null)
            StopCoroutine(errorCoroutine);

        string text = "";
        switch (errorID)
        {
            case ErrorMessageID.robotOrders:
                text = RobotOrderError;
                break;
            case ErrorMessageID.puzzlePlaying:
                text = PuzzlePlayingError;
                break;
            case ErrorMessageID.collision:
                text = RobotCollisionError;
                break;
            case ErrorMessageID.reset:
                text = PuzzleResetError;
                break;
            case ErrorMessageID.brokenrobot:
                text = RobotBrokenError;
                break;
        }

        errorCoroutine = StartCoroutine(ShowErrorForDuration(text, RobotErrorDuration));
    }
    public IEnumerator ShowErrorForDuration(string text, float dur)
    {
        ErrorText.text = text;
        ErrorText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(dur);
        ErrorText.gameObject.SetActive(false);
        errorCoroutine = null;
    }
    #endregion
    [Header("Error Messages")]
    public float RobotErrorDuration = 3f;
    public string RobotOrderError = "Not All Robots Have Been Given Orders!";
    public string PuzzlePlayingError = "Puzzle is already playing!";
    public string RobotCollisionError = "Robots have collided!";
    public string PuzzleResetError = "The puzzle was reset!";
    public string RobotBrokenError = "Glitch! This robot will walk in reverse!";
}
