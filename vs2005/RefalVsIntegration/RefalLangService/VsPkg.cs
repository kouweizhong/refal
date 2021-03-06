﻿
/*-------------------------------------------------------------------------*/
/*                                                                         */
/*      RefalLangServicePackage, Refal5.NET language service package       */
/*      This file is a part of Refal5.NET project                          */
/*      Project license: http://www.gnu.org/licenses/lgpl.html             */
/*      Written by Y [21-04-06] <yallie@yandex.ru>                         */
/*                                                                         */
/*      Copyright (c) 2006-2007 Alexey Yakovlev                            */
/*      All Rights Reserved                                                */
/*                                                                         */
/*-------------------------------------------------------------------------*/

using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.Win32;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;

namespace yallie.RefalLangService
{
	/// <summary>
	/// This is the class that implements the package exposed by this assembly.
	///
	/// The minimum requirement for a class to be considered a valid package for Visual Studio
	/// is to implement the IVsPackage interface and register itself with the shell.
	/// This package uses the helper classes defined inside the Managed Package Framework (MPF)
	/// to do it: it derives from the Package class that provides the implementation of the 
	/// IVsPackage interface and uses the registration attributes defined in the framework to 
	/// register itself and its components with the shell.
	/// </summary>

	// This attribute tells the registration utility (regpkg.exe) that this class needs
	// to be registered as package.
	[PackageRegistration(UseManagedResourcesOnly = true)]

	// A Visual Studio component can be registered under different regitry roots; for instance
	// when you debug your package you want to register it in the experimental hive. This
	// attribute specifies the registry root to use if no one is provided to regpkg.exe with
	// the /root switch.
	[DefaultRegistryRoot("Software\\Microsoft\\VisualStudio\\8.0Exp")] // TODO: remove "Exp" in the release version

	// This attribute is used to register the informations needed to show the this package
	// in the Help/About dialog of Visual Studio.
	[InstalledProductRegistration(false, "#110", "#112", "1.0", IconResourceID = 400)]

	// In order be loaded inside Visual Studio in a machine that has not the VS SDK installed, 
	// package needs to have a valid load key (it can be requested at 
	// http://msdn.microsoft.com/vstudio/extend/). This attributes tells the shell that this 
	// package has a load key embedded in its resources.
	[ProvideLoadKey("Standard", "0.00 Alpha", "Refal5 Language Service", "yallie, inc.", 1)]

	// This attribute is used to associate the ".rgx" file extension with a language service
	[ProvideLanguageExtension(typeof(RefalLanguageService), ".ref")]

	// This attribute is needed to indicate that the type proffers a service
	[ProvideService(typeof(RefalLanguageService))]

	// Indicates that this managed type is visible to COM
	[ComVisible(true)]
	[Guid(GuidList.guidRefalLangServicePkgString)]
	public sealed class RefalLangServicePackage : Package, IDisposable
	{
		RefalLanguageService langService;

		/// <summary>
		/// Default constructor of the package.
		/// Inside this method you can place any initialization code that does not require 
		/// any Visual Studio service because at this point the package object is created but 
		/// not sited yet inside Visual Studio environment. The place to do all the other 
		/// initialization is the Initialize method.
		/// </summary>
		public RefalLangServicePackage()
		{
			Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
		}

		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					// dispose managed resources
					if (langService != null)
					{
						langService.Dispose();
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Overriden Package Implementation
		#region Package Members

		/// <summary>
		/// Initialization of the package; this method is called right after the package is sited, so this is the place
		/// where you can put all the initilaization code that rely on services provided by VisualStudio.
		/// </summary>
		protected override void Initialize()
		{
			Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
			base.Initialize();

			// create new instance of language service
			langService = new RefalLanguageService();
			langService.SetSite(this);

			// add service to VSPackage service container
			IServiceContainer sc = (IServiceContainer)this;
			sc.AddService(typeof(RefalLanguageService), langService, true);
		}

		#endregion

		#region IDisposable members

		public void Dispose()
		{
			Dispose(true);

			// This object will be cleaned up by the Dispose() method.
			// We have to take it off the finalization queue to prevent the
			// clean up code for this object from executing a second time.
			GC.SuppressFinalize(this);
		}

		#endregion
    }
}