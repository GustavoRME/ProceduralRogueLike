using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerBoard
{		
	private readonly TileScriptable[] _outerWalls;
	private readonly TileScriptable[] _walls;
	private readonly TileScriptable[] _floors;

	private readonly CountScriptable _countWalls;

	public DrawerBoard(TileScriptable[] outerWalls, TileScriptable[] walls, TileScriptable[] floors, CountScriptable countWalls)
	{		
		_outerWalls = outerWalls;
		_walls = walls;
		_floors = floors;

		_countWalls = countWalls;		
	}    

	public void DrawBoard(Tile[,] boardNode)
	{
		_countWalls.Count = 0;

		int width = boardNode.GetLength(0);
		int heigth = boardNode.GetLength(1);

		for (int y = 0; y < heigth; y++)
		{
			for (int x = 0; x < width; x++)
			{
				if(IsOuterWallArea(y, x, width, heigth ))
				{					
					SetUpNode(boardNode[x, y], _outerWalls);
				}
				else if(IsFreeArea(x, y, width, heigth))
				{					
					SetUpNode(boardNode[x, y], _floors);
				}
				else
				{					
					if(Random.Range(0, 2) > 0 && !IsReachMaximum())
					{						
						SetUpNode(boardNode[x, y], _walls);
						_countWalls.Count++;
					}
					else
					{						
						SetUpNode(boardNode[x, y], _floors);
					}
				}
			}
		}

		if (!IsReachMinimum())
		{
			for (int y = 0; y < heigth; y++)
			{
				for (int x = 0; x < width; x++)
				{
					if (!IsOuterWallArea(x, y, width, heigth) && !IsFreeArea(x, y, width, heigth))
					{
						SetUpNode(boardNode[x, y], _walls);
						_countWalls.Count++;
					}
				}
			}
		}
	}

	private void SetUpNode(Tile tile, TileScriptable[] scriptables)
	{
		TileScriptable data = GetRandomTileScriptable(scriptables);

		tile.SetTile(data);
	}
	private bool IsOuterWallArea(int x, int y, int width, int heigth) => x == 0 || y == 0 || x == width - 1 || y == heigth - 1;
	private bool IsFreeArea(int x, int y, int width, int height) => x == 1 || y == 1 || x == width - 2 || y == height - 2;
	private bool IsReachMinimum() => _countWalls.Count >= _countWalls.minimum;
	private bool IsReachMaximum() => _countWalls.Count >= _countWalls.maximum;
	private TileScriptable GetRandomTileScriptable(TileScriptable[] datas) => datas[Random.Range(0, datas.Length)]; 
}
