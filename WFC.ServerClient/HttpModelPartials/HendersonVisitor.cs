namespace WFC.ServerClient.HttpModels
{
    [PropertyChanged.ImplementPropertyChanged]
    public partial class HendersonVisitor
    {
        public string AreaCode { get; set; } = "86";
        public string TenantAreaCode { get; set; } = "86";
    }
}
