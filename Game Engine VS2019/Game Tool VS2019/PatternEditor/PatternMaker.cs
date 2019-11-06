using System.Collections.Generic;

namespace Game_Tool_VS2019.PatternEditor
{
    public static class PatternMaker
    {
        public static List<Block> IfStatements = new List<Block>();
        public static List<Block> Behaviours = new List<Block>();

        public static void Initialize()
        {
            IfStatements.Add(new Block(0, "( ) 키를 누른다면", EBlockType.IfStatement));
            IfStatements.Add(new Block(1, "( ) 와 충돌한다면", EBlockType.IfStatement));
            IfStatements.Add(new Block(2, "( ) 가", EBlockType.IfStatement));
            IfStatements.Add(new Block(3, "( ) 나", EBlockType.IfStatement));
            IfStatements.Add(new Block(4, "( ) 다", EBlockType.IfStatement));
            IfStatements.Add(new Block(5, "( ) 라", EBlockType.IfStatement));
            IfStatements.Add(new Block(6, "( ) 마", EBlockType.IfStatement));
            IfStatements.Add(new Block(7, "( ) 바", EBlockType.IfStatement));
            IfStatements.Add(new Block(8, "( ) 사", EBlockType.IfStatement));
            IfStatements.Add(new Block(9, "( ) 아", EBlockType.IfStatement));
            IfStatements.Add(new Block(10, "( ) 자", EBlockType.IfStatement));
            IfStatements.Add(new Block(11, "( ) 차", EBlockType.IfStatement));

            Behaviours.Add(new Block(0, "( ) 의 속도로 좌우로 이동한다.", EBlockType.Behaviour));
            Behaviours.Add(new Block(1, "( ) 의 힘으로 점프한다.", EBlockType.Behaviour));
        }
    }
}
