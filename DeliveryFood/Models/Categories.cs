//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DeliveryFood.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class Categories
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Categories()
        {
            this.Products = new HashSet<Products>();
        }
    
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Img_category { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Products> Products { get; set; }

        public List<Categories> FetchCategories()
        {
            List<Categories> categories = new List<Categories>();

            using (DeliveryEntitiesDb db = new DeliveryEntitiesDb())
            {
                categories = db.Categories.ToList<Categories>();

            }

            return categories;
        }
    }
}