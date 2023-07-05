using NLayerWebAPI.Core.DTOs;
using NLayerWebAPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayerWebAPI.Core.Services
{
	public interface IProductService : IService<Product>
	{
		//Dönüş tipi özelleşeceği için Özel bir Response döneceğiz. Bu servis için bir Dto oluşturacağız.

		//Task<List<ProductsWithCategoryDto>> GetProductsWithCategory();

		// CustomResponseDto'yu direk kullanabiliriz. Controllerda kurtulmak için ve productService kullanacağımız için bu hale getirdik.
		Task<CustomResponseDto<List<ProductsWithCategoryDto>>> GetProductsWithCategory();
	}
}
