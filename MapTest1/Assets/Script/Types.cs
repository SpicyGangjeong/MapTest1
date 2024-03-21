using System.Collections.Generic;

public class Types
{
    public enum RoomType
    {
        Boss,
        Elite,
        Battle,
        Rest,
        Treasure,
        Event,
        Merchant
    }
    public Dictionary<RoomType, int> EncounterPairs = new Dictionary<RoomType, int>
    {
        { RoomType.Boss, 0 },
        { RoomType.Elite, 160 },
        { RoomType.Battle, 450 },
        { RoomType.Rest, 120 },
        { RoomType.Treasure, 0 },
        { RoomType.Event, 220 },
        { RoomType.Merchant, 50 },
    };
}
