using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;

namespace Cake.Xamarin.Tests.Fakes
{
    internal sealed class FakeCakeArguments : ICakeArguments
    {
        private readonly Dictionary<string, List<string>> _arguments;

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public IReadOnlyDictionary<string, List<string>> Arguments => _arguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeArguments"/> class.
        /// </summary>
        public FakeCakeArguments()
        {
            _arguments = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Initializes the argument list.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        public void SetArguments(IDictionary<string, List<string>> arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }
            _arguments.Clear();
            foreach (var argument in arguments)
            {
                _arguments.Add(argument.Key, argument.Value);
            }
        }

        /// <summary>
        /// Determines whether or not the specified argument exist.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <returns>
        ///   <c>true</c> if the argument exist; otherwise <c>false</c>.
        /// </returns>
        public bool HasArgument(string name)
        {
            return _arguments.ContainsKey(name);
        }

        public ICollection<string> GetArguments(string name)
        {
            _arguments.TryGetValue(name, out var arguments);
            return arguments ?? (ICollection<string>)Array.Empty<string>();
        }

        /// <summary>
        /// Gets an argument.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <returns>The argument value.</returns>
        public string GetArgument(string name)
        {
            return GetArguments(name).LastOrDefault();
        }
    }
}