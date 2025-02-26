using Godot;
using System;

public partial class AnimatedEntity : Entity
{
	[ExportGroup("Animation")]

	[Export] public AnimationLibrary animationLibrary;
	[Export] public AnimationPlayer animationPlayer;


    public override void _Ready()
    {
		animationPlayer.AddAnimationLibrary("default", animationLibrary);
    }
    public void PlayAnimation(String animationName){
		animationPlayer.Play(animationName);
	}
	public bool isAnimationFinished(){
		return animationPlayer.CurrentAnimation=="";
	}
}
