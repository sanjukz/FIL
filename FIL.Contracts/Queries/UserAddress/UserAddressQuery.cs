using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.UserAddress;
using System;

namespace FIL.Contracts.Queries.UserAddress
{
    public class UserAddressQuery : IQuery<UserAddressQueryResult>
    {
        public Guid UserAltId { get; set; }
        public AddressTypes AddressTypeId { get; set; }
    }
}