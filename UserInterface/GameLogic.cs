using MemoryGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame
{
    internal class GameLogic
    {
        private Player r_Player1;
        private Player r_Player2;
        public Player m_CurrentPlayer;
        private int m_Height;
        private int m_Width;
        public static Tile[,] m_GameBoard;
        public static eGameStatus m_CurrentGameStatus;
        public bool m_IsGameRestarted;
        private bool m_AllShown;
        
        public GameLogic()
        {
            m_CurrentGameStatus = eGameStatus.Playing;
            m_AllShown = false;
            m_IsGameRestarted = false;
        }
        
        
        public bool IsGameRestarted
        {
            get { return m_IsGameRestarted; }
            set { m_IsGameRestarted = value; }
        }
        public Player CurrentPlayer
        {
            get { return m_CurrentPlayer; }
            set { m_CurrentPlayer = value; }
        }

        public Player Player1
        {
            get { return r_Player1; }
            set { r_Player1 = value; }
        }
        public Player Player2
        {
            get { return r_Player2; }
            set { r_Player2 = value; }
        }

        public bool AllShown
        {
            get { return m_AllShown; }
            set { m_AllShown = value; }
        }

        public void isAllTilesShown()
        {
            bool allTilesAreShown = true;
            foreach(Tile tile in m_GameBoard)
            {
                if (!tile.Shown)
                {
                    allTilesAreShown = false;
                }
            }
            this.m_AllShown = allTilesAreShown;
        }

        
        public void ChangePlayer()
        {
            if(m_CurrentPlayer == this.r_Player1)
            {
                this.m_CurrentPlayer = this.r_Player2;
            }
            else
            {
                this.m_CurrentPlayer = this.r_Player1;
            }
        }
        
        public static Tile[,] CreateBoard(int i_BoardHeight, int i_BoardWidth)
        {
            char[] gameLetters = new char[i_BoardWidth * i_BoardHeight / 2];
            for (int i = 0; i < gameLetters.Length; i++)
            {
                gameLetters[i] = (char)('A' + i);
            }
            List<Tile> randomTiles = new List<Tile>(i_BoardWidth * i_BoardHeight);
            for (int i = 0; i < i_BoardHeight; i++)
            {
                for (int j = 0; j < i_BoardWidth; j++)
                {
                    randomTiles.Add(new Tile(i, j));
                }
            }
            Tile[,] gameBoard = new Tile[i_BoardHeight, i_BoardWidth];
            Random random = new Random();
            foreach (char letter in gameLetters)
            {
                int randomCoordinate1 = random.Next(randomTiles.Count);
                Tile matchingTile1 = randomTiles[randomCoordinate1];
                randomTiles.Remove(matchingTile1);

                int randomCoordinate2 = random.Next(randomTiles.Count);
                Tile matchingTile2 = randomTiles[randomCoordinate2];
                randomTiles.Remove(matchingTile2);

                int i = matchingTile1.Row;
                int j = matchingTile1.Column;
                gameBoard[i, j] = new Tile(i, j, letter);

                i = matchingTile2.Row;
                j = matchingTile2.Column;
                gameBoard[i, j] = new Tile(i, j, letter);
            }

            return gameBoard;
        }
        public int Width
        {
            get { return m_Width; }
            set { m_Width = value; }
        }
        public int Height
        {
            get { return m_Height; }
            set { m_Height = value; }
        }
        public Tile[,] GameBoard
        {
            get { return m_GameBoard; }
            set { m_GameBoard = value; }
        }


        public static eGameStatus CurrentGameStatus
        {
            get
            {
                return m_CurrentGameStatus;
            }

            set
            {
                m_CurrentGameStatus = value;
            }
        }




    }
    public enum eGameStatus
    {
        Playing,
        Finished
    }
}
