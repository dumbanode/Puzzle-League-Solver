using Godot;
using System;

public class GridBlock : Node2D
{
	// valid states for this block
	public enum States {
		Default,
		Cleared,
		Falling,
		Swapping
	}
	
	// current state
	private States state = States.Default;
	
	[Export]
	public BlockType thisBlock;
	
	public bool isCleared = false;
	
	private bool isFalling = false;
	
	private Vector2 Down = new Vector2(0, 1);
	
	public Tween MoveTween;
	
	private StateMachine stateMachine;
	
	//  --- Constructors
	public GridBlock(){
		this.thisBlock = BlockType.Empty;
	}
	
	
	public GridBlock(BlockType type){
		this.thisBlock = type;
	}
	
	// -- Ready and Process functions
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{		
		// get the state machine
		this.stateMachine = GetNode<StateMachine>("StateMachine");

		// get the tween node
		this.MoveTween = (Tween)GetNode("MoveTween");
		
		// set the image for this block
		Texture img;
		// set the image of this block
		if (this.thisBlock == BlockType.Square){
			img = (Texture)GD.Load("res://Sprites/Blocks/Square.png");
		}
		else if (this.thisBlock == BlockType.Diamond){
			img = (Texture)GD.Load("res://Sprites/Blocks/Diamond.png");
		}
		else if (this.thisBlock == BlockType.Up){
			img = (Texture)GD.Load("res://Sprites/Blocks/Up.png");
		}
		else if (this.thisBlock == BlockType.Down){
			img = (Texture)GD.Load("res://Sprites/Blocks/Down.png");
		}
		else if (this.thisBlock == BlockType.Heart){
			img = (Texture)GD.Load("res://Sprites/Blocks/Heart.png");
		}
		else if (this.thisBlock == BlockType.Star){
			img = (Texture)GD.Load("res://Sprites/Blocks/Star.png");
		}
		else {
			img = (Texture)GD.Load("res://Sprites/Blocks/Garbage.png");
		}
		var sprite = (Sprite)GetNode("BlockSprite");
		sprite.Texture = img;
		
	}
	
	public override void _Process(float delta){
		this.stateMachine._Process(delta);
	}
	
	// What if the current state is falling, swapping or cleared?
	// We only want to swap IF we are in the default state
	public void Move(Vector2 target){
		// Transition to swapping state
		// pass in where to swap to
		var methodAction = new Godot.Collections.Dictionary<string,object>();
		methodAction.Add("Move", target);
		this.stateMachine.HandleMethod(methodAction);
		
	}
	
	public void TransitionTo(String state){
		this.stateMachine.TransitionTo(state);
	}
	
	public bool CanMove(){
		var methodAction = new Godot.Collections.Dictionary<string,object>();
		methodAction.Add("CanMove", "");
		GD.Print("-- State Machine --");
		GD.Print(this.stateMachine);
		bool canMove = (bool) this.handleMethod(methodAction);
		return canMove;
	}
	
	public object handleMethod(Godot.Collections.Dictionary<string, object> msg = null){
		object toReturn = false;
		if (this.stateMachine != null){
			toReturn = this.stateMachine.HandleMethod(msg);
		}		
		return toReturn;
	}
	
	/*
	// --- Cleared.cs
	public void clear(){
		this.setType(BlockType.Empty);
		this.setIsCleared(false);
	}
	*/
	
	
	// --- Getters and Setters
	
	public void SetSize(float size){
		float toScale = (float)(size / 400);
		GD.Print(toScale);
		var sprite = (Sprite)GetNode("BlockSprite");
		sprite.Scale = new Vector2(toScale, toScale);
	}
	
	public BlockType getType(){
		return this.thisBlock;
	}
	
	public void setType(BlockType toSet){
		this.thisBlock = toSet;
	}
	
	
	
	// ----- return state and compare -----
	public bool getIsCleared(){
		return this.isCleared;
	}
	
	public bool getIsFalling(){
		return this.isFalling;
	}
	
	////////////////////////////////
	
	// ---- StateMachine.transitionto() -----
	public void markClear(){
		this.isCleared = true;
	}
	
	public void setIsCleared(bool toSet){
		this.isCleared = toSet;
	}

	public void setIsFalling(bool toSet){
		this.isFalling = toSet;
	}
	
	//////////////////////////////////////
	
	
	
	public string print(){
		string toReturn = "";
		if (this.isCleared){
			GD.Print(this.isCleared);
			toReturn = " X  ";
		}
		else if (this.thisBlock == BlockType.Empty){
			toReturn = "    ";
		}
		else if (this.thisBlock == BlockType.Square){
			toReturn = " [] ";
		}
		else if (this.thisBlock == BlockType.Diamond){
			toReturn = " <> ";
		}
		else if (this.thisBlock == BlockType.Up){
			toReturn = " ^  ";
		}
		else if (this.thisBlock == BlockType.Down){
			toReturn = " V  ";
		}
		else if (this.thisBlock == BlockType.Heart){
			toReturn = " H  ";
		}
		else if (this.thisBlock == BlockType.Star){
			toReturn = " S  ";
		}
		else if (this.thisBlock == BlockType.Garbage){
			toReturn = " G  ";
		}
		return toReturn;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
