# Service Management

Sometimes, or actually most of the time, we really don't want to create and initialize an object every time we want to use it. And I'm pretty sure you're pretty much thinking the same as well when you're programming a random simple or complex software.

So I decided to make a simple service manager that manages and receives registered services from a get-go, they are usually used in singleton static classes.

## Usage

Here's how we usually use it in Aridity Cereon, feel free to judge it whenever you like:

```cs
    /// <summary>
    /// A static entry-point class for retrieving Cereon singleton services without
    /// having to create them every time they're going to be used by the application.
    /// </summary>
    public static class AC
    {
        private static IServiceManager _manager = new ServiceManager();

        // (not related to Ascorbic's service management)
        private static RemoteServiceManager _remoteMgr = new RemoteServiceManager();

        /// <summary>
        /// Gets the main service container/manager for the whole Cereon application.
        /// </summary>
        public static IServiceManager Services
        {
            get { return _manager; }
            set { _manager = value; }
        }

        /// <summary>
        /// Gets the main remote service container/manager for the whole Cereon application.
        /// </summary>
        public static RemoteServiceManager RemoteServices
        {
            get { return _remoteMgr; }
            set { _remoteMgr = value; }
        }

        /// <summary>
		/// Gets a service. Returns null if service is not found.
		/// </summary>
		public static T GetService<T>() where T : class
        {
            return Services.GetService<T>();
        }

        /// <summary>
        /// Gets a service. Returns null if service is not found.
        /// </summary>
        public static T GetRequiredService<T>() where T : class
        {
            var service = GetService<T>();
            return service ?? throw new ServiceNotFoundException(
                string.Format(SR.ServiceMgr_NotRegistered, typeof(T).FullName)
            );
        }

        /// <summary>
		/// Gets a remote service. Returns null if service is not found.
		/// </summary>
		public static T GetRemoteService<T>() where T : class
        {
            return RemoteServices.TryGet<T>();
        }

        /// <summary>
        /// Gets a remote service. Returns null if service is not found.
        /// </summary>
        public static T GetRequiredRemoteService<T>() where T : class
        {
            return RemoteServices.Get<T>();
        }

        /// <inheritdoc cref="IThreadHelper"/>
        public static IThreadHelper ThreadHelper
        {
            get { return GetRequiredService<IThreadHelper>(); }
        }

        /// <inheritdoc cref="ILogger"/>
        public static ILogger Log
        {
            get { return GetRequiredService<ILogger>(); }
        }

        /// <inheritdoc cref="Window"/>
        public static Window MainWindow
        {
            get { return GetRequiredService<Window>(); }
        }

        public static void Shutdown()
        {
            if (RemoteServices != null)
            {
                ThreadHelper.Run(() => RemoteServices.DisposeAsync().AsTask());
                RemoteServices = null;
            }

            if (Services != null)
            {
                if (Services is DisposableObject obj)
                    obj.Dispose();

                Services = null;
            }
        }
    }
```
