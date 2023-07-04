using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayerWebAPI.Core.Entities
{
	public class Category : BaseEntity
	{
        public string Name { get; set; }
		public ICollection<Product> Product { get; set; }
    }
}
