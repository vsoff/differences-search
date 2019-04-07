using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleApp.ExampleClasses
{
    public class People : Entity
    {
        public DateTime BirdthDay { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public Home SweetHome { get; set; }

        public override bool Equals(object obj)
        {
            var people = obj as People;
            return people != null &&
                   Id == people.Id &&
                   BirdthDay == people.BirdthDay &&
                   FullName == people.FullName &&
                   Phone == people.Phone &&
                   EqualityComparer<Home>.Default.Equals(SweetHome, people.SweetHome);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, BirdthDay, FullName, Phone, SweetHome);
        }
    }
}
