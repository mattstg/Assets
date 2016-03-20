using UnityEngine;
using System.Collections;

public class GV
{
    public enum directionQuadrants { q1, q2, q3, q4 };
    public enum PhermoneTypes { Friendly, Hostile };
	public enum ResourceTypes { Empty, Food, Water };
    public static readonly float PATH_DECAY_RATE = 5f;  //1 point every 5 seconds
    public static readonly float ANT_ENERGY_DECAY = 1f; //1 per second
    public static readonly float ANT_ENERGY_START = 100f;
    public static readonly float ANT_SPEED = 1f;
    public static readonly float ANT_SCOUT_TIMER = 4f;
    public static readonly int PHEROMONE_START_ENERGY = 20; //by this logic should last 30 seconds
	public static readonly float ANT_MAX_HEALTH = 100f;
    public static readonly float PHEROMONE_PLACEMENT_ABSORPTION_RADIUS = 1f;

	//ANT VARS
	public static readonly float ANT_CARRY_CAPACITY = 1f; 
	public static readonly float RESOURCE_DRAIN_DORMANT = 0.5f; //per X second
	public static readonly float RESOURCE_DRAIN_ACTIVE = 1f; //per X second
	public static readonly float RESOURCE_DRAIN_TICK = 30f; //every X seconds <<<<< X
	public static readonly float ANT_DEATH_FROM_STARVATION = 0.05f; //percent of dormant ants death due to starvation
	public static readonly float ANT_DEATH_FROM_POISON = 10f; //flat death per quantity of poison brought back to colony
	public static readonly float RESOURCE_TO_ENRGY_HP = 20f; //+X energy per unity of food
	public static readonly float POISON_TO_ENRGY_HP = 60f; //-X energy % hp per unity of poison
	public static readonly float DMG_DUE_TO_STARVATION = 5f; //-X hp per second

	//Colony Scripts
	public static readonly float START_FOOD = 500;
	public static readonly float START_WATER = 500;
	public static readonly int START_ANTS = 10000;
	public static readonly float DESIRED_PERCENT_DORMANT_ANTS = .60f; //0-1
	public static readonly int TIME_BETWEEN_ANT_EXITS = 5; //seconds

	//Food and Water
	public static readonly Vector2 NUM_WATER = new Vector2 (500, 500);
	public static readonly Vector2 NUM_FOOD = new Vector2 (500, 500);
	public static readonly int TIME_BETWEEN_RESOURCE_UPDATES = 5; //seconds
	public static readonly float RESOURCE_GROWTH_PER_SECOND = 1f;
	public static readonly float MIN_WORLD_FOOD = 100f;
	public static readonly float MIN_WORLD_WATER = 100f;
	public static readonly Vector2 INITIAL_RESOURCE_RANGE = new Vector2(30,150);
	public static readonly float PERCENT_CHANCE_OF_POISON = 0.1f;
	public static readonly float QUANTITY_TO_VOLUMETIC_SCALE = 10f; //how much quantity of food = 1x1 volume in world


	//Map
	public static readonly float MAP_DIAMETER = 120;
	public static readonly Vector2 NUM_OF_ROCKS = new Vector2(500, 765);
	public static readonly Vector2 RANGE_OF_ROCK_SCALE = new Vector2 (0.5f, 5);
	public static readonly float MIN_OBJ_SPAWN_DISTANCE_FROM_CENTER = 2f; //unity meters squared


    public static Vector2 SubtractVectors(Vector2 v2, Vector3 v3)
    {
        return new Vector2(v2.x - v3.x, v2.y - v3.y);
    }

  
}