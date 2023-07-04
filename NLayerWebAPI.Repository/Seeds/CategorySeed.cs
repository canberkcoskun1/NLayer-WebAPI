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
	internal class CategorySeed : IEntityTypeConfiguration<CategorySeed>
	{
		public void Configure(EntityTypeBuilder<CategorySeed> builder)
		{
			builder.HasData(
				new Category { Id = 1, Name = "Pencils" },
				new Category { Id = 2, Name = "Books" },
				new Category { Id = 3, Name = "Notepads" });
		}
	}
}
