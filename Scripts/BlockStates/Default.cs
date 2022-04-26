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
		// Check if we should transition to falling state
		//GD.Print("Checking falling");
	}
	
	public override void HandleFunction(Godot.Collections.Dictionary<string, object> msg = null){
		foreach(System.Collections.Generic.KeyValuePair<string,object> i in msg){
			Type thisType = this.GetType();
			object[] toPass = new object[] {i.Value};
			GetType().GetMethod(i.Key).Invoke(this, toPass);
			//MethodInfo theFunction = thisType.GetMethod(i[0]);
		}
		//Type thisType = this.GetType();
		//var key = msg.First();
		//MethodInfo theFunction = thisType.GetMethod(key);
		//theFunction.Invoke(this, msg[key]);
	}
	
	public void test(string s){
		GD.Print(s);
	}
	

}
