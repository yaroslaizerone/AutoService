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
    
    public partial class OrderProduct
    {
        public int OrderID { get; set; }
        public string ProductArticleNumber { get; set; }
        public Nullable<int> CountProduct { get; set; }
    
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }

        public string ProductForList
        {
            get
            {
                return $"{this.Product.ProductName.ToString()} {CountProduct.ToString()}";
            }
        }
    }

    
}
