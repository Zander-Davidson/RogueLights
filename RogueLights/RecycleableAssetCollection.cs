using System;
using System.Collections.Generic;   
using System.Linq;
using Microsoft.Xna.Framework;


namespace RogueLights
{
    public class RecycleableAssetCollection<T> where T : IInitializeable
    {
        public List<T> Instances = new List<T>();

        private int NextInstance = 0;

        public RecycleableAssetCollection(Func<T> instanceFactory, int numInstances)
        {
            foreach (var i in Enumerable.Range(0, numInstances))
            {
                Instances.Add(instanceFactory());
            }
        }

        public void InitializeNextInstance(bool isAsleep, Vector2 initialPosition, Vector2 velocity)
        {
            // avoid accessing outside the bound of the list of Instances
            if (NextInstance >= Instances.Count)
            {
                NextInstance = 0;
            }

            Instances[NextInstance].Initialize(isAsleep, initialPosition, velocity);

            NextInstance++;
        }
    }
}
