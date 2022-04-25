using Godot;
using System;


/*
	* Default state for a grid block
	* ie: Not falling, not cleared, not swapping
*/
public class Swapping : State
{
	public override void Enter(){
		GD.Print("hi there");
	}

	public override void Update(float delta){ }
	

}
