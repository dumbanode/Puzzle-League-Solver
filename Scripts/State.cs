using Godot;
using System;

public class State : Node
{
	public StateMachine stateMachine = null;

	public virtual void HandleInput(InputEvent Event){
		
	}

	public virtual void Update(float _delta){
		
	}
	
	public virtual void PhysicsUpdate(float _delta){
		
	}
	
	public virtual void HandleMethod(Godot.Collections.Dictionary<string, object> msg = null){
		foreach(System.Collections.Generic.KeyValuePair<string,object> i in msg){
			Type thisType = this.GetType();
			object[] toPass = new object[] {i.Value};
			if (GetType().GetMethod(i.Key) != null){
				GetType().GetMethod(i.Key).Invoke(this, toPass);
			}
		}
	}
	
	public virtual void Enter(Godot.Collections.Dictionary<string, object> msg = null){
		
	}
	
	public virtual void Exit(){
		
	}
	
	public void TransitionTo(String targetState, 
				Godot.Collections.Dictionary<string, object> msg = null){
		this.stateMachine.TransitionTo(targetState, msg);
	}
	
	/*
	public override void _Ready()
	{
		
	}
	*/
}
