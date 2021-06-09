using AutoMapper;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;

namespace VehicleTracking.IdentityServer.Mappers {
    public static class PersistedGrantMappers {
        static PersistedGrantMappers() {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<PersistedGrantMapperProfile>()).CreateMapper();
        }

        internal static IMapper Mapper { get; }
        public static PersistedGrant ToModel(this Model.PersistedGrant token) {
            return token == null ? null : Mapper.Map<PersistedGrant>(token);
        }

        public static Model.PersistedGrant ToEntity(this PersistedGrant token) {
            return token == null ? null : Mapper.Map<Model.PersistedGrant>(token);
        }

        public static void UpdateEntity(this PersistedGrant token, PersistedGrant target) {
            Mapper.Map(token, target);
        }
    }
}