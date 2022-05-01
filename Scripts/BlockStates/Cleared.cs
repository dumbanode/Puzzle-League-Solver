using Godot;
using System;


/*
	* Default state for a grid block
	* ie: Not falling, not cleared, not swapping
*/
public class Cleared : State
{
	public override void Enter(Godot.Collections.Dictionary<string, object> msg = null){
		GD.Print("--- CLEARED ---");
		
		this.DestroySelf();
	}

	public override void Update(float delta){ }
	
	public void DestroySelf(){
		var owner = Owner as GridBlock;
		owner.QueueFree();
	}
	

}
