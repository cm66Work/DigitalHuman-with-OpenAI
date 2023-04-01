using System;
using UnityEngine;

namespace Utils.Pooling
{ 
    /// <summary>
    /// this needs to be attached to a prefab you wish to pool.
    /// </summary>
    public class PoolObject : MonoBehaviour, IPoolable<PoolObject>
    {
        /// <summary>
        /// The object itself is responsible for returning itself to the pool
        /// </summary>
        private Action<PoolObject> _returnToPool;


        private void OnDisable()
        {
            ReturnToPool();
        }

        #region IPoolable Interface
        public void Initialize(Action<PoolObject> returnAction)
        {
            // cache reference to return action.
            this._returnToPool = returnAction;
        }

        public void ReturnToPool()
        {
            // invoke and return this object to pool
            _returnToPool?.Invoke(this);
        }
        #endregion
    }
}