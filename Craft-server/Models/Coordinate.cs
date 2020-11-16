using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Craft_server.Interfaces;

namespace Craft_server.Models
{
    public enum Occupation
    {
        Empty,
        Ship,
        Missed,
        Destroyed
    }

    public class Coordinate : IObserver
    {
        public long Id { get; set; }
        public int Row { get; set; }
        public int Collumn { get; set; }
        public long GameBoardId { get; set; } //required
        public GameBoard GameBoard { get; set; } //required
        public long ShipId { get; set; } //can be null
        public Ship Ship { get; set; } //can be null
        public Occupation Occupation { get; set; } //enum occupation

        public void Update(ISubject subject)
        {
            if (Occupation == Occupation.Ship)
            {
                Occupation = Occupation.Destroyed;
            } else
            {
                Occupation = Occupation.Missed;
            }
        }
    }
}
