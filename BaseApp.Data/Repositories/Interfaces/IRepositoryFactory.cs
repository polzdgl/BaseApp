using BaseApp.Data.Company.Interfaces;
using BaseApp.Data.Context;
using BaseApp.Data.User.Interfaces;

namespace BaseApp.Data.Repositories.Interfaces
{
    public interface IRepositoryFactory
    {
        ApplicationDbContext Context { get; set; }

        IUserRepository UserRepository { get; }

        ICompanyInfoRepository CompanyInfoRepository { get; }

        IPublicCompanyRepository PublicCompanyRepository { get; }
    }
}
