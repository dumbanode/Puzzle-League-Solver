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
	
	public virtual void Enter(){
		
	}
	
	public virtual void Exit(){
		
	}
	
	/*
	public override void _Ready()
	{
		
	}
	*/
}
