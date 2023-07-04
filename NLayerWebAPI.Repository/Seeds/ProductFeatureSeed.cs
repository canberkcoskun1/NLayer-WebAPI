using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayerWebAPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayerWebAPI.Repository.Seeds
{
	internal class ProductFeatureSeed : IEntityTypeConfiguration<ProductFeature>
	{
		public void Configure(EntityTypeBuilder<ProductFeature> builder)
		{
			builder.HasData(
			new ProductFeature
			{
				Id = 1,
				ProductId = 1,
				Color = "Red",
				Width = 7,
				Height = 2,

			},
			new ProductFeature
			{
				Id = 2,
				ProductId = 2,
				Color = "Brown",
				Width = 7,
				Height = 2,

			}
			);
		}
	}
}
