using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty.Models
{
    public class UserEditViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string PostalCode { get; set; }

        public IList<string> Roles { get; set; } // Список ролей для выпадающего списка

        public string SelectedRole { get; set; } // Выбранная роль из выпадающего списка
    }
}
