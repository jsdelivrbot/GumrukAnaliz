using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gumruk.Entity;
using Gumruk.Repository;
using Gumruk.UnitOfWork.Contract;

namespace Gumruk.UnitOfWork
{
    public class BSUser : IUser , IDisposable
    {
        public void Dispose()
        {
            this.Dispose();
        }

        public List<users> GetAllUsers()
        {
            IRepositoryBase<users> _rep = new RepositoryBase<users>();

            List<users> response = new List<users>();

            response = _rep.All<users>().ToList();

            return response;
        }

        public users GetUserByID(int ID)
        {
            IRepositoryBase<users> _rep = new RepositoryBase<users>();

            users response = new users();

            response = _rep.Get(p => p.id == ID);

            return response;
        }

        public users Login(string loginName, string Pass)
        {
            IRepositoryBase<users> _rep = new RepositoryBase<users>();

            users user = new users();

            return  _rep.Get(p => p.name == loginName && p.password == Pass);

        }
    }
}
