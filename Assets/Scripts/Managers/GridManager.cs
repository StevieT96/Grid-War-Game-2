using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour {
    public static GridManager Instance;
    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _grassTile, _mountainTile, _forestTile;

    [SerializeField] private Transform _cam;

    public Dictionary<Vector2, Tile> _tiles;

    void Awake() {
        Instance = this;
    }

    public void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        int blocksPerBlock = 3;
        int[] blockCounts = new int[100];
        for (int i = 0;i<100;i++)
        {
            blockCounts[i] = 0;
        }
        int maxMountainsPerBlock = _height / 2;
        Debug.Log(maxMountainsPerBlock);
        for (int x = 0; x < _width; x++)
        {
            int blockNum = x / blocksPerBlock;
            Debug.Log(blockNum);
            for (int y = 0; y < _height; y++) {
                /* Maintain a count of 3 columns (e.g. if X div 3 == 0, you're in the first block of 3 columns)
                 * X div 3 (e.g. 4 div 3 = 1, 8 div 3 = 2
                 */
                var randomNum = Random.Range(0, 2);
                var randomTile = randomNum == 1 ? _mountainTile : _grassTile;
                

                // If quota met, just use grass tiles
                if (blockCounts[blockNum] >= maxMountainsPerBlock)
                {
                    Debug.Log("Too many mountain tiles in block: " + blockNum + ", block counts: " + blockCounts[blockNum] + ", max blocks: " + maxMountainsPerBlock);
                    randomTile = _grassTile;
                } else
                {
                    blockCounts[blockNum] = randomNum == 1 ? blockCounts[blockNum] + 1 : blockCounts[blockNum];
                }

                var spawnedTile = Instantiate(randomTile, new Vector3(x, y), Quaternion.identity, this.transform);
                spawnedTile.name = $"Tile {x} {y}";

              
                spawnedTile.Init(x,y);


                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -7);

        GameManager.Instance.ChangeState(GameState.SpawnHeroes);
    }

    public Tile GetHeroSpawnTile() {
        return _tiles.Where(t => t.Key.x < _width / 2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
    }

    public Tile GetEnemySpawnTile()
    {
        return _tiles.Where(t => t.Key.x > _width / 2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
    }




    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }
}