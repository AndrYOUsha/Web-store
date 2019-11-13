using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models.ProductViewModel;

namespace WebStore.Patterns.Interfaces
{
    /// <summary>
    /// Интерфейс служит для паттерна "Стратегия"
    /// </summary>
    /// <typeparam name="T">Возвращаемое значение</typeparam>
    /// <typeparam name="Q">Параметр</typeparam>
    public interface IStrategy<T, Q>
    {
        T Add();
        T Get(Q item);
    }
}
