using ECommerce.Application.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Common.Helpers
{
    public static class PaginationValidator
    {
        public static void Validate(PaginationParams pagination)
        {
            ArgumentNullException.ThrowIfNull(pagination);

            if (pagination.PageNumber < 1)
                throw new ArgumentOutOfRangeException(
                    nameof(pagination.PageNumber));

            if (pagination.PageSize < 1 || pagination.PageSize > 100)
                throw new ArgumentOutOfRangeException(
                    nameof(pagination.PageSize));
        }
    }
}
