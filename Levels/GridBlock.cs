using Godot;
using System;

public enum BlockType {
	Square,
	Diamond,
	Up,
	Down, 
	Heart,
	Garbage,
	Empty
}

public class GridBlock : Node
{

	private BlockType thisBlock;
	
	private bool isCleared = false;
	
		// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	public GridBlock(){
		this.thisBlock = BlockType.Empty;
	}
	
	
	public GridBlock(BlockType type){
		this.thisBlock = type;
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
	
	public void clear(){
		this.setType(BlockType.Empty);
		this.setIsCleared(false);
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
