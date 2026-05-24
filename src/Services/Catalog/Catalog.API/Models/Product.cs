namespace Catalog.API.Models
{
    public class Product
    {
        public Guid Id { get; set; } // value type, luon co gia tri, default = Guid.Empty
        public string Name { get; set; } = default!; //C#8 , default voi string la null, ! la tat warning nullable   (nen dung = string.Empty)

        public List<string> Category { get; set; } = new(); //reference type, can new neu khong Category.Add("abc) throw null reference exception

        public string Description { get; set; } = default!;  //C#8 , default voi string la null, ! la tat warning nullable   (nen dung = string.Empty)

        public string ImageFile { get; set; } = default!; //C#8 , default voi string la null, ! la tat warning nullable

        public decimal Price { get; set; } // value type,  luon co gia tri, default = 0
    }
}
