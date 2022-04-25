using Godot;
using System;


// otherIntVec?.x

/*
	* Default state for a grid block
	* ie: Not falling, not cleared, not swapping
*/
public class Default : State
{
	public override void Enter(){
		GD.Print("hi there");
		var owner = Owner as GridBlock;
		
	}

	public override void Update(float delta){ }
	

}
