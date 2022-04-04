using Godot;
using System;

public class GameState : Node
{
	
	public const int NUM_ROWS = 9;
	public const int NUM_COLS = 6;
	
	private GridBlock[,] gameGrid = new GridBlock[NUM_ROWS, NUM_COLS]; 

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		for (int i = 0; i < NUM_ROWS; i++){
			for (int j = 0; j < NUM_COLS; j++){
				gameGrid[i,j] = new GridBlock();
			}
		}
		

		
		gameGrid[8,3].setType(BlockType.Heart);
		gameGrid[7,3].setType(BlockType.Heart);
		gameGrid[6,3].setType(BlockType.Heart);
		gameGrid[5,3].setType(BlockType.Heart);
		
		//gameGrid[8,0].setType(BlockType.Up);
		//gameGrid[8,1].setType(BlockType.Up);
		
		
		//this.swapBlocks(6,2);

		while (true){
			this.applyPhysics();
			bool clearedBlocks = this.clearBlocks();
			if (!clearedBlocks){
				break;
			}
		}
		
		this.printGrid();
	}
	
	public void swapBlocks(int row, int col){
		// ensure that col+1 doesn't go outside the number of columns
		if (col + 1 < NUM_COLS){
			GridBlock tmp = this.gameGrid[row,col];
			this.gameGrid[row,col] = this.gameGrid[row, col+1];
			this.gameGrid[row, col+1] = tmp;
		}
	}

	/*
		* Loop through the gamegrid from the bottom
		* if the space is empty, shift the entire column down by one block
	*/	
	public void applyPhysics(){
		while(true){
			bool movedBlocks = this.dropBlocks();
			if (!movedBlocks){
				break;
			}
		}
	}
	
	public bool clearBlocks(){
		// go through each block and mark them clear if necessary
		bool clearedBlocks = this.markBlocksClear();
		
		for (int i = 0; i < NUM_ROWS; i++) {
			for (int j = 0; j < NUM_COLS; j++){
				if (this.gameGrid[i,j].getIsCleared()){
					this.gameGrid[i,j].clear();
				}
			}
		}
		
		return clearedBlocks;
		// go through each block mark them empty
	}
	
	/*
		* Blocks can be cleared in two ways
		* There are 3 or more blocks of the same type in a row vertically
		* There are 3 or more blocks of the same type in a row horizontally
		*
		* Go through each block and find all the blocks that are the same in a row
		
	*/
	public bool markBlocksClear(){
		bool clearedBlocks = false;
		for (int i = 0; i < NUM_ROWS; i++) {
			for (int j = 0; j < NUM_COLS; j++){
				
				// --- Check Horizontally --
				BlockType currBlock = this.gameGrid[i,j].getType();
				int currCol = j + 1;
				
				// ii will track how many in a row are the same
				int ii = 1;
				while (currCol < NUM_COLS){
					BlockType this_block = this.gameGrid[i,currCol].getType();
					if (
						this_block == currBlock 
						&& this_block != BlockType.Empty
					){
						ii++;
					}
					else if (ii > 2) {
						clearedBlocks = true;
						for (int k = j; k < (j+ii); k++){
							this.gameGrid[i, k].markClear();
						}
					}
					currCol++;
				}
				
				// --- Check Vertically ---
				int currRow = i + 1;
				// ii will track how many in a row are the same
				ii = 1;
				while (currRow < NUM_ROWS){
					BlockType this_block = this.gameGrid[currRow,j].getType();
					if (
						this_block == currBlock 
						&& this_block != BlockType.Empty
					){
						ii++;
					}
					else if (ii > 2) {
						GD.Print("Clearing Vertically");
						clearedBlocks = true;
						for (int k = j; k < (j+ii); k++){
							this.gameGrid[k, j].markClear();
						}
					}
					currRow++;
				}
				
			}
		}
		
		return clearedBlocks;
	}
	
	public bool dropBlocks(){
		// if at any point we moved blocks, return true
		bool movedBlocks = false;
		
		for (int i = NUM_ROWS -1; i >= 0; i--){
			for (int j = 0; j < NUM_COLS; j++){
				// if the space is empty, shift the column down
				BlockType currType  = this.gameGrid[i,j].getType();
				if (currType == BlockType.Empty){
					for (int k = i; k >= 0; k--){
						if (k-1 >= 0){
							// if we are moving down a block that isn't empty,
							// we applied physics
							if (gameGrid[k-1, j].getType() != BlockType.Empty){
								movedBlocks = true;
							}
							this.gameGrid[k,j] = gameGrid[k-1, j];
						}
					}
				}
			}
		}
		return movedBlocks;
	}
	
	public void printGrid(){
		string toPrint = "";
		for (int i = 0; i < NUM_ROWS; i++){
			toPrint += i + "------------------------------------------\n";
			for (int j = 0; j < NUM_COLS; j++){
				if (j == 0){
					toPrint += "| ";
				}
				toPrint += this.gameGrid[i,j].print() + " | ";
			}
			toPrint += "\n";
		}
		toPrint += "-------------------------------------------\n";
		GD.Print(toPrint);
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}


