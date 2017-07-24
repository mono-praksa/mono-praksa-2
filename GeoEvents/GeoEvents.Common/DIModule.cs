namespace GeoEvents.Common
{
    #region Methods

    public class DIModule : Ninject.Modules.NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IFilter>().To<IFilter>();
        }
    }

    #endregion Methods
}