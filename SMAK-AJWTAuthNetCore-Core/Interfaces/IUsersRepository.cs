namespace SMAK_AJWTAuthNetCore_Core.Interfaces
{
    public interface IUsersRepository<T>
    {
        public Task<T> Create(T _object);

        public void Delete(T _object);

        public void Update(T _object);

        public IEnumerable<T> GetAll();

        public T GetById(int Id);

        public T GetByUserName(string userName);

        public T GetByEmail(string email);
    }
}
