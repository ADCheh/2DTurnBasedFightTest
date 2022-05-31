using Spine.Unity;
using UnityEngine;
using UnityEngine.Events;

public class AnimationController : MonoBehaviour
{
    public UnityEvent attackComplete = new UnityEvent();

    public UnityEvent<GameObject> characterClicked = new UnityEvent<GameObject>();

    public void PlayAttack()
    {
        gameObject.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "DoubleShift", false);
        
        gameObject.GetComponent<SkeletonAnimation>().AnimationState.Complete += delegate
        {
            Debug.Log("Attack complete!");
            attackComplete?.Invoke();
        };

    }
    public void PlayDamage()
    {
        gameObject.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "Damage", true);
    }

    public void GoIdle()
    {
        Debug.Log(gameObject.GetComponent<SkeletonAnimation>().AnimationState);
        
        gameObject.GetComponent<SkeletonAnimation>().AnimationState.ClearTrack(0);
        gameObject.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "Idle", true);
    }

    public void OnMouseDown()
    {
        characterClicked?.Invoke(gameObject);
    }
}
