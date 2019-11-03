using System.Collections.Generic;

namespace Game_Tool_VS2019.Pattern
{
    public static class PatternMaker
    {
        public static List<Block> IfStatements = new List<Block>();
        public static List<Block> Behaviours = new List<Block>();

        public static void Initialize()
        {
            IfStatements.Add(new Block(0, "( ) 키를 누른다면", EBlockType.IfStatement));
            IfStatements.Add(new Block(1, "( ) 와 충돌한다면", EBlockType.IfStatement));

            Behaviours.Add(new Block(0, "( ) 의 속도로 좌우로 이동한다.", EBlockType.Behaviour));
            Behaviours.Add(new Block(1, "( ) 의 힘으로 점프한다.", EBlockType.Behaviour));
        }
    }
}
