using System;
using System.Collections.Generic;

public class WeightedRandomPicker<T>
{
    private Random random = new Random();
    private List<T> items = new List<T>();
    private List<int> weights = new List<int>();

    public void AddItem(T item, int weight)
    {
        items.Add(item);
        weights.Add(weight);
    }

    public T PickRandom()
    {
        int totalWeight = 0;
        foreach (int weight in weights)
        {
            totalWeight += weight;
        }

        int randomNumber = random.Next(0, totalWeight);
        int cumulativeWeight = 0;

        for (int i = 0; i < items.Count; i++)
        {
            cumulativeWeight += weights[i];
            if (randomNumber < cumulativeWeight)
            {
                return items[i];
            }
        }

        // 이 지점에 도달하는 것은 보통 발생하지 않지만, 에러 핸들링을 위해 기본값을 반환.
        return default(T);
    }
}