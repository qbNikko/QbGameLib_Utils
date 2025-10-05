using System.Collections.Generic;
using UnityEngine;

namespace QbGameLib_Utils.FSM
{
    public class AnimationSubState : SubState, IEnterSubState
    {
        private Animator _animator;
        private string _animationName;

        public AnimationSubState(Animator animator, string animationName)
        {
            _animator = animator;
            _animationName = animationName;
        }

        public void Enter(Dictionary<string, object> parameters = null)
        {
            if (parameters!=null && parameters.TryGetValue("animation", out object animation))
            {
                _animator.Play(animation as string);
            }else _animator.Play(_animationName);
            
        }
    }
}