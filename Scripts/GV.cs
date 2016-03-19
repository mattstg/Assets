using UnityEngine;
using System.Collections;

public class GV
{
    public enum PhermoneTypes { Friendly, Hostile };
	public enum ResourceTypes { Empty, Food, Water };
    public static readonly float PATH_DECAY_RATE = 5f;  //1 point every 5 seconds
    public static readonly float ANT_ENERGY_DECAY = 1f; //1 per second
    public static readonly float ANT_ENERGY_START = 100f; 

	//ANT VARS
	public static readonly float ANT_CARRY_CAPACITY = 1f; 

	//Colony Scripts
	public static readonly int START_FOOD = 150;
	public static readonly int START_WATER = 150;
	public static readonly int START_ANTS = 100;
	public static readonly float DESIRED_PERCENT_DORMANT_ANTS = .5f; //0-1
	public static readonly int TIME_BETWEEN_ANT_EXITS = 5; //seconds
	public static readonly Vector2 INITIAL_RESOURCE_RANGE = new Vector2(10,50);

	//Food and Water
	public static readonly int Min_FoodBundles = 5;
	public static readonly int Max_FoodBundles = 15;
	public static readonly int Min_WaterPuddes = 5;
	public static readonly int Max_WaterPuddes = 15;
	public static readonly int TIME_BETWEEN_RESOURCE_UPDATES = 5; //seconds
	public static readonly float RESOURCE_GROWTH_PER_SECOND = 1f;

	//Map
	public static readonly float MAP_DIAMETER = 100;
}