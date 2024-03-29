//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AutoService.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.OrderProduct = new HashSet<OrderProduct>();
        }
    
        public int OrderID { get; set; }
        public int IDOrderStatus { get; set; }
        public System.DateTime OrderDate { get; set; }
        public System.DateTime OrderDeliveryDate { get; set; }
        public int OrderPickupPoint { get; set; }
        public string ClientFullName { get; set; }
        public int ReceiptCode { get; set; }
    
        public virtual PickupPoint PickupPoint { get; set; }
        public virtual StatusOrders StatusOrders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderProduct> OrderProduct { get; set; }

        public string AdressPickPoint
        {
            get
            {
               return this.PickupPoint.Addres.ToString();
            }
        }

        public string StatusOrder
        {
            get
            {
                return this.StatusOrders.StatusOrder.ToString();
            }
        }

        public string NameCustomer
        {
            get
            {
                if (ClientFullName.ToString() == "")
                    return "Имя заказчика не узаканно";
                else
                    return ClientFullName.ToString();
            }
        }
    }
}
