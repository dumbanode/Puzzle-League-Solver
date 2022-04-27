// Default.cs
using Godot;
using System;


// otherIntVec?.x

/*
	* Default state the block is in (ie: not falling, swapping or cleared)
	* Only when a block is in default state can it be cleared
*/
public class Default : State
{
	public override void Enter(Godot.Collections.Dictionary<string, object> msg = null){
		GD.Print("In Default State");
		var owner = Owner as GridBlock;
	}

	public override void Update(float delta){
		// Check if we should transition to falling state
		this.CheckFalling();
	}
	
	private void CheckFalling(){
		
	}
	
	public void Move(Vector2 target){
		var whereToMove = new Godot.Collections.Dictionary<string,object>();
		whereToMove.Add("position", target);
		this.stateMachine.TransitionTo("Swapping", whereToMove);
	}
	

}
