using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPlacement
{
    private const int MIN_DISTANCE = 5;

    private readonly GameObject _player;
    private readonly GameObject[] _enemies;

    private readonly CountScriptable _countEnemies;

    private readonly Vector3 _upperLeft;
    private readonly Vector3 _upperRight;
    private readonly Vector3 _bottomLeft;
    private readonly Vector3 _bottomRight;

    public bool IsTop { get; private set; }

    public CharacterPlacement(GameObject player, GameObject[] enemies, CountScriptable countEnemies, int width, int heigth)
    {
        _player = player;

        _enemies = enemies;
        _countEnemies = countEnemies;

        _upperLeft = new Vector3(1, heigth - 2);
        _upperRight = new Vector3(width - 2, heigth - 2);
        _bottomLeft = new Vector3(1, 1);
        _bottomRight = new Vector3(width - 2, 1);
    }

    public void PlaceCharacters(Tile[,] board, bool startRandomizePlayerPosition)
    {
        PlacePlayer(startRandomizePlayerPosition);
        ClearEnemies();
        
        int lastX = 0;
        int lastY = 0;

        while (!IsReachMinimum())
        {
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    if (IsWithinDistance(lastX, lastY, x, y))
                    {
                        if (Random.Range(0, 10) > 8 && !IsReachMaximum())
                        {
                            if (board[x, y].IsWalkable)
                                PlaceEnemy(x, y);
                        }
                    }

                }
            }
        }

    }              

    private void PlacePlayer(bool start)
    {
        if(start)
        {
            //Think how to do
        }
        else
        {
            _player.transform.position = _bottomLeft;
            IsTop = false;
        }
    }
    private void PlaceEnemy(int x, int y)
    {
        int index = Random.Range(0, _enemies.Length);

        _enemies[index].transform.position = new Vector3(x, y);
        _enemies[index].SetActive(true);

        _countEnemies.Count++;
    }
    private void ClearEnemies()
    {
        _countEnemies.Count = 0;

        _enemies[0].SetActive(false);
        _enemies[1].SetActive(false);
    }

    private bool IsReachMinimum() => _countEnemies.Count >= _countEnemies.minimum;
    private bool IsReachMaximum() => _countEnemies.Count >= _countEnemies.maximum;
    private bool IsWithinDistance(int x0, int y0, int x1, int y1) => (Mathf.Abs(x0 - x1) + Mathf.Abs(y0 - y1)) > MIN_DISTANCE;
}
