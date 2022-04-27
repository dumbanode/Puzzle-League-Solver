using Godot;
using System;


/*
	* Default state for a grid block
	* ie: Not falling, not cleared, not swapping
*/
public class Swapping : State
{
	public override void Enter(Godot.Collections.Dictionary<string, object> msg = null){
		GD.Print("In Swapping State");
		if (msg.ContainsKey("position")){
			this.SwapBlocks((Vector2)msg["position"]);
		}
	}

	public override void Update(float delta){ }
	
	public void SwapBlocks(Vector2 target){
		var owner = Owner as GridBlock;
		
		owner.MoveTween.InterpolateProperty(owner, "position", owner.Position, 
					target, (float).3, Tween.TransitionType.Elastic, Tween.EaseType.Out);
		owner.MoveTween.InterpolateCallback(owner, (float).1, "TransitionTo", "Default");
		owner.MoveTween.Start();
	}
	

}
