using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class AssetManager
{
    public static SpriteAtlas atlas = Resources.Load<SpriteAtlas>("Atlas");
    public static Building_Scriptable[] Buildings = Resources.LoadAll<Building_Scriptable>("Building");

    public static Sprite GetAtlas(string temp)
    {
        //이름으로 찾기
        return atlas.GetSprite(temp);
    }
}
