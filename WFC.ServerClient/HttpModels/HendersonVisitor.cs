using System;

namespace WFC.ServerClient.HttpModels
{
    public partial class HendersonVisitor
    {
        public int Id { get; set; }
        public string IcCardNo { get; set; }
        public string CredentialId { get; set; }
        public string Name { get; set; }
        public string IdCardNo { get; set; }
        public string VisitorPhoto { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string Nation { get; set; }
        public string Address { get; set; }
        public string HendersonTenantId { get; set; }
        public string HendersonTenantName { get; set; }
        public string Floors { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string HendersonTenantPersonName { get; set; }
        public string HendersonTenantPersonPhone { get; set; }
        public int NumberOfAccess { get; set; }
        public string Buildings { get; set; }
    }
}
