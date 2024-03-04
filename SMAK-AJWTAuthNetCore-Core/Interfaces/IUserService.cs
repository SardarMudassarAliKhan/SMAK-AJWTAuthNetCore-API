namespace SMAK_AJWTAuthNetCore_Core.Interfaces
{
    public interface IUserService<TUser> where TUser : class
    {
        public Task<TUser> Create(TUser _object);

        public void Delete(TUser _object);

        public void Update(TUser _object);

        public IEnumerable<TUser> GetAll();

        public TUser GetById(string Id);
    }
}
