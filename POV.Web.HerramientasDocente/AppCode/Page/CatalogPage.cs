namespace POV.AppCode.Page
{
    public abstract class CatalogPage : PageBase
    {
        // Display CRUD acciones

        protected abstract void DisplayCreateAction();

        protected abstract void DisplayReadAction();

        protected abstract void DisplayUpdateAction();

        protected abstract void DisplayDeleteAction();

        
    }
}