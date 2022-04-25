using Godot;
using System;


/*
	* Default state for a grid block
	* ie: Not falling, not cleared, not swapping
*/
public class Falling : State
{
	public override void Enter(Godot.Collections.Dictionary<string, object> msg = null){
		GD.Print("hi there");
	}

	public override void Update(float delta){ }
	

}
