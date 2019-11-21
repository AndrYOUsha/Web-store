using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Patterns.Interfaces;
using WebStore.Patterns.StrategyPattern;
using WebStore.Models.ProductViewModel;

namespace WebStore.Patterns
{
    /// <summary>
    /// Паттерн реализующий паттерн "Стратегия"
    /// Унаследован от интерфейса IStrategy
    /// </summary>
    public class Strategy
    {
        /// <summary>
        /// Добавляет и сохраняет модель в базе данных
        /// </summary>
        /// <param name="strategy">служит объектом паттерна "Стратегия"</param>
        /// <returns></returns>
        public async Task<int> AddItemAsynk(IStrategy<Task<int>, object> strategy)
        {
            return await strategy.Add();
        }

        /// <summary>
        /// Передаёт элемент для viewModel типа ProductCharacteristicViewModel в зависимости от выбранного селектора
        /// </summary>
        /// <param name="strategy"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public async Task<ProductCharacteristicViewModel> GetItemViewModelAsynk(IStrategy<Task<ProductCharacteristicViewModel>, ItemSelectorPCVM> strategy, ItemSelectorPCVM selector)
        {
            return await strategy.Get(selector);
        }
    }
}
