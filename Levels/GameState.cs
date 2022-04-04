using Godot;
using System;

public class GameState : Node
{
	
	public const int NUM_ROWS = 9;
	public const int NUM_COLS = 6;
	
	private bool moveMade = false;
	private bool physicsLock = false;
	
	private GridBlock[,] gameGrid = new GridBlock[NUM_ROWS, NUM_COLS]; 

	public override void _Process(float delta)
	{

		this.applyPhysics();  
			
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		for (int i = 0; i < NUM_ROWS; i++){
			for (int j = 0; j < NUM_COLS; j++){
				gameGrid[i,j] = new GridBlock();
			}
		}
		

		
		gameGrid[8,2].setType(BlockType.Star);
		gameGrid[8,3].setType(BlockType.Square);
		gameGrid[8,4].setType(BlockType.Up);
		
		gameGrid[7,2].setType(BlockType.Heart);
		gameGrid[7,3].setType(BlockType.Heart);
		gameGrid[7,4].setType(BlockType.Star);
		
		gameGrid[6,2].setType(BlockType.Heart);
		gameGrid[6,3].setType(BlockType.Square);
		gameGrid[6,4].setType(BlockType.Up);
		
		gameGrid[5,3].setType(BlockType.Square);
		gameGrid[5,4].setType(BlockType.Up);
		
		gameGrid[4,3].setType(BlockType.Star);
		
		

		this.swapBlocks(7,3);
		this.swapBlocks(4,2);
		
		
		
		//this.swapBlocks(7,2);
		
		
		//this.swapBlocks(7,1);
		
		/*
		while (true){
			this.applyPhysics();
			bool clearedBlocks = this.clearBlocks();
			if (!clearedBlocks){
				break;
			}
		} 
		*/
		
		this.printGrid();
	}
	
	public void swapBlocks(int row, int col){
		// ensure that col+1 doesn't go outside the number of columns
		if (col + 1 < NUM_COLS){
			GridBlock tmp = this.gameGrid[row,col];
			this.gameGrid[row,col] = this.gameGrid[row, col+1];
			this.gameGrid[row, col+1] = tmp;
		}
		this.moveMade = true;
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
					
					bool matched = false;
					if (
						this_block == currBlock 
						&& this_block != BlockType.Empty
					){
						
						ii++;
						matched = true;
					}
					
					// If we are at the end of the grid OR this block wasn't matched
					// AND the number in a row that matched is more than two
					if (( !matched || currCol == NUM_COLS-1)) {
						
						if (ii > 2){						
							clearedBlocks = true;
							//GD.Print("Clearing Horizontally");
							for (int k = j; k < (j+ii); k++){
								this.gameGrid[i, k].markClear();
							}
						}
						break;
					}
					currCol++;
				}
				
				
				
				// --- Check Vertically ---
				int currRow = i + 1;
				// ii will track how many in a row are the same
				ii = 1;
				while (currRow < NUM_ROWS){
					BlockType this_block = this.gameGrid[currRow,j].getType();
					
					bool matched = false;
					if (
						this_block == currBlock 
						&& this_block != BlockType.Empty
					){
						//GD.Print("Comparing |" + i + ", " + j + "| - |" + currRow + ", " + j + "|");
						//GD.Print(this_block.ToString() + "==" + currBlock.ToString());
						ii++;
						matched = true;
					}
					
					// If we are at the end of the grid OR this block wasn't matched
					// AND the number in a row that matched is more than two
					if (( !matched || currRow == NUM_ROWS-1)) {
						
						if (ii > 2){
							
							//GD.Print("Clearing Vertically");
							clearedBlocks = true;
							//GD.Print("Clearing |" + i + ", " + j + "| - |" + (i+ii) + ", " + j + "|");
							for (int k = i; k < (i+ii); k++){
								//GD.Print("Marking " + k + ", " + j);
								this.gameGrid[k, j].markClear();
							}
						}
						break;
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

}


