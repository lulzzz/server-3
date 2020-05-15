﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sygic.Corona.Contracts.Responses;
using Sygic.Corona.Domain;
using Sygic.Corona.Infrastructure;

namespace Sygic.Corona.Application.Queries
{
    public class GetQuarantineQueryHandler : IRequestHandler<GetQuarantineQuery, GetQuarantineResponse>
    {
        private readonly IRepository repository;
        private readonly CoronaContext context;

        public GetQuarantineQueryHandler(IRepository repository, CoronaContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<GetQuarantineResponse> Handle(GetQuarantineQuery request, CancellationToken cancellationToken)
        {
            var profile = await repository.GetProfileAsyncNt(request.ProfileId, request.DeviceId, cancellationToken);

            if (profile?.QuarantineAddress == null)
            {
                return null;
            }

            return new GetQuarantineResponse
            {
                CovidPass = profile.CovidPass,
                QuarantineStart = profile.QuarantineBeginning.Value,
                QuarantineEnd = profile.QuarantineEnd.Value,
                BorderCrossedAt = profile.BorderCrossedAt.Value,
                Address = new AddressResponse
                {
                    Latitude = profile.QuarantineAddress.Latitude,
                    Longitude = profile.QuarantineAddress.Longitude,
                    Country = profile.QuarantineAddress.CountryCode.ToUpper(),
                    City = profile.QuarantineAddress.City,
                    ZipCode = profile.QuarantineAddress.ZipCode,
                    StreetName = profile.QuarantineAddress.StreetName,
                    StreetNumber = profile.QuarantineAddress.StreetNumber,
                }
            };
        }
    }
}