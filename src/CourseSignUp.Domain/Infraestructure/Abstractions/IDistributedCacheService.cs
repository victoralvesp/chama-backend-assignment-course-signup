using System.Collections.Immutable;
using System.Threading.Tasks;

namespace CourseSignUp.Infraestructure.Abstractions
{
    public interface IDistributedCacheService
    {
        Task StoreOnSet<T>(string key, T data);

        /// <summary>
        /// Atomic removes all itens in Set with <paramref name="key"/> and return itens removed
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        Task<ImmutableArray<T>> ConsumeAllStored<T>(string key);
    }
}