using ApplicationService.Model.UserModel;
using AutoMapper;
using BE.DAL.Entities;

namespace ApplicationManageUser.Mapping
{
    public class MappingProfile: Profile
    {

        /// <summary>Initializes a new instance of the <see cref="MappingProfile" /> class.</summary>
        /// <Modified>
        /// Name Date Comments
        /// tuannx 12/1/2022 created
        /// </Modified>
        public MappingProfile()
        {
            CreateMap<CreateUserModel,User>();
            CreateMap<User, UserModelPading>();
            CreateMap<UpdateUserModel, User>();
        }
    }
}
