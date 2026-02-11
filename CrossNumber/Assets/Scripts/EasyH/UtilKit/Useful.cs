using System.Collections.Generic;
using UnityEngine;

namespace EasyH.UtilKit {
    
    public class Useful
    {
        public static List<T> Combination<T>(T[] array, int count)
        {
            List<T> retval = new List<T>(count);

            for (int i = array.Length - 1, j = 0; j < count; i--, j++)
            {
                int rand = Random.Range(0, i);

                retval.Add(array[rand]);
                (array[i], array[rand]) = (array[rand], array[i]);

            }

            return retval;
        }

    }
}
