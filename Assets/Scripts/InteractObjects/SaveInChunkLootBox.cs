using System.Collections.Generic;
using ConfigScripts;

public class SaveInChunkLootBox : SaveInChunk<InteractLootBox>
{
    public void Load(List<ItemConfig> items)
    {
        _saveObject.Init(items);
    }
}