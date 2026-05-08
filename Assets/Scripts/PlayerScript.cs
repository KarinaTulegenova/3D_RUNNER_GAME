using System;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private const string WinParameter = "Win";
    private const string LoseParameter = "Lose";
    private const string HorizontalParameter = "Hor";
    private const string VerticalParameter = "Vert";
    private const string StateParameter = "State";
    private const string JumpParameter = "IsJump";

    public int score = 0;

    private Animator animator;
    private Controller.CharacterMover characterMover;
    private Controller.MovePlayerInput movePlayerInput;
    private bool canMove = true;
    private bool hasWinParameter;
    private bool hasLoseParameter;
    private bool hasHorizontalParameter;
    private bool hasVerticalParameter;
    private bool hasStateParameter;
    private bool hasJumpParameter;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        characterMover = GetComponent<Controller.CharacterMover>();
        movePlayerInput = GetComponent<Controller.MovePlayerInput>();

        CacheAnimatorParameters();
    }

    public void AddScore(int value)
    {
        score += value;
    }

    public void ResetScore()
    {
        score = 0;
    }

    public void SetCanMove(bool value)
    {
        canMove = value;

        if (movePlayerInput != null)
        {
            movePlayerInput.enabled = value;
        }

        if (characterMover != null)
        {
            characterMover.enabled = value;
        }

        if (!canMove)
        {
            SetMovementAnimatorIdle();
        }
    }

    public void PlayWinAnimation()
    {
        SetMovementAnimatorIdle();
        SetTrigger(WinParameter, hasWinParameter);
    }

    public void PlayLoseAnimation()
    {
        SetMovementAnimatorIdle();
        SetTrigger(LoseParameter, hasLoseParameter);
    }

    private void SetTrigger(string parameterName, bool hasParameter)
    {
        if (animator != null && hasParameter)
        {
            animator.SetTrigger(parameterName);
        }
    }

    private void SetMovementAnimatorIdle()
    {
        if (animator == null)
        {
            return;
        }

        if (hasHorizontalParameter)
        {
            animator.SetFloat(HorizontalParameter, 0f);
        }

        if (hasVerticalParameter)
        {
            animator.SetFloat(VerticalParameter, 0f);
        }

        if (hasStateParameter)
        {
            animator.SetFloat(StateParameter, 0f);
        }

        if (hasJumpParameter)
        {
            animator.SetBool(JumpParameter, false);
        }
    }

    private void CacheAnimatorParameters()
    {
        if (animator == null)
        {
            return;
        }

        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.name.Equals(WinParameter, StringComparison.Ordinal))
            {
                hasWinParameter = true;
            }
            else if (parameter.name.Equals(LoseParameter, StringComparison.Ordinal))
            {
                hasLoseParameter = true;
            }
            else if (parameter.name.Equals(HorizontalParameter, StringComparison.Ordinal))
            {
                hasHorizontalParameter = true;
            }
            else if (parameter.name.Equals(VerticalParameter, StringComparison.Ordinal))
            {
                hasVerticalParameter = true;
            }
            else if (parameter.name.Equals(StateParameter, StringComparison.Ordinal))
            {
                hasStateParameter = true;
            }
            else if (parameter.name.Equals(JumpParameter, StringComparison.Ordinal))
            {
                hasJumpParameter = true;
            }
        }
    }
}
