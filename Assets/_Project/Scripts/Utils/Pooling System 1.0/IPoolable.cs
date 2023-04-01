namespace Utils.Pooling
{
    public interface IPoolable<T>
    {
        /// <summary>
        /// Store an action inside the pool-able object to be performed at a later data.
        /// </summary>
        /// <param name="returnAction"></param>
        void Initialize(System.Action<T> returnAction);
        
        /// <summary>
        /// Needs to be Invoked inside returnAction ^
        /// </summary>
        void ReturnToPool();
    }
}