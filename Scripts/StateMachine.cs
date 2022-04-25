using Godot;
using System;

public class StateMachine : Node
{

	[Signal]
	public delegate void Transitioned();
	
	[Export]
	public NodePath initialState;
	
	private State state;
	

	public override void _Ready()
	{
		GD.Print("Ready");
		this.state = GetNode<State>(this.initialState);
		
		foreach (State child in GetChildren()){
			child.stateMachine = this;
		}
		
		this.state.Enter();
	}
	
	public void UnhandledInput(InputEvent Event){
		this.state.HandleInput(Event);
	}


	public override void _Process(float delta)
	{
		this.state.Update(delta);
	}
	
	public void _Physics_Process(float delta){
		this.state.PhysicsUpdate(delta);
	}
	
	public void TransitionTo(String targetState, 
				Godot.Collections.Dictionary<string, string> msg = null){
		if (HasNode(targetState)){
			this.state.Exit();
			this.state = GetNode<State>(targetState);
			this.state.Enter(msg);
			EmitSignal(nameof(Transitioned));
		}
	}
	
}
