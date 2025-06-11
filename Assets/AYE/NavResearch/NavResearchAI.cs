using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavResearchAI : AYENpc<NavResearchAIState>
{
    #region AddState
    protected override void Awake()
    {
        base.Awake();
        AddStatus(NavResearchAIState.Idle, OnIdle, UpdateIdle, ExitIdle);
        AddStatus(NavResearchAIState.Move, OnMove, UpdateMove, ExitMove);
    }
    #endregion
    #region Idle
    [SerializeField] LayerMask groundLayer;
    void OnIdle()
    {
        ResetNavigationSystem();
    }
    void UpdateIdle()
    {
        // 取得滑鼠點擊畫面的位置
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 999f, groundLayer))
            {
                // 移動到點擊的位置
                moveTarget = hit.point;
                status = NavResearchAIState.Move;
            }
        }
        look = headForward;
    }
    void ExitIdle()
    {

    }
    #endregion
    #region Move
    Vector3 moveTarget;
    float runSpeed = 0f;
    [SerializeField] Sensor 障礙物偵測器 = null;
    void OnMove()
    {
        障礙物偵測器.detect += OnStop;
        障礙物偵測器.run = true;
        runSpeed = 0f;
        animator.SetBool("Run", true);
    }
    void UpdateMove()
    {
        Vector3 targetPos = GetNavigationCorners(moveTarget);
        face = targetPos;
        faceSpeed = 3f;
        look = GetHeadSameYPos(moveTarget);
        lookSpeed = 10f;

        // 面向目標時加速 靠近目標時速度降半
        runSpeed = Mathf.Lerp(runSpeed, faceAngle < 30f && isPathPendingDone ? (pathLength > 1.5f? 1f : 0.5f) : 0f, Time.deltaTime * 2f);

        animator.SetFloat("RunSpeed", runSpeed);

        if (pathLength < 0.5f)
            status = NavResearchAIState.Idle;
    }
    void ExitMove()
    {
        animator.SetBool("Run", false);
        障礙物偵測器.detect -= OnStop;
        障礙物偵測器.run = false;
    }
    void OnStop(GameObject[] stuff)
    {
        foreach (GameObject obj in stuff)
        {
            NavMeshObstacle obstacle = obj.GetComponent<NavMeshObstacle>();
            if (obstacle != null)
            {
                obstacle.enabled = true;
                break;
            }
        }
    }
    #endregion
    #region 右手持燈
    bool rightHandLight
    {
        get { return _rightHandLight; }
        set
        {
            _rightHandLight = value;
        }
    }
    bool _rightHandLight = false;
    #endregion
}
public enum NavResearchAIState
{
    Idle,
    Move,
}
