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
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.OrderProduct = new HashSet<OrderProduct>();
        }
    
        public string ProductArticleNumber { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int IDProductCategory { get; set; }
        public string ProductPhoto { get; set; }
        public int IDManufacture { get; set; }
        public decimal ProductCost { get; set; }
        public Nullable<byte> ProductDiscountAmount { get; set; }
        public int ProductQuantityInStock { get; set; }
        public int IDUnit { get; set; }
        public int MaxDiscountAmount { get; set; }
        public int IDSupplier { get; set; }
        public Nullable<int> CountPack { get; set; }
        public Nullable<int> MinCount { get; set; }
    
        public virtual Manufacture Manufacture { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderProduct> OrderProduct { get; set; }
        public virtual ProductCategors ProductCategors { get; set; }
        public virtual Suppliers Suppliers { get; set; }
        public virtual Units Units { get; set; }

        
        public string Background
        {
            get
            {
                if (this.ProductDiscountAmount > 15)
                    return "#7fff00";
                else
                    return "#fff";
            }
        }

        public string GetNameManufacture
        {
            get
            {
                return this.Manufacture.ManufactureName.ToString();
            }
        }

        public string CostWithDiscount
        {
            get
            {
                if (this.MaxDiscountAmount > 0)
                {
                    var costWithDiscount = Convert.ToDouble(this.ProductCost) - Convert.ToDouble(this.ProductCost) * Convert.ToDouble(this.ProductDiscountAmount / 100.00);
                    return costWithDiscount.ToString();
                }
                return this.ProductCost.ToString();
            }
        }
        public string ImgPath
        {
            get
            {
                var path = "F:/ярослав/коды/AutoService/AutoService/Resources/Product/" + this.ProductPhoto;
                return path;
            }
        }
        public string CategoryProduct
        {
            get
            {
                return this.ProductCategors.ProductCategory.ToString();
            }
        }
    }
}
