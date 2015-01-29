using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Ucoin.Framework.Entities;

namespace Ucoin.Framework.CompareObjects
{
    public class ComparisonResult
    {
        public ComparisonConfig Config { get; private set; }

        //public Dictionary<Type, List<IObjectWithState>> NeedAddList { get; set; }

        //public Dictionary<Type, List<IObjectWithState>> NeedUpdateList { get; set; }

        //public Dictionary<Type, List<IObjectWithState>> NeedDeleteList { get; set; }

        //public Dictionary<Type, List<UpdatedPropertyInfo>> UpdatePropertyList { get; set; }

        public List<Difference> Differences { get; set; }

        internal Dictionary<int, int> Parents { get; set; }

        internal Stopwatch Watch { get; set; }

        public ComparisonResult(ComparisonConfig config)
        {
            Config = config;
            Differences = new List<Difference>();
            //NeedAddList = new Dictionary<Type, List<IObjectWithState>>();
            //NeedUpdateList = new Dictionary<Type, List<IObjectWithState>>();
            //NeedDeleteList = new Dictionary<Type, List<IObjectWithState>>();
            //UpdatePropertyList = new Dictionary<Type, List<UpdatedPropertyInfo>>();
            Parents = new Dictionary<int, int>();
            Watch = new Stopwatch();
        }        

        public long ElapsedMilliseconds
        {
            get { return Watch.ElapsedMilliseconds; }
        }

        /// <summary>
        /// 對象比較的不同點，可用於Log
        /// </summary>
        public string DifferencesString
        {
            get
            {
                var sb = new StringBuilder();
                if (Differences.Count > 0)
                {
                    sb.AppendLine("-----Updated Info-----");
                }
                foreach (var item in Differences)
                {
                    sb.AppendLine(item.ToString());
                }

                //if (NeedAddList.Count > 0)
                //{
                //    sb.AppendLine("-----Added Info-----");
                //}
                //foreach (var item in NeedAddList)
                //{
                //    
                //    sb.AppendLine("TODO: Add");
                //}

                //if (NeedDeleteList.Count > 0)
                //{
                //    sb.AppendLine("-----Deleted Info-----");
                //}
                //foreach (var item in NeedDeleteList)
                //{
                //    
                //    sb.AppendLine("TODO: Delete");
                //}

                return sb.ToString();
            }
        }

        public bool AreEqual
        {
            get 
            {
                return Differences.Count == 0; 
            }
        }

        internal void AddParent(int hash)
        {
            if (hash == 0)
            {
                return;
            }

            if (!Parents.ContainsKey(hash))
            {
                Parents.Add(hash, 1);
            }
            else
            {
                Parents[hash]++;
            }
        }

        internal void RemoveParent(int hash)
        {
            if (Parents.ContainsKey(hash))
            {
                if (Parents[hash] <= 1)
                {
                    Parents.Remove(hash);
                }
                else
                {
                    Parents[hash]--;
                }
            }
        }

        //public Dictionary<string, object> GetDifferentPropertys(Type type, object id)
        //{
        //    var diffList = new Dictionary<string, object>();
        //    if (UpdatePropertyList.ContainsKey(type))
        //    {
        //        var updateList = UpdatePropertyList[type];
        //        var propInfo = updateList.FirstOrDefault(p => p.Id.ToString() == id.ToString());
        //        if (propInfo != null)
        //        {
        //            foreach (var p in propInfo.PropertyList)
        //            {
        //                //只需要知道是哪個屬性改變，不需要知道其改變后的值
        //                diffList.Add(p, null);
        //            }
        //        }
        //    }
        //    return diffList;
        //}
    }

    //public class UpdatedPropertyInfo
    //{
    //    public object Id { get; set; }

    //    public List<string> PropertyList { get; set; }
    //}
}
