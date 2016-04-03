using UnityEngine;
using System.Collections;

public class Tunnelmanager : MonoBehaviour
{

    static int height = GV.UNDERGROUND_HEIGHT;
    static int width = GV.UNDERGROUND_WIDTH;
    static int tunnelDepth = GV.TUNNEL_DEPTH;
    public GameObject ClosedDirt;
    public GameObject OpenDirt;



    bool[,] SolidityArray = new bool[width, height];




    void Start()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                SolidityArray[(int)x, (int)y] = false;
            }
        for (int i = 0; i < 15; i++)
        {
            CrawlForOpenDirt(tunnelDepth, GV.crawlerType.main);
        }
        for (int i = 0; i < 15; i++)
        {
            CrawlForOpenDirt(tunnelDepth, GV.crawlerType.diagonalTunnels);
        }
        for (int i = 0; i < 10; i++)
        {
            CrawlForOpenDirt(tunnelDepth, GV.crawlerType.upDownTunnels);
        }
        for (int i = 0; i < 10; i++)
        {
            CrawlForOpenDirt(tunnelDepth, GV.crawlerType.leftRighttunnels);
        }
        DrawClosedDirt();

    }

    void CrawlForOpenDirt(int _tunnelDepth, GV.crawlerType crawler)
    {
        int xpos = Random.Range(-18, 18);
        int ypos = Random.Range(-18, 18);


        for (int i = 0; i < _tunnelDepth; i++)
        {
            Vector3 dirtPos = new Vector3(xpos, ypos, 200);
            if (SolidityArray[(int)xpos + (width / 2), (int)ypos + (height / 2)] == false)
            {
                SolidityArray[(int)xpos + (width / 2), (int)ypos + (height / 2)] = true;
                Instantiate(OpenDirt, dirtPos, Quaternion.identity);
            }
            if (xpos < -(width / 2 - 4)) xpos = 0;
            else if (xpos > (width / 2 - 4)) xpos = 0;
            else if (ypos < -(height / 2 - 4)) ypos = 0;
            else if (ypos > (height / 2 - 4)) ypos = 0;
            else
            {
                if (crawler == GV.crawlerType.main)
                {
                    int rando = Random.Range(1, 5);
                    if (rando == 1) xpos++;
                    else if (rando == 2) xpos--;
                    else if (rando == 3) ypos++;
                    else ypos--;
                }
                else if (crawler == GV.crawlerType.diagonalTunnels)
                {
                    int rando = Random.Range(1, 9);
                    if (xpos < 0 && ypos < 0)
                    {
                        if (rando <= 3) xpos--;
                        else if (rando <= 6) ypos--;
                        else if (rando == 7) xpos++;
                        else ypos++;
                    }
                    else if (xpos >= 0 && ypos < 0)
                    {
                        if (rando <= 3) xpos++;
                        else if (rando <= 6) ypos--;
                        else if (rando == 7) xpos--;
                        else ypos++;
                    }
                    else if (xpos >= 0 && ypos >= 0)
                    {
                        if (rando <= 3) xpos++;
                        else if (rando <= 6) ypos++;
                        else if (rando == 7) xpos--;
                        else ypos--;
                    }
                    else if (xpos < 0 && ypos >= 0)
                    {
                        if (rando <= 3) xpos--;
                        else if (rando <= 6) ypos++;
                        else if (rando == 7) xpos++;
                        else ypos--;
                    }
                }
                else if (crawler == GV.crawlerType.leftRighttunnels)
                {
                    int rando = Random.Range(1, 9);
                    if (xpos >= 0)
                    {
                        if (rando <= 3) xpos++;
                        else if (rando <= 5) ypos++;
                        else if (rando == 6) xpos--;
                        else ypos--;
                    }
                    else if (xpos < 0)
                    {
                        if (rando <= 3) xpos--;
                        else if (rando == 4) xpos++;
                        else if (rando <= 6) ypos++;
                        else ypos--;
                    }

                }
                else if (crawler == GV.crawlerType.upDownTunnels)
                {
                    int rando = Random.Range(1, 9);
                    if (ypos >= 0)
                    {
                        if (rando <= 3) ypos++;
                        else if (rando <= 5) xpos++;
                        else if (rando == 6) ypos--;
                        else xpos--;
                    }
                    else if (ypos < 0)
                    {
                        if (rando <= 3) ypos--;
                        else if (rando == 4) ypos++;
                        else if (rando <= 6) xpos++;
                        else xpos--;
                    }
                }



            }
        }

    }
    void DrawClosedDirt()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                if (SolidityArray[x, y] == true)
                    for (int a = -1; a < 2; a++)
                        for (int b = -1; b < 2; b++)
                            if (SolidityArray[x + a, y + b] == false)
                            {
                                Vector3 dirtPos = new Vector3(x + a - (width / 2), y + b - (height / 2), 200);
                                Instantiate(ClosedDirt, dirtPos, Quaternion.identity);
                            }
    }


}
