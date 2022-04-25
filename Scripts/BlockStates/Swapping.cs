using Godot;
using System;


/*
	* Default state for a grid block
	* ie: Not falling, not cleared, not swapping
*/
public class Swapping : State
{
	public override void Enter(Godot.Collections.Dictionary<string, string> msg = null){
		GD.Print("Swapping");
	}

	public override void Update(float delta){ }
	

}
