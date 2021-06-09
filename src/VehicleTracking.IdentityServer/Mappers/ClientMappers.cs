using AutoMapper;
using IdentityServer4.EntityFramework.Mappers;
using System.Linq;
using VehicleTracking.IdentityServer.Model;

namespace VehicleTracking.IdentityServer.Mappers {
    public static class ClientMappers {
        static ClientMappers() {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ClientMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static IdentityServer4.Models.Client ToModel(this Client client) {
            return Mapper.Map<IdentityServer4.Models.Client>(client);
        }

        public static Client ToEntity(this IdentityServer4.Models.Client client) {
            return Mapper.Map<Client>(client);
        }
    }
    public static class IdentityResourceMappers {
        static IdentityResourceMappers() {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<IdentityResourceMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static IdentityServer4.Models.IdentityResource ToModel(this IdentityResource resource) {
            return resource == null ? null : Mapper.Map<IdentityServer4.Models.IdentityResource>(resource);
        }

        public static IdentityResource ToEntity(this IdentityServer4.Models.IdentityResource resource) {
            return resource == null ? null : Mapper.Map<IdentityResource>(resource);
        }
    }
    public static class IdentityApiScopeMappers {
        static IdentityApiScopeMappers() {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<IdentityApiScopeMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static IdentityServer4.Models.ApiScope ToModel(this ApiScope resource) {
            return resource == null ? null : Mapper.Map<IdentityServer4.Models.ApiScope>(resource);
        }

        public static ApiScope ToEntity(this IdentityServer4.Models.ApiScope resource) {
            return resource == null ? null : Mapper.Map<ApiScope>(resource);
        }
    }
    public static class ApiResourceMappers {
        static ApiResourceMappers() {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ApiResourceMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static IdentityServer4.Models.ApiResource ToModel(this ApiResource resource) {
            return resource == null ? null : Mapper.Map<IdentityServer4.Models.ApiResource>(resource);
        }

        public static ApiResource ToEntity(this IdentityServer4.Models.ApiResource resource) {
            return resource == null ? null : Mapper.Map<ApiResource>(resource);
        }
    }
    public class IdentityResourceMapperProfile : Profile {
        /// <summary>
        /// <see cref="IdentityResourceMapperProfile"/>
        /// </summary>
        public IdentityResourceMapperProfile() {
            // entity to model
            CreateMap<IdentityResource, IdentityServer4.Models.IdentityResource>(MemberList.Destination)
                .ForMember(x => x.UserClaims, opt => opt.MapFrom(src => src.UserClaims.Select(x => x.Type)));

            // model to entity
            CreateMap<IdentityServer4.Models.IdentityResource, IdentityResource>(MemberList.Source)
                .ForMember(x => x.UserClaims, opts => opts.MapFrom(src => src.UserClaims.Select(x => new IdentityClaim { Type = x })));
        }
    }

    public class IdentityApiScopeMapperProfile : Profile {
        /// <summary>
        /// <see cref="IdentityResourceMapperProfile"/>
        /// </summary>
        public IdentityApiScopeMapperProfile() {
            // entity to model
            CreateMap<ApiScope, IdentityServer4.Models.ApiScope>(MemberList.Destination)
                .ForMember(x => x.UserClaims, opt => opt.MapFrom(src => src.UserClaims.Select(x => x.Type)));

            // model to entity
            CreateMap<IdentityServer4.Models.ApiScope, ApiScope>(MemberList.Source)
                .ForMember(x => x.UserClaims, opts => opts.MapFrom(src => src.UserClaims.Select(x => new IdentityClaim { Type = x })));
        }
    }
}
