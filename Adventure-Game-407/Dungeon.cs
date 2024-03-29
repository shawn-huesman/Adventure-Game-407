﻿namespace Adventure_Game_407
{
    // Dungeon class
    public class Dungeon
    {
        /*
         * Contains a 2d array of procedurally generated rooms with
         * some magic dampening rooms, regular rooms, 1 entry, and 1 exit
         */
        public int Rows { get; private set; }
        public int Cols { get; private set; }
        
        public Room CurrentRoom { get; private set; }
        public Room[,] Rooms { get; }
        public int StartRow { get; }
        public int StartCol { get; }
        public Room Up { get; private set; }
        public Room Down { get; private set; }
        public Room Left { get; private set; }
        public Room Right { get; private set;  }
        
        private int[] _lastVisited;
        private int[] _secondLastVisited;

        // Dungeon constructor that generates 8 by 8 dungeon rooms
        public Dungeon()
        {
            Rows = 4;
            Cols = 8;
            Rooms = new Room[Rows, Cols];
            var visited = new bool[Rows, Cols];
            
            // initialize all rooms to empty
            // initialize all visited to false
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Rooms[i, j] = new Room(' ', i, j);
                    visited[i, j] = false;
                }
            }
            
            // Randomly select a starting location (entry)
            StartRow = StaticRandom.Instance.Next(Rows);
            StartCol = StaticRandom.Instance.Next(Cols);
            _lastVisited = new[] {StartRow, StartCol};
            _secondLastVisited = new[] { 0, 0};
            
            // procedurally create rooms
            CreateRooms(Rooms, StartRow, StartCol, visited, 0);
            
            // Mark exit
            Rooms[_lastVisited[0], _lastVisited[1]].Type = 'X';
            
            // Mark entry
            Rooms[StartRow, StartCol].Type = 'E';
            
            // Mark boss room
            Rooms[_secondLastVisited[0], _secondLastVisited[1]].Type = 'B';
            
            // Start in entry room
            CurrentRoom = Rooms[StartRow, StartCol];
            // Remove monsters from entry room
            CurrentRoom.Monster = null;
            
            // Figure out relative position
            CalculateCurrentRoomPosition();
        }

        private void CreateRooms(Room[,] rooms, int row, int col, bool[,] visited, int roomCount)
        {
            /*
             * Procedurally creates dungeon rooms recursively using Depth-First Search (DFS)
             * and puts them into an array.
             * The first 8 connected rooms are guaranteed to be in the dungeon. Each consecutive
             * room has a 25% chance to be in the dungeon. Each room selected to be in the dungeon
             * has a 14.3% chance to be magic dampening.
             * Keeps track of the last visited spot in the array to later make an exit
             */

            Rows = rooms.GetLength(0);
            Cols = rooms.GetLength(1);
            const int minRoomCount = 8;

            // index out of bounds or room has been visited
            if (row < 0 || col < 0 || row >= Rows || col >= Cols || visited[row, col]) return;

            // chance to pick a room = 1/4
            var chanceToPickRoom = StaticRandom.Instance.Next(4);
            visited[row, col] = true;

            if (chanceToPickRoom == 1 || roomCount < minRoomCount)
            {
                // chance to pick magic dampening room = 1/7
                var chanceToPickMagicRoom = StaticRandom.Instance.Next(7);
                if (chanceToPickMagicRoom == 1)
                {
                    // Magic-dampening room
                    Rooms[row, col].Type = 'M';
                }
                else
                {
                    // Regular room
                    Rooms[row, col].Type = 'R';
                }

                roomCount += 1;
                // keep track of second last visited to make a boss room
                _secondLastVisited[0] = _lastVisited[0];
                _secondLastVisited[1] = _lastVisited[1];

                // keep track of last visited location to make an exit
                _lastVisited[0] = row;
                _lastVisited[1] = col;

                // go left
                CreateRooms(Rooms, row, col - 1, visited, roomCount);
                // go up
                CreateRooms(Rooms, row + 1, col, visited, roomCount);
                // go down
                CreateRooms(Rooms, row - 1, col, visited, roomCount);
                // go right
                CreateRooms(Rooms, row, col + 1, visited, roomCount);
            }
        }
        
        private void CalculateCurrentRoomPosition()
        {
            /*
             * Used to realign relative room position whenever room changes or is initalized
             */
            if (CurrentRoom.Row - 1 < 0)
            {
                Up = null;
            }
            else
            {
                Up = Rooms[CurrentRoom.Row - 1, CurrentRoom.Col];
            }

            if (CurrentRoom.Row + 1 >= Rows)
            {
                Down = null;
            }
            else
            {
                Down = Rooms[CurrentRoom.Row + 1, CurrentRoom.Col];
            }

            if (CurrentRoom.Col - 1 < 0)
            {
                Left = null;
            }
            else
            {
                Left = Rooms[CurrentRoom.Row, CurrentRoom.Col - 1];
            }

            if (CurrentRoom.Col + 1 >= Cols)
            {
                Right = null;
            }
            else
            {
                Right = Rooms[CurrentRoom.Row, CurrentRoom.Col + 1];
            }

        }

        // Returns true if Up is not null and not empty
        public bool CanMoveUp()
        {
            if (Up != null && !Up.IsEmpty())
            {
                return true;
            }

            return false;
        }

        // Returns true if Down is not null and not empty
        public bool CanMoveDown()
        {
            if (Down != null && !Down.IsEmpty())
            {
                return true;
            }

            return false;
        }

        // Returns true if Left is not null and not empty
        public bool CanMoveLeft()
        {
            if (Left != null && !Left.IsEmpty())
            {
                return true;
            }

            return false;
        }

        // Returns true if Right is not null and not empty
        public bool CanMoveRight()
        {
            if (Right != null && !Right.IsEmpty())
            {
                return true;
            }

            return false;
        }
        
        public void MoveUp()
        {
            if (CanMoveUp())
            {
                CurrentRoom = Up;
            }
            
            CalculateCurrentRoomPosition();
        }
        
        public void MoveDown()
        {
            if (CanMoveDown())
            {
                CurrentRoom = Down;
            }
            
            CalculateCurrentRoomPosition();
        }
        
        public void MoveLeft()
        {
            if (CanMoveLeft())
            {
                CurrentRoom = Left;
            }
            
            CalculateCurrentRoomPosition();
        }
        
        public void MoveRight()
        {
            if (CanMoveRight())
            {
                CurrentRoom = Right;
            }
            
            CalculateCurrentRoomPosition();
        }
    }
}