/* This is the floor controller which automatically generate maps in the project "Undercover". 
	Undercover is the first project that I have done with Unity3D. It is meaningful to me.

	This game is a 2.5D multiplayer PvP game in C# with Unity, about fighting the other players while hiding
	in a bunch of AIs. ​Applied knowledge of A* pathfinding, modelling with Blender
	● Build AI players: By applying different index of probability to move or attack, AI will behave
	with different aggressiveness.
	● Music beat detect: Used an open source library to detect the music beat and synchronize all the
	movements of models to make the AI more humanoid.*/

using UnityEngine;
using System.Collections;

public class floor_controller : MonoBehaviour {

	public int size_x =31;
	/**odd number***/
	public int size_y=10;

	public int map;
	//player1
	//player2
	public GameObject env1;
	public GameObject env2;
	public GameObject env3;
	public GameObject block;
	public GameObject block2;
	public GameObject block3;
	public GameObject function_block;
	public GameObject trap_block;
	public GameObject no_pass_block;
	public GameObject edge_block;
	public GameObject respwaning_block;
	public GameObject diamond_block;
	public GameObject wall;
	public int block_size;
	public int[][] block_array;
	public blockController[][] floor_array;
	public blockController[] diamond_array;
	public blockController[] main_respwaning_pool;
	public int number_of_diamond = 3;
	public game_controller game;
	public int number_of_no_pass = 2;
	public int number_of_wall = 4;
	public int number_of_function = 3;
	public int number_of_trap = 1;
	public int number_of_spawning_point = 3;
	/**min is 2**/
	/**
	0 normal block
	1 function block
	2 trap block 
	3 no pass lock
	4 spwaning point
	5 daimond	
	6 wall**/

	// Use this for initialization

	private void init_map(){
        if(map == 1 || map == 2)
        {
            number_of_diamond = 1;
            number_of_spawning_point = 1;
        }
		int middle_line = size_x / 2 + 1;
		main_respwaning_pool = new blockController[2];
		diamond_array = new blockController[number_of_diamond];
		block_array =new int[size_x][];
		if (map == 0) {
			/**standar map with a no pass middle line**/
			for (int line = 0; line < size_x; line++) {
				block_array [line] = new int[size_y];
				for (int roll = 0; roll < size_y; roll++) {
					/**init**/
					block_array [line] [roll] = 0;
				} 
			}
			/***spwanning point***/
			block_array [size_x-1] [1] = 4;
			block_array [size_x-1] [5] = 4;
			block_array [size_x-1] [9] = 4;
			block_array [0] [1] = 4;
			block_array [0] [5] = 4;
			block_array [0] [9] = 4;
			/***no_pass in the middle line***/
			for(int i=0;i<size_y;i++){
				block_array [middle_line] [i] = 3;
			}
			/***diamond***/
			for(int i=0;i<number_of_diamond;i++){
				while(!assign_block4diamond(middle_line,Random.Range(0,size_y))){
				}
			}
			/****function****/
			for(int i=0;i<number_of_function;i++){
				while(!assign_block2(Random.Range(1,middle_line-1),Random.Range(0,size_y),1)){
				}
			}
			for(int i=0;i<number_of_function;i++){
				while(!assign_block2(Random.Range(middle_line+1,size_x-1),Random.Range(0,size_y),1)){
				}
			}

			/***trap**/
			for(int i=0;i<number_of_trap;i++){
				while(!assign_block2(Random.Range(1,middle_line-1),Random.Range(0,size_y),2)){
				}
			}
			for(int i=0;i<number_of_trap;i++){
				while(!assign_block2(Random.Range(middle_line+1,size_x-1),Random.Range(0,size_y),2)){
				}
			}
			/***no pass***/
		
			for(int i=0;i<number_of_no_pass;i++){
				while(!no_adj_wall_nopass_assign_block2(Random.Range(1,middle_line-1),Random.Range(0,size_y),3)){
				}
			}
			for(int i=0;i<number_of_no_pass;i++){
				while(!no_adj_wall_nopass_assign_block2(Random.Range(middle_line+1,size_x-1),Random.Range(0,size_y),3)){
				}
			}
			/***wall**/
			for(int i=0;i<number_of_wall;i++){
				while(!no_adj_wall_nopass_assign_block2(Random.Range(1,middle_line-1),Random.Range(0,size_y),6)){
				}
			}
			for(int i=0;i<number_of_wall;i++){
				while(!no_adj_wall_nopass_assign_block2(Random.Range(middle_line+1,size_x-1),Random.Range(0,size_y),6)){
				}
			}
	

		} else if (map == 1) {
			for (int line = 0; line < size_x; line++) {
				block_array [line] = new int[size_y];
				for (int roll = 0; roll < size_y; roll++) {

					block_array [line] [roll] = 0;
				} 

			}
            block_array[0][size_y/2] = 4;
            block_array[size_x-1][size_y / 2] = 4;
            block_array[size_x/2][size_y / 2] = 5;
            //block_array[][]

        }
        else if (map == 2)
        {
            for (int line = 0; line < size_x; line++)
            {
                block_array[line] = new int[size_y];
                for (int roll = 0; roll < size_y; roll++)
                {

                    block_array[line][roll] = 0;
                }

            }
            block_array[0][size_y / 2] = 4;
            block_array[size_x - 1][size_y / 2] = 4;
            block_array[size_x / 2][size_y / 2] = 5;
            //block_array[][]
            /*for (int i = 0; i < number_of_trap; i++)
            {
                while (!assign_block2(Random.Range(1, middle_line - 1), Random.Range(0, size_y), 2))
                {
                }
            }
            for (int i = 0; i < number_of_trap; i++)
            {
                while (!assign_block2(Random.Range(middle_line + 1, size_x - 1), Random.Range(0, size_y), 2))
                {
                }
            }*/
            int wallRn = Random.Range(0, 3);
            if(wallRn == 1)
            {
                /***no pass***/

                for (int i = 0; i < number_of_no_pass; i++)
                {
                    while (!no_adj_wall_nopass_assign_block2(Random.Range(1, middle_line - 1), Random.Range(0, size_y), 3))
                    {
                    }
                }
                for (int i = 0; i < number_of_no_pass; i++)
                {
                    while (!no_adj_wall_nopass_assign_block2(Random.Range(middle_line + 1, size_x - 1), Random.Range(0, size_y), 3))
                    {
                    }
                }
            }
            else if(wallRn == 0)
            {
                /***wall**/
                for (int i = 0; i < number_of_wall; i++)
                {
                    while (!no_adj_wall_nopass_assign_block2(Random.Range(1, middle_line - 1), Random.Range(0, size_y), 6))
                    {
                    }
                }
                for (int i = 0; i < number_of_wall; i++)
                {
                    while (!no_adj_wall_nopass_assign_block2(Random.Range(middle_line + 1, size_x - 1), Random.Range(0, size_y), 6))
                    {
                    }
                }
            }
        }

    }
	public void get_position_diamond_n(int[] re,int n){
        if(diamond_array[n] != null)
        {
            re[0] = diamond_array[n].x;
            re[1] = diamond_array[n].y;
        }
		return;
	}
	public int[] get_position_diamond_random(int[] re){
		this.get_position_diamond_n(re,Random.Range(0,this.number_of_diamond));
		//print("position diamond: x"+re[0]+" y "+re[1]);
		return re;
	}
	public int[] get_main_respawning(bool side,int[] re){
		if (side) {
			/**when side is true is white which is on the left side***/
			re [0] = main_respwaning_pool[1].x;
			re [1] = main_respwaning_pool[1].y;
			return re;
		} else {
		/**when side is flase is black which is on the right side**/
			re [0] = main_respwaning_pool [0].x;
			re [1] = main_respwaning_pool [0].y;
			return re;
				}
			}
	public void generate_blocks () {
		if (size_x % 2 == 0)
			size_x++;
		/**make x to odd number**/
		//GameObject block_array[size_x][size_y];
		floor_array =new blockController[size_x][];
		for (int i = 0; i < size_x; i++) {
			floor_array [i] = new blockController[size_y];
		}
		//presetMap

		init_map ();
		//generate block by array
		int middle_line = size_x / 2 + 1;
		for (int line = 0; line < size_x; line++) {
			/**this is the counter for the mid respawnning pool**/
			int counter_mid_respawning_pool = number_of_spawning_point / 2 +1;
			int counter_diamond = number_of_diamond;
			for (int roll = 0; roll < size_y; roll++) {
				if (block_array [line] [roll] == 0) {
					int r = Random.Range (0, 101);
					blockController b;
					if(r<10)
					 b = ((GameObject)Instantiate (block, new Vector3 (line * block_size, -1, roll * block_size), Quaternion.identity)).GetComponent<blockController> ();
					else if(r<20)
					 b = ((GameObject)Instantiate (block2, new Vector3 (line * block_size, -1, roll * block_size), Quaternion.identity)).GetComponent<blockController> ();
					else
					 b = ((GameObject)Instantiate (block3, new Vector3 (line * block_size, -1, roll * block_size), Quaternion.identity)).GetComponent<blockController> ();
					b.x = line;
					b.y = roll;
				//	print ("init map x " + line + " y " + roll);
					floor_array [line] [roll] = b;
					b.type = block_array [line] [roll];
					b.transform.parent = this.transform;
				} else if (block_array [line] [roll] == 1) {
					blockController b = ((GameObject)Instantiate (function_block, new Vector3 (line * block_size, -1, roll * block_size), Quaternion.identity)).GetComponent<blockController> ();
					b.x = line;
					b.y = roll;floor_array [line] [roll] = b;b.transform.parent = this.transform;
					b.type = block_array [line] [roll];
				} else if (block_array [line] [roll] == 2) {
					blockController b = ((GameObject)Instantiate (trap_block, new Vector3 (line * block_size, -1, roll * block_size), Quaternion.identity)).GetComponent<blockController> ();
					b.x = line;
					b.y = roll;floor_array [line] [roll] = b;b.transform.parent = this.transform;b.type = block_array [line] [roll];
				} else if (block_array [line] [roll] == 3) {
					blockController b = ((GameObject)Instantiate (no_pass_block, new Vector3 (line * block_size,-1, roll * block_size), Quaternion.identity)).GetComponent<blockController> ();
					b.x = line;
					b.y = roll;floor_array [line] [roll] = b;b.transform.parent = this.transform;b.type = block_array [line] [roll];
				} else if (block_array [line] [roll] == 4) {
					blockController b;
					if(line<middle_line)
						b = ((GameObject)Instantiate (respwaning_block, new Vector3 (line * block_size,-1, roll * block_size),  Quaternion.Euler(0f, -90f, 0f))).GetComponent<blockController> ();
					else 
						b = ((GameObject)Instantiate (respwaning_block, new Vector3 (line * block_size,-1, roll * block_size),  Quaternion.Euler(0f, 90f, 0f))).GetComponent<blockController> ();
					
					b.x = line;
					b.y = roll;
					floor_array [line] [roll] = b;b.type = block_array [line] [roll];
                    if(map == 1 || map == 2)
                    {
                        main_respwaning_pool[1] = floor_array[0][roll];
                        main_respwaning_pool[0] = floor_array[size_x - 1][roll];
                    }
                    else
                    {
                        if (line == 0 && counter_mid_respawning_pool == 1)
                        {
                            /**if the left side(true)**/
                            main_respwaning_pool[1] = floor_array[line][roll];
                        }
                        else if (
                          line == size_x - 1 && counter_mid_respawning_pool == 1)
                        {
                            main_respwaning_pool[0] = floor_array[line][roll];
                        }
                    }
					
					b.transform.parent = this.transform;
					counter_mid_respawning_pool--;

				} else if (block_array [line] [roll] == 5) {
					
					blockController b = ((GameObject)Instantiate (diamond_block, new Vector3 (line * block_size,-1, roll * block_size), Quaternion.identity)).GetComponent<blockController> ();

					b.x = line;
					b.y = roll;
					floor_array [line] [roll] = b;
					diamond_array [counter_diamond - 1] = floor_array [line] [roll];b.type = block_array [line] [roll];
					counter_diamond--;
					b.transform.parent = this.transform;
				} else if (block_array [line] [roll] == 6) {
					blockController b = ((GameObject)Instantiate (wall, new Vector3 (line * block_size,-1, roll * block_size), Quaternion.identity)).GetComponent<blockController> ();
					b.x = line;
					b.y = roll;
					floor_array [line] [roll] = b;
					b.transform.parent = this.transform;b.type = block_array [line] [roll];
				} 

			}
		}
		/**build edge block***/
		for (int line = -1; line < size_x + 1; line++) {
			GameObject b = (GameObject)Instantiate (edge_block, new Vector3 (line * block_size, -1, -1 * block_size), Quaternion.identity);
			b.transform.parent = this.transform;
			b.tag = "edge";
			b = (GameObject)Instantiate (edge_block, new Vector3 (line * block_size, -1, (size_y) * block_size), Quaternion.identity);
			b.transform.parent = this.transform;b.tag = "edge";
		}
		for(int roll =0;roll<size_y;roll++){
			GameObject b = (GameObject)Instantiate (edge_block, new Vector3 (-1 * block_size, -1, roll * block_size), Quaternion.identity);
			b.transform.parent = this.transform;b.tag = "edge";
			b = (GameObject)Instantiate (edge_block, new Vector3 ((size_x) * block_size, -1, roll * block_size), Quaternion.identity);
			b.transform.parent = this.transform;b.tag = "edge";
		}
		/**build env**/
		for (int line = 0-5; line < size_x+5 + 1; line++) {
			for(int roll =0-7;roll<size_y+7;roll++){
				if (!(roll > -2 && roll < size_y + 1 && line > -2 && line < size_x + 1)) {
					int k = Random.Range (0, 101);
					GameObject b;
					if (k < 10)
						b = (GameObject)Instantiate (env3, new Vector3 (line * block_size, -1, roll * block_size), Quaternion.identity);
					else if (k < 10)
						b = (GameObject)Instantiate (env2, new Vector3 (line * block_size, -1, roll * block_size), Quaternion.identity);
					else
						b = (GameObject)Instantiate (env1, new Vector3 (line * block_size, -1, roll * block_size), Quaternion.identity);

					b.transform.parent = this.transform;

				}
			}

		}

	}
	public int get_x_spawning(bool side, int playerNum){
		if(side==false){ return size_x-1;
		}else{
			return 0;

		}

	}

	public int get_y_spawning(bool side, int playerNum){
        if (map == 2 || map == 1)
        {
            return size_y / 2;
        }
        else {
            if (side == false)
            {
                return 1 + playerNum * 4;
            }
            else {
                return 1 + playerNum * 4;

            }
        }

	}
	public Vector3 spwaning_point(bool side, int playerNum){
        if (map == 2 || map == 1)
        {
            if (side == false)
            {
                return getPosition(size_x - 1, size_y/2);
            }
            else {
                return getPosition(0, size_y/2);

            }
        }
        else {
            if (side == false)
            {
                return getPosition(size_x - 1, 1 + playerNum * 4);
            }
            else {
                return getPosition(0, 1 + playerNum * 4);

            }
        }
	}
	public Vector3 getPosition (int line,int roll){
		//print (roll);
		//print ("getposition: " + line + "  " + roll);
		return floor_array [line] [roll].transform.position;

	}
		

	public void updatePlayer2block(PlayerControl player, int x, int y){
		/**update the player to the block **/
	//	print ("update palyer 2 block x " + x + " y " + y);
		floor_array [x] [y].player = player;

	}
	public void updateBlock2NULL(int x,int y){
		floor_array [x] [y].player = null;
	}
	public bool check_enemy(bool side,int x,int y,int range){
	//	print ("start check ebnemy"+range);
		if (side) {
			/**left side ***/
			for (int i = 1; i <= range && x + i < size_x; i++) {
				//print("inside the left loop");
				if (floor_array[x+i][y].player!=null&&!floor_array [x+i] [y ].player.side)
					//print ("return true");
					return true;
				if (block_array [x+i] [y ] == 6) {
					return false;
				}
			}
			return false;
		} else {
			/**right side ***/
			for (int i = 1; i <= range && x -i >=0; i++) {
				//print("inside the left loop");
				if (floor_array[x-i][y].player!=null&& floor_array [x-i] [y ].player.side )
					//print ("return true");
					return true;
				if (block_array [x-i] [y ] == 6) {
					return false;
				}
			}
			return false;
		}		
	}
	public void check_blocks(){
		/**check diamond**/
		////
		/// check every trun for some block reaction
		for (int i = 0; i < number_of_diamond; i++) {
			diamond_array [i].check_effect ();
		
		}
		for (int i = 0; i < 2; i++) {
            print(main_respwaning_pool[0]);
			main_respwaning_pool [i].check_effect ();
		}
	}
	public bool check_moveable(int side,int x,int y){
		int t=0;
		/**check range**/

		if (side == 1) {
			/**up**/
			if (y + 1 >= size_y)
				return false;
			t = floor_array [x] [y + 1].type;
		} else if (side == 2) {
			if (y - 1 < 0)
				return false;
			t = floor_array [x] [y - 1].type;

		} else if (side == 3) {
			if (x - 1 < 0)
				return false;
			t = floor_array [x - 1] [y].type;
		} else if (side == 4) {
			if (x + 1 >= size_x)
				return false;
			t = floor_array [x + 1] [y].type;
		}
		if (t == 3 || t == 6)
			return false;

			return true;
	}
	public bool check_spy(int side,int x,int y){
		PlayerControl t = null;
		if (side == 1&&y+1<size_y) {
			/**up**/
			t = floor_array [x] [y + 1].player;
		} else if (side == 2&&y-1>=0) {
			t = floor_array [x] [y - 1].player;

		} else if (side == 3&&x-1>=0) {
			t = floor_array [x - 1] [y].player;
		} else if (side == 4&&x+1<size_x) {
			t = floor_array [x + 1] [y].player;
		}
		if (t != null)
			return false;
		return true;
	}
	private bool no_adj_wall_nopass_assign_block2(int x,int y,int type){
		//print ("x " + x + " y " + y);
		if (x==0||x==size_x-1||y==0||y==size_y-1||
			block_array[x][y]!=0||
			block_array [x + 1] [y] == 3 ||
			block_array [x + 1] [y] == 6 ||

			block_array [x - 1] [y] == 3 ||
			block_array [x - 1] [y] == 6 ||

			block_array [x + 1] [y+1] == 3 ||
			block_array [x + 1] [y+1] == 6 ||
			block_array [x + 1] [y-1] == 3 ||
			block_array [x + 1] [y-1] == 6 ||
			block_array [x - 1] [y-1] == 3 ||
			block_array [x - 1] [y-1] == 6 ||
			block_array [x - 1] [y+1] == 3 ||
			block_array [x - 1] [y+1] == 6 ||
			block_array [x  ] [y] == 3 ||
			block_array [x ] [y] == 6 ||
			block_array [x ] [y+1] == 3 ||
			block_array [x ] [y+1] == 6 ||

			block_array [x ] [y-1] == 3 ||
			block_array [x ] [y-1] == 6 
		)
			return false;
		block_array [x] [y] = type;
		return true;
	}
	private bool assign_block2(int x, int y,int type){
		if (block_array [x] [y] == 0) {
			block_array [x] [y] = type;
			return true;
		}
		return false;
	}
	private bool assign_block4diamond(int x,int y){
		if (block_array [x] [y] == 3) {
			block_array[x][y] = 5;
			return true;
		}return false;
	}
	public int detect_wall_nopass(int x,int y){
		if (y-1>=0&&(block_array [x] [y - 1] == 3 ||
			block_array [x] [y - 1] == 6))
			return 2;
		else if (y+1<size_y&&(block_array [x] [y + 1] == 3 ||
			block_array [x] [y + 1] == 6))
			return 1;
		else if(x-1>=0&& (block_array [x - 1] [y] == 3 ||
			block_array [x - 1] [y] == 6))
			return 3;
		else if (x+1<size_x&&(block_array [x + 1] [y] == 3 ||
			block_array [x + 1] [y] == 6))
			return 4;
		//print ("error in detect_wall");
		return 0;
	}
	public int detect_minion(int x,int y){
		if (y-1>=0&&floor_array[x][y-1].player!=null)
			return 2;
		else if (y+1<size_y&& floor_array[x][y+1].player!=null)
			return 1;
		else if (x-1>=0 &&floor_array[x-1][y].player!=null)

			return 3;
		else if (x+1<size_x&&floor_array[x+1][y].player!=null)

			return 4;
	//	print ("error in detect_wall");
		return 0;
	}

}
