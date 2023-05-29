using System.Collections.Generic;

public class SaveInChunkLootBox : SaveInChunk<InteractLootBox>
{
    public void Load(List<Item> items)
    {
        _saveObject.Init(items);
    }
}