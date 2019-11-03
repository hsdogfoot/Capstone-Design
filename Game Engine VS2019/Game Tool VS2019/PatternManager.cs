using System.Collections.Generic;

namespace Game_Tool_VS2019
{
    public static class PatternManager
    {
        public static List<Block> BehaviourList = new List<Block>();
        public static List<Block> IfList = new List<Block>();
        public static List<Pattern> PatternList = new List<Pattern>();

        public static void Initialize()
        {
            BehaviourList.Add(new Block(0, "X 키를 누른다면", eBlockType.Behaviour));
            BehaviourList.Add(new Block(1, "X 와 충돌한다면", eBlockType.Behaviour));

            IfList.Add(new Block(0, "X 키를 누른다면", eBlockType.IfStatement));
            IfList.Add(new Block(1, "X 와 충돌한다면", eBlockType.IfStatement));
        }
    }
}
