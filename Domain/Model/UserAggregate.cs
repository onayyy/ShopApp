using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class UserAggregate
    {
        public int Id { get; private set; }

        public string Name { get; private set; }

        public string Surname { get; private set; }

        public string Email { get; private set; }

        public string Password { get; private set; }

        public int Gender { get; private set; } // 1 erkek, 0 kadın , 2 Belirtmek istemiyorum

        public virtual List<AddressAggregate> Addresses { get; private set; }

        [JsonIgnore]
        public virtual List<OrderAggregate> Orders { get; private set; }

        public UserAggregate()
        {
            // only db
        }

        public UserAggregate(string name, string surname, string email, string password, int gender)
        {
            Name = name;
            Surname = surname;
            Email = email;
            Password = password;
            Gender = gender;
        }

        public static UserAggregate Create(string name, string surname, string email, string password, int gender)
        {
            return new UserAggregate(name, surname, email, password, gender);
        }

        public void Update(string name, string surname, string password, int gender)
        {
            Name = name;
            Surname = surname;
            Password = password;
            Gender = gender;
        }
  
    }
}
