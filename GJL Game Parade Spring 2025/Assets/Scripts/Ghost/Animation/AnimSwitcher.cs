using UnityEngine;
using Animancer;

[CreateAssetMenu(fileName = "Animation Switcher", menuName = "Animation/Animation Switcher")]
public class AnimSwitcher : ScriptableObject
{
    public DirectionalAnimationSet8 walkSet;
    public DirectionalAnimationSet8 sprintSet;
    public DirectionalAnimationSet8 crouchSet;

    public AnimationClip idle;
    public AnimationClip crouchIdle;
    public AnimationClip jump;
    public AnimationClip falling;
}