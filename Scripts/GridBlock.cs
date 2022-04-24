using Godot;
using System;

public class GridBlock : Node2D
{
	
	[Export]
	public BlockType thisBlock;
	
	private bool isCleared = false;
	
	private bool isFalling = false;
	
	public bool isSwapping = false;
	
	private Vector2 Down = new Vector2(0, 1);
	
	private Tween moveTween;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		/*
		string[] names = Enum.GetNames(typeof(Tween.EaseType));
		
		foreach (string i in names){
			GD.Print(i);
		}*/
		
		// get the tween node
		this.moveTween = (Tween)GetNode("MoveTween");
		
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
	
	public GridBlock(){
		this.thisBlock = BlockType.Empty;
	}
	
	
	public GridBlock(BlockType type){
		this.thisBlock = type;
	}
	
	public void Move(Vector2 target){
			moveTween.InterpolateProperty(this, "position", this.Position, 
								target, (float).3, Tween.TransitionType.Elastic, Tween.EaseType.Out);
			moveTween.Start();
	}
	
	public void SetSize(float size){
		float toScale = (float)(size / 400);
		GD.Print(toScale);
		var sprite = (Sprite)GetNode("BlockSprite");
		sprite.Scale = new Vector2(toScale, toScale);
	}
	
	public void drawBlock(){
		
	}
	
	public BlockType getType(){
		return this.thisBlock;
	}
	
	public void setType(BlockType toSet){
		this.thisBlock = toSet;
	}
	
	public void markClear(){
		this.isCleared = true;
	}
	
	public bool getIsCleared(){
		return this.isCleared;
	}
	
	public void setIsCleared(bool toSet){
		this.isCleared = toSet;
	}
	
	public void setIsFalling(bool toSet){
		this.isFalling = toSet;
	}
	
	public bool getIsFalling(){
		return this.isFalling;
	}
	
	public void clear(){
		this.setType(BlockType.Empty);
		this.setIsCleared(false);
	}
	
	/*
	public void applyPhysics(){
		// - Asks gameboard what is below it
		// - If gameboard returns 'Empty'
			// - signal gameboard to move it's position
	}
	*/
	
	/*
		* If there is nothing below this block, fall downwards
	*/
	public void ApplyGravity(){
		
	}
	
	public override void _Process(float delta){
		this.ApplyGravity();
	}
	
	
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
