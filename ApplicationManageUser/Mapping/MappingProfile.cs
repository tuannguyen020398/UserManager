using ApplicationService.Model.UserModel;
using AutoMapper;
using BE.DAL.Entities;

namespace ApplicationManageUser.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateUserModel,User>();
            CreateMap<User, UserModelPading>();

        }
    }
}
