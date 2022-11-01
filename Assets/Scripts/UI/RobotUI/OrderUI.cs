using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    public Sprite[] Arrows;

    Image[] OrderDisplays;
    private void Awake()
    {
        OrderDisplays = GetComponentsInChildren<Image>();
    }

    public void UpdateOrders()
    {
        UpdateOrdersForRobot( ((RobotPuzzleController)RobotPuzzleController.main).GetSelectedRobot());  //TODO nullcheck
    }
    void UpdateOrdersForRobot(RobotNPC mRobot)
    {
        for (int iDisplay = 0; iDisplay < OrderDisplays.Length; iDisplay++)
        {
            if (iDisplay< mRobot.MaxMoves)
            {
                OrderDisplays[iDisplay].gameObject.SetActive(true);
                int order = (int)mRobot.GetOrderAt(iDisplay);
                OrderDisplays[iDisplay].sprite = Arrows[order];      //TODO nullcheck
            }
            else
            {
                OrderDisplays[iDisplay].gameObject.SetActive(false);
            }
        }
    }
}
