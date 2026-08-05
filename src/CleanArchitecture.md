tại sao họ nói là api layer sẽ bring all application layer và inject vào api layer.
 Nhưng hình bên phải tôi lại đang hiểu theo chiều mũi tên là như sau: domain -> application layer tức là domain nó sẽ giao tiếp với  application layer qua lớp abtract/interface, thì lớp application layer này nó sẽ implement logic của interface gọi  application layer trong domain, 

tương tự ,  application layer sẽ có abtract/interface của infractructure layer cũng như presentation. Vì vậy infractructure  và presentation sẽ implement logic của interface gọi infractructure  và presentation trong application. thế thì tại sao họ lại nói là api layer sẽ bring all application layer và inject vào api layer.

toi đang hiểu luồng đúng không, bạn giải thích thật kỹ và dễ hiểu cho tôi 

Bạn đang chạm đúng vào chỗ mà rất nhiều người học DDD + Clean Architecture bị nhầm. Thực ra có 2 loại dependency khác nhau đang bị trộn lẫn:

Dependency lúc compile (code dependency) → thể hiện bằng mũi tên trong hình.
Dependency lúc runtime (Dependency Injection) → chính là câu họ nói "API layer brings everything together and injects dependencies".

Hai cái này hoàn toàn khác nhau.

Mình sẽ giải thích từ từ.

1. Đầu tiên hãy đọc đúng ý nghĩa của hình

Ở hình bên phải:

Domain -----> Application -----> Infrastructure
                      \
                       \
                        -----> Presentation(API)

Nhiều người nhìn mũi tên và nghĩ là:

Domain gọi Application

Thực ra không phải.

Mũi tên này không phải call flow.

Nó là dependency direction.

Có nghĩa là:

Application phụ thuộc Domain
Infrastructure phụ thuộc Application
Presentation(API) phụ thuộc Application

Nói cách khác:

Application
   using Domain

Infrastructure
   using Application

Presentation
   using Application

không phải

Domain using Application
2. Domain có biết Application không?

Không.

Đây là nguyên tắc quan trọng nhất.

+--------------------+
| Presentation(API)  |
+--------------------+
          |
          V
+--------------------+
| Application        |
+--------------------+
          |
          V
+--------------------+
| Domain             |
+--------------------+

Domain là trung tâm.

Nó không biết:

API
Database
EF Core
Redis
RabbitMQ

Nó chỉ biết business.

Ví dụ

public class Order
{
    public void Confirm()
    {
        ...
    }
}

Order không biết:

SQL
HTTP
Controller
3. Interface nên nằm ở đâu?

Đây là chỗ bạn đang hiểu hơi lệch.

Bạn nói:

Domain giao tiếp với Application qua interface.

Thực ra là ngược.

Thông thường:

Application
      |
      V
Domain

Application gọi Domain.

Ví dụ

public class CreateOrderHandler
{
    public void Handle(...)
    {
        Order order = new Order();

        order.Confirm();
    }
}

Ở đây

Application

↓

gọi

↓

Domain

Domain không biết Application tồn tại.

4. Interface Repository nằm ở đâu?

Ví dụ:

Ta cần lưu Order.

Application cần lưu.

Application không muốn biết SQL.

Nó chỉ cần

public interface IOrderRepository
{
    Task Save(Order order);
}

Interface này nằm trong

Application

hoặc nhiều project sẽ đặt trong Domain.

Quan trọng là:

Interface nằm ở Core.

Implementation nằm ở ngoài.

Ví dụ

Application
-----------------

interface IOrderRepository

Infrastructure

class SqlOrderRepository
    : IOrderRepository
{
}

Cho nên dependency sẽ là

Infrastructure
      |
implements
      |
Application Interface
5. API có implement interface của Application không?

Thông thường:

Không.

API thường chỉ gọi Application.

Ví dụ

POST /orders

Controller

public class OrderController
{
    private readonly ICreateOrderUseCase _useCase;

    public OrderController(ICreateOrderUseCase useCase)
    {
        _useCase = useCase;
    }

    public async Task<IActionResult> Create(...)
    {
        await _useCase.Execute(...);
    }
}

Ở đây

Controller

↓

gọi

↓

Application

Controller không implement gì cả.

6. Infrastructure mới là nơi implement interface

Ví dụ

Application

public interface IEmailService
{
    Task Send();
}

Infrastructure

public class SmtpEmailService
    : IEmailService
{
}

Hay

Application

public interface IOrderRepository
{
}

Infrastructure

public class SqlOrderRepository
    : IOrderRepository
{
}
7. Vậy câu "API layer brings together all application layer and injects them" nghĩa là gì?

Đây chính là phần Composition Root.

Ví dụ ASP.NET Core

Program.cs

builder.Services.AddScoped<
    IOrderRepository,
    SqlOrderRepository>();

builder.Services.AddScoped<
    IEmailService,
    SmtpEmailService>();

builder.Services.AddScoped<
    CreateOrderHandler>();

Ở đây

API project

đang làm nhiệm vụ:

tạo object
nối các dependency
inject chúng

Nó không implement business.

Nó chỉ lắp ráp.

Ví dụ

Controller
      |
      | cần
      V
ICreateOrderHandler
      |
      |
DI Container
      |
      |
CreateOrderHandler
      |
      |
IOrderRepository
      |
      |
SqlOrderRepository

API chính là nơi đăng ký

IRepository
        |
        |
SqlRepository

và

IEmailService
        |
        |
SmtpEmailService

Sau đó khi Controller chạy

DI container sẽ tự tạo

Controller
    |
CreateOrderHandler
    |
SqlRepository

Đó chính là ý họ nói

API layer brings everything together.

Nó giống như người lắp Lego.

8. Tại sao không inject trong Infrastructure?

Vì Infrastructure không phải điểm bắt đầu của chương trình.

Điểm bắt đầu luôn là

Program.cs

hay

Main()

Chính nơi này quyết định:

Repository dùng SQL

hay

Repository dùng Mongo

hay

Repository Fake

Ví dụ

Development

services.AddScoped<IOrderRepository, FakeRepository>();

Production

services.AddScoped<IOrderRepository, SqlRepository>();

Application không cần sửa dòng nào.

9. Luồng chạy thực tế

Khi client gọi API:

Browser

↓

Controller (Presentation)

↓

Application

↓

Domain

↓

Application

↓

Infrastructure (Repository)

↓

Database

Luồng dữ liệu là như vậy.

10. Nhưng dependency của source code lại là
Presentation
      |
      V
Application
      |
      V
Domain

Infrastructure
      |
      V
Application

Đây là điều rất quan trọng:

Luồng thực thi (runtime call flow) và
Hướng phụ thuộc của mã nguồn (compile-time dependency)

không nhất thiết giống nhau.

Infrastructure có thể được Application gọi thông qua interface, nhưng ở mức mã nguồn, Infrastructure mới là phía tham chiếu (implements) interface của Application. Việc kết nối giữa interface và implementation được thực hiện bởi DI container trong API.

11. Sơ đồ đầy đủ
                Presentation(API)
                       |
                       |
              uses Application
                       |
        +--------------+--------------+
        |                             |
        | calls                      |
        V                             |
                Application
        |                             ^
        | uses Domain                 |
        V                             |
                  Domain              |
                                      |
                interface IRepository |
                interface IEmail      |
                                      |
                                      |
                 implements           |
                                      |
                Infrastructure -------+
                 SqlRepository
                 SmtpEmail

Cuối cùng, Program.cs trong API sẽ "lắp ráp" tất cả:

services.AddScoped<IOrderRepository, SqlOrderRepository>();
services.AddScoped<IEmailService, SmtpEmailService>();
services.AddScoped<ICreateOrderUseCase, CreateOrderUseCase>();

Đó chính là ý của câu "API layer brings together all application layer components and injects them": API không chứa nghiệp vụ hay implement các interface của Application, mà đóng vai trò Composition Root — nơi đăng ký các implementation của Infrastructure, tạo DI container và kết nối (wire up) toàn bộ các thành phần để ứng dụng có thể chạy. Đây là khái niệm về Dependency Injection ở runtime, khác với các mũi tên trong sơ đồ vốn mô tả dependency giữa các project/layer khi biên dịch.