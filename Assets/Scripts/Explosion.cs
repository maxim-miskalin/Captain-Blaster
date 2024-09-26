using UnityEngine;

public class Explosion : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(animator.TryGetComponent(out IPoolObject objectPool))
            objectPool.Return();
    }
}
