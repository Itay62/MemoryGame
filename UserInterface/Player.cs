using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame
{
    internal class Player
    {
        private readonly string r_Name;
        private int m_Score;
        private readonly ePlayerType r_Type;
        public Dictionary<char, List<int[]>> ComputersAI;

        public Player(string i_Name, int i_playerType)
        {
            r_Name = i_Name;
            m_Score = 0;
            r_Type = (ePlayerType)i_playerType;
        }

        public Player()
        {
            r_Name = "Computer";
            m_Score = 0;
            r_Type = ePlayerType.Computer;
            ComputersAI = new Dictionary<char, List<int[]>>();
        }

        public string Name
        {
            get { return r_Name; }
        }

        public int Score
        {
            get { return m_Score; }
            set { m_Score = value; }
        }

        public ePlayerType Type
        {
            get { return r_Type; }
        }

        public enum ePlayerType
        {
            Human,
            Computer
        }
    }
}