using UnityEngine;
using System.Collections;

public class GV
{
    public enum directionQuadrants { q1, q2, q3, q4 };

	//ANT VARS
    public enum PhermoneTypes { Friendly, Hostile };
	public enum ResourceTypes { Empty, Food, Water };
    public static readonly float PATH_DECAY_RATE = 2f;  //1 point every 5 seconds
    public static readonly float ANT_ENERGY_DECAY = 0f; //1 per second
    public static readonly float ANT_ENERGY_START = 100f;
    public static readonly float ANT_SPEED = 1f;
    public static readonly int PHEROMONE_START_ENERGY = 8; //by this logic should last 30 seconds
    public static readonly float ANT_STATE_TIMER = 4f;
    public static readonly float PHEROMONE_PLACEMENT_ABSORPTION_RADIUS = 1f;
	public static readonly float DMG_FROM_CORPSES = 10f; //5dmg per second
    public static readonly int COLONY_NUM_SCOUT_SPAWN = 30;  //number of scouts spawned first

	public static readonly float ANT_CARRY_CAPACITY = 1f; 
	public static readonly float RESOURCE_TO_ENRGY = 20f; //+X energy per unity of food
	public static readonly float POISON_TO_ENRGY = 60f; //-X energy % hp per unity of poison

	//Colony Scripts
	public static readonly float COL_RESOURCE_DRAIN_DORMANT = 0.5f; //per X second
	public static readonly float COL_RESOURCE_DRAIN_TICK = 30f; //every X seconds <<<<< X
	public static readonly float ANT_DEATH_FROM_POISON = 10f; //flat death per quantity of poison brought back to colony
	public static readonly float START_FOOD = 500;
	public static readonly float START_WATER = 500;
	public static readonly int START_ANTS = 300;
	public static readonly float DESIRED_PERCENT_DORMANT_ANTS = .60f; //0-1
	public static readonly int ANT_EXIT_PER_SECOND = 5;
	public static readonly float TIME_BETWEEN_RES_UPDATE = 6f;
	//public static readonly int NUM_RES_UPDATED_PER_CYCLE = 10;

	//Food and Water
	public static readonly Vector2 NUM_WATER = new Vector2 (50, 100);
	public static readonly Vector2 NUM_FOOD = new Vector2 (50, 100);
	public static readonly int TIME_BETWEEN_RESOURCE_UPDATES = 5; //seconds
	public static readonly float RESOURCE_GROWTH_PER_SECOND = 1f;
	public static readonly float PERCENT_CHANCE_OF_POISON = 0.1f;
	public static readonly float QUANTITY_TO_VOLUMETIC_SCALE = 10f; //how much quantity of food = 1x1 volume in world


	//Map
	public static readonly Vector2 INITIAL_RESOURCE_RANGE = new Vector2(2,10);
	public static readonly float MIN_WORLD_FOOD = 100f;
	public static readonly float MIN_WORLD_WATER = 100f;
	public static readonly float MAP_DIAMETER = 120;
	public static readonly Vector2 NUM_OF_ROCKS = new Vector2(100, 200);
	public static readonly Vector2 RANGE_OF_ROCK_SCALE = new Vector2 (0.5f, 5);
	public static readonly float MIN_OBJ_SPAWN_DISTANCE_FROM_CENTER = 3f; //unity meters squared


    public static Vector2 SubtractVectors(Vector2 v2, Vector3 v3)
    {
        return new Vector2(v2.x - v3.x, v2.y - v3.y);
    }
    //add vectors,  ILL FIX AFTER THE MERGE I SWEAR
    public static Vector2 AddVectors(Vector2 v2, Vector3 v3)
    {
        return new Vector2(v2.x + v3.x, v2.y + v3.y);
    }
  
}