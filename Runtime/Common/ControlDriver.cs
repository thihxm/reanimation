using System;
using UnityEngine;

namespace Aarthificial.Reanimation.Nodes
{
    [Serializable]
    public class ControlDriver
    {
        [SerializeField] protected string name;
        [SerializeField] protected bool autoIncrement;
        [SerializeField] protected bool runOnce;
        [SerializeField] protected bool percentageBased;
        [HideInInspector] [SerializeField] protected string guid = Guid.NewGuid().ToString();

        public ControlDriver()
        {
        }

        public ControlDriver(string name = null, bool autoIncrement = false, bool runOnce = false, bool percentageBased = false)
        {
            this.name = name;
            this.autoIncrement = autoIncrement;
            this.runOnce = runOnce;
            this.percentageBased = percentageBased;
        }

        public int ResolveDriver(IReadOnlyReanimatorState previousState, ReanimatorState nextState, int size)
        {
            if (size == 0) return 0;
            string driverName = string.IsNullOrEmpty(name) ? guid : name;

            if (percentageBased)
            {
                float floatDriverValue = Mathf.Clamp01(previousState.GetFloat(driverName));
                if (floatDriverValue < 1)
                    return Mathf.FloorToInt(floatDriverValue * size);

                return size - 1;
            }

            int driverValue = previousState.Get(driverName) % size;
            if (autoIncrement) {
                var nextValue = (driverValue + 1) % size;
                if (runOnce && nextValue == 0) {
                    return driverValue;
                } else {
                  nextState.Set(driverName, nextValue);
                }
            }

            return driverValue;
        }
    }
}