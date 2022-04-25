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
public class BlockManager : Node2D
{
	public const int PRINT_LOOP = 50;
	private int curr_loop = 0;
	
	/*
		* The number of rows the game grid will have
	*/
	[Export]
	public int num_rows = 0;
	
	/*
		* The number of columns the game grid will have
	*/
	[Export]
	public int num_cols = 0;
	
	/*
		* The size of the grid block in pixels
	*/
	[Export]
	public int blockSize = 100;
	
	public const int DEFAULT_ROWS = 9;
	public const int DEFAULT_COLS = 6;
	
	private int widthOfGrid;
	private int heightOfGrid;
	
	// The scene of a grid block
	PackedScene blockScene = GD.Load<PackedScene>("res://Scenes/Blocks/Block.tscn");
	
	// Where the cursor is currently positioned
	// Includes this position and the position to the right of it
	private int cursorPosition;
	
	private Vector2 touchPosition;
	
	// the current cursor position is going to be converted into a gameGrid position
	// 
	
	// public signal swap_block(row, col)
	// public signal setup_level(arrOfGrid);
	
	
	//private GridBlock[,] gameGrid = new GridBlock[NUM_ROWS, NUM_COLS]; 
	private GridBlock[,] gameGrid;

	public BlockManager(){

	}

	public override void _Ready()
	{
		// allocate the appropriate memory
		this.AllocatedGameGrid();
		this.UpdateSizeOfGrid();
		
		// add the blocks to the array
		this.setBlock(8,5,BlockType.Diamond);
		this.setBlock(8,4,BlockType.Star);
		this.setBlock(8,3,BlockType.Diamond);
		this.setBlock(8,2,BlockType.Star);
		this.setBlock(8,1,BlockType.Star);
		this.setBlock(8,0,BlockType.Square);
		this.setBlock(7,3,BlockType.Diamond);
		
		// populate the blocks on the game grid
		this.PopulateBlocks();
	}
	
	
	public override void _Process(float delta){
		this.TouchInput();
		
		//this.curr_loop++;

		this.dropBlocks();
		this.clearBlocks();
		
		if (this.curr_loop == PRINT_LOOP){
			this.printGrid();
			this.curr_loop = 0;	
		}
	}
	

	
	public Vector2 PixelToArray(Vector2 position){
		double row = -1;
		double col = -1;
		
		if (IsInGrid(position)){
			row = Math.Floor(position.x / this.blockSize);
			col = Math.Floor(position.y / this.blockSize);		
		}
		
		return new Vector2((float)row, (float)col);
	}
	
	public Vector2 ArrayToPixel(int row, int col){
		float new_row = row * this.blockSize;
		float new_col = col * this.blockSize;
		return new Vector2(new_col, new_row);
	}
	
	
	public bool IsInGrid(Vector2 position){
		bool toReturn = false;
		int xoob = this.num_cols * this.blockSize;
		int yoob = this.num_rows * this.blockSize;
		if (
			position.x >= 0 && position.x <  xoob
			&& position.y>= -1 && position.y < yoob
		){
			toReturn = true;
		}
		return toReturn;
	}
	
	public void TouchInput(){
		if (Input.IsActionJustPressed("ui_touch")){
			touchPosition = GetViewport().GetMousePosition();
		
			Vector2 gridPosition = Position;
			touchPosition = ToLocal(touchPosition);
			
			Vector2 touchedIndex = PixelToArray(touchPosition);
			GD.Print(touchedIndex);
			
			// get the block
			BlockType thisBlock = this.getBlock((int)touchedIndex.y, (int)touchedIndex.x);
			
			// swap the blocks
			this.SwapBlocks(touchPosition);
			
			GD.Print(thisBlock);
			
		}
	}
	
	
	
	/*
		* Set the size of the grid visually based on how large
		* the game grid is
	*/
	public void UpdateSizeOfGrid(){
		// calculate the width
		this.widthOfGrid = this.num_cols * this.blockSize;
		
		// calculate the height
		this.heightOfGrid = this.num_rows * this.blockSize;
		
		var newPosition = new Vector2(this.widthOfGrid,this.heightOfGrid);
		
		// set the size of the grid
		var ColorGrid = (ColorRect)GetNode("ColorRect");
		
		ColorGrid.SetSize(newPosition);
		GD.Print(newPosition);
	}
	
	public void setBlock(int row, int col, BlockType type){
		gameGrid[row,col].setType(type);
	}
	
	public BlockType getBlock(int row, int col){
		if (IsInGrid(new Vector2((float)col, (float)row))){
			return gameGrid[row,col].getType();
		}
		else {
			return BlockType.Empty;
		}
	}
	
	public void SwapBlocks(Vector2 position){
		// get the array index
		var selectedBlockPosition = PixelToArray(position);
		// ensure we are not out of bounds
		if (IsInGrid(position) && CanSwap((int)selectedBlockPosition.y, (int)selectedBlockPosition.x)){
			// get the block positions in the array
			// get the block to swap with
			var blockToSwapPosition = selectedBlockPosition;
			blockToSwapPosition.x += 1;

			// get the reference to the block in the grid
			var selectedBlock = this.gameGrid[(int)selectedBlockPosition.y, (int)selectedBlockPosition.x];
			var blockToSwap = this.gameGrid[(int)blockToSwapPosition.y, (int)blockToSwapPosition.x];
			
			SwapBlocksInArray((int)selectedBlockPosition.y, (int)selectedBlockPosition.x);
			
			// move selected block
			if (selectedBlock.getType() != BlockType.Empty){
				Vector2 selectedToMove = new Vector2((float)selectedBlockPosition.x * this.blockSize,
													(float)selectedBlockPosition.y * this.blockSize);
				selectedToMove.x += this.blockSize;
				selectedBlock.Move(selectedToMove);
			}
			
			// move the block to swap
			if (blockToSwap.getType() != BlockType.Empty){
				Vector2 swapToMove = new Vector2((float)blockToSwapPosition.x * this.blockSize,
														(float)blockToSwapPosition.y * this.blockSize);
				swapToMove.x -= this.blockSize;
				blockToSwap.Move(swapToMove);
			}
		}
		
		
		
	}
	
	public void SwapBlocksInArray(int row, int col){
		// check if we can swap the blocks (Can't swap if the blocks have been cleared)
		//this.checkSwap(row, col);
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
		* Returns false if can't swap (Out of bounds, garbage blocks, etc.)
	*/
	public bool CanSwap(int row, int col){
		// is the next position out of bounds?
		if (col+1 >= this.num_cols){
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
	
	
	/*
		* Allocate space for the Game Grid
	*/
	public void AllocatedGameGrid(){
		// if no size has been specified, use the default number of rows and cols
		if (this.num_rows == 0){
			this.num_rows = DEFAULT_ROWS;
		}
		if (this.num_cols == 0){
			this.num_cols = DEFAULT_COLS;
		}

		// allocated the gamegrid
		this.gameGrid = new GridBlock[this.num_rows, this.num_cols];
		
		for (int i = 0; i < this.num_rows; i++){
			for (int j = 0; j < this.num_cols; j++){
				gameGrid[i,j] = new GridBlock();
			}
		}
	}
	
	
	/*
		* Go through the game grid array,
		* populate the blocks stored in memory to visual game board
	*/
	private void PopulateBlocks(){
		// go through the game grid array
		for (int i = this.num_rows -1; i >= 0; i--){
			for (int j = 0; j < this.num_cols; j++){
				// if there is a block in this position,
				// populate it on the game board
				if (this.gameGrid[i,j].getType() != BlockType.Empty){
					// get the pixel coordinates
					Vector2 gridPixelPosition = this.ArrayToPixel(i,j);
					
					GD.Print(gridPixelPosition);
					
					// Add a new block
					var blockInstance = (GridBlock)blockScene.Instance();
					blockInstance.setType(this.gameGrid[i,j].getType());
					AddChild(blockInstance);
					blockInstance.Position = gridPixelPosition;
					
					// set the size of this block
					blockInstance.SetSize(this.blockSize);
					
					this.gameGrid[i,j] = blockInstance;
				}

			}
		}
		
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
