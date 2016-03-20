using UnityEngine;
using System.Collections;

public class Tunnelmanager : MonoBehaviour {

    static int height = GV.UNDERGROUND_HEIGHT;
    static int width = GV.UNDERGROUND_WIDTH;
    static int tunnelDepth = GV.TUNNEL_DEPTH;
    public GameObject ClosedDirt;
    public GameObject OpenDirt;

    bool[,] SolidityArray = new bool[width, height];

    


void Start ()
    {   
        for(int x = 0; x < width; x++)
            for(int y = 0; y < height; y++)
            {
                SolidityArray[(int)x, (int)y] = false;
            }
        for (int i = 0; i < 5; i++)
        {
            CrawlForOpenDirt(tunnelDepth);
        }
	}
	
	void CrawlForOpenDirt(int _tunnelDepth)
    {
        int xpos = Random.Range(30, 70);
        int ypos = Random.Range(30, 70);


        for (int i = 0; i < _tunnelDepth; i++)
        {
            Vector3 dirtPos = new Vector3(xpos, ypos, 20);
            if (SolidityArray[(int)xpos, (int)ypos] == false)
            {
                SolidityArray[(int)xpos, (int)ypos] = true;
                Instantiate(OpenDirt, dirtPos, Quaternion.identity);
            }
            if (xpos < 3) xpos++;
            else if (xpos > 97) xpos--;
            else if (ypos < 3) ypos++;
            else if (ypos > 97) ypos--;
            else
            {
                int rando = Random.Range(1, 5);
                if (rando == 1) xpos++;
                else if (rando == 2) xpos--;
                else if (rando == 3) ypos++;
                else ypos--;
            }

            

        }
    }

}
