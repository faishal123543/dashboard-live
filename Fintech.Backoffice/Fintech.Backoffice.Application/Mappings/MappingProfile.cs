using AutoMapper;
using Fintech.Backoffice.Application.DTOs;
using Fintech.Backoffice.Application.ViewModels;
using Fintech.Backoffice.Domain.Enums;
using ApplicationEntity = Fintech.Backoffice.Domain.Entities.Application;
using PartnerEntity = Fintech.Backoffice.Domain.Entities.Partner;
using CustomerEntity = Fintech.Backoffice.Domain.Entities.Customer;

namespace Fintech.Backoffice.Application.Mappings
{
    /// <summary>
    /// AutoMapper profile defining entity to DTO mappings and vice versa.
    /// Why: Reduces boilerplate code for object-to-object mapping.
    /// Uses type aliases to avoid namespace collision between
    /// 'Fintech.Backoffice.Application' (namespace) and 'Application' (entity).
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ============================================================================
            // Application Entity -> ApplicationDto
            // ============================================================================
            CreateMap<ApplicationEntity, ApplicationDto>()
                .ForMember(
                    dest => dest.ApplicationId,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest.CustomerName,
                    opt => opt.MapFrom(src => src.Customer != null ? src.Customer.CustomerName : ""))
                .ForMember(
                    dest => dest.CustomerMobileNumber,
                    opt => opt.MapFrom(src => src.Customer != null ? src.Customer.MobileNumber : ""))
                .ForMember(
                    dest => dest.PartnerName,
                    opt => opt.MapFrom(src => src.Partner != null ? src.Partner.PartnerName : ""))
                .ForMember(
                    dest => dest.PartnerCode,
                    opt => opt.MapFrom(src => src.Partner != null ? src.Partner.PartnerCode : ""))
                .ForMember(
                    dest => dest.StatusCode,
                    opt => opt.MapFrom(src => (int)src.Status))
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()));

            // ApplicationDto -> Application (for updates)
            CreateMap<ApplicationDto, ApplicationEntity>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.ApplicationId))
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => (ApplicationStatus)src.StatusCode))
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.Partner, opt => opt.Ignore());

            // ============================================================================
            // Partner Mappings
            // ============================================================================
            CreateMap<PartnerEntity, PartnerDto>()
                .ForMember(
                    dest => dest.PartnerId,
                    opt => opt.MapFrom(src => src.Id));
        }
    }
}
