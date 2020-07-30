#region Namespaces

using Microsoft.Extensions.DependencyInjection;
using SievoParser.Domain.Configuration;
using System;

#endregion

namespace SievoParser.Domain
{
    /// <summary>
    /// Bootstrapper class responsible for initializing DI, logging , configuration etc.
    /// </summary>
    public class Bootstrapper
    {
        #region Fields

        /// <summary>
        /// The lazy instance
        /// </summary>
        private static readonly Lazy<Bootstrapper> Lazy = new Lazy<Bootstrapper>(() => new Bootstrapper());

        /// <summary>
        /// The service provider
        /// </summary>
        private static IServiceProvider ServiceProvider;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static Bootstrapper Instance { get { return Lazy.Value; } }

        /// <summary>
        /// Initializes the related work for this instance.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IConfiguration Config
        {
            get
            {
                var service = ServiceProvider.GetService<IConfiguration>();
                return service;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="Bootstrapper" /> class from being created.
        /// </summary>
        private Bootstrapper()
        {            
        }

        #endregion

        #region Methods               

        /// <summary>
        /// Registers the services.
        /// </summary>
        public void RegisterServices()
        {
            // Setup DI
            ServiceProvider = new ServiceCollection()
                .AddSingleton<IConfiguration, DefaultConfiguration>()
                .BuildServiceProvider();
        }

        /// <summary>
        /// Disposes the services.
        /// </summary>
        public void DisposeServices()
        {
            if (ServiceProvider == null)
            {
                return;
            }
            if (ServiceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        #endregion
    }
}
