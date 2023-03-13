using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.Pooling
{
    public class ObjectPool<T> : IPool<T> where T : MonoBehaviour, IPoolable<T>
    {
        private System.Action<T> _pullObjectAction; // gets called when object is pulled from the pool.
        private System.Action<T> _pushObjectAction; // gets called when object is pushed back into the pool.
                                                    //! for example actions could create a sound, or trigger a particle effect.


        private Stack<T> _poolObjects = new Stack<T>(); // first in first out collection.
        private GameObject _prefab; // the prefab for the pool.

        public int _poolCount
        {
            get
            {
                return _poolObjects.Count;
            }
        }

        public ObjectPool(GameObject pooledObject, int numberToSpawn = 0)
        {
            this._prefab = pooledObject;
            Spawn(numberToSpawn);
        }

        public ObjectPool(GameObject pooledObject, Action<T> pullObjectAction, Action<T> pushObjectAction, int numberToSpawn = 0)
        {
            this._prefab = pooledObject;
            this._pullObjectAction = pullObjectAction;
            this._pushObjectAction = pushObjectAction;
        }

        #region IPool Interface
        /// <summary>
        /// Pulls the object from the pool with a reference to it.
        /// </summary>
        /// <returns></returns>
        public T Pull()
        {
            T t;
            if(_poolCount > 0)
                t = _poolObjects.Pop();
            else
                t = GameObject.Instantiate(_prefab).GetComponent<T>();

            t.gameObject.SetActive(true); // ensure the object is on.
                                          //! The Push() function is what is responsible for return itself to the pool it came from.
            t.Initialize(Push); // Here we are passing the push method from the interface

            // allow default behavior and turning object back on.
            _pullObjectAction?.Invoke(t); // will trigger event for pulling the object into the world.


            /*! 
             * We return the object, so what ever object that asked fro this object can have a
             * reference to it.
            */
            return t;
        }

        /// <summary>
        /// Adds the object back to the pool.
        /// </summary>
        /// <param name="t"></param>
        public void Push(T t)
        {
            _poolObjects.Push(t);

            // Create default behavior to turn off objects
            _pushObjectAction?.Invoke(t);

            t.gameObject.SetActive(false);
        }
        #endregion

        #region IPool Extra Functions, NOT Interface
        public T Pull(Vector3 position)
        {
            T t = Pull();
            t.transform.position = position;
            return t;
        }

        public T Pull(Vector3 position, Quaternion rotation)
        {
            T t = Pull(position);
            t.transform.rotation = rotation;
            return t;
        }

        public GameObject PullGameObject()
        {
            return Pull().gameObject;
        }

        public GameObject PullGameObject(Vector3 position)
        {
            GameObject poolGameObject = PullGameObject();
            poolGameObject.transform.position = position;
            return poolGameObject;
        }

        public GameObject PullGameObject(Vector3 position, Quaternion rotation)
        {
            GameObject poolGameObject = PullGameObject(position);
            poolGameObject.transform.rotation = rotation;
            return poolGameObject;
        }
        #endregion

        private void Spawn(int numberToSpawn)
        {
            //! the type T !!IN MOST CASES!! is a mono behavior that is "attached" to all unity game objects.
            //! from the Mono Behavior we can get the gameObject it is attacked to.

            T t;

            for(int i = 0; i < numberToSpawn; i++)
            {
                t = GameObject.Instantiate(_prefab).GetComponent<T>(); // T will be the objects mono behavior
                _poolObjects.Push(t); // add the Mono Behavior to the stack of pooled objects.
                t.gameObject.SetActive(false); // makes sure the newly Spawned object is disabled.
            }
        }


    }
}