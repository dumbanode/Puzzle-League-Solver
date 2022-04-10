using Godot;
using System;


public enum BlockType {
	Square,
	Diamond,
	Up,
	Down, 
	Heart,
	Star,
	Garbage,
	Empty
}

/*
	* Block Manager
	* This class handles the management of blocks for the game.
	* This includes the game grid, applying physics, swapping blocks, clearing blocks, etc.
*/
public class BlockManager : Node
{
	public const int PRINT_LOOP = 50;
	private int curr_loop = 0;
	
	public int num_rows;
	public int num_cols;
	
	public const int DEFAULT_ROWS = 9;
	public const int DEFAULT_COLS = 6;
	
	// public signal swap_block(row, col)
	// public signal setup_level(arrOfGrid);
	
	
	//private GridBlock[,] gameGrid = new GridBlock[NUM_ROWS, NUM_COLS]; 
	private GridBlock[,] gameGrid;

	public BlockManager(){
		this.num_rows = DEFAULT_ROWS;
		this.num_cols = DEFAULT_COLS;
		
		this.gameGrid = new GridBlock[this.num_rows, this.num_cols];
		
		for (int i = 0; i < this.num_rows; i++){
			for (int j = 0; j < this.num_cols; j++){
				gameGrid[i,j] = new GridBlock();
			}
		}
		
		this.setBlock(8,2,BlockType.Star);
	}

	public override void _Ready()
	{
	}

	
	public void update(float delta){
		this.curr_loop++;

		this.dropBlocks();
		this.clearBlocks();
		
		if (this.curr_loop == PRINT_LOOP){
			this.printGrid();
			this.curr_loop = 0;	
		}
	}
	
	public void setBlock(int row, int col, BlockType type){
		gameGrid[row,col].setType(type);
	}
	
	public void swapBlocks(int row, int col){
		// check if we can swap the blocks (Can't swap if the blocks have been cleared)
		this.checkSwap(row, col);
		// ensure that col+1 doesn't go outside the number of columns
		if (col + 1 < this.num_cols){
			GridBlock tmp = this.gameGrid[row,col];
			this.gameGrid[row,col] = this.gameGrid[row, col+1];
			this.gameGrid[row, col+1] = tmp;
		}
	}
	
	/*
		* Check if we can swap the blocks
		* Blocks can't be swap if either one of the blocks are cleared
	*/
	public bool checkSwap(int row, int col){
		if (this.gameGrid[row,col].getIsCleared() || this.gameGrid[row,col+1].getIsCleared()){
			return false;
		}
		return true;
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
	
	/*
		* Go through each block
		* If the space below them is empty, move them down
	*/
	public bool dropBlocks(){
		// if at any point we moved blocks, return true
		bool movedBlocks = false;
		
		for (int i = this.num_rows -1; i >= 0; i--){
			for (int j = 0; j < this.num_cols; j++){
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
	
	
	/*
		* Blocks can be cleared in two ways
		* There are 3 or more blocks of the same type in a row vertically
		* There are 3 or more blocks of the same type in a row horizontally
		*
		* Go through each block and find all the blocks that are the same in a row
		
	*/
	public bool markBlocksClear(){
		bool clearedBlocks = false;
		for (int i = 0; i < this.num_rows; i++) {
			for (int j = 0; j < this.num_cols; j++){
				
				// --- Check Horizontally --
				BlockType currBlock = this.gameGrid[i,j].getType();
				int currCol = j + 1;
				
				// ii will track how many in a row are the same
				int ii = 1;
				while (currCol < this.num_cols){
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
					if (( !matched || currCol == this.num_cols -1)) {
						
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
				while (currRow < this.num_rows){
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
					if (( !matched || currRow == this.num_rows -1)) {
						
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
	
	public bool clearBlocks(){
		// go through each block and mark them clear if necessary
		bool clearedBlocks = this.markBlocksClear();
		
		for (int i = 0; i < this.num_rows; i++) {
			for (int j = 0; j < this.num_cols; j++){
				if (this.gameGrid[i,j].getIsCleared()){
					this.gameGrid[i,j].clear();
				}
			}
		}
		
		return clearedBlocks;
		// go through each block mark them empty
	}
	

	public void printGrid(){
		string toPrint = "";
		for (int i = 0; i < this.num_rows; i++){
			toPrint += i + "------------------------------------------\n";
			for (int j = 0; j < this.num_cols; j++){
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
