using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.WorldGen.Map
{
    class LODProvider
    {
        private List<Tuple<float, int>> lodValues = new List<Tuple<float, int>>();


        public void AddLOD(float cutoff, int multiplier)
        {
            for(int i = 0; i < lodValues.Count; i++)
            {
                var t = lodValues[i];
                if(cutoff < t.Item1)
                {
                    lodValues.Insert(i, new Tuple<float, int>(cutoff, multiplier));
                    return;
                }
            }
            lodValues.Add(new Tuple<float, int>(cutoff, multiplier));
        }

        public int GetMultiplier(float cutoff)
        {
            for (int i = 0; i < lodValues.Count; i++)
            {
                var t = lodValues[i];
                if (cutoff < t.Item1)
                {
                    return t.Item2;
                }
            }
            return lodValues[lodValues.Count - 1].Item2;
        }
    }
}
