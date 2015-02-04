using Ucoin.ServiceModel.Server.Configuration;
using System;
using System.Collections.Generic;

namespace Ucoin.ServiceModel.Server
{
    public static class Unity
    {
        internal static string GetBasiceAddress()
        {
            var baseAddress = WcfServerSection.Current.Service.Address;
            return baseAddress;
        }

        /// <summary>
        /// 扩展Foreach方式
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        /// <param name="ie">迭代器</param>
        /// <param name="action">行为委托</param>
        public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
        {
            foreach (var item in ie)
            {
                action(item);
            }
        }
    }
}
