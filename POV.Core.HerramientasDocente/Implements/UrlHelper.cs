using System;
using System.Web;

namespace POV.Core.HerramientasDocente.Implement
{
	public class UrlHelper
	{
		private const string LOGIN = "~/Auth/Login.aspx";
		private const string LOGOUT = "~/Auth/Logout.aspx";
		private const string DEFAULT = "~/Default.aspx";
		private const string PAGE404 = "~/404.aspx";
	
		public static string GetLoginURL()
		{
			return Path(LOGIN);
		}
		
		public static string GetLogoutURL()
		{
			return Path(LOGOUT);
		}
		public static string Get404URL()
		{
			return Path(PAGE404);
		}

		public static string GetDefaultURL()
		{
			return Path(DEFAULT);
		}

		#region auxiliares
		private static string Path(string virtualPath)
		{
			return VirtualPathUtility.ToAbsolute(virtualPath);
		}

		private static string Path(string virtualPath, params object[] args)
		{
			return Path(string.Format(virtualPath, args));
		}
		#endregion		
	}
}
