using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame
{
    internal struct Tile
    {

        private char m_Letter;
        private readonly int r_Row;
        private readonly int r_Column;
        private bool m_Shown;

        public Tile(int i_Row, int i_Column)
        {
            m_Letter = ' ';
            r_Row = i_Row;
            r_Column = i_Column;
            m_Shown = false;
        }

        public Tile(int i_Row, int i_Column, char letter)
        {
            m_Letter = letter;
            r_Row = i_Row;
            r_Column = i_Column;
            m_Shown = false;
        }

        public char Letter
        {
            get { return m_Letter; }
            set { m_Letter = value; }
        }

        public bool Shown
        {
            get { return m_Shown; }
            set { m_Shown = value; }
        }

        public int Row
        {
            get { return r_Row; }
        }

        public int Column
        {
            get { return r_Column; }
        }

    }
}