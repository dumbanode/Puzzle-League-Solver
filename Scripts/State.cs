using Godot;
using System;
using System.Collections.Generic;

public class State : Node
{
	public StateMachine stateMachine = null;

	public virtual void HandleInput(InputEvent Event){
		
	}

	public virtual void Update(float _delta){
		
	}
	
	public virtual void PhysicsUpdate(float _delta){
		
	}
	
	public virtual IList<object> HandleMethod(Godot.Collections.Dictionary<string, object[]> msg = null){
		List<object> toReturn = new List<object>();
		foreach(System.Collections.Generic.KeyValuePair<string,object[]> i in msg){
			Type thisType = this.GetType();
			var toPass = i.Value;
			if (GetType().GetMethod(i.Key) != null){
				object results = new object();
				var method = GetType().GetMethod(i.Key);
				if (method.ReturnType != typeof(void)){
					results = method.Invoke(this, toPass);
				}
				else {
					method.Invoke(this, toPass);
					results = false;
				}
				toReturn.Add(results);
			}
		}
		return toReturn;
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
