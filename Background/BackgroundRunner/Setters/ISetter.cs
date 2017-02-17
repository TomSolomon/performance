namespace Performance.Setters
{
    /// <summary>
    /// Providing an way to set the original\newly created instance back to its container
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISetter<T>
    {
        void SetBack(T instance);
    }
}