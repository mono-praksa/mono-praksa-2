namespace GeoEvents.Common
{
    #region Methods

    public class DIModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IFilter>().To<IFilter>();
        }
    }

    #endregion Methods
}