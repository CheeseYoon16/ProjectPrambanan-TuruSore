using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundEnter : StateMachineBehaviour
{
    [SerializeField] private SoundType sound;
    [SerializeField] public string soundName;
    //[SerializeField, Range(0, 1)] private float volume = 1;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundManager.Instance.PlaySound(sound, soundName);
    }
}
