using Ninject;
using System;

namespace DataExtractService.NinjectKernel
{
	public class ComponentKernel
	{
		static IKernel Kernel { get; set; }

		/// <summary>
		/// Loads the dependancies to the Kernel either from Code or from a XML file
		/// </summary>
		/// <param name="NinjectBindingsXml">path of the NinjectBindingsXml</param>
		public static void LoadKernel(string NinjectBindingsXml)
		{
			try
			{
				if (Kernel == null)
				{
					Kernel = new StandardKernel();
                    string filePath = String.Concat(AppDomain.CurrentDomain.BaseDirectory, NinjectBindingsXml);
                    //Loads the dependancies from XML file
                    Kernel.Load(filePath);
				}
			}
			catch (Exception)
			{
				throw new Exception("Failed to load the Kernel");
			}
		}

		/// <summary>
		/// Resolves the given generic type to the concrete implementation from the Kernel
		/// </summary>
		/// <typeparam name="T">Generic type for which instance is required</typeparam>
		/// <returns>Impelementation that is configured for the type passed</returns>
		public static T GetInstance<T>() => Kernel.Get<T>();
	}
}
