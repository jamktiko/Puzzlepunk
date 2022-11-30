using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    RobotOrderButton[] OrderDisplays;
    private void Awake()
    {
        OrderDisplays = GetComponentsInChildren<RobotOrderButton>();
    }

    public void UpdateOrders()
    {
        if (RobotPuzzleController.main == null)
            return;

        RobotPawn.Memory selection = ((RobotPuzzleController)RobotPuzzleController.main).GetSelectedRobot();
        UpdateOrdersForRobot(selection);  
    }
    void UpdateOrdersForRobot(RobotPawn.Memory mRobot)
    {
        RobotOrderButton.ButtonState currentState = RobotOrderButton.ButtonState.used;

        for (int iDisplay = 0; iDisplay < OrderDisplays.Length; iDisplay++)
        {
            if (iDisplay< mRobot.orders.Length)
            {
                int order = (int)mRobot.GetOrderAt(iDisplay);
                OrderDisplays[iDisplay].ChangeOrder(order);      //TODO nullcheck
                if (order == 0 && currentState == RobotOrderButton.ButtonState.used)
                {
                    OrderDisplays[iDisplay].ChangeState(RobotOrderButton.ButtonState.current);
                    currentState = RobotOrderButton.ButtonState.remaining;
                }
                else
                {
                    OrderDisplays[iDisplay].ChangeState(currentState);
                }
            }
            else
            {
                OrderDisplays[iDisplay].ChangeState(RobotOrderButton.ButtonState.inactive);
            }
        }
    }
}
