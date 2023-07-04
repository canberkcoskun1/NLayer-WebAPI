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
	internal class ProductSeed : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
			builder.HasData(
			new Product
			{
				Id = 1,
				
				Name = "Faber Castell",
				Stock = 10,
				Price = 100,
				CreatedDate = DateTime.Now,
			},
			new Product
			{
				Id = 1,
				CategoryId = 1,
				Name = "Rotring",
				Stock = 12,
				Price = 90,
				CreatedDate = DateTime.Now,
			},
			new Product
			{
				Id = 1,
				CategoryId = 2,
				Name = "Nutuk",
				Stock = 15,
				Price = 150,
				CreatedDate = DateTime.Now,
			},
			new Product
			{
				Id = 2,
				CategoryId = 2,
				Name = "The 100",
				Stock = 5,
				Price = 100,
				CreatedDate = DateTime.Now,
			},
			new Product
			{
				Id = 1,
				CategoryId = 3,
				Name = "GIPTA",
				Stock = 4,
				Price = 45,
				CreatedDate = DateTime.Now,
			}
			);
		}
	}
}
