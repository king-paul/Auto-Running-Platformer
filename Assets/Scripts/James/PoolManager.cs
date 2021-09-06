using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: James Kemeny

public class PoolManager : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        // Pool class storing its size, tag and prefab
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton

    public static PoolManager m_Instance;

    private void Awake()
    {
        m_Instance = this;
    }

    #endregion

    public List<Pool> m_Pools;
    public Dictionary<string, Queue<GameObject>> m_PoolDictionary;

    private void Start()
    {
        // Create a new dictionary of game objects
        m_PoolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in m_Pools)
        {
            // For each queue we want to make, we create a new queue of objects
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                // Add all the desired objects to the queue
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            m_PoolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string _tag, Vector3 _position, Quaternion _rotation)
    {
        // Check the pool tag is valid
        if(!m_PoolDictionary.ContainsKey(_tag))
        {
            Debug.LogWarning("Pool with tag " + _tag + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = m_PoolDictionary[_tag].Dequeue();
        if(objectToSpawn != null)
        {
            objectToSpawn.transform.position = _position;
            objectToSpawn.transform.rotation = _rotation;
            objectToSpawn.SetActive(true);
        }

        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();

        if(pooledObject != null)
        {
            pooledObject.OnObjectSpawn();
        }

        m_PoolDictionary[_tag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }
}