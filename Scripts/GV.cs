﻿using UnityEngine;
using System.Collections;

public class GV
{
    public enum directionQuadrants { q1, q2, q3, q4 };
    public enum crawlerType {main, diagonalTunnels, leftRighttunnels, upDownTunnels};


    public enum PhermoneTypes { Friendly, Hostile };
	public enum ResourceTypes { Empty, Food, Water };

	//ANT VARS
    public static readonly float ANT_ENERGY_DECAY = 2f; //1 per second
    public static readonly float ANT_ENERGY_MAX = 100f;
    public static readonly float ANT_SPEED = 1.5f;
    public static readonly float ANT_STATE_TIMER = 6f; //length of time they do
	public static readonly float DMG_FROM_CORPSES = 5f; //5dmg per second
	public static readonly float PRCNT_ENRGY_THRESHHOLD = .4f; //percent below which ants want to return home or above which they nolonger want to
	public static readonly int HOLDING_RES_PHER_MULTIPLIER = 10;
	public static readonly float ANT_CARRY_CAPACITY = 1f; 
	public static readonly float RESOURCE_TO_ENRGY = 10f; //+X energy per unity of food
	public static readonly float POISON_TO_ENRGY = 5f; //-X energy % hp per unity of poison
	public static readonly float UNIT_EAT_PER_SEC = 0.5f;

	//Phero 
	public static readonly float PHEROMONE_PLACEMENT_ABSORPTION_RADIUS = 1.5f;
	public static readonly float PATH_DECAY_RATE = 10f;  //paths decay once every X seconds
	public static readonly float PATH_DECAY_PCNT = 0.15f; //paths decay at a rate of X every PATH_DECAY_RATE seconds
	public static readonly int FLAT_PATH_DECAY = 1; //decay per PATH_DECAY_RATE seconds, flat
	public static readonly int PHEROMONE_START_ENERGY = 11; 
	public static readonly int PHEROMONE_MAX_ENERGY = 100;  

    public static readonly float PHEROMONE_MAX_OPACITY = .9f;
	public static readonly float PHEROMONE_MIN_OPACITY = .4f;

	public static readonly int BASE_PHER_STRENTH = 1; //per going over pher trail
	public static readonly int BACK_PHER_WEIGHT_FOOD = 5;
	public static readonly int BACK_PHER_WEIGHT_ENRGY = 5; //per 100% of energy
	public static readonly float FEAR_DECAY_PER_SEC = 20f;
	public static readonly float FEAR_SIZE = 3f;

	//Colony Scripts
	public static readonly int COLONY_NUM_SCOUT_SPAWN = 20;  //number of scouts spawned first
	public static readonly float COL_RESOURCE_DRAIN_DORMANT = 0.5f; //per X second
	public static readonly float COL_RESOURCE_DRAIN_TICK = 30f; //every X seconds <<<<< X
	public static readonly float ANT_DEATH_FROM_POISON = 10f; //flat death per quantity of poison brought back to colony
	public static readonly float START_FOOD = 2000;
	public static readonly float START_WATER = 2000;
	public static readonly int START_ANTS = 500;
	public static readonly float DESIRED_PERCENT_DORMANT_ANTS = .6f; //0-1
	public static readonly int ANT_EXIT_PER_SECOND = 5;
	public static readonly float ENRGY_LOSS_FROM_STARV = 0.05f; //energy loss if starving (multiplied by two if missing both resources)
	public static readonly int MAX_BORN_ANTS_PER_SEC = 2;
	//public static readonly int NUM_RES_UPDATED_PER_CYCLE = 10;

	//Food and Water
	public static readonly float TIME_BETWEEN_RES_UPDATE = 15f;
	public static readonly Vector2 NUM_WATER = new Vector2 (50, 100);
	public static readonly Vector2 NUM_FOOD = new Vector2 (50, 100);
	public static readonly int TIME_BETWEEN_RESOURCE_UPDATES = 10; //seconds
	public static readonly float RESOURCE_GROWTH_PER_SECOND = 1f;
	public static readonly float PERCENT_CHANCE_OF_POISON = 0.05f;
	public static readonly float QUANTITY_TO_VOLUMETIC_SCALE = 10f; //how much quantity of food = 1x1 volume in world


	//Map
	public static readonly Vector2 INITIAL_RESOURCE_RANGE = new Vector2(50,250);
	public static readonly float MIN_WORLD_FOOD = 100f;
	public static readonly float MIN_WORLD_WATER = 100f;
	public static readonly float MAP_DIAMETER = 120;
	public static readonly Vector2 NUM_OF_ROCKS = new Vector2(100, 200);
	public static readonly Vector2 RANGE_OF_ROCK_SCALE = new Vector2 (0.5f, 5);
    public static readonly float MIN_OBJ_SPAWN_DISTANCE_FROM_CENTER = 3f; //unity meters squared

    public static readonly int UNDERGROUND_WIDTH = 50;
    public static readonly int UNDERGROUND_HEIGHT = 50;
    public static readonly int TUNNEL_DEPTH = 80;

    //Player
    public static readonly float PLAYER_MAX_HEALTH = 100;
    public static readonly float PLAYER_MOVE_E_COST = 2; //e cost per second moving
    public static readonly float PLAYER_CLICK_E_COST = 8; //e cost per click
    public static readonly float PLAYER_FLY_E_COST = 10; //e cost per mouse wheel?
    public static readonly float PLAYER_MOVE_SPEED = 10; //e cost per mouse wheel?
    public static readonly float PLAYER_TIRED_PENALTY = .5f;
    public static readonly float PLAYER_TIRED_THRESHOLD = .1f; //at 10% max hp
    public static readonly float PLAYER_ZOOM_MAX = 32;
    public static readonly float PLAYER_ZOOM_MIN = 4;
    public static readonly float PLAYER_ZOOM_SPEED = 7;
    public static readonly float PLAYER_E_REGEN = 4;
    public static readonly float PLAYER_DMG = 33;
    public static readonly float PLAYER_DMG_TAKEN = 11;
    public static readonly float PLAYER_PHER_START = 15;
	public static bool DRAW_ALL_PREDICTIONS = false;
	



    public static Vector2 SubtractVectors(Vector2 v2, Vector3 v3)
    {
        return new Vector2(v2.x - v3.x, v2.y - v3.y);
    }
    //add vectors,  ILL FIX AFTER THE MERGE I SWEAR, dont know why id make a subtract one..
    public static Vector2 AddVectors(Vector2 v2, Vector3 v3)
    {
        return new Vector2(v2.x + v3.x, v2.y + v3.y);
    }

    
  
}