namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
        : ICommand<CreateProductResult>;  

    public record CreateProductResult(Guid Id);

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
            //RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }

    internal class CreateProductHandler(IDocumentSession session)
        //,IValidator<CreateProductCommand> validator  // dung directi như vậy khiến lặp lại ở các handler khác, nên ta sẽ tạo 1 pipeline để xử lý validation
        : ICommandHandler<CreateProductCommand, CreateProductResult> //IDocumentSession la lop interface cua Marten de thao tac voi database, no se duoc inject vao qua constructor
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            //create a new product entity from the command data 
            //save to database

            //dung block sau thi se bi lap lai o cac handler khac, nen tach no thanh 1 pipline xử lý validation 
            //var result = await validator.ValidateAsync(command, cancellationToken);
            //var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
            //if (errors.Any())
            //{
            //    throw new ValidationException(errors.FirstOrDefault());
            //}

            //return createproductresult with the generated id
            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price
            };

            //save to database and get the generated Id

            session.Store(product); //store method cua Marten de luu entity vao database, no se tu dong generate id cho product
            await session.SaveChangesAsync(cancellationToken);

            return new CreateProductResult(product.Id); 

        }
    }
}
