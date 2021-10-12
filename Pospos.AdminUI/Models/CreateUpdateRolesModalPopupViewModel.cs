namespace Pospos.AdminUI.Models
{
    public class CreateUpdateRolesModalPopupViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SystemName { get; set; }
        public bool IsSystemRole { get; set; }
    }
}
