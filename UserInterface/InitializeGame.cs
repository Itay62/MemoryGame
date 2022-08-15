using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoryGame;
using Ex02.ConsoleUtils;
namespace MemoryGame
{
    internal class InitializeGame
    {
        private static GameLogic m_LogicManager;
        
        public static void RunFirstGame()
        {
            m_LogicManager = new GameLogic();
            StartGame();
        }

        public static void StartGame()
        {
            if (!m_LogicManager.IsGameRestarted)
            {
                GetInitialUserInput();
            }
            GetGameBoardDimensionsInput();
            m_LogicManager.GameBoard = GameLogic.CreateBoard(m_LogicManager.Height, m_LogicManager.Width);
            DrawBoard();
            bool isRight = false;
            while(m_LogicManager.AllShown){
                
                if(m_LogicManager.CurrentPlayer.Type == Player.ePlayerType.Computer)
                {
                    isRight = ComputerTurn(); 
                }
                else
                {
                    isRight = PlayerTurn();
                }
                if (isRight)
                {
                    Console.WriteLine("Great you found a match!");
                    Console.WriteLine("{0}'s current score is: {1}", m_LogicManager.CurrentPlayer.Name
                        , m_LogicManager.CurrentPlayer.Score);
                    Sleep(2000);
                }
                else
                {
                    m_LogicManager.ChangePlayer();
                    Console.WriteLine("The turn goes to: {0}", m_LogicManager.CurrentPlayer.Name);
                    Sleep(2000);
                }
                DrawBoard();
                m_LogicManager.isAllTilesShown();
            }
            string scoreBoard = String.Format(
@"             {0}       {1}
Final score:    {2}    -      {3}", m_LogicManager.Player1.Name, m_LogicManager.Player2.Name,
                               m_LogicManager.Player1.Score, m_LogicManager.Player2.Score);
            Console.WriteLine(scoreBoard);
            WhoWon();
            Console.WriteLine("Would you like to play another game?");
            Console.WriteLine("Please enter Y/N");
            string restartGame = Console.ReadLine();
            if (restartGame == "Y")
            {
                RestartGame();
            }
            EndGame();
        }

        private static void GetInitialUserInput()
        {
            Console.WriteLine("Please enter your name: ");
            string playerNameOne = Console.ReadLine();
            string playerNameTwo = "";
            Player playerOne = new Player(playerNameOne, 0);
            Player playerTwo;
            int componentPick;
            Console.WriteLine("Enter 1 to play against another player, or 2 to play against the computer.");
            string playersChoiceInput = Console.ReadLine();
            bool isCorrectInput = int.TryParse(playersChoiceInput, out componentPick);
            while ((componentPick!= 1 && componentPick != 2) || !isCorrectInput)
            {
                Console.WriteLine("You have entered a wrong input, please try again:");
                playersChoiceInput = Console.ReadLine();
                isCorrectInput = int.TryParse(playersChoiceInput, out componentPick);
            }
            if (componentPick == 1)
            {
                Console.WriteLine("Great! You chose to play against another player.");
                Console.WriteLine("Please enter the 2nd player name: ");
                playerNameTwo = Console.ReadLine();
            }
            else if (componentPick == 2)
            {
                Console.WriteLine("Great! You will now play against the computer.");
            }
            if(playerNameTwo == "")
            {
                playerTwo = new Player();
            }
            else
            {
                playerTwo = new Player(playerNameTwo, 0);
            }
            m_LogicManager.Player1 = playerOne;
            m_LogicManager.Player2 = playerTwo;
            m_LogicManager.CurrentPlayer = playerOne;
        }

        private static void GetGameBoardDimensionsInput()
        {
            int widthChoice = 0, heightChoice = 0;
            Console.WriteLine("Enter width(4 or 6): ");
            string widthInput = Console.ReadLine();
            bool isCorrectInput = int.TryParse(widthInput, out widthChoice);
            while ((widthChoice != 4 && widthChoice != 6) || !isCorrectInput)
            {
                Console.WriteLine("You have entered a wrong input, please enter 4 or 6: ");
                widthInput = Console.ReadLine();
                isCorrectInput = int.TryParse(widthInput, out widthChoice);
            }
            Console.WriteLine("Enter height(4 or 6): ");
            string heightInput = Console.ReadLine();
            isCorrectInput = int.TryParse(heightInput, out heightChoice);
            while ((heightChoice != 4 && heightChoice != 6) || !isCorrectInput)
            {
                Console.WriteLine("You have entered a wrong input, please enter 4 or 6: ");
                heightInput = Console.ReadLine();
                isCorrectInput = int.TryParse(heightInput, out heightChoice);
            }
            m_LogicManager.Width = widthChoice;
            m_LogicManager.Height = heightChoice;
        }

        private static void DrawBoard()
        {
            ClearWindow();
            int numOfEqualSigns = (m_LogicManager.Width * 4) + 1;
            string equalLine = string.Format("  {0}", new string('=', numOfEqualSigns));
            DrawLetterRow(m_LogicManager.Width);
            Console.WriteLine(equalLine);
            for (int i = 0; i < m_LogicManager.Height; i++)
            {
                DrawRow(i);
                Console.WriteLine(equalLine);
            }
            Console.WriteLine();
        }

        private static void DrawLetterRow(int i_Length)
        {
            StringBuilder Row = new StringBuilder(" ");
            for (int i = 0; i < i_Length; i++)
            {
                Row.Append(string.Format("   {0}", (char)(i + 'A')));
            }
            Console.WriteLine(Row.ToString());
        }

        private static void DrawRow(int i_IndexOfRow)
        {
            string startOfRow = string.Format("{0} |", i_IndexOfRow + 1);
            Console.Write(startOfRow);
            for (int i = 0; i < m_LogicManager.Width; i++)
            {
                string tileToPrint = string.Format(" {0} |", m_LogicManager.GameBoard[i_IndexOfRow, i].Shown ? m_LogicManager.GameBoard[i_IndexOfRow, i].Letter : ' ');
                Console.Write(tileToPrint);
            }
            Console.WriteLine();
        }

        private static void ClearWindow()
        {
            Ex02.ConsoleUtils.Screen.Clear();
        }  

        public static bool PlayerTurn()
        {
            bool playerIsRight = false;
            string firstTileCoordinate = "";
            string secondTileCoordinate = "";
            Console.Write("Enter a Column(Letter) and a Row(number) to pick a tile: ");
            InputValidation(true, ref firstTileCoordinate);
            Tile firstTile = GetTileAndShow(firstTileCoordinate);
            DrawBoard(); 
            Console.WriteLine("Please pick the second tile!");
            Console.Write("Enter a Column(Letter) and a Row(number) to pick a tile: ");
            InputValidation(true, ref secondTileCoordinate);
            Tile secondTile = GetTileAndShow(secondTileCoordinate);
            DrawBoard(); 
            char firstTileLetter = firstTile.Letter;
            char secondTileLetter = secondTile.Letter;
            if (firstTileLetter.Equals(secondTileLetter))
            {
                UpdateScoreOfTheCurrentPlayer();
                playerIsRight = true; 
            }
            else
            {
                Console.WriteLine("You failed! try to memorize the tiles!");
                Sleep(2000);
                DrawBoard();
                HideTile(firstTile.Row, firstTile.Column);
                HideTile(secondTile.Row, secondTile.Column);
                DrawBoard();
                Sleep(1000);
            }
            return playerIsRight;
        }

        public static bool CheckIfTwoCoordinatesExist()
        {
            foreach (KeyValuePair<char, List<int[]>> kvp in m_LogicManager.CurrentPlayer.ComputersAI)
            {
                if(kvp.Value.Count == 2 && kvp.Value[0] != kvp.Value[1])
                {
                    int firstRow = m_LogicManager.CurrentPlayer.ComputersAI[kvp.Key][0][0];
                    int firstColumn = m_LogicManager.CurrentPlayer.ComputersAI[kvp.Key][0][1];
                    int secondRow = m_LogicManager.CurrentPlayer.ComputersAI[kvp.Key][1][0];
                    int secondColumn = m_LogicManager.CurrentPlayer.ComputersAI[kvp.Key][1][1];
                    m_LogicManager.GameBoard[firstRow, firstColumn].Shown = true;
                    Sleep(1000);
                    m_LogicManager.GameBoard[secondRow, secondRow].Shown = true;
                    Sleep(1000);
                    m_LogicManager.CurrentPlayer.Score += 1;
                    return true;
                }
            }
            return false;
        }
        public static bool IsNewLetter(Tile i_TilePick)
        {
            if (!m_LogicManager.CurrentPlayer.ComputersAI.ContainsKey(i_TilePick.Letter))
            {
                return true;
            }
            return false;
        }

        public static void Sleep(int x)
        {
            System.Threading.Thread.Sleep(x);
        }

        public static void ShowTile(int i_Row, int i_Column)
        {
            m_LogicManager.GameBoard[i_Row, i_Column].Shown = true;
        }

        public static void HideTile(int i_Row, int i_Column)
        {
            m_LogicManager.GameBoard[i_Row, i_Column].Shown = false;
        }

        public static void UpdateScoreOfTheCurrentPlayer()
        {
            m_LogicManager.CurrentPlayer.Score += 1;
        }
        public static bool ComputerTurn()
        {
            if (CheckIfTwoCoordinatesExist())
            {
                return true;
            }
            List<int[]> coordinatesMemory = new List<int[]>();
            int[] singleCoordinate = new int[2];
            bool isComputerRight = false;
            Random randomObjectInstance = new Random();
            int randomColumn = randomObjectInstance.Next(m_LogicManager.Width);
            int randomRow = randomObjectInstance.Next(m_LogicManager.Height);
            while (m_LogicManager.GameBoard[randomRow, randomColumn].Shown)
            {
                randomColumn = randomObjectInstance.Next(m_LogicManager.Width);
                randomRow = randomObjectInstance.Next(m_LogicManager.Height);
            }
            singleCoordinate[0] = randomRow;
            singleCoordinate[1] = randomColumn;
            coordinatesMemory.Add(singleCoordinate);
            Tile firstTilePick = m_LogicManager.GameBoard[randomRow, randomColumn];
            ShowTile(randomRow, randomColumn);
            DrawBoard();
            Sleep(1000);
            
            if (IsNewLetter(firstTilePick)){
                m_LogicManager.CurrentPlayer.ComputersAI.Add(firstTilePick.Letter, coordinatesMemory);
            }
            if (!IsNewLetter(firstTilePick) 
                && m_LogicManager.CurrentPlayer.ComputersAI[firstTilePick.Letter].Count == 1 
                && (m_LogicManager.CurrentPlayer.ComputersAI[firstTilePick.Letter][0][0] 
                != singleCoordinate[0] 
                || m_LogicManager.CurrentPlayer.ComputersAI[firstTilePick.Letter][0][1] != singleCoordinate[1]))
            {
                m_LogicManager.CurrentPlayer.ComputersAI[firstTilePick.Letter].Add(singleCoordinate);
            }
            if(m_LogicManager.CurrentPlayer.ComputersAI[firstTilePick.Letter].Count == 2
                && m_LogicManager.CurrentPlayer.ComputersAI[firstTilePick.Letter][0] != m_LogicManager.CurrentPlayer.ComputersAI[firstTilePick.Letter][1])
            {
                int firstRow = m_LogicManager.CurrentPlayer.ComputersAI[firstTilePick.Letter][0][0];
                int firstColumn = m_LogicManager.CurrentPlayer.ComputersAI[firstTilePick.Letter][0][1];
                int secondRow = m_LogicManager.CurrentPlayer.ComputersAI[firstTilePick.Letter][1][0];
                int secondColumn = m_LogicManager.CurrentPlayer.ComputersAI[firstTilePick.Letter][1][1];
                ShowTile(firstRow, firstColumn);
                DrawBoard();
                Sleep(1000);
                ShowTile(secondRow, secondColumn);
                DrawBoard();
                Sleep(1000);
                isComputerRight = true;
                UpdateScoreOfTheCurrentPlayer();
                m_LogicManager.CurrentPlayer.ComputersAI.Remove(firstTilePick.Letter);
            }
            else
            {
                List<int[]> MemoryAI2 = new List<int[]>();
                int[] AiCoordinate2 = new int[2];
                randomColumn = randomObjectInstance.Next(m_LogicManager.Width);
                randomRow = randomObjectInstance.Next(m_LogicManager.Height);
                while (m_LogicManager.GameBoard[randomRow, randomColumn].Shown)
                {
                    randomColumn = randomObjectInstance.Next(m_LogicManager.Width);
                    randomRow = randomObjectInstance.Next(m_LogicManager.Height);
                }
                AiCoordinate2[0] = randomRow;
                AiCoordinate2[1] = randomColumn;
                MemoryAI2.Add(AiCoordinate2);
                Tile secondTilePick = m_LogicManager.GameBoard[randomRow, randomColumn];
                if (IsNewLetter(secondTilePick))
                {
                    m_LogicManager.CurrentPlayer.ComputersAI.Add(secondTilePick.Letter, MemoryAI2);
                }
                ShowTile(randomRow, randomColumn);
                DrawBoard();
                if (firstTilePick.Letter.Equals(secondTilePick.Letter))
                {
                    UpdateScoreOfTheCurrentPlayer();
                    isComputerRight = true;
                }
                else
                {
                    Sleep(2000);
                    HideTile(firstTilePick.Row, firstTilePick.Column);
                    HideTile(secondTilePick.Row, secondTilePick.Column);
                    DrawBoard();
                    Sleep(1000);
                }
            }         
            return isComputerRight;
        }

        public static void InputValidation(bool i_TilePick,ref string io_Coordinate)
        {
            while (i_TilePick)
            {
                io_Coordinate = Console.ReadLine();
                if(io_Coordinate.Equals("Q"))
                {
                    EndGame();
                }
                bool isValid = IsCoordinateValid(io_Coordinate);
                if (!isValid)
                {
                    Console.Write("Input is not valid." +
                        " Enter a Column(Letter) and a Row(number) to pick a tile: ");
                    continue;
                }
                bool isOutOfBounds = IsCoordinateOutOfBounds(io_Coordinate);
                if (isOutOfBounds)
                {
                    Console.Write("Tile is out of board bounds. Choose another tile: ");
                    continue;
                }

                bool isShown = IsCoordinateShown(io_Coordinate);
                if (isShown)
                {
                    Console.Write("Tile has been revealed already. Choose another tile: ");
                }
                else
                {
                    i_TilePick = false;
                }
            }
        }

        public static Tile GetTileAndShow(string i_Coordinate)
        {
            int i = GetRowFromInput(i_Coordinate);
            int j = GetColumnFromInput(i_Coordinate);
            Tile TileByCoordinate = m_LogicManager.GameBoard[i - 1, j];
            ShowTile(i - 1, j);
            return TileByCoordinate;
        }

        public static int GetRowFromInput(string i_Coordinate)
        {
            char row = i_Coordinate[1];
            int j;
            int.TryParse(row.ToString(), out j);
            return j;
        }

        public static int GetColumnFromInput(string i_Coordinate)
        {
            char column = i_Coordinate[0];
            char col = Char.ToUpper(column);
            int i = (int)(col - 65);
            return i;
        }

        public static bool IsCoordinateValid(string i_Coordinate)
        {
            bool isValid;
            if (i_Coordinate.Length != 2)
            {
                isValid = false;
            }
            else
            {
                char firstCoordinate = i_Coordinate[0];
                char secondCoordinate = i_Coordinate[1];
                if (Char.IsLetter(firstCoordinate) && Char.IsDigit(secondCoordinate))
                {
                    isValid = true;
                }
                else
                {
                    isValid = false;
                }
            }
            return isValid;
        }

        public static bool IsCoordinateOutOfBounds(string i_Coordinate)
        {
            bool isOutOfBounds;
            int i = GetRowFromInput(i_Coordinate);
            int j = GetColumnFromInput(i_Coordinate);
            if (i > m_LogicManager.GameBoard.GetLength(0) || j > m_LogicManager.GameBoard.GetLength(1))
            {
                isOutOfBounds = true;
            }
            else
            {
                isOutOfBounds = false;
            }
            
            return isOutOfBounds;
        }

        public static bool IsCoordinateShown(string i_Coordinate)
        {
            bool isCoordinateShown;
            int i = GetRowFromInput(i_Coordinate);
            int j = GetColumnFromInput(i_Coordinate);
            Tile chosenTile = m_LogicManager.GameBoard[i - 1, j];
            if (chosenTile.Shown)
            {
                isCoordinateShown = true;
            }
            else
            {
                isCoordinateShown = false;
            }
            return isCoordinateShown;
        }

        private static void RestartGame()
        {
            ClearWindow();
            m_LogicManager.AllShown = false;
            m_LogicManager.Player1.Score = 0;
            m_LogicManager.Player2.Score = 0;
            m_LogicManager.IsGameRestarted = true;
            StartGame();
        }

        private static void EndGame()
        {
            ClearWindow();
            Console.WriteLine("Bye Bye!");
            Sleep(2000);
            Environment.Exit(0);
        }

        private static void WhoWon()
        {
            if(m_LogicManager.Player1.Score > m_LogicManager.Player2.Score)
            {
                Console.WriteLine("{0} won the game!", m_LogicManager.Player1.Name);
            }
            else if(m_LogicManager.Player1.Score == m_LogicManager.Player2.Score)
            {
                Console.WriteLine("Its a TIE!");
            }
            else
            {
                Console.WriteLine("{0} won the game!", m_LogicManager.Player2.Name);
            }
        }
    } 
}


