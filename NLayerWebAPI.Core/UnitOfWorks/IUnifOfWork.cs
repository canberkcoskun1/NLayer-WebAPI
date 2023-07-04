﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayerWebAPI.Core.UnitOfWorks
{
	public interface IUnifOfWork
	{
		void Commit();
		Task CommitAsync();
	}
}
