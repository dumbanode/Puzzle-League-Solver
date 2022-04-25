// Default.cs
using Godot;
using System;


// otherIntVec?.x

/*
	* Default state the block is in (ie: not falling, swapping or cleared)
	* Only when a block is falling can it be cleared
*/
public class Default : State
{
	public override void Enter(Godot.Collections.Dictionary<string, object> msg = null){
		GD.Print("Default");
		var owner = Owner as GridBlock;
	}

	public override void Update(float delta){
		
		
		
	}
	

}
