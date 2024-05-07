using UnityEngine;

namespace QGMiniGame
{
    public static class GlobalDefines
    {
        public const string MINIGAME_MENU_ITEM_ROOT = "OPPO小游戏";
        public const string BUILD_TOOL_MODULE = "BuildTool";

        public static bool IsValid(this string str) => !string.IsNullOrEmpty(str);
    }
}