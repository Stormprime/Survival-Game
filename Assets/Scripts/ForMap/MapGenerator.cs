using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CountOfObject
{
    public int minCount;
    public int maxCount;

    public CountOfObject(int minCount, int maxCount)
    {
        this.minCount = minCount;
        this.maxCount = maxCount;
    }
}

namespace Assets.Scripts.ForMap
{
    public class MapGenerator : MonoBehaviour
    {
        public int ColumnsOnFloor = 8;
        public int RowsOnFloor = 8;
        public CountOfObject BarrierCount = new CountOfObject(25, 50);
        public CountOfObject BonusCount = new CountOfObject(150, 300);
        public GameObject[] FloorPrefab;
        public GameObject[] BarrierPrefab;
        public GameObject[] BonusPrefab;
        public GameObject[] EnemyPrefab;
        public GameObject[] DarkParticlesPrefab;

        private List<Vector3> _grid = new List<Vector3>();
        private Transform _floor;


        void Start()
        {
            DateTime beforeTime = DateTime.Now;
            GenerateFloor();
            GenerateGrid();
            PutPrefabsOnGrid(BarrierPrefab, BarrierCount.minCount, BarrierCount.maxCount);
            PutPrefabsOnGrid(BonusPrefab, BonusCount.minCount, BonusCount.maxCount);
            PutPrefabsOnGrid(EnemyPrefab, 5, 5);
            DateTime afterTime = DateTime.Now;
            TimeSpan duration = afterTime.Subtract(beforeTime);
            Debug.Log("Time: " + "Minutes: " + duration.Minutes +
                     " Second: " + duration.Seconds +
                     " Milliseconds " + duration.Milliseconds);
            AstarPath.active.Scan();
        }


        void GenerateGrid()
        {
            _grid.Clear();

            for (int coordX = 1; coordX < ColumnsOnFloor - 1; coordX++)
            {
                for (int coordY = 1; coordY < RowsOnFloor - 1; coordY++)
                {
                    _grid.Add(new Vector3(coordX, coordY));
                }
            }
        }

        Vector3 RandomPosition()
        {
            int randId = Random.Range(0, _grid.Count);
            Vector3 randPos = _grid[randId];
            _grid.RemoveAt(randId);
            return randPos;
        }

        void GenerateFloor()
        {
            _floor = new GameObject("Map").transform;
            for (int coordX = -6; coordX < ColumnsOnFloor + 6; coordX++)
            {
                for (int coordY = -6; coordY < RowsOnFloor + 6; coordY++)
                {
                    int floorRandId = Random.Range(0, FloorPrefab.Length);
                    GameObject objectToInstantiate = FloorPrefab[floorRandId];

                    int particlesRandId = Random.Range(0, DarkParticlesPrefab.Length);
                    if (coordX == -6 || coordY == -6 ||
                        coordX == ColumnsOnFloor + 4 ||
                        coordY == RowsOnFloor + 4)
                        objectToInstantiate = DarkParticlesPrefab[particlesRandId];

                    Vector3 position = new Vector3(coordX, coordY);
                    GameObject spawnObject =
                        (GameObject)Instantiate(objectToInstantiate,
                        position, Quaternion.identity);
                    spawnObject.transform.parent = _floor.transform;
                }
            }
        }

        void PutPrefabsOnGrid(GameObject[] objectArr, int minCount, int maxCount)
        {
            GameObject objectsOnScene = new GameObject("Objects On Scene");
            int objCount = Random.Range(minCount, maxCount + 1);

            for (int i = 0; i < objCount; i++)
            {
                Vector3 randPos = RandomPosition();
                int objectRandId = Random.Range(0, objectArr.Length);
                GameObject objectChoice = (GameObject)Instantiate(objectArr[objectRandId], randPos, Quaternion.identity);
                objectChoice.transform.parent = objectsOnScene.transform;
            }
        }
    }
}

