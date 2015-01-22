using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Ucoin.Framework.Entities;

namespace Ucoin.Framework.CompareObjects
{
    public class ComparisonResult
    {
        public ComparisonConfig Config { get; private set; }

        public Dictionary<Type, List<IObjectWithState>> NeedAddList { get; set; }

        public Dictionary<Type, List<IObjectWithState>> NeedUpdateList { get; set; }

        public Dictionary<Type, List<IObjectWithState>> NeedDeleteList { get; set; }

        public List<Difference> Differences { get; set; }

        internal Dictionary<int, int> Parents { get; set; }

        internal Stopwatch Watch { get; set; }

        public ComparisonResult(ComparisonConfig config)
        {
            Config = config;
            Differences = new List<Difference>();
            NeedAddList = new Dictionary<Type, List<IObjectWithState>>();
            NeedUpdateList = new Dictionary<Type, List<IObjectWithState>>();
            NeedDeleteList = new Dictionary<Type, List<IObjectWithState>>();
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

                if (NeedAddList.Count > 0)
                {
                    sb.AppendLine("-----Added Info-----");
                }
                foreach (var item in NeedAddList)
                {
                    //TODO: Add info format
                    sb.AppendLine("TODO: Add");
                }

                if (NeedDeleteList.Count > 0)
                {
                    sb.AppendLine("-----Deleted Info-----");
                }
                foreach (var item in NeedDeleteList)
                {
                    //TODO: Delete info format
                    sb.AppendLine("TODO: Delete");
                }

                return sb.ToString();
            }
        }

        public bool AreEqual
        {
            get 
            {
                return Differences.Count == 0 && NeedAddList.Count == 0
                    && NeedUpdateList.Count == 0 && NeedDeleteList.Count == 0; 
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
    }
}
