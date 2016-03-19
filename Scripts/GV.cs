using UnityEngine;
using System.Collections;

public class GV
{
    public enum PhermoneTypes { Friendly, Hostile };
	public enum ResourceTypes { Empty, Food, Water };
    public static readonly float PATH_DECAY_RATE = 5f;  //1 point every 5 seconds
    public static readonly float ANT_ENERGY_DECAY = 1f; //1 per second
    public static readonly float ANT_ENERGY_START = 100f;
    public static readonly float ANT_SPEED = 2f; 

	//ANT VARS
	public static readonly float ANT_CARRY_CAPACITY = 1f; 

	//Colony Scripts
	public static readonly float START_FOOD = 150;
	public static readonly float START_WATER = 150;
	public static readonly int START_ANTS = 100;
	public static readonly float DESIRED_PERCENT_DORMANT_ANTS = .6f; //0-1
	public static readonly int TIME_BETWEEN_ANT_EXITS = 5; //seconds

	//Food and Water
	public static readonly int Min_FoodBundles = 5;
	public static readonly int Max_FoodBundles = 25;
	public static readonly int Min_WaterPuddes = 5;
	public static readonly int Max_WaterPuddes = 25;
	public static readonly int TIME_BETWEEN_RESOURCE_UPDATES = 5; //seconds
	public static readonly float RESOURCE_GROWTH_PER_SECOND = 1f;
	public static readonly float MIN_WORLD_FOOD = 100f;
	public static readonly float MIN_WORLD_WATER = 100f;
	public static readonly Vector2 INITIAL_RESOURCE_RANGE = new Vector2(10,50);
	public static readonly float PERCENT_CHANCE_OF_POISON = 0.1f;
	public static readonly float QUANTITY_TO_VOLUMETIC_SCALE = 10f; //how much quantity of food = 1x1 volume in world


	//Map
	public static readonly float MAP_DIAMETER = 100;
	public static readonly int NUM_OF_ROCKS = 30;
	public static readonly Vector2 RANGE_OF_ROCK_SCALE = new Vector2 (0.5f, 5);


    public static Vector2 SubtractVectors(Vector2 v2, Vector3 v3)
    {
        return new Vector2(v2.x - v3.x, v2.y - v3.y);

    }
}