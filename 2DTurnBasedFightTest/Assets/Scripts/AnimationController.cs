using System;
using Infrastructure.Battle;
using Infrastructure.Services;
using Infrastructure.States;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Events;

public class AnimationController : MonoBehaviour
{
    public UnityEvent attackComplete = new UnityEvent();
    public UnityEvent characterClicked = new UnityEvent();

    private void Start()
    {
        gameObject.GetComponent<SkeletonAnimation>().AnimationState.Complete += AttackComplete;
    }

    public void PlayAttack()
    {
        gameObject.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "DoubleShift", false);
    }
    public void PlayDamage()
    {
        gameObject.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "Damage", false);
        gameObject.GetComponent<SkeletonAnimation>().AnimationState.AddAnimation(0, "Damage", false, 1.2f);
    }

    public void GoIdle()
    {
        gameObject.GetComponent<SkeletonAnimation>().AnimationState.ClearTrack(0);
        gameObject.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "Idle", true);
    }

    public void OnMouseDown()
    {
        AllServices.Container.Single<IBattleController>().SetActiveEnemyCharacter(gameObject);
        characterClicked?.Invoke();
    }

    private void AttackComplete(TrackEntry trackEntry)
    {
        if (trackEntry.Animation.name != "DoubleShift")
            return;
        attackComplete?.Invoke();
    }
}
