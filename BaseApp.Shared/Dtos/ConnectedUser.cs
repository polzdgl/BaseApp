using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApp.Shared.Dtos
{
    public class ConnectedUser
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool Isactive { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
