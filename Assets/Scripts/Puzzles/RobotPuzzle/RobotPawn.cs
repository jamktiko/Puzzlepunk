using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class RobotPawn : PuzzlePiece
{
    [Header("Configuration")]
    public int CommandID = 0;
    public int MaxMoves = 1;
    public bool OppositeOrders = false;
    //public Color robotColor;
    public Sprite robotSprite;

    [Header("Components")]

    public MovementComponent movement;
    public SpriteRenderer paint;
    public GridNav.Node OriginalNode;

    private void Init()
    {
        if (movement==null)
        movement = GetComponent<MovementComponent>();
        if (anim==null)
        anim = GetComponent<Animator>();
    }
    private void Awake()
    {
        Init();
    }
    public override void TieToPuzzle(PuzzleController parent)
    {
        Init();
        if ((RobotPuzzleController)parent != null)
        {
            RobotPuzzleController rpc = (RobotPuzzleController)parent;
            movement.grid = rpc.mGrid;
            movement.MovementSpeed = 1f / rpc.StepTime;
            OriginalNode = rpc.mGrid.GetNodeAt(rpc.mGrid.TranslateCoordinate(transform.position));
        }
        //if (paint != null)
         //   paint.color = robotColor;
        puzzleParent = parent;
        InitOrders();
       // OnReset(true);
    }
    public override void OnReset(bool hard)
    {
        if (puzzleParent == null) return;
        movement.Stop();
        movement.JumpToPoint(OriginalNode);
        SetCrashed(false);
        if (hard)
            ClearOrders();
        SetMoving(false);
        SetSelected(false);
        anim.Play("robotidle");
    }
    #region Orders
    public enum WalkDirection
    {
        empty = 0,
        up = 1,
        down = 2,
        left = 3,
        right = 4
    }
    public static WalkDirection ReverseDirection(WalkDirection dir)
    {
        switch (dir)
        {
            case WalkDirection.up:
                return WalkDirection.down;
            case WalkDirection.down:
                return WalkDirection.up;
            case WalkDirection.left:
                return WalkDirection.right;
            case WalkDirection.right:
                return WalkDirection.left;
        }
        return WalkDirection.empty;
    }
    public class Memory
    {
        //public Color color;
        public Sprite Icon;
        public bool Reversed;
        public WalkDirection[] orders;
        public Memory(Sprite micon, int nOrders, bool broken)
        {
            Icon = micon;
            //color = mcol;
            orders = new WalkDirection[nOrders];
        Reversed = broken;
    }
        public int iOrder = 0;
        public bool IssueOrder(WalkDirection order)
        {
            WalkDirection lastDir = WalkDirection.empty;
            for (int iO = 0; iO < orders.Length; iO++)
            {
                if (orders[iO] == WalkDirection.empty)
                {
                    if (order != ReverseDirection(lastDir))
                    {
                        iOrder++;
                        orders[iO] = order;
                        UIController.main.robotController.ResetOrderUI();//todo nullcheck
                        return true;
                    }
                }
                else
                {
                    lastDir = orders[iO];
                }
            }
            return false;
        }
        public WalkDirection GetOrderAt(int iO)
        {
            if (iO < orders.Length)
                return orders[iO];
            return WalkDirection.empty;
        }
        public void ClearLastOrder()
        {
                for (int iO = 1; iO <= orders.Length; iO++)
                {
                    if (iO == orders.Length || orders[iO] == WalkDirection.empty)
                    {
                        iOrder = Mathf.Max(0, iOrder - 1);
                        orders[iO - 1] = WalkDirection.empty;
                        break;
                    }
                }
            
        }
        public void ClearOrders()
        {
            iOrder = 0;
            for (int iO = 0; iO < orders.Length; iO++)
            {
                orders[iO] = WalkDirection.empty;
            }
        }
        public int GetRemainingOrders()
        {
            if (orders == null) return 0;
            return orders.Length - iOrder;
        }
    }
    void InitOrders()
    {
        RobotPuzzleController rpc = (RobotPuzzleController)puzzleParent;
       if (rpc.RobotCommands[CommandID] == null)
            rpc.RobotCommands[CommandID] = new Memory(robotSprite, MaxMoves, OppositeOrders);

        rpc.UpdateMoveLimit(MaxMoves);
    }
    public Memory GetOrders()
    {
        var rpuzzleParent = (RobotPuzzleController)puzzleParent;
        if (rpuzzleParent != null && rpuzzleParent.RobotCommands != null && CommandID < rpuzzleParent.RobotCommands.Length )
            return ((RobotPuzzleController)puzzleParent).RobotCommands[CommandID];
        return null;
    }
    public int GetRemainingOrders()
    {
        Memory memory = GetOrders();
        if (memory == null) return 0;
        return  memory.GetRemainingOrders();
    }
    public void ClearOrders()
    {
        Memory memory = GetOrders();
        memory.ClearOrders();
    }
    #endregion
    #region Moving Cycle
    public bool IsMoving()
    {
        return movement.IsWalking();
    }
    int CurrentStep()
    {
        if ((RobotPuzzleController)puzzleParent != null)
        {
            return  ((RobotPuzzleController)puzzleParent).cStep;
        }
        return 0;
        }
    public bool IsIdle()
    {
        return CurrentStep() < MaxMoves;
    }
    Vector2Int GetNextStepDirection()
    {
        Vector2Int nPoint = Vector2Int.zero;

        Memory memory = GetOrders();
        if (memory == null || CurrentStep() < 0 || CurrentStep() >= memory.orders.Length)
            return nPoint;

        WalkDirection walkDir = memory.orders[CurrentStep()];
        if (OppositeOrders)
            walkDir = ReverseDirection(walkDir);

        switch (walkDir)
        {
            case WalkDirection.up:
                nPoint = Vector2Int.up;
                break;
            case WalkDirection.down:
                nPoint = Vector2Int.down;
                break;
            case WalkDirection.left:
                nPoint = Vector2Int.left;
                break;
            case WalkDirection.right:
                nPoint = Vector2Int.right;
                break;
            default:
                float randomValue = Random.value * 4;
                if (randomValue <= 1)
                {
                    nPoint = Vector2Int.right;
                }
                else if (randomValue <= 2)
                {
                    nPoint = Vector2Int.left;
                }
                else if (randomValue <= 3)
                {
                    nPoint = Vector2Int.up;
                }
                else if (randomValue <= 4)
                {
                    nPoint = Vector2Int.down;
                }
                break;
        }
        return nPoint;
    }
    Vector2Int PredictPosition()
    {
        return movement.GetGridPosition() + GetNextStepDirection();
    }
    public void Step()
    {
        if (IsIdle())
        {
            movement.Stop();
            Vector2Int nPoint = GetNextStepDirection();
            CheckCrash(movement.GetGridPosition() + nPoint);
            if (!HasCrashed())
                movement.MoveToPoint(movement.GetGridPosition() + nPoint);
        }

    }
    #endregion
    #region Crashing
    bool hasCrashed = false;
    void CheckCrash(Vector2Int node)
    {
        if (movement == null || movement.grid == null || puzzleParent == null)
            return;
        var nNode = movement.grid.GetNodeAt(node);
        if (nNode == null || !nNode.IsPassible())
        {
            SetCrashed(true);
            return;
        }

        foreach (RobotPawn robot in ((RobotPuzzleController)puzzleParent).Robots)
        {
            if (robot != this)
            {
                var predicted = robot.PredictPosition();

                if (
                    predicted == node ||
                    (robot.PredictPosition() == movement.GetGridPosition() && PredictPosition() == robot.movement.GetGridPosition())    //hack prevent robots from swapping positions
                    )
                {
                    SetCrashed(true);
                    return;
                }
            }
        }
    }
    public bool HasCrashed()
    {
        return hasCrashed;
    }
    void SetCrashed(bool value)
    {
        if (value)
            Debug.Log(name + " HAS CRASHED! x.x");
        hasCrashed = value;
        anim.SetBool("crashed", value);
    }

    #endregion
    #region Animation
    Animator anim;
    public void SetMoving(bool value)
    {
        anim.SetBool("moving", value);
        SetDirty(false);
    }
    public void SetSelected(bool value)
    {
        if (PuzzleController.main != puzzleParent || puzzleParent.WasSolved())
        {
            anim.SetBool("selected", false);
            return;
        }

        anim.SetBool("selected", value);
        SetDirty(!value);
    }
    void SetDirty(bool value)
    {

        if (paint != null)
        {
            bool grey = value && !((RobotPuzzleController)puzzleParent).IsPlaying();
            paint.color = grey ? Color.grey : Color.white;
        }
    }

    #endregion
}
