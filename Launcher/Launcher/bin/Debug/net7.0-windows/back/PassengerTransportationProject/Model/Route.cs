//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PassengerTransportationProject.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Route
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Route()
        {
            this.MyTicket = new HashSet<MyTicket>();
        }
    
        public int RouteID { get; set; }
        public Nullable<System.DateTime> DateTargetStation { get; set; }
        public Nullable<System.DateTime> DateDepartureStation { get; set; }
        public Nullable<decimal> ReservedSeatCost { get; set; }
        public Nullable<decimal> CompartmentCost { get; set; }
        public Nullable<int> StationDepartureID { get; set; }
        public Nullable<int> StationTargetID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MyTicket> MyTicket { get; set; }
        public virtual StationDeparture StationDeparture { get; set; }
        public virtual StationTarget StationTarget { get; set; }
    }
}
