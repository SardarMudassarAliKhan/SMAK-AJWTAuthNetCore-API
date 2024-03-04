using SMAK_AJWTAuthNetCore_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMAK_AJWTAuthNetCore_Core.Interfaces
{
    public interface IUsersRepository<T>
    {
        public Task<T> Create(T _object);

        public void Delete(T _object);

        public void Update(T _object);

        public IEnumerable<T> GetAll();

        public T GetById(string Id);
    }
}
