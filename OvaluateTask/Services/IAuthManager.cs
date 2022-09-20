using Microsoft.AspNetCore.Mvc;
using OvaluateTask.Models;
using System.Threading.Tasks;

namespace OvaluateTask.Services
{
    public interface IAuthManager
    {
        Task<ResponseModel> RegisterUserAsync(RegisterModel model);
        Task<ResponseModel> LoginUser(LoginModel model);
    }
}
