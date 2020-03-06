using AutoMapper;
using deft_pay_backend.Models;
using deft_pay_backend.ModelsDTO.Requests;
using deft_pay_backend.ModelsDTO.Responses;

namespace deft_pay_backend.AutoMapper
{
    /// <summary>
    /// AutoMapper profile class for mapping Objects to Data Transfer Objects
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// public constructor
        /// </summary>
        public MappingProfile()
        {
            // Accounts Controller
            CreateMap<UserRegisterDTO, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(
                    src => src.BVN))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Users Controller
            CreateMap<ApplicationUser, UserProfileSummaryDTO>();
            CreateMap<TransactionToken, TransactionTokenResponseDTO>()
                .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(
                    src => src.ExpiryDate.ToLongDateString()))
                .ForMember(dest => dest.ExpiryTime, opt => opt.MapFrom(
                    src => src.ExpiryDate.ToLongTimeString()));
        }
    }
}
