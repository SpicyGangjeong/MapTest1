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

        // �� ������ �����ϴ� ���� ���� �߻����� ������, ���� �ڵ鸵�� ���� �⺻���� ��ȯ.
        return default(T);
    }
}