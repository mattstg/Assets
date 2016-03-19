using UnityEngine;
using System.Collections;

public class GV
{
    public enum PhermoneTypes { Friendly, Hostile };
    public static readonly float PATH_DECAY_RATE = 5f;  //1 point every 5 seconds


	//Food and Water
	public static readonly int Min_FoodBundles = 5;
	public static readonly int Max_FoodBundles = 15;
	public static readonly int Min_WaterPuddes = 5;
	public static readonly int Max_WaterPuddes = 15;
	public static readonly int TIME_BETWEEN_RESOURCE_UPDATES = 5;

	//Map
	public static readonly float Map_Diameter = 100;
}