using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gumruk.Entity;

namespace Gumruk.UnitOfWork.Contract
{
    public interface IUser
    {
        List<users> GetAllUsers();

        users GetUserByID(int ID);

        users Login(string loginName,string Pass);

    }
}
