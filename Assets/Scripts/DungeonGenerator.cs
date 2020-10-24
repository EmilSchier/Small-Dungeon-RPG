using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{

  [SerializeField]
  private Tile groundTile;
  [SerializeField]
  private Tile pitTile;
  [SerializeField]
  private Tile wallTopHorizontalTile;
  [SerializeField]
  private Tile wallTopVerticalTile;
  [SerializeField]
  private Tile wallTopInnerCornerTile;
  [SerializeField]
  private Tile wallTopOuterCornerTile;
  [SerializeField]
  private Tile wallTopLeftTile;
  [SerializeField]
  private Tile wallCornerTopTile;
  [SerializeField]
  private Tile wallCornerTile;
  [SerializeField]
  private Tile wallEndTile;
  [SerializeField]
  private Tile wallLeftTile;
  [SerializeField]
  private Tile wallRightTile;
  [SerializeField]
  private Tile wallTile;
  [SerializeField]
  private Tilemap groundMap;
  [SerializeField]
  private Tilemap pitMap;
  [SerializeField]
  private Tilemap wallMap;
  [SerializeField]
  private Tilemap wallTopMap;
  [SerializeField]
  private GameObject player;
  [SerializeField]
  private int deviationRate = 10;
  [SerializeField]
  private int roomRate = 15;
  [SerializeField]
  private int maxRouteLength;
  [SerializeField]
  private int maxRoutes = 20;


  private int routeCount = 0;

  private void Start()
  {
    int x = 0;
    int y = 0;
    int routeLength = 0;
    GenerateSquare(x, y, 1);
    Vector2Int previousPos = new Vector2Int(x, y);
    y += 3;
    GenerateSquare(x, y, 1);
    NewRoute(x, y, routeLength, previousPos);

    FillWalls();
  }

  private void FillWalls()
  {
    BoundsInt bounds = groundMap.cellBounds;
    for (int xMap = bounds.xMin - 10; xMap <= bounds.xMax + 10; xMap++)
    {
      for (int yMap = bounds.yMin - 10; yMap <= bounds.yMax + 10; yMap++)
      {
        Vector3Int pos = new Vector3Int(xMap, yMap, 0);
        Vector3Int posBelow = new Vector3Int(xMap, yMap - 1, 0);
        Vector3Int posAbove = new Vector3Int(xMap, yMap + 1, 0);
        Vector3Int posLeft = new Vector3Int(xMap - 1, yMap, 0);
        Vector3Int posRight = new Vector3Int(xMap + 1, yMap, 0);
        Vector3Int posTwoBelow = new Vector3Int(xMap, yMap - 2, 0);
        Vector3Int posTwoAbove = new Vector3Int(xMap, yMap + 2, 0);
        Vector3Int posTwoAboveRight = new Vector3Int(xMap + 1, yMap + 2, 0);
        Vector3Int posTwoAboveLeft = new Vector3Int(xMap + 1, yMap - 2, 0);
        Vector3Int posAboveLeft = new Vector3Int(xMap - 1, yMap + 1, 0);
        Vector3Int posBelowLeft = new Vector3Int(xMap - 1, yMap - 1, 0);
        Vector3Int posAboveRight = new Vector3Int(xMap + 1, yMap + 1, 0);
        Vector3Int posBelowRight = new Vector3Int(xMap + 1, yMap - 1, 0);
        TileBase tile = groundMap.GetTile(pos);
        TileBase tileBelow = groundMap.GetTile(posBelow);
        TileBase tileAbove = groundMap.GetTile(posAbove);
        TileBase tileLeft = groundMap.GetTile(posLeft);
        TileBase tileRight = groundMap.GetTile(posRight);
        TileBase tileTwoBelow = groundMap.GetTile(posTwoBelow);
        TileBase tileTwoAbove = groundMap.GetTile(posTwoAbove);
        TileBase tileTwoAboveRight = groundMap.GetTile(posTwoAboveRight);
        TileBase tileTwoAboveLeft = groundMap.GetTile(posTwoAboveLeft);
        TileBase tileAboveLeft = groundMap.GetTile(posAboveLeft);
        TileBase tileBelowLeft = groundMap.GetTile(posBelowLeft);
        TileBase tileAboveRight = groundMap.GetTile(posAboveRight);
        TileBase tileBelowRight = groundMap.GetTile(posBelowRight);
        if (tile == null)
        {

          pitMap.SetTile(pos, pitTile);
          // Wall Above Placement ***********************************************************************
          if (null != tileBelow && null == tileAbove)
          {
            wallMap.SetTile(pos, wallTile);
            if (null != tileLeft)
            {
              wallTopMap.SetTile(posAbove, wallTopInnerCornerTile);
              wallMap.SetTile(pos, wallLeftTile);
              if (null != tileRight)
              {
                wallMap.SetTile(posRight, wallEndTile);
              }
              else if (null == tileRight && null == tileBelowRight)
              {
                wallTopMap.SetTile(posRight, wallTopVerticalTile);
                wallTopMap.SetTile(posAboveRight, wallTopOuterCornerTile);
                wallMap.SetTile(pos, wallRightTile);
              }
            }
            else if (null == tileLeft && null == tileBelowLeft)
            {
              wallMap.SetTile(pos, wallCornerTile);
              wallTopMap.SetTile(posAbove, wallCornerTopTile);
            }
            else
            {
              wallTopMap.SetTile(posAbove, wallTopHorizontalTile);
              if (null == tileRight && null == tileBelowRight)
              {
                wallTopMap.SetTile(posRight, wallTopVerticalTile);
                wallTopMap.SetTile(posAboveRight, wallTopOuterCornerTile);
              }
              else if (null != tileRight)
              {
                wallMap.SetTile(posRight, wallEndTile);
              }
            }
          }
          else if (null != tileAbove) // lower walls *****************************************************
          {
            wallMap.SetTile(pos, wallTile);
            if (null != tileLeft)
            {
              wallMap.SetTile(pos, wallCornerTile);
              wallTopMap.SetTile(posAbove, wallCornerTopTile);
              if (null != tileRight)
              {
                wallTopMap.SetTile(posAboveRight, wallTopOuterCornerTile);
              }
              else if (null == tileRight)
              {
                wallMap.SetTile(posRight, wallEndTile);
              }
              if (null != tileBelow)
              {
                wallMap.SetTile(pos, wallLeftTile);
                wallTopMap.SetTile(posAbove, wallTopLeftTile);
              }
            }
            else if (null == tileAboveRight)
            {
              wallMap.SetTile(posRight, wallEndTile);
              wallTopMap.SetTile(posAbove, wallTopHorizontalTile);

            }
            else if (null == tileAboveLeft)
            {
              wallTopMap.SetTile(posAbove, wallTopInnerCornerTile);
              wallMap.SetTile(pos, wallLeftTile);
              if ( null != tileRight)
              {
                wallTopMap.SetTile(posAboveRight, wallTopOuterCornerTile);
              }
            }
            else if (null != tileRight)
            {
              wallTopMap.SetTile(posAboveRight, wallTopOuterCornerTile);
              wallTopMap.SetTile(posAbove, wallTopHorizontalTile);
              if (null != tileBelow)
              {
                wallMap.SetTile(posRight, wallEndTile);
              }
            }
            else
            {
              wallTopMap.SetTile(posAbove, wallTopHorizontalTile);
            }
          }
          //wertical Wall top placement ********************************************************************
          if (null != tileLeft && null == tileAbove && null == tileTwoBelow)
          {
            wallTopMap.SetTile(pos, wallTopVerticalTile);
            if (null == tileAbove && (null != tileTwoAbove || null != tileTwoAboveLeft))
            {
              wallTopMap.SetTile(posAbove, wallTopVerticalTile);
            }
          }
          if(null != tileRight && null != tileBelowRight && null == tileBelow)
          {
            wallTopMap.SetTile(posRight, wallTopVerticalTile);
            if (null == tileAboveRight && null != tileTwoAboveRight)
            {
              wallTopMap.SetTile(posAboveRight, wallTopVerticalTile);
            }
          }
        }
      }
    }
  }

  private void NewRoute(int x, int y, int routeLength, Vector2Int previousPos)
  {
    if (routeCount < maxRoutes)
    {
      routeCount++;
      while (++routeLength < maxRouteLength)
      {
        //Initialize
        bool routeUsed = false;
        int xOffset = x - previousPos.x; //0
        int yOffset = y - previousPos.y; //3
        int roomSize = 1; //Hallway size
        if (Random.Range(1, 100) <= roomRate)
          roomSize = Random.Range(3, 6);
        previousPos = new Vector2Int(x, y);

        //Go Straight
        if (Random.Range(1, 100) <= deviationRate)
        {
          if (routeUsed)
          {
            GenerateSquare(previousPos.x + xOffset, previousPos.y + yOffset, roomSize);
            NewRoute(previousPos.x + xOffset, previousPos.y + yOffset, Random.Range(routeLength, maxRouteLength), previousPos);
          }
          else
          {
            x = previousPos.x + xOffset;
            y = previousPos.y + yOffset;
            GenerateSquare(x, y, roomSize);
            routeUsed = true;
          }
        }

        //Go left
        if (Random.Range(1, 100) <= deviationRate)
        {
          if (routeUsed)
          {
            GenerateSquare(previousPos.x - yOffset, previousPos.y + xOffset, roomSize);
            NewRoute(previousPos.x - yOffset, previousPos.y + xOffset, Random.Range(routeLength, maxRouteLength), previousPos);
          }
          else
          {
            y = previousPos.y + xOffset;
            x = previousPos.x - yOffset;
            GenerateSquare(x, y, roomSize);
            routeUsed = true;
          }
        }
        //Go right
        if (Random.Range(1, 100) <= deviationRate)
        {
          if (routeUsed)
          {
            GenerateSquare(previousPos.x + yOffset, previousPos.y - xOffset, roomSize);
            NewRoute(previousPos.x + yOffset, previousPos.y - xOffset, Random.Range(routeLength, maxRouteLength), previousPos);
          }
          else
          {
            y = previousPos.y - xOffset;
            x = previousPos.x + yOffset;
            GenerateSquare(x, y, roomSize);
            routeUsed = true;
          }
        }

        if (!routeUsed)
        {
          x = previousPos.x + xOffset;
          y = previousPos.y + yOffset;
          GenerateSquare(x, y, roomSize);
        }
      }
    }
  }

  private void GenerateSquare(int x, int y, int radius)
  {
    for (int tileX = x - radius; tileX <= x + radius; tileX++)
    {
      for (int tileY = y - radius; tileY <= y + radius; tileY++)
      {
        Vector3Int tilePos = new Vector3Int(tileX, tileY, 0);
        groundMap.SetTile(tilePos, groundTile);
      }
    }
  }
}
