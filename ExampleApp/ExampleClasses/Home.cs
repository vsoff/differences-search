using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleApp.ExampleClasses
{
    public class Home : Entity
    {
        public string Address { get; set; }
        public bool IsPrivate { get; set; }
        public int RoomNumber { get; set; }
        public int Floor { get; set; }
        public float Square { get; set; }

        public override bool Equals(object obj)
        {
            var home = obj as Home;
            return home != null &&
                   Id == home.Id &&
                   Address == home.Address &&
                   IsPrivate == home.IsPrivate &&
                   RoomNumber == home.RoomNumber &&
                   Floor == home.Floor &&
                   Square == home.Square;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Address, IsPrivate, RoomNumber, Floor, Square);
        }
    }
}
