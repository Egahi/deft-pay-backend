using AutoMapper;
using deft_pay_backend.Models;
using deft_pay_backend.ModelsDTO.Requests;
using deft_pay_backend.ModelsDTO.Responses;
using System;

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
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(
                    src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(
                    src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(
                    src => src.Email))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(
                    src => src.PhoneNumber ?? src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(
                    src => src.PhoneNumber))
                .ForMember(dest => dest.ProfilePictureUrl, opt => opt.MapFrom(
                    src => src.ProfilePictureUrl))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(
                    src => src.Gender))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(
                    src => DateTime.ParseExact(src.DateOfBirth, "dd/MM/yyyy", null)))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Users Controller
            CreateMap<ApplicationUser, UserProfileSummaryDTO>();
        }
    }
}
