using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacement
{
    private const int MIN_DISTANCE = 3;

    private List<GameObject> _foods;

    private readonly GameObject _tomatoe;
    private readonly GameObject _soda;

    private readonly CountScriptable _countTomatoe;
    private readonly CountScriptable _countSoda;

    private readonly Transform _parent;

    public int MaxItems { get; set; }

    public ItemPlacement(GameObject tomatoe, GameObject soda, CountScriptable countTomatoe, CountScriptable countSoda, Transform parent, int maxItems)
    {
        _tomatoe = tomatoe;
        _soda = soda;

        _countTomatoe = countTomatoe;
        _countSoda = countSoda;

        _parent = parent;

        MaxItems = maxItems;

        _foods = new List<GameObject>();
    }

    public void PlaceItems(Tile[,] board)
    {
        Clear();
        
        int itemsCount = 0;

        int lastX = 0;
        int lastY = 0;

        //Place items on middle of board
        for (int x = 3; x < board.GetLength(0) - 3; x++)
        {
            for (int y = 3; y < board.GetLength(1) - 3; y++)
            {
                if(itemsCount < MaxItems)
                {
                    if(board[x, y].IsWalkable && Random.Range(0, 2) > 0)
                    {             
                        if(IsWithinDistance(lastX, lastY, x, y))
                        {
                            GameObject food = null;

                            if(Random.Range(0, 2) == 0 && !IsReachMaximum(_countTomatoe))
                            {
                                food = _tomatoe;                                
                            }
                            else if(!IsReachMaximum(_countSoda))
                            {
                                food = _soda;                                
                            }

                            if (food != null)
                            {
                                _foods.Add(CreateFood(food, x, y));
                                itemsCount++;

                                lastX = x;
                                lastY = y;                                                                
                            }
                        }
                                            
                    }
                }
            }
        }
    }

    private void Clear()
    {
        foreach (var go in _foods)
            Object.Destroy(go);

        _foods.Clear();
        
        _countSoda.Count = 0;
        _countTomatoe.Count = 0;
    }
    private GameObject CreateFood(GameObject food, int x, int y) => Object.Instantiate(food, new Vector3(x, y, 0f), Quaternion.identity, _parent);
    private bool IsReachMaximum(CountScriptable counter) => counter.Count >= counter.maximum;
    private bool IsReachMinimum(CountScriptable counter) => counter.Count >= counter.minimum;
    private bool IsWithinDistance(int x0, int y0, int x1, int y1) => (Mathf.Abs(x0 - x1) + Mathf.Abs(y0 - y1)) > MIN_DISTANCE;
}
