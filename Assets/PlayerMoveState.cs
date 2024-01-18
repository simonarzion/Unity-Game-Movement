using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerMoveState : PlayerBaseState {
    private readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
    private readonly int MoveBlendTreeHash = Animator.StringToHash("MoveBlendTree");
    private const float AnimationDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f;

    public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter() {
        stateMachine.Velocity.y = Physics.gravity.y;
        stateMachine.Animator.CrossFadeInFixedTime(MoveBlendTreeHash, CrossFadeDuration);
        stateMachine.InputReader.OnJumpPerformed += SwitchToJumpState;
    }

    public override void Exit() {
        stateMachine.InputReader.OnJumpPerformed -= SwitchToJumpState;
    }

    public override void Tick() {
        if (!stateMachine.Controller.isGrounded) {
            stateMachine.SwitchState(new PlayerFallState(stateMachine));
        }

        CalculateMoveDirection();
        FaceMoveDirection();
        Move();

        stateMachine.Animator.SetFloat(MoveSpeedHash,
                                       stateMachine.InputReader.IsSprinting 
                                            ? 2f 
                                            : stateMachine.InputReader.MoveComposite.sqrMagnitude > 0f ? 1f : 0f,
                                       AnimationDampTime,
                                       Time.deltaTime);
    }

    private void SwitchToJumpState() {
        stateMachine.SwitchState(new PlayerJumpState(stateMachine));
    }
}
