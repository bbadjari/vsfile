////////////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2021 Bernard Badjari
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Linq;
using System.Reflection;
using VSFile.Properties;

namespace VSFile.Solution
{
	/// <summary>
	/// Visual Studio solution file project reference path resolver factory.
	/// </summary>
	internal class SolutionFileProjectPathResolverFactory
	{
		/// <summary>
		/// No solution file project reference path resolver.
		/// </summary>
		public const ISolutionFileProjectPathResolver NoPathResolver = null;

		////////////////////////////////////////////////////////////////////////
		// Methods

		/// <summary>
		/// Create solution file project reference path resolver.
		/// </summary>
		/// <param name="typeGuid">
		/// String representing project type GUID.
		/// </param>
		/// <param name="formatVersion">
		/// Integer representing solution file format version.
		/// </param>
		/// <returns>
		/// ISolutionFileProjectPathResolver instance representing solution file project reference path resolver.
		/// </returns>
		public static ISolutionFileProjectPathResolver Create(string typeGuid, int formatVersion)
		{
			if (string.IsNullOrWhiteSpace(typeGuid))
				throw new ArgumentException(ExceptionMessages.InvalidProjectTypeGuid);

			Type pathResolverType = GetPathResolverType(typeGuid, formatVersion);

			return Create(pathResolverType);
		}

		/// <summary>
		/// Create solution file project reference path resolver from given type.
		/// </summary>
		/// <param name="pathResolverType">
		/// Type representing solution file project reference path resolver type.
		/// </param>
		/// <returns>
		/// ISolutionFileProjectPathResolver instance representing solution file project reference path resolver.
		/// </returns>
		private static ISolutionFileProjectPathResolver Create(Type pathResolverType)
		{
			if (pathResolverType != null)
				return (ISolutionFileProjectPathResolver) Activator.CreateInstance(pathResolverType);

			return NoPathResolver;
		}

		/// <summary>
		/// Get solution file project reference path resolver type matching given attribute values.
		/// </summary>
		/// <param name="typeGuid">
		/// String representing project type GUID.
		/// </param>
		/// <param name="formatVersion">
		/// Integer representing solution file format version.
		/// </param>
		/// <returns>
		/// Type representing solution file project reference path resolver type, null if no match found.
		/// </returns>
		private static Type GetPathResolverType(string typeGuid, int formatVersion)
		{
			return Assembly.GetExecutingAssembly().GetTypes()
				.Where(type => typeof(ISolutionFileProjectPathResolver).IsAssignableFrom(type) && !type.IsInterface)
				.FirstOrDefault(type => type.GetCustomAttribute<SolutionFileProjectAttribute>().IsMatch(typeGuid, formatVersion));
		}
	}
}
