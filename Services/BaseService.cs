using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeoLib.Repositories;
using TeoLib.Utils;

namespace TeoLib.Services
{
	public class BaseService : IDisposable
	{
		public static DebugWrite DebugWriteLine { get; set; }
		public ITeoLibRepository Repository { get; protected set; }

		public BaseService()
		{
			Repository = TeoLibRepositoryXML.Instance;

			if (null == Repository.DebugWriteLine && null != BaseService.DebugWriteLine)
				Repository.DebugWriteLine = BaseService.DebugWriteLine;
		}

		protected void DebugWrite(string text)
		{
			if (null != BaseService.DebugWriteLine)
				BaseService.DebugWriteLine(text);
		}

		#region IDisposable
		private bool disposed = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				Repository.Dispose();
			}

			this.disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}
