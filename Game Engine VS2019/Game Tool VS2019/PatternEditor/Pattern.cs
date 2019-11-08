using System.Collections.Generic;

namespace Game_Tool_VS2019.PatternEditor
{
    public class Pattern
    {
        public string Name { get; private set; }
        
        public LinkedList<Block> IfStatements = new LinkedList<Block>();
        public Block Behaviour;

        public Pattern(string name)
        {
            Name = name;
        }

        public void SetName(string name)
        {
            Name = name;
        }
    }
}
